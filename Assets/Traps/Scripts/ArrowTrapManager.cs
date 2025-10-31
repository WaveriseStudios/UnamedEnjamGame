using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArrowTrapManager : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private float detectionRange = 0.5f;
    [SerializeField] private Cooldown cooldown;
    public float cooldownTime;
    [Header("Other")]
    [SerializeField] private CircleCollider2D detectionCollider;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform trapTransform;
    [SerializeField] private GameObject arrowPrefab;

    private bool isCooldown = false;
    private AudioSource audioSource;

    private void Start()
    {
        detectionCollider.radius = detectionRange;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(playerTransform)
        {
            // Direction from turret to player
            Vector2 direction = playerTransform.position - transform.position;

            // Get the angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Because the turret sprite points UP, subtract 90 degrees to align correctly
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isCooldown || !other.CompareTag("Player")) return;
        ShootArrow();
    }

    private void ShootArrow()
    {
        if (cooldown.IsCoolingDown) return;

        Vector2 dir = (playerTransform.position - trapTransform.position).normalized;
        GameObject arrow = Instantiate(arrowPrefab, trapTransform.position, Quaternion.identity);
        arrow.GetComponent<ArrowProjectile>().SetDirection(dir);

        audioSource.Play();

        cooldown.StartCooldown(cooldownTime);
    }

}
