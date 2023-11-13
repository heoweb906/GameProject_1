using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuShuGif : MonoBehaviour
{
    public Image[] images;  // UI Image 배열
    public Image[] images_Sleep;  // UI Image 배열
    public float displaySpeed = 1.0f;  // 이미지 표시 간격 (초 단위)

    private int currentIndex = 0;

    private void Start()
    {
        InvokeRepeating("DisplayNextImage", 0f, displaySpeed);
    }

    private void DisplayNextImage()
    {
        // 현재 이미지를 활성화하고 나머지는 비활성화
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(i == currentIndex);
        }

        currentIndex += 1;
        if(currentIndex == 2)
        {
            currentIndex = 0;
        }
    }

    public void StopDisplay()
    {
        CancelInvoke("DisplayNextImage");
    }



    public IEnumerator DisplayImages()
    {
        foreach (Image image in images_Sleep)
        {
            // 현재 이미지를 활성화하고 나머지는 비활성화
            foreach (Image otherImage in images_Sleep)
            {
                otherImage.gameObject.SetActive(otherImage == image);
            }

            // 정해진 시간 동안 대기
            yield return new WaitForSeconds(0.3f);
        }
    }
}
