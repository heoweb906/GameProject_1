using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndingCredit : MonoBehaviour
{
    public RectTransform imageTransform; // 이동시킬 이미지의 RectTransform
    public float moveSpeed = 100f; // 이동 속도

    void Update()
    {
        // 이미지를 무한히 위쪽 방향으로 이동
        imageTransform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }
}
