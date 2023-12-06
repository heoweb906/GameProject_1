using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Door : MonoBehaviour
{
    public Boss_Swan boss;


    // # 문 이동 관련 변수
    public GameObject wall;
    public float duration = 1f;

    // # 커튼 연출 관련 변수
    public GameObject Curtain;
    public Animator anim_Curtain;


    public Slider bossHPBar;
    public float moveDistance = 60f;
    public float moveDuration = 2f;


    private void Start()
    {
        anim_Curtain.SetTrigger("doStart");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!(boss.isBossStart))
            {
                boss.BossStart();
                MoveWall();
                anim_Curtain.SetTrigger("doEnd");
                MoveSliderDown();
                Destroy(Curtain, 2f);
            }
        }
    }

    public void MoveWall()
    {
        wall.SetActive(true);
    }

    public void MoveSliderDown()
    {
        float screenHeight = Screen.height;
        float targetY = bossHPBar.transform.position.y - (screenHeight * 0.15f);

        // 슬라이더를 아래로 이동
        bossHPBar.transform.DOMoveY(targetY, moveDuration);
    }

    public void MoveSliderUp()
    {
        float screenHeight = Screen.height;
        float targetY = bossHPBar.transform.position.y + (screenHeight * 0.2f);

        // 슬라이더를 위로 이동
        bossHPBar.transform.DOMoveY(targetY, moveDuration);
    }
}