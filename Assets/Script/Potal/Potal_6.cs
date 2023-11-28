using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Potal_6 : MonoBehaviour
{
    [Header("�������� �Ŵ���")]
    public StageManager stageManager;
    public GameManager gameManager;

    [Header("���� ������")]
    public Animator animDoor;
    public BoxCollider colliderBox;
    private bool b_isDoorOpen;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        gameManager.bpmCount = 0;
        PlayerPrefs.SetInt("Stage_1_MaxFloor", 5);
    }

    private void Update()
    {
        if (stageManager.MonsterCount == 0 && !b_isDoorOpen)
        {
            // #. ���� �������� �Լ�
            MoveWall();
        }
    }

    public void MoveWall()
    {
        Debug.Log("���� ���������� ���� ���� ���Ƚ��ϴ�.");
        b_isDoorOpen = true;

        colliderBox.enabled = true;


        //animDoor.SetTrigger("doDoor");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // "Player" �±׸� ���� ������Ʈ�� �浹���� ��
        {
            Debug.Log("���� ���������� �̵��մϴ�.");
            SceneManager.LoadScene("Loading_Lobby"); // Play �� 1�� 
        }
    }
}