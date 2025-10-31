using System.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform playerSpawn;

    public GameObject winMenu;

    public GameObject player;

    public bool isInGame = false;

    public string currentPlayerName = "...";

    public List<AudioClip> echos;

    public void AddDeathVocal(AudioClip vocal)
    {
        echos.Add(vocal);
        return;
    }

    public void SetName(string name)
        { currentPlayerName = name; return; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SpawnNewPlayer()
    {
        Instantiate<GameObject>(player, playerSpawn);
    }
}
