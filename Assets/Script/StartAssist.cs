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


        // #. 게임을 최초로 실행할 때만 초기화 실행
        // 추후에 챕터가 추가된다면, 각 Floor를 초기화 해주는 작업을 추가
        if (PlayerPrefs.GetInt("FirstRun") == 1)
        {
            PlayerPrefs.SetInt("Stage_1_MaxFloor", 1);


            PlayerPrefs.SetInt("FirstRun", 2);
        }

    }


}
