using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public PlayerCharacter character;
    public GameObject deadCanvas;

    public Slider echoSlider;
    public Slider healthSlider;
    public Image itemImg;

    public GameObject smallMap;
    public GameObject largeMap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(character.GetComponent<PlayerHealth>().health <= 0)
        {
            deadCanvas.SetActive(true);
            this.gameObject.SetActive(false);
        }



        healthSlider.value = character.GetComponent<PlayerHealth>().health;

        if (character.isRecording)
        {
            echoSlider.value = (10 - character.recordingTime);
        }
        else
        {
            echoSlider.value += Time.deltaTime;
        }

        if (character.cooldown.IsCoolingDown)
        {
            echoSlider.interactable = false;
        }
        else
        {
            echoSlider.interactable = true;
        }

        if (character.itemObj)
        {
            itemImg.gameObject.SetActive(true);
        }
        else
        {
            itemImg.gameObject.SetActive(false);
        }

        if (character.wantMap)
        {
            smallMap.SetActive(false);
            largeMap.SetActive(true);
        }
        else
        {
            smallMap.SetActive(true);
            largeMap.SetActive(false);
        }

    }
}
