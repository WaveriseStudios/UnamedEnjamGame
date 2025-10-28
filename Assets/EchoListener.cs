using UnityEngine;

public class EchoListener : MonoBehaviour
{
    [Header("Components")]
    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        audioSource = transform.GetChild(0).GetComponent<AudioSource>();
    }


    public void CreateEcho(AudioClip clip)
    {
        audioSource.clip = clip;
        audioClip = clip;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (audioSource.isPlaying) return;

            audioSource.Play();
        }
    }
}
