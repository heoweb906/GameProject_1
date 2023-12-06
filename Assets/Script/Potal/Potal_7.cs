using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Potal_7 : MonoBehaviour
{
    [Header("스테이지 매니저")]
    public StageManager stageManager;
    public GameManager gameManager;

    [Header("관련 변수들")]
    public Animator animDoor;
    public BoxCollider colliderBox;
    private bool b_isDoorOpen;

    public GameObject PowerBox;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        gameManager.bpmCount = 0;
        PlayerPrefs.SetInt("Stage_1_MaxFloor", 7);
    }

    private void Update()
    {
        if (stageManager.MonsterCount == 0 && !b_isDoorOpen)
        {

            PowerBox.SetActive(true);
            // #. 다음 스테이지 함수
            MoveWall();
        }
    }

    public void MoveWall()
    {
        Debug.Log("다음 스테이지로 가는 문이 열렸습니다.");
        b_isDoorOpen = true;

        colliderBox.enabled = true;


        animDoor.SetTrigger("doDoor");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // "Player" 태그를 가진 오브젝트와 충돌했을 때
        {
            Debug.Log("다음 스테이지로 이동합니다.");
            SceneManager.LoadScene("Boss"); // Play 씬 1로 
        }
    }
}