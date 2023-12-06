using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Toruso : MonoBehaviour
{
    [Header("���� ����")]
    public int monsterColor;


    [Header("���� ������")]

    // #. �ϴ� ������ �����Ű�� ���� �־����
    public new Renderer renderer; // ������ ������Ʈ
    public Color originalColor; // ���� ����
    public Color newColor; // �����Ϸ��� ����
    private Material monsterMaterial; // ������ ��Ƽ���� �߰�


    private void Awake()
    {

        if (renderer != null)
        {
            monsterMaterial = renderer.material;
            originalColor = monsterMaterial.GetColor("_BaseColor"); // ���� ���� ����
        }
        else
        {
            Debug.LogError("Renderer ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    public void TakeDamage()
    {
        Debug.Log("�¾ҽ��ϴ�.");

        if (renderer != null)
        {
            monsterMaterial = renderer.material;
            originalColor = monsterMaterial.GetColor("_BaseColor"); // ���� ���� ����
        }
        else
        {
            Debug.LogError("Renderer ������Ʈ�� ã�� �� �����ϴ�.");
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