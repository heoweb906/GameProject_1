using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Door : MonoBehaviour
{
    public Boss_Swan boss;


    // # �� �̵� ���� ����
    public GameObject wall;
    public float duration = 1f;

    // # Ŀư ���� ���� ����
    public GameObject Curtain;
    public Animator anim_Curtain;


    public Slider bossHPBar;
    public float moveDistance = 60f;
    public float moveDuration = 2f;


    [Header("���� ���� ����")]
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


        // esc Ű�� ������ �۵�
        if (Input.GetKeyDown(KeyCode.K))
        {
            GoEnding();
        }
    }


    private void GoEnding()
    {
        // ��� Monster �±׸� ���� ������Ʈ���� ã�Ƽ� Monster ��ũ��Ʈ�� �Լ� ����
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            if (monsterScript != null)
            {
                // Monster ��ũ��Ʈ�� Ư�� �Լ� ���� (����: Die() �Լ�)
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

        // �����̴��� �Ʒ��� �̵�
        bossHPBar.transform.DOMoveY(targetY, moveDuration);
    }

    public void MoveSliderUp()
    {
        float screenHeight = Screen.height;
        float targetY = bossHPBar.transform.position.y + (screenHeight * 0.2f);

        // �����̴��� ���� �̵�
        bossHPBar.transform.DOMoveY(targetY, moveDuration);
    }













}