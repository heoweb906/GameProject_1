using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingDoor : MonoBehaviour
{
    public Image FadeOut;
    public float fadeDuration = 4.0f;
    private bool isFading = false;



    public GameManager gameManager;
    public PlayerInformation playerInformation;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerInformation = FindObjectOfType<PlayerInformation>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;
            Player playerScript = playerObject.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.isStun = true;
                playerScript.moveSpeed = 0;
                playerScript.hAxis = 0;
                playerScript.vAxis = 0;
            }

            // 서서히 알파 값을 올리는 코루틴 실행
            StartCoroutine(FadeOutImage());
        }
    }


    IEnumerator FadeOutImage()
    {
        isFading = true;
        float timer = 0.0f;
        Color startColor = FadeOut.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            FadeOut.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            yield return null;
        }

        // 알파값이 1이 되면 실행될 함수 호출
        OnFadeOutComplete();
    }

    void OnFadeOutComplete()
    {
        UnlockCursor(); // 커서 락 해제
        playerInformation.IsMenu = true;
        playerInformation.IsGame = false;
        gameManager.soundManager.Stop();
        gameManager.iconOn = false;
 
        // 실행될 코드 작성
        SceneManager.LoadScene("Ending");
    }




    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


}
