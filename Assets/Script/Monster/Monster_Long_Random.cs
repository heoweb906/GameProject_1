using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Long_Random : Monster
{
    [Header("���� ������Ʈ / ����")]
    public bool isAttack;
    public GameObject bulletPrefab; // �Ѿ� �������� �����ؾ� �մϴ�.

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

        if (!isAttack && !doDie && gameManager.bpmCount % 5 == 0 && gameManager.bpmCount != 0)  // 5��° bpm ���� �ѹ��� ����
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


    // #.�÷��̾� �������� ȸ����Ű�� �Լ�
    public void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0f; // y�� ȸ���� ������� ����
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
        }
    }



    // #. �Ѿ� �߻� �Լ�_(�Ź���)

    public void GrowAndShrink()
    {
        float growDuration = 2.5f;
        float maxSize = 0.1f;

        bulletFake.SetActive(true);
        bulletFake.transform.localScale = Vector3.zero;

        // ���� ���� ü���� �̹� 0�̶�� ��� �Լ� ����
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
                    ShotBullet(); // ����̰� ���� �����̸� �����ϸ� �ȵ�
                }

            });
    }

    public void ShotBullet()
    {
        // �÷��̾� ��ġ ���� ������
        Vector3 playerPosition = player.position;
        Vector3 offset = new Vector3(0f, -1.2f, 0f); // �Ʒ��� 1 ������ŭ ������ ������ // �Ѿ��� ��ǥ�� �ϴ� ��ġ�� ����
        Vector3 direction = (playerPosition + offset) - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject bullet = Instantiate(bulletPrefab, positon_Bullet.position, rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // �Ѿ˿� ���� ���� �߻� (���ϴ� ���� �������� �����ؾ� ��)
        bulletRb.velocity = direction.normalized * bulletSpeed;

        // �Ѿ��� �߻��� �� �� �� �Ŀ� �ڵ����� ���� (���ϴ� �ð����� ���� ����)
        float bulletDestroyDelay = 5f; // �� �� �Ŀ� �������� ����
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
        float groundCheckDistance = 0.1f; // �ٴڰ��� �Ÿ� Ȯ��
        Vector3 lowerDirection = Vector3.down;

        while (true)
        {
            // ���� ��ġ���� �Ʒ������� Ray�� ��� �ٴڰ��� �Ÿ��� Ȯ��
            Ray ray = new Ray(transform.position, lowerDirection);
            RaycastHit hit;

            // ����ĳ��Ʈ�� ���� �ٴڰ��� �Ÿ��� Ȯ��
            if (Physics.Raycast(ray, out hit, groundCheckDistance))
            {
                // ���� �ٴڰ��� �Ÿ��� groundCheckDistance ���϶�� �̵��� ����
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
            // ������Ʈ�� Renderer ������Ʈ�� �ִٰ� ����
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.material = randomColorMaterail[index];
            }
        }
        else
        {
            Debug.LogWarning("��ȿ���� ���� �ε����Դϴ�.");
        }
    }
}