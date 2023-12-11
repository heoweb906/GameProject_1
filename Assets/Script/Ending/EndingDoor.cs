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

            // ������ ���� ���� �ø��� �ڷ�ƾ ����
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

        // ���İ��� 1�� �Ǹ� ����� �Լ� ȣ��
        OnFadeOutComplete();
    }

    void OnFadeOutComplete()
    {
        UnlockCursor(); // Ŀ�� �� ����
        playerInformation.IsMenu = true;
        playerInformation.IsGame = false;
        gameManager.soundManager.Stop();
        gameManager.iconOn = false;
 
        // ����� �ڵ� �ۼ�
        SceneManager.LoadScene("Ending");
    }




    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


}
