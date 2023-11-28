using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Long_Random : Monster
{
    [Header("관련 오브젝트 / 변수")]
    public bool isAttack;
    public GameObject bulletPrefab; // 총알 프리팹을 연결해야 합니다.

    public GameObject bulletFake;
    public Transform positon_Bullet;
    public float bulletSpeed;

    public Animator anim;
    private Transform player;
    private Rigidbody rigid;






    public Material[] randomColorMaterail;
    public GameObject[] randombulletFakes;
    public GameObject[] randombulletPrefabs;



    void Start()
    {
        int randomIndex = Random.Range(0, randomColorMaterail.Length);
        monsterColor = randomIndex + 1;
        ApplyRandomMaterial(randomIndex);

        bulletFake = randombulletFakes[randomIndex];
        bulletPrefab = randombulletPrefabs[randomIndex];


        player = GameObject.FindGameObjectWithTag("Player").transform;

        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (currentHealth <= 0 && !doDie)
        {
            bulletFake.SetActive(false);

            Invoke("StartLowering",0.7f);


            anim.SetTrigger("doDie");
            doDie = true;
        }

        if (!isAttack && !doDie && gameManager.bpmCount % 5 == 0 && gameManager.bpmCount != 0)  // 5번째 bpm 마다 한번씩 공격
        {
            isAttack = true;
            Attack();
        }

        if (!doDie)
        {
            LookAtPlayer(); 
        }
    }

    
     private void Attack()
    {
        anim.SetTrigger("doAttack");
        GrowAndShrink();
    }


    // #.플레이어 방향으로 회전시키는 함수
    public void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0f; // y축 회전을 고려하지 않음
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
        }
    }



    // #. 총알 발사 함수_(신버전)

    public void GrowAndShrink()
    {
        float growDuration = 2.5f;
        float maxSize = 0.1f;

        bulletFake.SetActive(true);
        bulletFake.transform.localScale = Vector3.zero;

        // 만약 현재 체력이 이미 0이라면 즉시 함수 종료
        if (currentHealth <= 0)
        {
            bulletFake.SetActive(false);
            return;
        }

        bulletFake.transform.DOScale(maxSize, growDuration)
            .OnComplete(() =>
            {
                bulletFake.SetActive(false);
                if (!doDie)
                {
                    ShotBullet(); // 고양이가 죽은 상태이면 공격하면 안됨
                }

            });
    }

    public void ShotBullet()
    {
        // 플레이어 위치 값을 가져옴
        Vector3 playerPosition = player.position;
        Vector3 offset = new Vector3(0f, -1.2f, 0f); // 아래로 1 단위만큼 내리는 오프셋 // 총알이 목표로 하는 위치를 낮춤
        Vector3 direction = (playerPosition + offset) - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject bullet = Instantiate(bulletPrefab, positon_Bullet.position, rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // 총알에 힘을 가해 발사 (원하는 힘과 방향으로 수정해야 함)
        bulletRb.velocity = direction.normalized * bulletSpeed;

        // 총알을 발사한 후 몇 초 후에 자동으로 삭제 (원하는 시간으로 수정 가능)
        float bulletDestroyDelay = 5f; // 몇 초 후에 삭제할지 설정
        Destroy(bullet, bulletDestroyDelay);


        Invoke("CanAttack", 1.5f);
    }


    private void CanAttack()
    {
        isAttack = false;
    }



    public void StartLowering()
    {
        StartCoroutine(LowerObjectCoroutine(10f));
    }

    private IEnumerator LowerObjectCoroutine(float loweringSpeed)
    {
        float groundCheckDistance = 0.1f; // 바닥과의 거리 확인
        Vector3 lowerDirection = Vector3.down;

        while (true)
        {
            // 현재 위치에서 아래쪽으로 Ray를 쏘아 바닥과의 거리를 확인
            Ray ray = new Ray(transform.position, lowerDirection);
            RaycastHit hit;

            // 레이캐스트를 통해 바닥과의 거리를 확인
            if (Physics.Raycast(ray, out hit, groundCheckDistance))
            {
                // 만약 바닥과의 거리가 groundCheckDistance 이하라면 이동을 멈춤
                break;
            }

            transform.position += lowerDirection * loweringSpeed * Time.deltaTime;
            yield return null;
        }
    }





    void ApplyRandomMaterial(int index)
    {
        if (index >= 0 && index < randomColorMaterail.Length)
        {
            // 오브젝트의 Renderer 컴포넌트가 있다고 가정
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.material = randomColorMaterail[index];
            }
        }
        else
        {
            Debug.LogWarning("유효하지 않은 인덱스입니다.");
        }
    }
}