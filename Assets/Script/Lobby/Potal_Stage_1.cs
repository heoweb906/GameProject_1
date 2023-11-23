using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Potal_Stage_1 : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerInformation playerInformation;

    public Elevator_1 elevator_1;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerInformation = FindObjectOfType<PlayerInformation>();

        gameManager.b_ActionCnt = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UnlockCursor(); // 커서 락 해제
            playerInformation.IsMenu = true;
            playerInformation.IsGame = false;
            gameManager.soundManager.Stop();
            gameManager.iconOn = false;

            playerInformation.Elevator_1 = elevator_1.nowFloor;
            SceneManager.LoadScene("Loading_Stage"); // "Timing" 씬으로 전환
        }
    }

    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}