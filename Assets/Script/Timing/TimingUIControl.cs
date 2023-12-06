using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;
using static Unity.Burst.Intrinsics.X86;


public class TimingUIControl : MonoBehaviour
{
    [Header("판정선 관련")]
    public GameManager gameManager;
    public PlayerInformation playerInformation;


    public ShuShuGif gifImage;
    public GameObject fade;
    public Image fadeImage;
    bool timingfOff = false;


    public Canvas canvasTiming;
    public TMP_Text jugdeNumber;
    public bool iconOn;
    public Image centerIcon;
    public Image timingIcon;
    public Image timingIcon_Sand;
    public RectTransform position_timingIcon;
    public float moveDistance = 1000f; // 이동 거리
    public float iconDestroydeay = 4f; // 파괴 시간
    public float iconSpeed = 1f; 
    public float iconFadeDuration = 1f; // 페이드 인(서서히 나타나기) 시간


    private float timeSinceLastCreation = 0f;
    private float creationInterval = 1f; // 1초

    private bool hasExecuted = false;

    private float jugdeNum;
    [Space(30f)]


    [Header("마우스 감도 / 사운드 설정")]
    public float mouseFloat;
    public Slider mouseSensitivitySlider; 
    public float volumeBGM;
    public float volumeEffect;
    public Slider volumeBGMSlider;
    public Slider volumeEffectSlider;

    public AudioSource clickButtonSound_;

