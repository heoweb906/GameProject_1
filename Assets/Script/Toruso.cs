using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Toruso : MonoBehaviour
{
    [Header("색상 정보")]
    public int monsterColor;


    [Header("관련 변수들")]

    // #. 일단 색상을 변경시키기 위해 넣어놨음
    public new Renderer renderer; // 렌더러 컴포넌트
    public Color originalColor; // 원래 색상
    public Color newColor; // 변경하려는 색상
    private Material monsterMaterial; // 몬스터의 머티리얼 추가


    private void Awake()
    {

        if (renderer != null)
        {
            monsterMaterial = renderer.material;
            originalColor = monsterMaterial.GetColor("_BaseColor"); // 원래 색상 저장
        }
        else
        {
            Debug.LogError("Renderer 컴포넌트를 찾을 수 없습니다.");
        }
    }

    public void TakeDamage()
    {
        Debug.Log("맞았습니다.");

        if (renderer != null)
        {
            monsterMaterial = renderer.material;
            originalColor = monsterMaterial.GetColor("_BaseColor"); // 원래 색상 저장
        }
        else
        {
            Debug.LogError("Renderer 컴포넌트를 찾을 수 없습니다.");
        }
        renderer.material.color = Color.black;
        if (monsterMaterial != null)
        {
            monsterMaterial.SetColor("_BaseColor", newColor);
        }
        Invoke("ColorBack", 0.1f);
    }

    private void ColorBack()
    {
        if (monsterMaterial != null)
        {
            monsterMaterial.SetColor("_BaseColor", originalColor);
        }
    }

}