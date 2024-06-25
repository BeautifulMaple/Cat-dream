using UnityEngine;
using TMPro;

public class BoxInteraction : MonoBehaviour
{
    public TextMeshProUGUI messageText; // �޽����� ǥ���� TextMeshPro �ؽ�Ʈ
    public AudioClip rattleSound; // "���ȴ���" �Ҹ�
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = rattleSound;

        // messageText�� null���� Ȯ��
        if (messageText == null)
        {
            Debug.LogError("Message Text�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        else
        {
            messageText.gameObject.SetActive(false); // ó������ ��Ȱ��ȭ
        }

        // rattleSound�� null���� Ȯ��
        if (rattleSound == null)
        {
            Debug.LogError("Rattle Sound�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // audioSource�� null���� Ȯ��
        if (audioSource == null)
        {
            Debug.LogError("AudioSource�� �Ҵ���� �ʾҽ��ϴ�.");
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
            messageText.text = "���ڰ� ������ �ʴ´�";
            Invoke("HideMessage", 2f); // 2�� �Ŀ� �޽��� ��Ȱ��ȭ
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
