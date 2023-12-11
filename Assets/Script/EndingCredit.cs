using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine.SceneManagement;

public class EndingCredit : MonoBehaviour
{
    public RectTransform imageTransform; // �̵���ų �̹����� RectTransform
    public float moveSpeed = 100f; // �̵� �ӵ�

    public AudioSource endingMusic;


    void Start()
    {
        Invoke("MusicPlay", 2f);
        Invoke("GameReStart", 15f);
    }

    void Update()
    {
        // �̹����� ������ ���� �������� �̵� 
        imageTransform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    void MusicPlay()
    {
        endingMusic.Play();
    }

    void GameReStart()
    {
        SceneManager.LoadScene("CutScene");
    }
}
