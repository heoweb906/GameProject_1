using UnityEngine.SceneManagement;
using UnityEngine;

public class Potal_3 : MonoBehaviour
{
    [Header("스테이지 매니저")]
    public StageManager stageManager;

    [Header("관련 변수들")]
    public Animator animDoor;
    public BoxCollider colliderBox;
    private bool b_isDoorOpen;


    private void Update()
    {
        if (stageManager.MonsterCount == 0 && !b_isDoorOpen)
        {
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
            SceneManager.LoadScene("Play1"); // Play 씬 1로 
        }
    }
}