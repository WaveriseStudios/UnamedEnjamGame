using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [Header("Components")]
    public Collider2D PlayerCollider;
    public PlayerCharacter playerCharacter;

    [Header("Settings")]
    public int health = 3;

    bool takeDamageCooldown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!takeDamageCooldown && collision.CompareTag("dealDamage"))
        {
            TakeDammage();
        }
    }

    private void TakeDammage()
    {
        health--;
        Debug.Log("1 damage taken");
        if(health <= 0)
        {
            OnDead();
        }
    }

    private void OnDead()
    {
        playerCharacter.DropItem();
        Debug.Log("Dead");
    }
}