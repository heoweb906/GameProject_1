using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Rabbit_Random : Monster
{
    [Header("관련 오브젝트 / 변수")]
    public BoxCollider attackArea;
    public bool isChase = true;
    public bool isAttack;

    public Animator anim;
    public Transform player;
    private Rigidbody rb;
    private NavMeshAgent nav;
    public new CapsuleCollider collider;




    public Material[] randomColorMaterail;




    void Start()
    {
        int randomIndex = Random.Range(0, randomColorMaterail.Length);
        monsterColor = randomIndex + 1;
        ApplyRandomMaterial(randomIndex);



        player = GameObject.FindGameObjectWithTag("PlayerStep").transform;

        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        nav.SetDestination(player.position);

        if (currentHealth <= 0 && !doDie)
        {
            AttackAreaOff();
            collider.enabled = false;
            FixPosition(transform.position);
            doDie = true;
            isChase = false;
            nav.isStopped = true; // NavMeshAgent 멈춤
            nav.speed = 0;
            nav.angularSpeed = 0;
            anim.SetTrigger("doDie");

            nav.velocity = Vector3.zero;  // NavMeshAgent의 이동 속도 초기화
            rb.velocity = Vector3.zero;   // Rigidbody의 이동 속도 초기화
            rb.angularVelocity = Vector3.zero;  // Rigidbody의 각속도 초기화
        }

        if(!isChase)
        {
            FixPosition(transform.position);
            nav.isStopped = true; // NavMeshAgent 멈춤
            nav.speed = 0;
            nav.angularSpeed = 0;
            anim.SetBool("isWalk",false);

            nav.velocity = Vector3.zero;  // NavMeshAgent의 이동 속도 초기화
            rb.velocity = Vector3.zero;   // Rigidbody의 이동 속도 초기화
            rb.angularVelocity = Vector3.zero;  // Rigidbody의 각속도 초기화
        }
        else
        {
            nav.isStopped = false; // NavMeshAgent 멈춤
            nav.speed = 12;
            nav.angularSpeed = 720;
            anim.SetBool("isWalk", true);
        }
        Targetting();
    }

    void FixedUpdate() 
    {
        
        FreezeVelocity();
    }


    void Targetting()
    {
        float targetRadius = 3f;
        float targetRange = 2f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
                                    LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack && !doDie)
        {
            StartCoroutine(Attack());
        }
    }


    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetTrigger("doAttack");

        // 공격 시작 시 모든 속도와 회전을 0으로 설정
        nav.velocity = Vector3.zero;  // NavMeshAgent의 이동 속도 초기화
        rb.velocity = Vector3.zero;   // Rigidbody의 이동 속도 초기화
        rb.angularVelocity = Vector3.zero;  // Rigidbody의 각속도 초기화

        yield return new WaitForSeconds(0.6f);
        AttackAreaOn();
        yield return new WaitForSeconds(0.1f);
        AttackAreaOff();

        yield return new WaitForSeconds(1.3f);
        if (!doDie)
        {
            isChase = true;
        }
        isAttack = false;
    }


    public void AttackAreaOn()
    {
        attackArea.enabled = true;
    }
    public void AttackAreaOff()
    {
        attackArea.enabled = false;
    }


    void FreezeVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
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
