using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using MoreMountains.Feedbacks;
using System.Security.Cryptography;

public class Player : MonoBehaviour
{
    // 죽거나 메인화면으로 나갈 때 피버타임이 끝나도록 수정해야 함

    public GameManager gameManager;
    public PlayerInformation playerInformation;
    public UI_InGame ingame_UI;

    [Header("플레이어 정보")]
    public int hp = 4; // 플레이어 체력
    public int weaponNumber; // 1 = 빨강, 2 = 노랑, 3 = 파랑
    public float moveSpeed = 20;  // 플레이어 스피드
    public float jumpForce = 210; // 점프 힘
    public float rollSpeed = 30f; // 구르기 속도
    public float rollDuration = 0.3f; // 구르기 지속 시간
    public int attackDamage = 10;    // 공격 데미지

    public bool isDie;
    [Space(10f)]


    [Header("오브젝트")]
    public GameObject weapon_main;
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public GameObject weapon4_Gatling;
    [Space(10f)]


    [Header("조작 관련 변수")]
    public float hAxis; // 이동 시 수평 값을 위한 변수
    public float vAxis; // 이동 시 수직 값을 위한 변수
    public float turnSpeed; // 회전 속도
    public float attackRange = 50000.0f; // 공격 범위

    private float timeSinceLastAttack = 0f;  // FeverAttack
    public float attackInterval = 0.3f; // everAttack Ray를 발사하는 주기
    [Space(10f)]


    [Header("사운드")]
    public AudioSource soundGun; // 총소리
    public AudioSource soundGun_Fail; // 리듬 맞추기 실패 공격
    public AudioSource soundReload; // 장전 소리 (탄창 장착)
    public AudioSource soundNotBullet; // 총알 없을 때 나는 소리
    public AudioSource soundRoll; // 구르기 소리
    public AudioSource soundColorChange; // 구르기 소리
    public AudioSource soundMiniGun; // 미니건 소리
    public AudioSource clickButtonSound; // 버튼 클릭 소리
   

    [Header("이펙트 관련")]
    public ParticleSystem effect_Pistol;
    public ParticleSystem effect_Minigun;


    [Header("FEEL 관련")]
    public MMF_Player mmfPlayer_OnDamage;
    public MMF_Player mmfPlayer_OnDie;


    // #. 플레이어 키 입력 
    private bool jDown; // 점프 키 
    private bool wDown; // 웅크리기 키 
    private bool shiftDown; // 구르기 키 
    private bool rDown; // 재장전 키 
    private int key_weapon = 1; 

    // #. 플레이어 상태
    private bool isJumping; // 점프 중인지 여부를 나타내는 변수
    private bool isRolling; // 구르고 있는 중인지 여부를 나타내는 변수
    private bool isDamaging; // 공격을 받아 무적인 상태
    public bool isSafeZone; // 보스 즉사 장판기에서 안전한 장소에 있는지
    public bool isStun; // 보스 즉사 장판기에서 안전한 장소에 있는지

    public Rigidbody rigid;
    public CameraControl mainCamera;



    // #. 애니메이터 관련
    public Animator anim_Gun;
    public Animator anim_MiniGun;
    Vector3 moveVec; // 플레이어의 이동 값

    // #. 레이어 변경 관련
    private float layerChangeDuration = 1.5f;     // 레이어 변경 지속 시간 (초)
    private bool isChangingLayer = false;
    private int playerLayer; // Player의 초기 레이어

    // #. 레이 관련
    private Vector3 rayDirection; // 레이캐스트가 발사된 방향을 저장하는 변수


    // #. 돌진 블러 관련 변수
    public RadialBlurImageEffect blueEffect;
    public int rushBlur = 1; // 초기 값 설정
    public float rayDistance = 1f; // 레이의 길이


    // #. 경사로 검사
    public bool isRamp;


    // #. 치트 관련 기능
    private bool b_invin = false;







    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager를 찾아서 할당
        playerInformation = FindObjectOfType<PlayerInformation>();
        rigid = GetComponent<Rigidbody>();

