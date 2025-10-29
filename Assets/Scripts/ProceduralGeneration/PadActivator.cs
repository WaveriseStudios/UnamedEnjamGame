using UnityEngine;

public class PadActivator : MonoBehaviour
{
    public TileManager tileManager;
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;
        if (other.CompareTag("Player"))
        {
            activated = true;
            GetComponent<SpriteRenderer>().color = Color.green; // feedback
            tileManager.PadPressed();
        }
    }
}
