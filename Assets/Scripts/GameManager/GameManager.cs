using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioClip[] echos;

    public void AddDeathVocal(AudioClip vocal)
    {
        echos.Append(vocal);
        return;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
