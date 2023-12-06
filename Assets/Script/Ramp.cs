using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    public string targetTag = "Player"; // 검사할 대상의 태그명
    public float stickyDragValue = 10.0f; // 붙어있을 때의 drag 값
    public float fallingDragValue = 0.0f; // 떨어졌을 때의 drag 값

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (targetRigidbody != null)
            {
                targetRigidbody.drag = stickyDragValue;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (targetRigidbody != null)
            {
                targetRigidbody.drag = fallingDragValue;
            }
        }
    }
}
