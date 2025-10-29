using System.Linq;
using UnityEngine;

public class EndTile : MonoBehaviour
{
    public GameObject[] particles;
    public int calledTimes = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPadPressed()
    {
        calledTimes++;
        particles[calledTimes].SetActive(true);

        if (calledTimes == particles.Length - 1)
        {

        }
    }
}
