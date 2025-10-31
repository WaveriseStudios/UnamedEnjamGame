using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [Header("Components")]
    public Collider2D PlayerCollider;
    public PlayerCharacter playerCharacter;
    public GameObject playerDeadPrefab;

    [Header("Settings")]
    public int health = 3;
    public int maxHealth = 3;

    bool takeDamageCooldown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!takeDamageCooldown && collision.CompareTag("dealDamage"))
        {
            TakeDammage();
        }
    }

    public void TakeDammage()
    {
        health--;
        Debug.Log("1 damage taken");
        if(health <= 0)
        {
            OnDead();
        }
    }

    public void Reset()
    {
        health = maxHealth;
    }

    private void OnDead()
    {
        GameObject deadbody = Instantiate<GameObject>(playerDeadPrefab, transform.position, Quaternion.identity);
        playerCharacter.DropItem();
        deadbody.GetComponent<DeadPlayer>().nameText.text = GameManager.instance.currentPlayerName;
        this.gameObject.SetActive(false);
        this.transform.position = new Vector3(10, 10, 0);
        playerCharacter.Reset();
        Debug.Log("Dead");
    }
}