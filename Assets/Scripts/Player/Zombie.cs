using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Transform playerPos;

    private bool playerInRoom = true;

    public float speed = 1.5f;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerInRoom)
        {
            if(playerPos)
            {
                Vector2 direction = (playerPos.position - transform.position).normalized;
                rb.linearVelocity = direction * speed * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDammage();
        }
    }
}