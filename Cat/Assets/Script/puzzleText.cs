using System.Collections;
using TMPro;
using UnityEngine;

public class puzzleText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject textBox; // �ؽ�Ʈ â�� GameObject�� ����
    public string[] messages;
    public float typingSpeed = 0.05f;
    public Transform playerTransform; // �÷��̾��� Transform�� ����
    public Transform targetPoint; // �÷��̾ ���� ������ Transform�� ����
    public float stopDistance = 1f; // ���� �Ÿ�

    private int index = 0;
    private bool isTyping = false;
    private bool isPlayerStopped = false;

    private void Start()
    {
        textBox.SetActive(false); // ���� �� �ؽ�Ʈ â ��Ȱ��ȭ
    }

    private void Update()
    {
        // �÷��̾ ����� �� ���� üũ
        if (!isPlayerStopped && Vector3.Distance(playerTransform.position, targetPoint.position) <= stopDistance)
        {
            isPlayerStopped = true;
            textBox.SetActive(true); // �ؽ�Ʈ â Ȱ��ȭ
            StartCoroutine(TypeSentence(messages[index])); // ù ��° �޽��� ��� ����
        }

        // �ؽ�Ʈ ��� �� �����̽��� �Է� ó��
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping && isPlayerStopped)
        {
            if (index < messages.Length - 1)
            {
                index++;
                StartCoroutine(TypeSentence(messages[index]));
            }
            else
            {
                textBox.SetActive(false); // ������ �޽����� ��µ� �� �ؽ�Ʈ â ��Ȱ��ȭ
                isPlayerStopped = false; // �÷��̾� ������ �����ϰ� ����
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
    }
}
