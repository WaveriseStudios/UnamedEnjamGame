using System;
using System.Linq;
using UnityEngine;

public class EndTile : MonoBehaviour
{
    public GameObject[] particles;
    public int calledTimes = 0;

    public GameObject door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        door.SetActive(false);
        calledTimes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPadPressed()
    {
        particles[calledTimes].SetActive(true);
        calledTimes++;

        if (calledTimes == particles.Length)
        {
            door.SetActive(true);
        }
    }
}
