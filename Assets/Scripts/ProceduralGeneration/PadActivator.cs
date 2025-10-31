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
            GetComponent<Animator>().SetBool("play", true);
            transform.Find("LightNormal").gameObject.SetActive(false);
            transform.Find("LightActive").gameObject.SetActive(true);
            tileManager.PadPressed();
        }
    }
}
