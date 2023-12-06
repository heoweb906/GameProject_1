using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndingCredit : MonoBehaviour
{
    public RectTransform imageTransform; // �̵���ų �̹����� RectTransform
    public float moveSpeed = 100f; // �̵� �ӵ�

    void Update()
    {
        // �̹����� ������ ���� �������� �̵�
        imageTransform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }
}
