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
    // �װų� ����ȭ������ ���� �� �ǹ�Ÿ���� �������� �����ؾ� ��

    public GameManager gameManager;
    public PlayerInformation playerInformation;
    public UI_InGame ingame_UI;

    [Header("�÷��̾� ����")]
    public int hp = 4; // �÷��̾� ü��
    public int weaponNumber; // 1 = ����, 2 = ���, 3 = �Ķ�
    public float moveSpeed = 20;  // �÷��̾� ���ǵ�
    public float jumpForce = 210; // ���� ��
    public float rollSpeed = 30f; // ������ �ӵ�
    public float rollDuration = 0.3f; // ������ ���� �ð�
    public int attackDamage = 10;    // ���� ������

    public bool isDie;
    [Space(10f)]


    [Header("������Ʈ")]
    public GameObject weapon_main;
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public GameObject weapon4_Gatling;
    [Space(10f)]


    [Header("���� ���� ����")]
    public float hAxis; // �̵� �� ���� ���� ���� ����
    public float vAxis; // �̵� �� ���� ���� ���� ����
    public float turnSpeed; // ȸ�� �ӵ�
    public float attackRange = 50000.0f; // ���� ����

    private float timeSinceLastAttack = 0f;  // FeverAttack
    public float attackInterval = 0.3f; // everAttack Ray�� �߻��ϴ� �ֱ�
    [Space(10f)]


    [Header("����")]
    public AudioSource soundGun; // �ѼҸ�
    public AudioSource soundGun_Fail; // ���� ���߱� ���� ����
    public AudioSource soundReload; // ���� �Ҹ� (źâ ����)
    public AudioSource soundNotBullet; // �Ѿ� ���� �� ���� �Ҹ�
    public AudioSource soundRoll; // ������ �Ҹ�
    public AudioSource soundColorChange; // ������ �Ҹ�
    public AudioSource soundMiniGun; // �̴ϰ� �Ҹ�
    public AudioSource clickButtonSound; // ��ư Ŭ�� �Ҹ�
   

    [Header("����Ʈ ����")]
    public ParticleSystem effect_Pistol;
    public ParticleSystem effect_Minigun;


    [Header("FEEL ����")]
    public MMF_Player mmfPlayer_OnDamage;
    public MMF_Player mmfPlayer_OnDie;


    // #. �÷��̾� Ű �Է� 
    private bool jDown; // ���� Ű 
    private bool wDown; // ��ũ���� Ű 
    private bool shiftDown; // ������ Ű 
    private bool rDown; // ������ Ű 
    private int key_weapon = 1; 

    // #. �÷��̾� ����
    private bool isJumping; // ���� ������ ���θ� ��Ÿ���� ����
    private bool isRolling; // ������ �ִ� ������ ���θ� ��Ÿ���� ����
    private bool isDamaging; // ������ �޾� ������ ����
    public bool isSafeZone; // ���� ��� ���Ǳ⿡�� ������ ��ҿ� �ִ���
    public bool isStun; // ���� ��� ���Ǳ⿡�� ������ ��ҿ� �ִ���

    public Rigidbody rigid;
    public CameraControl mainCamera;



    // #. �ִϸ����� ����
    public Animator anim_Gun;
    public Animator anim_MiniGun;
    Vector3 moveVec; // �÷��̾��� �̵� ��

    // #. ���̾� ���� ����
    private float layerChangeDuration = 1.5f;     // ���̾� ���� ���� �ð� (��)
    private bool isChangingLayer = false;
    private int playerLayer; // Player�� �ʱ� ���̾�

    // #. ���� ����
    private Vector3 rayDirection; // ����ĳ��Ʈ�� �߻�� ������ �����ϴ� ����


    // #. ���� �� ���� ����
    public RadialBlurImageEffect blueEffect;
    public int rushBlur = 1; // �ʱ� �� ����
    public float rayDistance = 1f; // ������ ����


    // #. ���� �˻�
    public bool isRamp;


    // #. ġƮ ���� ���
    private bool b_invin = false;







    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager�� ã�Ƽ� �Ҵ�
        playerInformation = FindObjectOfType<PlayerInformation>();
        rigid = GetComponent<Rigidbody>();

        CamLock();
        playerLayer = gameObject.layer;
    }

    private void Start()
    {
        hp = PlayerPrefs.GetInt("PlayerHp");
        gameManager.canRoll = true;


        CamLock(); // ���� ���� �� ī�޶� ��
        SetPlayerSound(); // ȯ�� ������ �µ��� ȿ���� ���� ����; 
                          // �޴�ȭ���� UI ��Ʈ�ѷ��� �÷��̾��� �Լ��� �����ų �� ���� ������ 1�� ��������ִ� ����


        if (!(gameManager.isFever))
        {
            WeaponChange_SceneChange(playerInformation.WeponColor); // ���� ��ȯ�� �� ��� �ִ� ������ ������ �̾������� ���� ��ü �Լ� 1ȸ ����
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
                Debug.Log("�̴ϰ� �ִϸ��̼��� �����մϴ�");
                Invoke("StopMiniGunAnim",0.1f);
            }
                
        }
        



        // ġƮŰ ~~~~~~~~~~~~~~~
        // ġƮŰ ~~~~~~~~~~~~~~~
        if (Input.GetKeyDown(KeyCode.Y))  // �÷��̾� ü���� 4��
        {
            b_invin = !b_invin;
        }
        if (Input.GetKeyDown(KeyCode.U))  // �÷��̾� ü���� 4��
        {
            HpUp(4);
        }
        if (Input.GetKeyDown(KeyCode.I))  // ���� Ŭ���� ������ �ʱ�ȭ
        {
            PlayerPrefs.SetInt("Stage_1_MaxFloor", 1);
        }
        // ġƮŰ ~~~~~~~~~~~~~~~
        // ġƮŰ ~~~~~~~~~~~~~~~
    }

    private void FixedUpdate()
    {
        // �÷��̾�� ���� �߷��� ��
        rigid.AddForce(Vector3.down * 60f, ForceMode.Acceleration);

    }







    private void GetInput()  // �Է��� �޴� �Լ�
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





        // esc Ű�� ������ �۵�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ingame_UI.OnOffSettingPanel();
        }
    }


    void Move() // �̵��� �����ϴ� �Լ�
    {
        // �÷��̾��� �ٶ󺸴� ������ �̿��Ͽ� �̵� ���͸� ���
        Vector3 moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // Rigidbody�� �ӵ� ����
        Vector3 newVelocity = transform.forward * moveVec.z * moveSpeed + transform.right * moveVec.x * moveSpeed;
        newVelocity.y = rigid.velocity.y; // ���� ���� �ӵ� ����

        if (!isDie)
        {
            rigid.velocity = newVelocity;
        }



        // ���� üũ
        if (jDown && !isJumping)
        {
            Jump();
        }

        // ������ üũ
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

        // ������ ���� �̵� �ӵ��� ������Ű��, ������ ���� �̵� �������� ����
        soundRoll.Play();

        float originalMoveSpeed = moveSpeed;
        moveSpeed = rollSpeed;
        Vector3 rollDirection = moveVec;

        // ���� �ð� ���� ������
        yield return new WaitForSeconds(rollDuration);

        // ������ ���� �� ���� �̵� �ӵ��� �������� ����
        moveSpeed = originalMoveSpeed;
        isRolling = false;
    }
    private IEnumerator EffectBlur()
    {
        float raiseDuration = 0.2f; // �ö󰡴µ� �ɸ��� �ð�
        int endValue = 10; // ��ǥ ��

        float timer = 0.0f; // ��� �ð� �ʱ�ȭ
        int startValue = blueEffect.samples; // ���� �� ����

        // �ö󰡴� �κ�
        while (timer < raiseDuration)
        {
            timer += Time.deltaTime; // ��� �ð� ����
            float progress = timer / raiseDuration; // ����� ���

            // ������ ����Ͽ� ���� ������ ����
            blueEffect.samples = (int)Mathf.Lerp(startValue, endValue, progress);

            yield return null; // �� �����Ӿ� ���
        }

        blueEffect.samples = endValue; // ��ǥ ������ ����


        float lowerDuration = 0.2f; // �������µ� �ɸ��� �ð�
        startValue = blueEffect.samples; // ���� �� �ٽ� ����

        timer = 0.0f; // ��� �ð� �ʱ�ȭ

        // �������� �κ�
        while (timer < lowerDuration)
        {
            timer += Time.deltaTime; // ��� �ð� ����
            float progress = timer / lowerDuration; // ����� ���

            // ������ ����Ͽ� ���� ������ ����
            blueEffect.samples = (int)Mathf.Lerp(startValue, 1, progress);

            yield return null; // �� �����Ӿ� ���
        }

        blueEffect.samples = 1; // ���� ������ ����
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

                Debug.DrawRay(ray.origin, ray.direction * attackRange, hasHit ? Color.red : Color.green, 0.1f); // ���� �ð�ȭ
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
                            // �浹 �������� ������ ���� ���͸� ���
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
                    // �θ� �Ǵ� �ڽ� ������Ʈ�� �ݶ��̴��� �˻�
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
        else if(Input.GetButtonDown("Fire1") && !(gameManager.rhythmCorrect))   // ���� Ʋ�� Ÿ�ֿ̹� �����ϸ�
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

    private void FeverAttack() // �ǹ� ���� �Լ�
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit, attackRange);

        Debug.DrawRay(ray.origin, ray.direction * attackRange, hasHit ? Color.red : Color.green, 0.1f); // ���� �ð�ȭ
        gameManager.bulletCount--;

        if (hasHit && (hit.collider.CompareTag("Monster") || hit.collider.CompareTag("Boss")))
        {
            Monster monster = hit.collider.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(attackDamage, hit.point);
            }

            // �θ� �Ǵ� �ڽ� ������Ʈ�� �ݶ��̴��� �˻�
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
    

    // #. ������ ���
    private void Reload()
    {
        if (rDown && gameManager.rhythmCorrect && gameManager.b_ActionCnt)
        {
            gameManager.b_ActionCnt = false;

            gameManager.bulletCount = 15;

            // #. �ִϸ��̼� �ð� ������ �� ƽ ���� �Է��� �� ����
            StartCoroutine(SetBoolAfterDelay(0.7f));

            // #. �ִϸ��̼� �ð� ������ �� ƽ ���� �Է��� �� ����
            StartCoroutine(SetBoolAfterDelay(0.7f));
            anim_Gun.SetTrigger("Reload");


            Invoke("SoundReload", 0.1f);
        }
    }
    private void SoundReload()
    {
        soundReload.Play();
    }




    public void OnDamage(int dmg) // �������� �޾��� ���� �Լ�, ���͵��� ����� �� �ֵ��� public���� ��
    {
        if(!b_invin) // ������ �ƴҶ���
        {
            if (hp >= 1 && !isDie && !isDamaging)
            {
                isDamaging = true;
                hp -= dmg;
                gameManager.ActivateHpImage(hp);


                mmfPlayer_OnDamage?.PlayFeedbacks();

                // ���̾� ������ ���� ������ ���� ���� ����
                if (!isChangingLayer)
                {
                    StartCoroutine(ChangeLayerTemporarily());
                }

                // �÷��̾��� hp ������ ����
                PlayerPrefs.SetInt("PlayerHp", hp);
            }

            // #. ���� �Լ� ���
            if (hp <= 0 && !isDie)
            {
                isDie = true;
                if (ingame_UI.isSettingPanel)
                {
                    ingame_UI.OnOffSettingPanel_PlayerDie();  // ������ Ȱ��ȭ�Ǿ� �ִ� ����â�� ����
                }

                PlayerDie();
            }
        }
        
    }

    private IEnumerator ChangeLayerTemporarily()
    {
        // ���̾ "PlayOnDamage"�� ����
        gameObject.layer = LayerMask.NameToLayer("PlayerOnDamage");
        isChangingLayer = true;

        // ���� �ð� �� Player ���̾�� �ٽ� ����
        yield return new WaitForSeconds(layerChangeDuration);

        // Player ���̾�� �ٽ� ����
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
        // �ڷ� �и��� �Ÿ��� �ð��� ����
        float pushBackDistance = 2.0f; // �ڷ� �и� �Ÿ�
        float pushBackTime = 0.8f; // �ڷ� �и��� �ð� (��)
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
            

            // #. �ִϸ��̼� �ð� ������ �� ƽ ���� �Է��� �� ����
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

    public void WeaponChange_SceneChange(int number)    // ���� ��ȯ�� �� ��� �ִ� ������ ������ �̾������� �ϱ� ���� �Լ�
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


    // #. �÷��̾� ���� ����
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
    public void CamLock() // ���콺 Ŀ���� ����� �Լ�
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void CamUnlock()
    {
        Cursor.visible = true; // ���콺 Ŀ���� ���̰� �մϴ�.
        Cursor.lockState = CursorLockMode.None; // ���콺 Ŀ���� ������ �����մϴ�.
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
