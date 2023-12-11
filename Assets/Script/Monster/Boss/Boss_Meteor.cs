using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Meteor : MonoBehaviour
{

    public float initialVelocity; // �ʱ� �ӵ�
    private Rigidbody rb;


    public GameObject destroyEffectPrefab; // �ı� �� ����� ��ƼŬ ȿ�� ������
    public float destroyEffectDuration = 2.0f; // �ı� ȿ�� ���� �ð�


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.down * initialVelocity; // �Ʒ� �������� �ʱ� �ӵ� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;
            Player playerScript = playerObject.GetComponent<Player>();

            Debug.Log("�÷��̾�� ��ҽ��ϴ�.");

            // �÷��̾� ��ũ��Ʈ�� �����ϸ� �÷��̾��� ü���� ���ҽ�Ŵ
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
        Debug.Log("��ƼŬ�� �����մϴ�.");

        if (destroyEffectPrefab != null)
        {
            // ��ƼŬ ȿ�� ���
            GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, destroyEffectDuration); // ȿ�� �ı�
        }
    }
}
