using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea_Bear : MonoBehaviour
{
    public int damage = 1;
    public float pushBackForce = 10f; 
    public float upwardForce = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;
            Player playerScript = playerObject.GetComponent<Player>();

            if (playerScript != null)
            {
                playerScript.isStun = true;
                playerScript.OnDamage(damage);
                playerScript.PushBackAndLift(pushBackForce, upwardForce, transform.forward); // 여기서 transform.forward는 곰 몬스터 쪽을 향한 방향입니다.
            }
        }
    }
}