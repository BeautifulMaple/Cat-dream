using System.Collections;
using TMPro;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject textBox; // 텍스트 창의 GameObject를 참조
    public string[] messages;
    public float typingSpeed = 0.05f;



    private int index = 0;
    private bool isTyping = false;

    private void Start()
    {
        textBox.SetActive(true); // 텍스트 창 활성화
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
                textBox.SetActive(false); // 마지막 메시지가 출력된 후 텍스트 창 비활성화
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
