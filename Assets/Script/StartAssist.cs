using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAssist : MonoBehaviour
{
    
    void Start()
    {
        if (!PlayerPrefs.HasKey("FirstRun"))
        {
            PlayerPrefs.SetInt("FirstRun", 1); // 실행 여부를 나타내는 키를 설정
        }
        else
        {

        }

        if(PlayerPrefs.GetInt("FirstRun") == 1)
        {
            PlayerPrefs.SetInt("Stage_1_MaxFloor", 1);




            PlayerPrefs.SetInt("FirstRun", 2);
        }

    }


}
