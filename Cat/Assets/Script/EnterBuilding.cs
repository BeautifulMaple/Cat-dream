using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBuilding : MonoBehaviour
{
    public GameObject buildingInterior; // 건물 내부 GameObject
    public GameObject exteriorCamera;
    public GameObject interiorCamera;

    // Start is called before the first frame update
    void Start()
    {
        // 건물 내부를 처음엔 비활성화
        buildingInterior.SetActive(false);
        // 외부 카메라 활성화
        exteriorCamera.SetActive(true);
        // 내부 카메라 비활성화
        interiorCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 트리거에 진입할 경우
        if(other.CompareTag("player"))
        {
            // 건물 내부 활성화
            buildingInterior.SetActive(true);
            // 외부 카메라 비활성화
            exteriorCamera.SetActive(false);
            // 내부 카메라 활성화
            interiorCamera.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 트리거에서 나갔는지 확인
        if (other.CompareTag("player"))
        {
            // 건물 내부를 처음엔 비활성화
            buildingInterior.SetActive(false);
            // 외부 카메라 활성화
            exteriorCamera.SetActive(true);
            // 내부 카메라 비활성화
            interiorCamera.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
