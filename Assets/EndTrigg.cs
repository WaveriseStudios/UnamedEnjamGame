using UnityEngine;

public class EndTrigg : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.winMenu.SetActive(true);
    }
}
