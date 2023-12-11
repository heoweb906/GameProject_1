using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameManager gameManager;
    public StageManager stagemanager;

    [Header("몬스터 정보")]
    public int currentHealth;
    public int monsterColor;
    public int damage = 1;
    public bool doDie;


    [Header("관련 변수들")]

    // #. 일단 색상을 변경시키기 위해 넣어놨음
    public new Renderer renderer; // 렌더러 컴포넌트
    public Color originalColor; // 원래 색상
    public Color newColor; // 변경하려는 색상
    private Material monsterMaterial; // 몬스터의 머티리얼 추가

    [Header("이펙트")]
    public ParticleSystem[] hitParticle;
    private Vector3 hitPosition;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager를 찾아서 할당
        stagemanager = FindObjectOfType<StageManager>();

        if (renderer != null)
        {
            monsterMaterial = renderer.material;
            originalColor = monsterMaterial.GetColor("_BaseColor"); // 원래 색상 저장
        }
        else
        {
            Debug.LogError("Renderer 컴포넌트를 찾을 수 없습니다.");
        }
    }

    public void TakeDamage(int damageAmount, Vector3 hitPosition)
    {
        // #. 파티클 생성 부분
        this.hitPosition = hitPosition;

        // 피격 파티클 재생
        if (hitParticle != null)
        {
            // 파티클 위치와 회전을 피격 위치와 플레이어를 향하도록 설정
            Vector3 direction = hitPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            // 파티클 생성 및 위치, 회전 설정 후 재생
            ParticleSystem newParticle = Instantiate(hitParticle[monsterColor - 1], hitPosition, rotation);
            newParticle.Play();
        }

        gameManager.ComboBarBounceUp();



        if (renderer != null)
        {
            monsterMaterial = renderer.material;
            originalColor = monsterMaterial.GetColor("_BaseColor"); // 원래 색상 저장
        }
        else
        {
            Debug.LogError("Renderer 컴포넌트를 찾을 수 없습니다.");
        }

        currentHealth -= damageAmount;
        renderer.material.color = Color.black;

        if (monsterMaterial != null)
        {
            monsterMaterial.SetColor("_BaseColor", newColor);
        }

        if (currentHealth <= 0)
        {
            gameObject.layer = LayerMask.NameToLayer("MonsterDie");
            Invoke("Die", 3.0f);
        }

        Invoke("ColorBack", 0.1f);
    }

    private void ColorBack()
    {
        if (monsterMaterial != null)
        {
            monsterMaterial.SetColor("_BaseColor", originalColor);
        }
    }

    public void Die()
    {
        stagemanager.MonsterCount--;
        FixPosition(transform.position); // 현재 위치로 고정
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;
            Player playerScript = playerObject.GetComponent<Player>();

            // 플레이어 스크립트가 존재하면 플레이어의 체력을 감소시킴
            if (playerScript != null)
            {
                if(!doDie)
                {
                    playerScript.OnDamage(damage);
                }
            }
        }
    }

    public void FixPosition(Vector3 desiredPosition)
    {
        transform.position = desiredPosition;
    }
}