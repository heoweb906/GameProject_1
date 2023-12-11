using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    [Header("엔딩 연출 관련")]
    public GameObject effectDoor;
    public GameObject endingDoor;

    private void Start()
    {
        anim_Curtain.SetTrigger("doStart");
    }

    private void Update()
    {
        if(boss.doDie)
        {
            Invoke("GoEnding",5f);
        }


        // esc 키를 누르면 작동
        if (Input.GetKeyDown(KeyCode.K))
        {
            GoEnding();
        }
    }


    private void GoEnding()
    {
        // 모든 Monster 태그를 가진 오브젝트들을 찾아서 Monster 스크립트의 함수 실행
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            if (monsterScript != null)
            {
                // Monster 스크립트의 특정 함수 실행 (예시: Die() 함수)
                monsterScript.Die();
            }
        }

        wall.SetActive(false);
        effectDoor.SetActive(true);
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

                endingDoor.SetActive(true);
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