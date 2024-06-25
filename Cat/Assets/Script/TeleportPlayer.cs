using TMPro;
using UnityEngine;
using System.Collections;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject normalStatue;  // ������ �ʴ� ����
    [SerializeField] private GameObject glowingStatue; // ������ ����
    [SerializeField] private AudioClip glowingStatueSound; // ȿ����

    private AudioSource audioSource; // ȿ������ ����� AudioSource
    private bool gameEnded = false; // ���� ���� ����

    public TextMeshProUGUI textComponent;
    public GameObject textBox; // �ؽ�Ʈ â�� GameObject�� ����
    public string[] firstMessages;
    public string[] endGameMessages;
    public float typingSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false;
    private string[] currentMessages;

    void Start()
    {
        // AudioSource�� �߰��ϰ� �����մϴ�.
        audioSource = gameObject.AddComponent<AudioSource>();
        textBox.SetActive(false); // ó������ �ؽ�Ʈ �ڽ��� ��Ȱ��ȭ
    }

    void Update()
    {
        if (!gameEnded)
        {
            CheckPlayerPosition();
            CheckEndGameCondition();
        }

        // �����̽��ٸ� ������ �� ���� �޽����� �̵�
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping && textBox.activeSelf)
        {
            DisplayNextMessage();
        }
    }

    private void DisplayNextMessage()
    {
        if (currentMessages != null && index < currentMessages.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence(currentMessages[index]));
        }
        else
        {
            StartCoroutine(HideTextBoxAfterDelay(0f)); // ��� �޽����� �� ������ٸ� �ؽ�Ʈ �ڽ� ��Ȱ��ȭ
        }
    }

    private void CheckPlayerPosition()
    {
        Vector2 playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);

        // ù ��° ����: x: -3 ~ -4, y: -9 ~ -11
        if (playerPosition.x >= -4f && playerPosition.x <= -3f && playerPosition.y >= -11f && playerPosition.y <= -9f)
        {
            TeleportPlayerTo(new Vector2(-28.5f, -25f), new Vector2(-29f, -25.5f));
        }
        // �� ��° ����: x: -25.5 ~ -20.5, y: -7 ~ -8
        else if (playerPosition.x >= -25.5f && playerPosition.x <= -20.5f && playerPosition.y >= -8f && playerPosition.y <= -7f)
        {
            TeleportPlayerTo(new Vector2(-14f, 14f));
            ActivateGlowingStatue(); // ������ ������ Ȱ��ȭ�մϴ�.
            ShowMessages(firstMessages); // �޽��� ���
        }
    }

    private void TeleportPlayerTo(Vector2 position1, Vector2 position2 = default(Vector2))
    {
        if (position2 == default(Vector2))
        {
            playerTransform.position = new Vector3(position1.x, position1.y, playerTransform.position.z);
        }
        else
        {
            // ������ ��ǥ ����
            float randomX = Random.Range(position1.x, position2.x);
            float randomY = Random.Range(position1.y, position2.y);
            playerTransform.position = new Vector3(randomX, randomY, playerTransform.position.z);
        }
    }

    private void ActivateGlowingStatue()
    {
        // ������ �ʴ� ������ ��Ȱ��ȭ�մϴ�.
        if (normalStatue != null)
        {
            normalStatue.SetActive(false);
        }

        // ������ ������ Ȱ��ȭ�մϴ�.
        if (glowingStatue != null)
        {
            glowingStatue.SetActive(true);
        }

        // ȿ������ ����մϴ�.
        if (glowingStatueSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(glowingStatueSound);
        }
    }

    private void CheckEndGameCondition()
    {
        Vector2 playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);

        // ��� ������ ������ Ȱ��ȭ�Ǿ� �ְ� �÷��̾ ������ ������ �������� ��
        if (AreAllGlowingStatuesActive() &&
            playerPosition.x >= -14.5f && playerPosition.x <= 15.5f && playerPosition.y >= 17.5f && playerPosition.y <= 18.5f)
        {
            ShowMessages(endGameMessages); // �޽��� ���
            EndGame(); // ���� ����
        }
    }

    private bool AreAllGlowingStatuesActive()
    {
        // stone statue1, stone statue2, stone statue3 �±װ� �ִ� ��� ������Ʈ�� Ȱ��ȭ �������� Ȯ���մϴ�.
        GameObject[] glowingStatues1 = GameObject.FindGameObjectsWithTag("stone statue1");
        GameObject[] glowingStatues2 = GameObject.FindGameObjectsWithTag("stone statue2");
        GameObject[] glowingStatues3 = GameObject.FindGameObjectsWithTag("stone statue3");

        foreach (GameObject statue in glowingStatues1)
        {
            if (!statue.activeSelf)
            {
                return false;
            }
        }
        foreach (GameObject statue in glowingStatues2)
        {
            if (!statue.activeSelf)
            {
                return false;
            }
        }
        foreach (GameObject statue in glowingStatues3)
        {
            if (!statue.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    private void EndGame()
    {
        // �÷��̾��� y ��ǥ�� 5��ŭ ������ŵ�ϴ�.
        playerTransform.position += new Vector3(0, 5f, 0);
        

        // ȿ���� ���
        if (glowingStatueSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(glowingStatueSound);
        }

        gameEnded = true;

        // ���� ���� ���� �߰�
        Debug.Log("���� ����!");
        // ���� ���Ḧ ó���ϴ� �߰� ������ ���⿡ �߰��� �� �ֽ��ϴ�.

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void ShowMessages(string[] messages)
    {
        index = 0;
        currentMessages = messages;
        textBox.SetActive(true);
        StartCoroutine(TypeSentence(currentMessages[index]));
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
    }

    private IEnumerator HideTextBoxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        textBox.SetActive(false);
    }
}
