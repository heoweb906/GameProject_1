using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    public string targetTag = "Player"; // �˻��� ����� �±׸�
    public float stickyDragValue = 10.0f; // �پ����� ���� drag ��
    public float fallingDragValue = 0.0f; // �������� ���� drag ��

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
