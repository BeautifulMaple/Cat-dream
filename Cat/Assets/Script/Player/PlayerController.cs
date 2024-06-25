using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using TMPro;
using UnityEngine;

namespace MyGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        public Transform playerTransform; // 플레이어의 Transform
        public GameObject nonGlowingStatue; // 빛나지 않는 석상 GameObject
        public GameObject glowingStatue; // 빛나는 석상 GameObject
        public float targetXRangeMin = 10f; // 멈출 지점의 X 좌표 최소값
        public float targetXRangeMax = 20f; // 멈출 지점의 X 좌표 최대값
        public float yRangeMin = 13f; // 멈출 지점의 Y 좌표 범위 최소값
        public float yRangeMax = 15f; // 멈출 지점의 Y 좌표 범위 최대값
        public float stopDuration = 3f; // 멈추는 시간 (초)
        public float messageXRangeMin = 13.0f;
        public float messageXRangeMax = 20.5f;
        public float messageYRangeMin = 0f;
        public float messageYRangeMax = 1f;

        private bool isPlayerStopped = false;
        private bool hasStoppedOnce = false; // 한 번 멈춘 상태를 기록
        private bool hasShownFirstMessages = false; // 첫 메시지 출력을 기록
        private TopDownCharacterController characterController;
        private Rigidbody2D rb;
        private Vector2 originalVelocity;
        private AudioSource audioSource; // 오디오를 재생할 AudioSource

        public AudioClip glowingSound; // 빛나는 석상이 활성화될 때 재생할 오디오 클립

        public TextMeshProUGUI textComponent;
        public GameObject textBox; // 텍스트 창의 GameObject를 참조
        public string[] firstMessages;
        public string[] secondMessages;
        public float typingSpeed = 0.05f;

        private int index = 0;
        private bool isTyping = false;
        private string[] currentMessages;

        private void Start()
        {
            glowingStatue.SetActive(false); // 처음에는 빛나는 석상을 비활성화
            characterController = GetComponent<TopDownCharacterController>();
            rb = GetComponent<Rigidbody2D>();
            originalVelocity = rb.velocity; // 초기 속도 저장

            // AudioSource 컴포넌트 초기화 및 오디오 클립 할당
            audioSource = glowingStatue.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = glowingStatue.AddComponent<AudioSource>();
            }
            audioSource.clip = glowingSound;

            textBox.SetActive(false); // 처음에는 텍스트 창 비활성화

            // 변수 초기화
            currentMessages = null;
        }

        private void Update()
        {
            // 첫 메시지를 출력할 위치에 도달했는지 체크
            if (!hasShownFirstMessages && IsPlayerInMessageRange())
            {
                hasShownFirstMessages = true;
                ShowMessages(firstMessages);
            }

            // 플레이어가 특정 좌표에 도달했는지 체크
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
                    StartCoroutine(HideTextBoxAfterDelay(0f)); // 메시지가 끝나면 즉시 텍스트 박스 비활성화
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

            // 5초 후에 텍스트 박스 비활성화
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
            // 플레이어의 위치가 특정 X 좌표와 Y 좌표 범위 내에 있는지 체크
            return playerTransform.position.x >= targetXRangeMin &&
                   playerTransform.position.x <= targetXRangeMax &&
                   playerTransform.position.y >= yRangeMin &&
                   playerTransform.position.y <= yRangeMax;
        }

        private void StopPlayerAndChangeStatue()
        {
            // 플레이어의 움직임을 멈추게 설정
            rb.velocity = Vector2.zero;
            characterController.enabled = false;

            // 한 번 멈춘 상태 기록
            hasStoppedOnce = true;

            // 3초 후에 석상을 변경하고 플레이어를 다시 움직이게 설정
            StartCoroutine(ChangeStatueAndResumePlayerMovement());
        }

        private IEnumerator ChangeStatueAndResumePlayerMovement()
        {
            yield return new WaitForSeconds(stopDuration); // 3초 대기

            // 빛나지 않는 석상을 비활성화하고 빛나는 석상을 활성화
            nonGlowingStatue.SetActive(false);
            glowingStatue.SetActive(true);

            // 플레이어의 움직임을 다시 활성화
            characterController.enabled = true;
            isPlayerStopped = false;

            // 오디오 재생
            if (audioSource != null && glowingSound != null)
            {
                audioSource.Play();
            }

            // 두 번째 메시지 출력
            ShowMessages(secondMessages);

            // 원래 속도로 복구
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
