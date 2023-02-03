using UnityEngine;

public class Enemy : MonoBehaviour
{
    // TODO: Extract enemy animation
    [SerializeField]
    private Animator animator;
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    [SerializeField]
    private int maxHealth = 100;

    public int MaxHealth => maxHealth;

    public int CurrentHealth { get; private set; }

    public void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void Attack(int damage)
    {
        CurrentHealth -= damage;
        animator.SetTrigger(Hurt);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool(IsDead, true);
        
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
    }
}
