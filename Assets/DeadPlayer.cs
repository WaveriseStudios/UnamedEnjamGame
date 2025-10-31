using TMPro;
using UnityEngine;

public class DeadPlayer : MonoBehaviour
{
    public GameObject echoObject;
    public TextMeshPro nameText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEcho(AudioClip clip)
    {
        GameObject echo = Instantiate<GameObject>(echoObject, transform.position, transform.rotation);
        echo.GetComponent<EchoListener>().CreateEcho(clip);
        GameManager.instance.AddDeathVocal(clip);

        Destroy(this.GetComponent<DeadPlayer>());
    }
}
