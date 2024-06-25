using System.Collections;
using TMPro;
using UnityEngine;

public class puzzleText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject textBox; // 텍스트 창의 GameObject를 참조
    public string[] messages;
    public float typingSpeed = 0.05f;
    public Transform playerTransform; // 플레이어의 Transform을 참조
    public Transform targetPoint; // 플레이어가 멈출 지점의 Transform을 참조
    public float stopDistance = 1f; // 멈출 거리

    private int index = 0;
    private bool isTyping = false;
    private bool isPlayerStopped = false;

    private void Start()
    {
        textBox.SetActive(false); // 시작 시 텍스트 창 비활성화
    }

    private void Update()
    {
        // 플레이어가 멈춰야 할 조건 체크
        if (!isPlayerStopped && Vector3.Distance(playerTransform.position, targetPoint.position) <= stopDistance)
        {
            isPlayerStopped = true;
            textBox.SetActive(true); // 텍스트 창 활성화
            StartCoroutine(TypeSentence(messages[index])); // 첫 번째 메시지 출력 시작
        }

        // 텍스트 출력 후 스페이스바 입력 처리
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping && isPlayerStopped)
        {
            if (index < messages.Length - 1)
            {
                index++;
                StartCoroutine(TypeSentence(messages[index]));
            }
            else
            {
                textBox.SetActive(false); // 마지막 메시지가 출력된 후 텍스트 창 비활성화
                isPlayerStopped = false; // 플레이어 움직임 가능하게 설정
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
