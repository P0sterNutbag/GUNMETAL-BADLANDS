using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public float health = 40;
    public GameObject lootToSpawn;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        SpawnLoot();
        print("Dead");
        Destroy(gameObject);
    }

    void SpawnLoot()
    {
        GameObject loot = Instantiate(lootToSpawn);
        print(transform.position);
        loot.transform.position = transform.position;
    }
}
