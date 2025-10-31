using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField nameInputField;

    GameObject player;
    public GameObject hud;
    public GameObject deadCanvas;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nameInputField.onValueChanged.AddListener(value => SetName(nameInputField.text));
        GameManager.instance.isInGame = false;
        player = FindFirstObjectByType(typeof(PlayerCharacter)).GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetName(string name)
    {
        GameManager.instance.SetName(name);
    }

    public void StartGame()
    {
        GameManager.instance.isInGame = true;
        player.SetActive(true);
        player.GetComponent<PlayerCharacter>().OnEnable();
        player.GetComponent<PlayerHealth>().Reset();
        hud.SetActive(true);
        deadCanvas.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
