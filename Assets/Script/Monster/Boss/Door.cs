using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Boss_Swan boss;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if(!(boss.isBossStart))
            {
                boss.BossStart();
            }
        }
    }
}
