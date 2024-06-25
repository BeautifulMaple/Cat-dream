using System.Collections;
using TMPro;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject textBox; // �ؽ�Ʈ â�� GameObject�� ����
    public string[] messages;
    public float typingSpeed = 0.05f;



    private int index = 0;
    private bool isTyping = false;

    private void Start()
    {
        textBox.SetActive(true); // �ؽ�Ʈ â Ȱ��ȭ
        StartCoroutine(TypeSentence(messages[index]));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            if (index < messages.Length - 1)
            {
                index++;
                StartCoroutine(TypeSentence(messages[index]));
            }
            else
            {
                textBox.SetActive(false); // ������ �޽����� ��µ� �� �ؽ�Ʈ â ��Ȱ��ȭ
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
