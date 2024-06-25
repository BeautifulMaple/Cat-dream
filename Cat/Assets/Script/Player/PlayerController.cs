using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using TMPro;
using UnityEngine;

namespace MyGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        public Transform playerTransform; // �÷��̾��� Transform
        public GameObject nonGlowingStatue; // ������ �ʴ� ���� GameObject
        public GameObject glowingStatue; // ������ ���� GameObject
        public float targetXRangeMin = 10f; // ���� ������ X ��ǥ �ּҰ�
        public float targetXRangeMax = 20f; // ���� ������ X ��ǥ �ִ밪
        public float yRangeMin = 13f; // ���� ������ Y ��ǥ ���� �ּҰ�
        public float yRangeMax = 15f; // ���� ������ Y ��ǥ ���� �ִ밪
        public float stopDuration = 3f; // ���ߴ� �ð� (��)
        public float messageXRangeMin = 13.0f;
        public float messageXRangeMax = 20.5f;
        public float messageYRangeMin = 0f;
        public float messageYRangeMax = 1f;

        private bool isPlayerStopped = false;
        private bool hasStoppedOnce = false; // �� �� ���� ���¸� ���
        private bool hasShownFirstMessages = false; // ù �޽��� ����� ���
        private TopDownCharacterController characterController;
        private Rigidbody2D rb;
        private Vector2 originalVelocity;
        private AudioSource audioSource; // ������� ����� AudioSource

        public AudioClip glowingSound; // ������ ������ Ȱ��ȭ�� �� ����� ����� Ŭ��

        public TextMeshProUGUI textComponent;
        public GameObject textBox; // �ؽ�Ʈ â�� GameObject�� ����
        public string[] firstMessages;
        public string[] secondMessages;
        public float typingSpeed = 0.05f;

        private int index = 0;
        private bool isTyping = false;
        private string[] currentMessages;

        private void Start()
        {
            glowingStatue.SetActive(false); // ó������ ������ ������ ��Ȱ��ȭ
            characterController = GetComponent<TopDownCharacterController>();
            rb = GetComponent<Rigidbody2D>();
            originalVelocity = rb.velocity; // �ʱ� �ӵ� ����

            // AudioSource ������Ʈ �ʱ�ȭ �� ����� Ŭ�� �Ҵ�
            audioSource = glowingStatue.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = glowingStatue.AddComponent<AudioSource>();
            }
            audioSource.clip = glowingSound;

            textBox.SetActive(false); // ó������ �ؽ�Ʈ â ��Ȱ��ȭ

            // ���� �ʱ�ȭ
            currentMessages = null;
        }

        private void Update()
        {
            // ù �޽����� ����� ��ġ�� �����ߴ��� üũ
            if (!hasShownFirstMessages && IsPlayerInMessageRange())
            {
                hasShownFirstMessages = true;
                ShowMessages(firstMessages);
            }

            // �÷��̾ Ư�� ��ǥ�� �����ߴ��� üũ
            if (!hasStoppedOnce && !isPlayerStopped && IsPlayerAtTargetPosition())
            {
                isPlayerStopped = true;
                StopPlayerAndChangeStatue();
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
            {
                if (currentMessages != null && index < currentMessages.Length - 1)
                {
                    index++;
                    StartCoroutine(TypeSentence(currentMessages[index]));
                }
                else
                {
                    StartCoroutine(HideTextBoxAfterDelay(0f)); // �޽����� ������ ��� �ؽ�Ʈ �ڽ� ��Ȱ��ȭ
                }
            }
        }

        private IEnumerator TypeSentence(string sentence)
        {
            isTyping = true;
            textComponent.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                textComponent.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            isTyping = false;

            // 5�� �Ŀ� �ؽ�Ʈ �ڽ� ��Ȱ��ȭ
            //StartCoroutine(HideTextBoxAfterDelay(5f));
        }

        private IEnumerator HideTextBoxAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            textBox.SetActive(false);
        }

        private bool IsPlayerInMessageRange()
        {
            return playerTransform.position.x >= messageXRangeMin &&
                   playerTransform.position.x <= messageXRangeMax &&
                   playerTransform.position.y >= messageYRangeMin &&
                   playerTransform.position.y <= messageYRangeMax;
        }

        private bool IsPlayerAtTargetPosition()
        {
            // �÷��̾��� ��ġ�� Ư�� X ��ǥ�� Y ��ǥ ���� ���� �ִ��� üũ
            return playerTransform.position.x >= targetXRangeMin &&
                   playerTransform.position.x <= targetXRangeMax &&
                   playerTransform.position.y >= yRangeMin &&
                   playerTransform.position.y <= yRangeMax;
        }

        private void StopPlayerAndChangeStatue()
        {
            // �÷��̾��� �������� ���߰� ����
            rb.velocity = Vector2.zero;
            characterController.enabled = false;

            // �� �� ���� ���� ���
            hasStoppedOnce = true;

            // 3�� �Ŀ� ������ �����ϰ� �÷��̾ �ٽ� �����̰� ����
            StartCoroutine(ChangeStatueAndResumePlayerMovement());
        }

        private IEnumerator ChangeStatueAndResumePlayerMovement()
        {
            yield return new WaitForSeconds(stopDuration); // 3�� ���

            // ������ �ʴ� ������ ��Ȱ��ȭ�ϰ� ������ ������ Ȱ��ȭ
            nonGlowingStatue.SetActive(false);
            glowingStatue.SetActive(true);

            // �÷��̾��� �������� �ٽ� Ȱ��ȭ
            characterController.enabled = true;
            isPlayerStopped = false;

            // ����� ���
            if (audioSource != null && glowingSound != null)
            {
                audioSource.Play();
            }

            // �� ��° �޽��� ���
            ShowMessages(secondMessages);

            // ���� �ӵ��� ����
            rb.velocity = originalVelocity;
        }

        private void ShowMessages(string[] messages)
        {
            index = 0;
            currentMessages = messages;
            textBox.SetActive(true);
            StartCoroutine(TypeSentence(currentMessages[index]));
        }
    }
}
