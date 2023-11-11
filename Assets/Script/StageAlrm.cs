using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageAlrm : MonoBehaviour
{
    public Image[] stageAlrms;
    public float fadeTime;

   


    void Start()
    {
        Invoke("FadeStart", fadeTime);
        
    }

    void FadeStart()
    {
        StartCoroutine(FadeOutAndDeactivate(3f));
    }

    IEnumerator FadeOutAndDeactivate(float fadeDuration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            foreach (Image stageAlrm in stageAlrms)
            {
                Color newColor = stageAlrm.color;
                newColor.a = alpha;
                stageAlrm.color = newColor;
            }

            yield return null;

            elapsedTime += Time.deltaTime;
        }

        // ��� Image ������Ʈ�� ������ ��Ȱ��ȭ
        foreach (Image stageAlrm in stageAlrms)
        {
            stageAlrm.gameObject.SetActive(false);
        }

        // �ڱ� �ڽ� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}
