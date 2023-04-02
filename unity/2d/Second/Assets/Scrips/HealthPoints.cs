using UnityEngine;
using System;

public class HealthPoints : MonoBehaviour
{
    [SerializeField] int MaxHealthPoints = 10;
    [SerializeField] ParticleSystem DamageEffect;

    public static Action<int> InformAboutHeartsFullAction;
    public static Action<int> InformAboutHeartsLeftAction;

    private Animator animator;
    private int healthPoints;
    private void Start()
    {
        healthPoints = MaxHealthPoints;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        InformAboutHeartsFull(MaxHealthPoints);
    }

    public void TakeDamege(int damage, Vector2 pos)
    {
        if (DamageEffect)
        {
            DamageEffect.transform.position = pos;
            DamageEffect.Play();
        }
        else
        {
            Debug.Log("System particle is not set for HealthPoint.TakeDamege " + gameObject.name);
        }

        healthPoints -= damage;
        InformAboutHeartsLeft(healthPoints);

        if (healthPoints <= 0)
        {
            if(animator)
            {
                animator.SetTrigger("EnemyDie");
                Destroy(gameObject, 0.6f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if(animator)
            {
                animator.SetTrigger("EnemyTakeDamage");
            }
        }
    }

    public void InformAboutHeartsFull(int maxHealthPoints)
    {
        if (null != InformAboutHeartsFullAction)
        {
            int hearts = maxHealthPoints;
            if (maxHealthPoints > 10)
            {
                hearts = 10;
            }
            InformAboutHeartsFullAction.Invoke(hearts);
        }
    }

    public void InformAboutHeartsLeft(int heartsLeft)
    {
        if (null != InformAboutHeartsLeftAction)
        {
            InformAboutHeartsLeftAction.Invoke(heartsLeft);
        }
    }
}
