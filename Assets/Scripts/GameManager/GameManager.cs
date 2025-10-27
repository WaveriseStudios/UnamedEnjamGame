using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioClip[] deathVocals; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDeathVocal(AudioClip vocal)
    {
        deathVocals.Append(vocal);
        return;
    }
}