    public void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerInformation = FindObjectOfType<PlayerInformation>();
        playerInformation.IsMenu = true;
    }
    public void Start()
    {
        // #. 현재 수치 정보를 슬라이더에 가져옴
        mouseFloat = playerInformation.MouseSpeed; 
        mouseSensitivitySlider.value = mouseFloat; 
        volumeBGM = playerInformation.VolumeBGM;
        volumeBGMSlider.value = volumeBGM;
        volumeEffect = playerInformation.VolumeEffect;
        volumeEffectSlider.value = volumeEffect;

        // #. 가운데 아이콘 이동
        Vector3 centerIconPosition__ = centerIcon.rectTransform.anchoredPosition;
        centerIconPosition__.x = playerInformation.Jugde * 5f; // 원하는 이동 거리
        centerIcon.rectTransform.anchoredPosition = centerIconPosition__;

        TimingStart();
    }

    public void TimingStart()
    {
        Invoke("SetStartTimiig", playerInformation.Jugde * 0.01f + 1.5f);  // 이걸로 판정을 맞출 거임
                                                                         // 1.5초는 최초 딜레이
    }
    private void SetStartTimiig()
    {
        iconOn = true;
    }


    private void Update()
    {
        ShowJugdeNumber();
    }
    private void FixedUpdate()
    {
        if (iconOn)
        {
            timeSinceLastCreation += Time.fixedUnscaledDeltaTime;

            if (timeSinceLastCreation >= creationInterval)
            {
                CreateTimingIcon();
                timeSinceLastCreation = 0.0f; // 초기화해서 다음 호출까지 기다립니다.
            }
        }
    }

    private void CreateTimingIcon()
    {
        float startTime = Time.time;

        Image timingIcon_ = Instantiate(timingIcon, position_timingIcon.position, Quaternion.identity);
        timingIcon_.transform.SetParent(position_timingIcon.transform);

        timingIcon_.transform.localScale = Vector3.one;
        timingIcon_.rectTransform.anchoredPosition = Vector2.zero;
        Tweener timingIcon_main = timingIcon_.rectTransform.DOAnchorPosX(moveDistance, iconSpeed).SetEase(Ease.Linear);

        timingIcon_main.OnUpdate(() =>
        {
            // centerIcon과 timingIcon_의 중점을 계산합니다.
            Vector3 centerPosition = centerIcon.rectTransform.position;
            Vector3 timingIconPosition = timingIcon_.rectTransform.position;

            // 두 아이콘이 일정 거리 안에 들어왔을 때 동작을 실행하도록 설정
            float distanceThreshold = 10f; // 아이콘 간의 거리 임계값
            if (!hasExecuted && Vector3.Distance(centerPosition, timingIconPosition) <= distanceThreshold)
            {
                // centerIcon과 timingIcon_이 만났을 때 실행할 코드를 여기에 작성
                RhythmAnimationCompleted(centerIcon);
                hasExecuted = true; // 한 번만 실행되도록 플래그를 설정
            }
        });
        StartCoroutine(DestroyAfterDelay(timingIcon_.gameObject, iconDestroydeay, startTime));
    }

    public void RhythmAnimationCompleted(Image rhythmImage)
    {
        if (!timingfOff)
        {
            Image timingIconSand_ = Instantiate(timingIcon_Sand, rhythmImage.rectTransform.position, Quaternion.identity);
            timingIconSand_.transform.SetParent(canvasTiming.transform);

            timingIconSand_.DOFade(0f, 0.5f).OnComplete(() => DestroyTimingIcon(timingIconSand_)); 

            Debug.Log("이미지가 겹쳤습니다");
        }
    }

    private void DestroyTimingIcon(Image timingIcon)
    {
        Destroy(timingIcon.gameObject);
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay, float startTime)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null)
        {
            hasExecuted = false;

            Destroy(obj);
        }
    }
    public void jugdeMinus()
    {
        clickButtonSound_.Play();

        if (playerInformation.Jugde > -50)
        {
            playerInformation.Jugde -= 1;

            // #. 가운데 아이콘 이동
            Vector3 centerIconPosition = centerIcon.rectTransform.anchoredPosition;
            centerIconPosition.x -= 5f; // 원하는 이동 거리
            centerIcon.rectTransform.anchoredPosition = centerIconPosition;
        }
    }
    public void jugdePlus()
    {
        clickButtonSound_.Play();

        if (playerInformation.Jugde < 50)
        {
            playerInformation.Jugde += 1;

            // #. 가운데 아이콘 이동
            Vector3 centerIconPosition = centerIcon.rectTransform.anchoredPosition;
            centerIconPosition.x += 5f; // 원하는 이동 거리
            centerIcon.rectTransform.anchoredPosition = centerIconPosition;
        }
    }

    private void ShowJugdeNumber()
    {
        if(playerInformation.Jugde < 0)
        {
            jugdeNum = playerInformation.Jugde * (0.1f);
            jugdeNumber.text = jugdeNum.ToString();
        }
        else if(playerInformation.Jugde == 0)
        {
            jugdeNumber.text = "0";
        }
        else if(playerInformation.Jugde > 0)
        {
            jugdeNum = playerInformation.Jugde * (0.1f);
            jugdeNumber.text = "+" + jugdeNum.ToString();
        }
    }



    // @@@@@@@@@@@@@@@@@@@@@@@
    // #. 기타 설정 관련
    // @@@@@@@@@@@@@@@@@@@@@@@

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
    }

    public void SetOptionReset()
    {
        clickButtonSound_.Play();

        // 마우스 감도, 배경음악, 효과음 초기화
        // 슬라이더 바의 값도 같이 초기화
        OnMouseSensitivityChanged(0.5f);
        OnBGM_SoundSensitivityChanged(0.3f);
        OnEffect_SoundSensitivityChanged(0.3f);
        mouseSensitivitySlider.value = mouseFloat;
        volumeBGMSlider.value = volumeBGM;
        volumeEffectSlider.value = volumeEffect;


        // 판정도 0으로 수정, 판정선도 0위치로 초기화
        playerInformation.Jugde = 0;
        Vector3 centerIconPosition = centerIcon.rectTransform.anchoredPosition;
        centerIconPosition.x = 0f; // x축 위치를 0으로 설정
        centerIcon.rectTransform.anchoredPosition = centerIconPosition;
    }




    public void ShuShuSleep()
    {
        clickButtonSound_.Play();

        timingfOff = true;
        gifImage.StopDisplay();
        StartCoroutine(gifImage.DisplayImages());

        fade.SetActive(true);

        Invoke("startFade",1f);
    }

    private void startFade()
    {
        StartCoroutine(FadeOutAndTurnMenu());
    }

    private IEnumerator FadeOutAndTurnMenu()
    {
        // sss 이미지를 서서히 투명하게 만듦
        Color originalColor = fadeImage.color;
        float elapsedTime = 0f;
        float fadeDuration = 2.5f; // 페이드 아웃 기간

        while (elapsedTime < fadeDuration)
        {
            // 투명도 조절
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 한 프레임 대기
            yield return null;
        }


        // 아래의 코드 내용을 실행
        Invoke("SceneTurnMenu",0.2f);
    }

    public void SceneTurnMenu()
    {
        gameManager.iconOn = false;

        gameManager.soundTime.Stop();

        gameManager.bulletCount = 10; // 게임 첫 시작시의 총알
        //gameManager.soundManager.Play();
        gameManager.GameStart();
        playerInformation.IsMenu = false;
        playerInformation.IsGame = true;

        if (gameManager.isFever) // @@@@@@@@@@@@@@@ 임시로 피버를 끝나게 해놓은 거임
        {
            gameManager.SubFever();
        }

        gameManager.ActivateImage(playerInformation.WeponColor); // 무기 정보에 맞게 바늘 UI 업데이트
        gameManager.ActivateHpImage(4); // 체력바 활성화
        gameManager.SetVolume(); // 슬라이더 작업에서 하는 거지만 혹시 모르니까... 





        SceneManager.LoadScene("Loading_Lobby");

    }

}