        CamLock();
        playerLayer = gameObject.layer;
    }

    private void Start()
    {
        hp = PlayerPrefs.GetInt("PlayerHp");
        gameManager.canRoll = true;


        CamLock(); // 게임 시작 시 카메라 락
        SetPlayerSound(); // 환경 설정에 맞도록 효과음 사운드 조절; 
                          // 메뉴화면의 UI 컨트롤러는 플레이어의 함수를 실행시킬 수 없기 때문에 1번 실행시켜주는 거임


        if (!(gameManager.isFever))
        {
            WeaponChange_SceneChange(playerInformation.WeponColor); // 씬이 전환될 때 들고 있던 무기의 정보가 이어지도록 무기 교체 함수 1회 실행
        }
        else if(gameManager.isFever)
        {
            FeverOn();
            weaponNumber = playerInformation.WeponColor;
        }
    }


    private void Update()
    {
        GetInput();

        if (!isStun)
        {
            Move();
        }
       
        
        if(!(gameManager.isFever) && !(ingame_UI.isSettingPanel))
        {
            Attack();
        }
        else if(gameManager.isFever && !(ingame_UI.isSettingPanel))
        {
            if (Input.GetButton("Fire1") && !isDie)
            {
                timeSinceLastAttack += Time.deltaTime;

                if (timeSinceLastAttack >= attackInterval)
                {
                    FeverAttack();
                    timeSinceLastAttack = 0f; 
                }
            }


            if(Input.GetButtonUp("Fire1"))
            {
                Debug.Log("미니건 애니메이션을 종료합니다");
                Invoke("StopMiniGunAnim",0.1f);
            }
                
        }
        



        // 치트키 ~~~~~~~~~~~~~~~
        // 치트키 ~~~~~~~~~~~~~~~
        if (Input.GetKeyDown(KeyCode.Y))  // 플레이어 체력을 4로
        {
            b_invin = !b_invin;
        }
        if (Input.GetKeyDown(KeyCode.U))  // 플레이어 체력을 4로
        {
            HpUp(4);
        }
        if (Input.GetKeyDown(KeyCode.I))  // 현재 클리어 정보를 초기화
        {
            PlayerPrefs.SetInt("Stage_1_MaxFloor", 1);
        }
        // 치트키 ~~~~~~~~~~~~~~~
        // 치트키 ~~~~~~~~~~~~~~~
    }

    private void FixedUpdate()
    {
        // 플레이어에게 강한 중력을 줌
        rigid.AddForce(Vector3.down * 60f, ForceMode.Acceleration);

    }







    private void GetInput()  // 입력을 받는 함수
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");
        wDown = Input.GetButton("Bowingdown");
        shiftDown = Input.GetButtonDown("Roll");
        rDown = Input.GetButtonDown("Reload");

        
        if(rDown)
        {
            Reload();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(weaponNumber == 1)
            {
                key_weapon = 2;
                WeaponChange(key_weapon);
            }
            else if(weaponNumber == 2)
            {
                key_weapon = 3;
                WeaponChange(key_weapon);
            }
            else if (weaponNumber == 3)
            {
                key_weapon = 1;
                WeaponChange(key_weapon);
            }
        }

        if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
        {
            if (weaponNumber == 1)
            {
                key_weapon = 3;
                WeaponChange(key_weapon);
            }
            else if (weaponNumber == 2)
            {
                key_weapon = 1;
                WeaponChange(key_weapon);
            }
            else if (weaponNumber == 3)
            {
                key_weapon = 2;
                WeaponChange(key_weapon);
            }
        }





        // esc 키를 누르면 작동
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ingame_UI.OnOffSettingPanel();
        }
    }


    void Move() // 이동을 관리하는 함수
    {
        // 플레이어의 바라보는 방향을 이용하여 이동 벡터를 계산
        Vector3 moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // Rigidbody에 속도 적용
        Vector3 newVelocity = transform.forward * moveVec.z * moveSpeed + transform.right * moveVec.x * moveSpeed;
        newVelocity.y = rigid.velocity.y; // 현재 수직 속도 유지

        if (!isDie)
        {
            rigid.velocity = newVelocity;
        }



        // 점프 체크
        if (jDown && !isJumping)
        {
            Jump();
        }

        // 구르기 체크
        if (shiftDown && !isRolling && gameManager.rhythmCorrect && gameManager.canRoll && gameManager.b_ActionCnt)
        {
            isRolling = true;
            StartCoroutine(PerformRoll());
        }

        if(shiftDown && !isRolling && gameManager.isFever && gameManager.canRoll && gameManager.b_ActionCnt)
        {
            isRolling = true;
            StartCoroutine(PerformRoll());
        }
    }

    private IEnumerator PerformRoll()
    {
        gameManager.b_ActionCnt = false;
        StartCoroutine(SetBoolAfterDelay(0.2f));
        StartCoroutine(EffectBlur());

        gameManager.canRoll = false;
        gameManager.rollSkill_Image.fillAmount = 0f;
        gameManager.RollCoolTime();

        // 구르기 동안 이동 속도를 증가시키고, 방향은 현재 이동 방향으로 설정
        soundRoll.Play();

        float originalMoveSpeed = moveSpeed;
        moveSpeed = rollSpeed;
        Vector3 rollDirection = moveVec;

        // 일정 시간 동안 구르기
        yield return new WaitForSeconds(rollDuration);

        // 구르기 종료 후 원래 이동 속도와 방향으로 복구
        moveSpeed = originalMoveSpeed;
        isRolling = false;
    }
    private IEnumerator EffectBlur()
    {
        float raiseDuration = 0.2f; // 올라가는데 걸리는 시간
        int endValue = 10; // 목표 값

        float timer = 0.0f; // 경과 시간 초기화
        int startValue = blueEffect.samples; // 현재 값 저장

        // 올라가는 부분
        while (timer < raiseDuration)
        {
            timer += Time.deltaTime; // 경과 시간 증가
            float progress = timer / raiseDuration; // 진행률 계산

            // 보간을 사용하여 값을 서서히 변경
            blueEffect.samples = (int)Mathf.Lerp(startValue, endValue, progress);

            yield return null; // 한 프레임씩 대기
        }

        blueEffect.samples = endValue; // 목표 값으로 설정


        float lowerDuration = 0.2f; // 내려가는데 걸리는 시간
        startValue = blueEffect.samples; // 현재 값 다시 저장

        timer = 0.0f; // 경과 시간 초기화

        // 내려오는 부분
        while (timer < lowerDuration)
        {
            timer += Time.deltaTime; // 경과 시간 증가
            float progress = timer / lowerDuration; // 진행률 계산

            // 보간을 사용하여 값을 서서히 변경
            blueEffect.samples = (int)Mathf.Lerp(startValue, 1, progress);

            yield return null; // 한 프레임씩 대기
        }

        blueEffect.samples = 1; // 최종 값으로 설정
    }


    private void Jump()
    {
        isJumping = true;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }



    public void PushBackAndLift(float pushBackForce, float upwardForce, Vector3 direction)
    {
        rigid.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
        rigid.AddForce(direction.normalized * pushBackForce, ForceMode.Impulse);
        Invoke("RelaxStun",1f);
    }
    public void RelaxStun()
    {
        isStun = false;
    }



    private IEnumerator SetBoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.b_ActionCnt = true;
    }

    private void Attack()
    {
       
        if (Input.GetButtonDown("Fire1") && gameManager.rhythmCorrect && !isDie && gameManager.b_ActionCnt && !(ingame_UI.isSettingPanel))
        {            

            if (gameManager.bulletCount > 0)
            {
                Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward );
                RaycastHit hit;
                bool hasHit = Physics.Raycast(ray, out hit, attackRange);

                Debug.DrawRay(ray.origin, ray.direction * attackRange, hasHit ? Color.red : Color.green, 0.1f); // 레이 시각화
                gameManager.bulletCount--;

                effect_Pistol.Play();

                gameManager.b_ActionCnt = false;
                StartCoroutine(SetBoolAfterDelay(0.2f));

                if (hasHit && (hit.collider.CompareTag("Monster") || hit.collider.CompareTag("Boss")))
                {
                    rayDirection = ray.direction.normalized;

                    Monster monster = hit.collider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        if (monster.monsterColor == weaponNumber)
                        {
                            monster.TakeDamage(attackDamage, hit.point);
                           
                        }
                    }

                    Toruso toruso = hit.collider.GetComponent<Toruso>();
                    if (toruso != null)
                    {
                        if (toruso.monsterColor == weaponNumber)
                        {
                            toruso.TakeDamage();
                        }
                    }

                    Boss_Bullet_Color bullet = hit.collider.GetComponent<Boss_Bullet_Color>();
                    if (bullet != null)
                    {
                        if (bullet.numColor == weaponNumber)
                        {
                            // 충돌 지점에서 나가는 방향 벡터를 사용
                            bullet.SetNewDirection(rayDirection);
                            bullet.b_PlayerHit = true;
                        }
                    }


                    ElevatorButton button = hit.collider.GetComponent<ElevatorButton>();
                    if (button != null)
                    {
                        if (button.numColor == weaponNumber)
                        {
                            button.PushButton();
                        }
                    }


                }

                if (hasHit &&  hit.collider.CompareTag("Boss"))
                {
                    // 부모 또는 자식 오브젝트의 콜라이더를 검사
                    Transform hitTransform = hit.collider.transform;
                    Boss_Swan boss = hitTransform.GetComponent<Boss_Swan>();

                    while (boss == null && hitTransform.parent != null)
                    {
                        hitTransform = hitTransform.parent;
                        boss = hitTransform.GetComponent<Boss_Swan>();
                    }

                    if (boss != null)
                    {
                        if (boss.monsterColor == weaponNumber)
                        {
                            boss.TakeDamage(attackDamage, hit.point);
                            
                        }
                    }
                }


                anim_Gun.SetTrigger("Fire");
                soundGun.Play();
            }
            else
            {
                soundNotBullet.Play();
            }
        }
        else if(Input.GetButtonDown("Fire1") && !(gameManager.rhythmCorrect))   // 만약 틀린 타이밍에 공격하면
        {
            gameManager.ComboBarDown();

            if(gameManager.bulletCount > 0)
            {
                soundGun_Fail.Play();
            }else
            {
                soundNotBullet.Play();
            }
           
        }
    }

    private void FeverAttack() // 피버 공격 함수
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit, attackRange);

        Debug.DrawRay(ray.origin, ray.direction * attackRange, hasHit ? Color.red : Color.green, 0.1f); // 레이 시각화
        gameManager.bulletCount--;

        if (hasHit && (hit.collider.CompareTag("Monster") || hit.collider.CompareTag("Boss")))
        {
            Monster monster = hit.collider.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(attackDamage, hit.point);
            }

            // 부모 또는 자식 오브젝트의 콜라이더를 검사
            Transform hitTransform = hit.collider.transform;
            Boss_Swan boss = hitTransform.GetComponent<Boss_Swan>();

            while (boss == null && hitTransform.parent != null)
            {
                hitTransform = hitTransform.parent;
                boss = hitTransform.GetComponent<Boss_Swan>();
            }

            if (boss != null)
            {
                boss.TakeDamage(attackDamage, hit.point);
                gameManager.ComboBarBounceUp();


            }
        }

        anim_MiniGun.SetTrigger("doShot");
        effect_Minigun.Play();
        soundMiniGun.Play();
    }
    void StopMiniGunAnim()
    {
        for (int i = 0; i < 10; i++)
        {
            anim_MiniGun.SetTrigger("doShotEnd");
        }
        
    }
    

    // #. 재장전 기능
    private void Reload()
    {
        if (rDown && gameManager.rhythmCorrect && gameManager.b_ActionCnt)
        {
            gameManager.b_ActionCnt = false;

            gameManager.bulletCount = 15;

            // #. 애니메이션 시간 때문에 한 틱 동안 입력을 안 받음
            StartCoroutine(SetBoolAfterDelay(0.7f));

            // #. 애니메이션 시간 때문에 한 틱 동안 입력을 안 받음
            StartCoroutine(SetBoolAfterDelay(0.7f));
            anim_Gun.SetTrigger("Reload");


            Invoke("SoundReload", 0.1f);
        }
    }
    private void SoundReload()
    {
        soundReload.Play();
    }




    public void OnDamage(int dmg) // 데미지를 받았을 때의 함수, 몬스터들이 사용할 수 있도록 public으로 함
    {
        if(!b_invin) // 무적이 아닐때만
        {
            if (hp >= 1 && !isDie && !isDamaging)
            {
                isDamaging = true;
                hp -= dmg;
                gameManager.ActivateHpImage(hp);


                mmfPlayer_OnDamage?.PlayFeedbacks();

                // 레이어 변경이 진행 중이지 않을 때만 실행
                if (!isChangingLayer)
                {
                    StartCoroutine(ChangeLayerTemporarily());
                }

                // 플레이어의 hp 정보를 저장
                PlayerPrefs.SetInt("PlayerHp", hp);
            }

            // #. 죽음 함수 기능
            if (hp <= 0 && !isDie)
            {
                isDie = true;
                if (ingame_UI.isSettingPanel)
                {
                    ingame_UI.OnOffSettingPanel_PlayerDie();  // 죽으면 활성화되어 있는 설정창을 꺼줌
                }

                PlayerDie();
            }
        }
        
    }

    private IEnumerator ChangeLayerTemporarily()
    {
        // 레이어를 "PlayOnDamage"로 변경
        gameObject.layer = LayerMask.NameToLayer("PlayerOnDamage");
        isChangingLayer = true;

        // 일정 시간 후 Player 레이어로 다시 변경
        yield return new WaitForSeconds(layerChangeDuration);

        // Player 레이어로 다시 변경
        isDamaging = false;
        gameObject.layer = playerLayer;
        isChangingLayer = false;
    }

    public void PlayerDie()
    {
        StartCoroutine(DoDie());
    }
    public IEnumerator DoDie()
    {
        mmfPlayer_OnDie?.PlayFeedbacks();
        // 뒤로 밀리는 거리와 시간을 설정
        float pushBackDistance = 2.0f; // 뒤로 밀릴 거리
        float pushBackTime = 0.8f; // 뒤로 밀리는 시간 (초)
        Vector3 currentPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < pushBackTime)
        {
            Vector3 targetPosition = currentPosition - transform.forward * pushBackDistance;
            transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / pushBackTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        yield return new WaitForSeconds(1.9f);

        ingame_UI.OnOffGameoverPanel();
    }



    

    public void FeverOn()
    {
        weapon_main.SetActive(false);
        weapon1.SetActive(false);
        weapon2.SetActive(false);
        weapon3.SetActive(false);
        weapon4_Gatling.SetActive(true);

        attackDamage = 9;
    }

    public void FeverOff()
    {
        weapon_main.SetActive(true);
        weapon4_Gatling.SetActive(false);
        WeaponChange_SceneChange(weaponNumber);
        gameManager.bulletCount = 10;
        attackDamage = 10;
    }

    public void WeaponChange(int number) 
    {
        if (gameManager.rhythmCorrect && gameManager.b_ActionCnt && !(ingame_UI.isSettingPanel))
        {
            if (number == 1)
            {
                weaponNumber = 1;
                playerInformation.WeponColor = 1;
            }
            if (number == 2)
            {
                weaponNumber = 2;
                playerInformation.WeponColor = 2;
            }
            if (number == 3)
            {
                weaponNumber = 3;
                playerInformation.WeponColor = 3;
            }


            Invoke("WeaponChangeSound", 0.1f);
            Invoke("WeaponChangeAssist",0.3f);
            

            gameManager.b_ActionCnt = false;
            gameManager.ActivateImage(number);
            

            // #. 애니메이션 시간 때문에 한 틱 동안 입력을 안 받음
            StartCoroutine(SetBoolAfterDelay(0.7f));
            anim_Gun.SetTrigger("Color");

        }
    }
    public void WeaponChangeAssist()
    {
        if(weaponNumber == 1)
        {
            weapon1.SetActive(true);
            weapon2.SetActive(false);
            weapon3.SetActive(false);
        }
        if (weaponNumber == 2)
        {
            weapon1.SetActive(false);
            weapon2.SetActive(true);
            weapon3.SetActive(false);
        }
        if (weaponNumber == 3)
        {
            weapon1.SetActive(false);
            weapon2.SetActive(false);
            weapon3.SetActive(true);
        }
    }
    public void WeaponChangeSound()
    {
        soundColorChange.Play();
    }

    public void WeaponChange_SceneChange(int number)    // 씬이 전환될 때 들고 있던 무기의 정보가 이어지도록 하기 위한 함수
    {
        if (number == 1)
        {
            weaponNumber = 1;
            playerInformation.WeponColor = 1;
            weapon1.SetActive(true);
            weapon2.SetActive(false);
            weapon3.SetActive(false);
        }
        if (number == 2)
        {
            weaponNumber = 2;
            playerInformation.WeponColor = 2;
            weapon1.SetActive(false);
            weapon2.SetActive(true);
            weapon3.SetActive(false);
        }
        if (number == 3)
        {
            weaponNumber = 3;
            playerInformation.WeponColor = 3;
            weapon1.SetActive(false);
            weapon2.SetActive(false);
            weapon3.SetActive(true);
        }

        gameManager.ActivateImage(number);
    }


    // #. 플레이어 사운드 세팅
    public void SetPlayerSound()
    {
        soundGun.volume = playerInformation.VolumeEffect;
        soundGun_Fail.volume = playerInformation.VolumeEffect;
        soundReload.volume = playerInformation.VolumeEffect;
        soundNotBullet.volume = playerInformation.VolumeEffect;
        soundRoll.volume = playerInformation.VolumeEffect;
        soundColorChange.volume = playerInformation.VolumeEffect;
        soundMiniGun.volume = playerInformation.VolumeEffect;
    }


    #region camLock
    public void CamLock() // 마우스 커서를 숨기는 함수
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void CamUnlock()
    {
        Cursor.visible = true; // 마우스 커서를 보이게 합니다.
        Cursor.lockState = CursorLockMode.None; // 마우스 커서의 제한을 해제합니다.
    }

    public void HpUp(int score)
    {
        hp += score + 1;
        if(hp > 4)
        {
            hp = 4;
        }
        PlayerPrefs.SetInt("PlayerHp", hp);
        gameManager.ActivateHpImage(hp);
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
        }
    }
    #endregion
}
