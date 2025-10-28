using UnityEngine;

public class EchoListener : MonoBehaviour
{
    [Header("Components")]
    public AudioSource audioSource;
    public AudioClip audioClip;


    public void CreateEcho(AudioClip clip)
    {
        audioSource.clip = clip;
        audioClip = clip;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
