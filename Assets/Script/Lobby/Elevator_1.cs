using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_1 : MonoBehaviour
{
    public int nowFloor;
    public GameObject[] floorArrow;


    public GameObject[] clearStage;
    public int maxFloor;

    public void Start()
    {
        maxFloor = PlayerPrefs.GetInt("Stage_1_MaxFloor");
        ActivateObjectAtIndex_2(maxFloor - 1);
    }

    public void FloorUp()
    {
        if(nowFloor < maxFloor) // 이거 maxFloor로 바꿔야 함
        {
            nowFloor++;
            ActivateObjectAtIndex(nowFloor - 1);
        }
    }

    public void FloorDown()
    {
        if (nowFloor > 1)
        {
            nowFloor--;
            ActivateObjectAtIndex(nowFloor - 1);
        }
    }

    public void ActivateObjectAtIndex(int index)
    {
        // 범위 내에 있는지 확인
        if (index >= 0 && index < floorArrow.Length)
        {
            for (int i = 0; i < floorArrow.Length; i++)
            {
                if (i == index)
                {
                    floorArrow[i].SetActive(true); // 선택한 인덱스의 오브젝트 활성화
                }
                else
                {
                    floorArrow[i].SetActive(false); // 다른 모든 오브젝트 비활성화
                }
            }
        }
        else
        {
            Debug.LogWarning("인덱스가 범위를 벗어났습니다.");
        }
    }


    public void ActivateObjectAtIndex_2(int index)
    {
        // 범위 내에 있는지 확인
        if (index >= 0 && index < floorArrow.Length)
        {
            for (int i = 0; i <= index; i++)
            {
                clearStage[i].SetActive(true); // 해당 인덱스까지의 모든 오브젝트 활성화
            }

            for (int i = index + 1; i < floorArrow.Length; i++)
            {
                clearStage[i].SetActive(false); // 나머지 오브젝트 비활성화
            }
        }
        else
        {
            Debug.LogWarning("인덱스가 범위를 벗어났습니다.");
        }
    }
}
