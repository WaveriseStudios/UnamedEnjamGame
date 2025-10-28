using System.Collections;
using UnityEngine;

public class SpikeTrapManager : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField]
    private float activeDuration = 0.5f;
    [SerializeField]
    private float cooldown = 2.0f;
    [Tooltip("Game object with the collider that deals the dammage (dealDamage tag).")]
    [SerializeField]
    private GameObject DamageCollider;

    [Header("Sprites")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite spikesUp;
    [SerializeField]
    private Sprite spikesDown;


    private bool isCooldown = false;

    private void Start()
    {
        DamageCollider.SetActive(false);
        spriteRenderer.sprite = spikesDown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCooldown || !other.CompareTag("Player")) return;
        StartCoroutine(ActivateTrap());
    }

    private IEnumerator ActivateTrap()
    {
        isCooldown = true;
        spriteRenderer.sprite = spikesUp;
        DamageCollider.SetActive(true);

        yield return new WaitForSeconds(activeDuration);

        spriteRenderer.sprite = spikesDown;
        DamageCollider.SetActive(false);

        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }
}
