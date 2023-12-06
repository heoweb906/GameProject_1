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

    public GameObject arrowObject;

    public void Start()
    {
        maxFloor = PlayerPrefs.GetInt("Stage_1_MaxFloor");
        ActivateObjectAtIndex(0);
        ActivateObjectAtIndex_2(maxFloor - 1);
        SetAttow(nowFloor - 1);
    }

    public void FloorUp()
    {
        if(nowFloor < maxFloor) 
        {
            nowFloor++;
            SetAttow(nowFloor - 1);
        }
    }

    public void FloorDown()
    {
        if (nowFloor > 1)
        {
            nowFloor--;
            SetAttow(nowFloor - 1);
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
                floorArrow[i].SetActive(true); // �ش� �ε��������� ��� ������Ʈ Ȱ��ȭ
            }

            for (int i = index + 1; i < floorArrow.Length; i++)
            {
                floorArrow[i].SetActive(false); // ������ ������Ʈ ��Ȱ��ȭ
            }
        }
        else
        {
            Debug.LogWarning("�ε����� ������ ������ϴ�.");
        }
    }


    void SetAttow(int floor)
    {
        if(floor == 0)
        {
            arrowObject.transform.rotation = Quaternion.Euler(0f, 0f, -75f);
        }
        else if(floor == 1)
        {
            arrowObject.transform.rotation = Quaternion.Euler(0f, 0f, -55f);
        }
        else if (floor == 2)
        {
            arrowObject.transform.rotation = Quaternion.Euler(0f, 0f, -35f);
        }
        else if (floor == 3)
        {
            arrowObject.transform.rotation = Quaternion.Euler(0f, 0f, -10f);
        }
        else if (floor == 4)
        {
            arrowObject.transform.rotation = Quaternion.Euler(0f, 0f, 10f);
        }
        else if (floor == 5)
        {
            arrowObject.transform.rotation = Quaternion.Euler(0f, 0f, 35f);
        }
        else if (floor == 6)
        {
            arrowObject.transform.rotation = Quaternion.Euler(0f, 0f, 55f);
        }
        else if (floor == 7)
        {
            arrowObject.transform.rotation = Quaternion.Euler(0f, 0f, 80f);
        }

    }
}
