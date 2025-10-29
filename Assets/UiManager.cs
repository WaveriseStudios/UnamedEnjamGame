using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public PlayerCharacter character;

    public Slider slider;
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
        if(character.isRecording)
        {
            slider.value = (10-character.recordingTime);
        }
        else
        {
            slider.value += Time.deltaTime;
        }

        if(character.cooldown.IsCoolingDown)
        {
            slider.interactable = false;
        }
        else
        {
            slider.interactable = true;
        }

        if(character.itemObj)
        {
            itemImg.gameObject.SetActive(true);
        }
        else
        {
            itemImg.gameObject.SetActive(false);
        }

        if(character.wantMap)
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
