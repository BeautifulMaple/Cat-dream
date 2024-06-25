using TMPro;
using UnityEngine;
using System.Collections;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject normalStatue;  // 빛나지 않는 석상
    [SerializeField] private GameObject glowingStatue; // 빛나는 석상
    [SerializeField] private AudioClip glowingStatueSound; // 효과음

    private AudioSource audioSource; // 효과음을 재생할 AudioSource
    private bool gameEnded = false; // 게임 종료 여부

    public TextMeshProUGUI textComponent;
    public GameObject textBox; // 텍스트 창의 GameObject를 참조
    public string[] firstMessages;
    public string[] endGameMessages;
    public float typingSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false;
    private string[] currentMessages;

    void Start()
    {
        // AudioSource를 추가하고 설정합니다.
        audioSource = gameObject.AddComponent<AudioSource>();
        textBox.SetActive(false); // 처음에는 텍스트 박스를 비활성화
    }

    void Update()
    {
        if (!gameEnded)
        {
            CheckPlayerPosition();
            CheckEndGameCondition();
        }

        // 스페이스바를 눌렀을 때 다음 메시지로 이동
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
            StartCoroutine(HideTextBoxAfterDelay(0f)); // 모든 메시지를 다 보여줬다면 텍스트 박스 비활성화
        }
    }

    private void CheckPlayerPosition()
    {
        Vector2 playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);

        // 첫 번째 영역: x: -3 ~ -4, y: -9 ~ -11
        if (playerPosition.x >= -4f && playerPosition.x <= -3f && playerPosition.y >= -11f && playerPosition.y <= -9f)
        {
            TeleportPlayerTo(new Vector2(-28.5f, -25f), new Vector2(-29f, -25.5f));
        }
        // 두 번째 영역: x: -25.5 ~ -20.5, y: -7 ~ -8
        else if (playerPosition.x >= -25.5f && playerPosition.x <= -20.5f && playerPosition.y >= -8f && playerPosition.y <= -7f)
        {
            TeleportPlayerTo(new Vector2(-14f, 14f));
            ActivateGlowingStatue(); // 빛나는 석상을 활성화합니다.
            ShowMessages(firstMessages); // 메시지 출력
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
            // 무작위 좌표 선택
            float randomX = Random.Range(position1.x, position2.x);
            float randomY = Random.Range(position1.y, position2.y);
            playerTransform.position = new Vector3(randomX, randomY, playerTransform.position.z);
        }
    }

    private void ActivateGlowingStatue()
    {
        // 빛나지 않는 석상을 비활성화합니다.
        if (normalStatue != null)
        {
            normalStatue.SetActive(false);
        }

        // 빛나는 석상을 활성화합니다.
        if (glowingStatue != null)
        {
            glowingStatue.SetActive(true);
        }

        // 효과음을 재생합니다.
        if (glowingStatueSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(glowingStatueSound);
        }
    }

    private void CheckEndGameCondition()
    {
        Vector2 playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);

        // 모든 빛나는 석상이 활성화되어 있고 플레이어가 지정된 영역에 도달했을 때
        if (AreAllGlowingStatuesActive() &&
            playerPosition.x >= -14.5f && playerPosition.x <= 15.5f && playerPosition.y >= 17.5f && playerPosition.y <= 18.5f)
        {
            ShowMessages(endGameMessages); // 메시지 출력
            EndGame(); // 게임 종료
        }
    }

    private bool AreAllGlowingStatuesActive()
    {
        // stone statue1, stone statue2, stone statue3 태그가 있는 모든 오브젝트가 활성화 상태인지 확인합니다.
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
        // 플레이어의 y 좌표를 5만큼 증가시킵니다.
        playerTransform.position += new Vector3(0, 5f, 0);
        

        // 효과음 재생
        if (glowingStatueSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(glowingStatueSound);
        }

        gameEnded = true;

        // 게임 종료 로직 추가
        Debug.Log("게임 종료!");
        // 게임 종료를 처리하는 추가 로직을 여기에 추가할 수 있습니다.

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
