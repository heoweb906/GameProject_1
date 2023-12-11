using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Meteor : MonoBehaviour
{

    public float initialVelocity; // 초기 속도
    private Rigidbody rb;


    public GameObject destroyEffectPrefab; // 파괴 시 재생할 파티클 효과 프리팹
    public float destroyEffectDuration = 2.0f; // 파괴 효과 지속 시간


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

            Debug.Log("플레이어와 닿았습니다.");

            // 플레이어 스크립트가 존재하면 플레이어의 체력을 감소시킴
            if (playerScript != null)
            {
                if (!(playerScript.isSafeZone))
                {
                    playerScript.PlayerDie();
                    PlayDestroyEffect();
                    Destroy(gameObject);
                }
            }
        }

        if (other.CompareTag("Floor") || other.CompareTag("EEE"))
        {
            PlayDestroyEffect();

            Destroy(gameObject);
        }


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
