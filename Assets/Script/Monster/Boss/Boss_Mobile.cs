using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Mobile : MonoBehaviour
{
    public int damage;
    public float initialVelocity; // �ʱ� �ӵ�
    private Rigidbody rb;

    // #. ���� ���� ���� ����
    public Transform[] spawnPosition;
    public GameObject[] randomMonster;
    public bool isSShot;


    public GameObject destroyEffectPrefab; // �ı� �� ����� ��ƼŬ ȿ�� ������
    public float destroyEffectDuration = 2.0f; // �ı� ȿ�� ���� �ð�


    public GameObject powerBox;
    public Transform spawnPoint;

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

            // �÷��̾� ��ũ��Ʈ�� �����ϸ� �÷��̾��� ü���� ���ҽ�Ŵ
            if (playerScript != null)
            {
                playerScript.OnDamage(damage);
            }
        }

        if (other.CompareTag("Floor"))
        {
            SpawnRandomMonsters();

            // �ı� ȿ�� ���
            PlayDestroyEffect();

            Destroy(gameObject);
        }
    }

    // ���� ���� ���� �Լ�
    private void SpawnRandomMonsters()
    {
        if(!isSShot)
        {
            for (int i = 0; i < spawnPosition.Length; i++)
            {
                // ������ ���� ������ ����
                int randomMonsterIndex = Random.Range(0, randomMonster.Length);
                GameObject selectedMonster = randomMonster[randomMonsterIndex];

                // ���͸� ���� ��ġ�� ����
                Instantiate(selectedMonster, spawnPosition[i].position, Quaternion.identity);
            }

            int randomBoxChance = Random.Range(0, 100); // ���� ������ Ȯ���� ���� ���� ����
            if (randomBoxChance < 25)
            {
                Instantiate(powerBox, spawnPoint.position, Quaternion.identity);
            }

        }
        isSShot = true;
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
