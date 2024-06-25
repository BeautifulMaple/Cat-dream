using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform targetPosition; // 이동할 목표 문 위치
    public Transform player; // 플레이어
    public Transform cameraTransform; // 카메라
    public Animator doorAnimator; // 문 오브젝트의 애니메이터

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 목표 위치 앞쪽으로 오프셋 설정
            Vector3 offset;
            if (targetPosition.position.y > transform.position.y)
            {
                // 외부 문에서 내부 문으로 이동할 때
                offset = new Vector3(0, -2, 0);
            }
            else
            {
                // 내부 문에서 외부 문으로 이동할 때
                offset = new Vector3(0, 2, 0);
            }

            Vector3 newPosition = targetPosition.position + offset;

            // 플레이어와 카메라의 위치를 목표 위치로 이동
            player.position = newPosition;
            player.rotation = targetPosition.rotation;

            // 카메라 위치를 적절히 조정 (여기서는 z축 이동은 없음)
            cameraTransform.position = new Vector3(newPosition.x, newPosition.y, cameraTransform.position.z);
            cameraTransform.rotation = targetPosition.rotation;

            // 문 열기 애니메이션 트리거
            //doorAnimator.SetTrigger("isOpen");
        }
    }
}
