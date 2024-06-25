using UnityEngine;
using TMPro;

public class BoxInteraction : MonoBehaviour
{
    public TextMeshProUGUI messageText; // 메시지를 표시할 TextMeshPro 텍스트
    public AudioClip rattleSound; // "덜컹덜컹" 소리
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = rattleSound;

        // messageText가 null인지 확인
        if (messageText == null)
        {
            Debug.LogError("Message Text가 할당되지 않았습니다.");
        }
        else
        {
            messageText.gameObject.SetActive(false); // 처음에는 비활성화
        }

        // rattleSound가 null인지 확인
        if (rattleSound == null)
        {
            Debug.LogError("Rattle Sound가 할당되지 않았습니다.");
        }

        // audioSource가 null인지 확인
        if (audioSource == null)
        {
            Debug.LogError("AudioSource가 할당되지 않았습니다.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowMessage();
            PlayRattleSound();
        }
    }

    void ShowMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = "상자가 열리지 않는다";
            Invoke("HideMessage", 2f); // 2초 후에 메시지 비활성화
        }
    }

    void PlayRattleSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }
}
