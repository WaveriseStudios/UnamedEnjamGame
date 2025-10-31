using Unity.VisualScripting;
using UnityEngine;

public class DeadUI : MonoBehaviour
{
    public AudioClip deathEcho;
    public GameObject menu;
    [SerializeField]
    public Cooldown cooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Record()
    {
        string device = Microphone.devices[0];
        int sampleRate = 44100;
        int lengthSec = 10;

        deathEcho = Microphone.Start(device, false, lengthSec, sampleRate);
    }

    public void StopRecord()
    {
        Microphone.End(null);

        FindFirstObjectByType(typeof(DeadPlayer)).GetComponent<DeadPlayer>().SpawnEcho(deathEcho);

        menu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
