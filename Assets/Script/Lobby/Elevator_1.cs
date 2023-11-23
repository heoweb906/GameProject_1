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
        if(nowFloor < maxFloor) // �̰� maxFloor�� �ٲ�� ��
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
        // ���� ���� �ִ��� Ȯ��
        if (index >= 0 && index < floorArrow.Length)
        {
            for (int i = 0; i < floorArrow.Length; i++)
            {
                if (i == index)
                {
                    floorArrow[i].SetActive(true); // ������ �ε����� ������Ʈ Ȱ��ȭ
                }
                else
                {
                    floorArrow[i].SetActive(false); // �ٸ� ��� ������Ʈ ��Ȱ��ȭ
                }
            }
        }
        else
        {
            Debug.LogWarning("�ε����� ������ ������ϴ�.");
        }
    }


    public void ActivateObjectAtIndex_2(int index)
    {
        // ���� ���� �ִ��� Ȯ��
        if (index >= 0 && index < floorArrow.Length)
        {
            for (int i = 0; i <= index; i++)
            {
                clearStage[i].SetActive(true); // �ش� �ε��������� ��� ������Ʈ Ȱ��ȭ
            }

            for (int i = index + 1; i < floorArrow.Length; i++)
            {
                clearStage[i].SetActive(false); // ������ ������Ʈ ��Ȱ��ȭ
            }
        }
        else
        {
            Debug.LogWarning("�ε����� ������ ������ϴ�.");
        }
    }
}
