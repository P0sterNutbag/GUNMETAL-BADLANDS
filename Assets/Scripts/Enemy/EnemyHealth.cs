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
        Destroy(gameObject);
    }

    void SpawnLoot()
    {
        if (lootToSpawn != null)
        {
            GameObject loot = Instantiate(lootToSpawn);
            loot.transform.position = transform.position;
        }
    }
}
