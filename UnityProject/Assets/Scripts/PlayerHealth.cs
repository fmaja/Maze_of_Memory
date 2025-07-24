using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private int hitCounter = 0; 
    public int hitsPerLifeLoss = 3; 

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"Player took {amount} damage. Current health: {currentHealth}");

        hitCounter++;
        if (hitCounter >= hitsPerLifeLoss)
        {
            hitCounter = 0;
            Zycia.Instance.LoseLife();
            Debug.Log("Lost 1 life due to hits.");
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        hitCounter = 0; // Mo¿na te¿ resetowaæ liczniki
    }
}
