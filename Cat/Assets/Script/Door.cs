using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform targetPosition; // �̵��� ��ǥ �� ��ġ
    public Transform player; // �÷��̾�
    public Transform cameraTransform; // ī�޶�
    public Animator doorAnimator; // �� ������Ʈ�� �ִϸ�����

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ��ǥ ��ġ �������� ������ ����
            Vector3 offset;
            if (targetPosition.position.y > transform.position.y)
            {
                // �ܺ� ������ ���� ������ �̵��� ��
                offset = new Vector3(0, -2, 0);
            }
            else
            {
                // ���� ������ �ܺ� ������ �̵��� ��
                offset = new Vector3(0, 2, 0);
            }

            Vector3 newPosition = targetPosition.position + offset;

            // �÷��̾�� ī�޶��� ��ġ�� ��ǥ ��ġ�� �̵�
            player.position = newPosition;
            player.rotation = targetPosition.rotation;

            // ī�޶� ��ġ�� ������ ���� (���⼭�� z�� �̵��� ����)
            cameraTransform.position = new Vector3(newPosition.x, newPosition.y, cameraTransform.position.z);
            cameraTransform.rotation = targetPosition.rotation;

            // �� ���� �ִϸ��̼� Ʈ����
            //doorAnimator.SetTrigger("isOpen");
        }
    }
}
