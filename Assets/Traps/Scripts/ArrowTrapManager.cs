using System.Collections;
using UnityEngine;

public class ArrowTrapManager : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private float detectionRange = 0.5f;
    [SerializeField] private float cooldown = 3.0f;
    [Header("Other")]
    [SerializeField] private CircleCollider2D detectionCollider;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform trapTransform;
    [SerializeField] private GameObject arrowPrefab;

    private bool isCooldown = false;

    private void Start()
    {
        detectionCollider.radius = detectionRange;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isCooldown || !other.CompareTag("Player")) return;
        StartCoroutine(ShootArrow());
        playerTransform = other.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopCoroutine(ShootArrow());
    }

    private IEnumerator ShootArrow()
    {
        isCooldown = true;

        if(playerTransform)
        {
            Vector2 dir = (playerTransform.position - trapTransform.position).normalized;
            GameObject arrow = Instantiate(arrowPrefab, trapTransform.position, Quaternion.identity);
            arrow.GetComponent<ArrowProjectile>().SetDirection(dir);
        }

        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }

}
