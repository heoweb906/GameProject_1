using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Net.NetworkInformation;
using Unity.VisualScripting;

public class UI_InGame : MonoBehaviour
{
    //@@@@@@@@@@@@@@@@@@@@@@@@@@
    // 해당 스테이지에 몬스터가 있는 동안에는 설정할 수 없도록 수정해야 함
    // or 설정창을 열면 판정선을 모두 지우도록 수정해야함 (피버타임처럼)
    //@@@@@@@@@@@@@@@@@@@@@@@@@@


    public GameManager gameManager;
    public PlayerInformation playerInformation;
    public new CameraControl camera;
    public Player player;
    public StageManager stageManager;

    [Header("게임 오버 패널")]
    public GameObject gameoverPanel;

    [Header("설정창 패널")]
    public bool isSettingPanel; // 세팅 패널이 켜져 있는지 아닌지
    public GameObject settingPanel;

    public float mouseFloat;
    public Slider mouseSensitivitySlider; // 슬라이더 변수 추가

    public float volumeBGM;
    public float volumeEffect;
    public Slider volumeBGMSlider;
    public Slider volumeEffectSlider;

    public GameObject textPanel; // "아직 필드에 몬스터가 있습니다."


    [Header("튜토리얼 패널")]
    public GameObject tutorialPanel;
    public bool isTutorialPanel = false;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerInformation = FindObjectOfType<PlayerInformation>();

        mouseFloat = playerInformation.MouseSpeed; 
        mouseSensitivitySlider.value = mouseFloat; 

        volumeBGM = playerInformation.VolumeBGM;
        volumeBGMSlider.value = volumeBGM;

        volumeEffect = playerInformation.VolumeEffect;
        volumeEffectSlider.value = volumeEffect;
    }




    public void OnOffSettingPanel()
    {
        player.clickButtonSound.Play();

        if (stageManager.MonsterCount > 0)
        {
            textPanel.SetActive(true);
            CamLock();
            Invoke("TextPanelOff",0.5f);
        }
        else if(stageManager.MonsterCount <= 0)
        {
            if (!(player.isDie))
            {
                if(!isTutorialPanel)
                {
                    if (!(settingPanel.activeSelf))
                    {
                        player.CamUnlock();

                        isSettingPanel = true;
                        settingPanel.gameObject.SetActive(true);
                    }
                    else if (settingPanel.activeSelf)
                    {
                        camera.SetMouseSpeed();

                        isSettingPanel = false;
                        settingPanel.gameObject.SetActive(false);

                        player.CamLock();
                    }
                }
                else
                {
                    OnOffTutorailPanel();
                }
                
            }
        }
    }
    public void TextPanelOff()
    {
        textPanel.SetActive(false);
    }

    public void OnOffSettingPanel_PlayerDie()
    {
        if (!(settingPanel.activeSelf))
        {
            player.CamUnlock();

            isSettingPanel = true;
            settingPanel.gameObject.SetActive(true);
        }
        else if (settingPanel.activeSelf)
        {
            camera.SetMouseSpeed();

            isSettingPanel = false;
            settingPanel.gameObject.SetActive(false);

            player.CamLock();
        }
    }

    public void OnOffGameoverPanel()
    {

        player.CamUnlock();
        gameoverPanel.gameObject.SetActive(true);
    }

    public void OnMouseSensitivityChanged(float value) // 마우스 감도 조절 슬라이더 함수
    {
        // 소수점 2자리까지 반올림하여 mouseFloat에 할당
        mouseFloat = Mathf.Round(value * 100) / 100;
        playerInformation.MouseSpeed = mouseFloat;
    }

    public void OnBGM_SoundSensitivityChanged(float value) // 배경음악 크기 조절 슬라이더 함수
    {
        volumeBGM = Mathf.Round(value * 100) / 100;
        playerInformation.VolumeBGM = volumeBGM;
        gameManager.SetVolume();
    }

    public void OnEffect_SoundSensitivityChanged(float value) // 효과음 크기 조절 슬라이더 함수
    {
        volumeEffect = Mathf.Round(value * 100) / 100;
        playerInformation.VolumeEffect = volumeEffect;
        gameManager.SetVolume();
        player.SetPlayerSound();
    }


    public void OnOffTutorailPanel()
    {
        player.clickButtonSound.Play();

        if (!isTutorialPanel)
        {
            isTutorialPanel = true;
            tutorialPanel.gameObject.SetActive(true);
        }
        else if (isTutorialPanel)
        {
            isTutorialPanel = false;
            tutorialPanel.gameObject.SetActive(false);
        }
    }


    public void GoMenu()
    {
        player.clickButtonSound.Play();

        UnlockCursor(); // 커서 락 해제

        PlayerPrefs.SetInt("PlayerHp", 4);

        SceneManager.LoadScene("Loading_Lobby"); // "YourSceneName"은 이동하고자 하는 씬의 이름으로 바꿔주세요.
        gameManager.soundManager.Stop();
        playerInformation.IsMenu = true;
        playerInformation.IsGame = false;
        gameManager.iconOn = false;
    }
    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void CamLock() // 마우스 커서를 숨기는 함수
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame() // 게임 종료 버튼 클릭(게임 종료)
    {
        player.clickButtonSound.Play();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


   
}
