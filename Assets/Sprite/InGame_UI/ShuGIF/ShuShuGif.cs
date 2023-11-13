using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuShuGif : MonoBehaviour
{
    public Image[] images;  // UI Image �迭
    public Image[] images_Sleep;  // UI Image �迭
    public float displaySpeed = 1.0f;  // �̹��� ǥ�� ���� (�� ����)

    private int currentIndex = 0;

    private void Start()
    {
        InvokeRepeating("DisplayNextImage", 0f, displaySpeed);
    }

    private void DisplayNextImage()
    {
        // ���� �̹����� Ȱ��ȭ�ϰ� �������� ��Ȱ��ȭ
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
            // ���� �̹����� Ȱ��ȭ�ϰ� �������� ��Ȱ��ȭ
            foreach (Image otherImage in images_Sleep)
            {
                otherImage.gameObject.SetActive(otherImage == image);
            }

            // ������ �ð� ���� ���
            yield return new WaitForSeconds(0.3f);
        }
    }
}
