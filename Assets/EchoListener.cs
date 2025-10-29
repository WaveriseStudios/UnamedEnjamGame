using UnityEngine;
using TMPro;

public class EchoListener : MonoBehaviour
{
    [Header("Components")]
    public AudioSource audioSource;
    public AudioClip audioClip;
    public TextMeshPro NameText;

    private void Start()
    {
        audioSource = transform.Find("AudioSource").GetComponent<AudioSource>();
        NameText.text = GameManager.instance.currentPlayerName;
    }


    public void CreateEcho(AudioClip clip)
    {
        audioSource = transform.Find("AudioSource").GetComponent<AudioSource>();
        NameText.text = GameManager.instance.currentPlayerName;

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
