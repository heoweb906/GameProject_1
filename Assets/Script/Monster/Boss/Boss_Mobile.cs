using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Mobile : MonoBehaviour
{
    public int damage;
    public float initialVelocity; // 초기 속도
    private Rigidbody rb;

    // #. 몬스터 생성 관련 변수
    public Transform[] spawnPosition;
    public GameObject[] randomMonster;
    public bool isSShot;


    public GameObject destroyEffectPrefab; // 파괴 시 재생할 파티클 효과 프리팹
    public float destroyEffectDuration = 2.0f; // 파괴 효과 지속 시간


    public GameObject powerBox;
    public Transform spawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.down * initialVelocity; // 아래 방향으로 초기 속도 설정
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
                playerScript.OnDamage(damage);
            }
        }

        if (other.CompareTag("Floor"))
        {
            SpawnRandomMonsters();

            // 파괴 효과 재생
            PlayDestroyEffect();

            Destroy(gameObject);
        }
    }

    // 랜덤 몬스터 생성 함수
    private void SpawnRandomMonsters()
    {
        if(!isSShot)
        {
            for (int i = 0; i < spawnPosition.Length; i++)
            {
                // 랜덤한 몬스터 프리팹 선택
                int randomMonsterIndex = Random.Range(0, randomMonster.Length);
                GameObject selectedMonster = randomMonster[randomMonsterIndex];

                // 몬스터를 생성 위치에 생성
                Instantiate(selectedMonster, spawnPosition[i].position, Quaternion.identity);
            }

            int randomBoxChance = Random.Range(0, 100); // 상자 생성의 확률을 위한 랜덤 숫자
            if (randomBoxChance < 25)
            {
                Instantiate(powerBox, spawnPoint.position, Quaternion.identity);
            }

        }
        isSShot = true;
    }

    private void PlayDestroyEffect()
    {
        Debug.Log("파티클을 생성합니다.");

        if (destroyEffectPrefab != null)
        {
            // 파티클 효과 재생
            GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, destroyEffectDuration); // 효과 파괴
        }
    }
}
