using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [Header("ΩÃ±€≈ÊµÈ")]
    public GameManager gameManager;
    public PlayerInformation playerInformation;


    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerInformation = FindObjectOfType<PlayerInformation>();



        playerInformation.MouseSpeed = 0.5f;
        playerInformation.VolumeBGM = 0.3f;
        playerInformation.VolumeEffect = 0.3f;

        PlayerPrefs.SetInt("PlayerHp", 4);
    }

}
