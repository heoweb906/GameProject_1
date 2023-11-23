using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    public Elevator_1 elevator_1;
    public bool UporDown; // true면 위로 올라가기, fasle면 내려가기
    public int numColor;

    public void PushButton()
    {
        if(UporDown)
        {
            elevator_1.FloorUp();
        }
        else
        {
            elevator_1.FloorDown();
        }

    }
}
