using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBuilding : MonoBehaviour
{
    public GameObject buildingInterior; // �ǹ� ���� GameObject
    public GameObject exteriorCamera;
    public GameObject interiorCamera;

    // Start is called before the first frame update
    void Start()
    {
        // �ǹ� ���θ� ó���� ��Ȱ��ȭ
        buildingInterior.SetActive(false);
        // �ܺ� ī�޶� Ȱ��ȭ
        exteriorCamera.SetActive(true);
        // ���� ī�޶� ��Ȱ��ȭ
        interiorCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ Ʈ���ſ� ������ ���
        if(other.CompareTag("player"))
        {
            // �ǹ� ���� Ȱ��ȭ
            buildingInterior.SetActive(true);
            // �ܺ� ī�޶� ��Ȱ��ȭ
            exteriorCamera.SetActive(false);
            // ���� ī�޶� Ȱ��ȭ
            interiorCamera.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �÷��̾ Ʈ���ſ��� �������� Ȯ��
        if (other.CompareTag("player"))
        {
            // �ǹ� ���θ� ó���� ��Ȱ��ȭ
            buildingInterior.SetActive(false);
            // �ܺ� ī�޶� Ȱ��ȭ
            exteriorCamera.SetActive(true);
            // ���� ī�޶� ��Ȱ��ȭ
            interiorCamera.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
