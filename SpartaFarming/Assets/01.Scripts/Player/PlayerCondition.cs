using System;
using UnityEngine;

public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    Condition farmingProficiency { get { return uiCondition.farmingProficiency; } }
    Condition fishingProficiendy { get { return uiCondition.fishingProficiency; } }

    public event Action onTakeDamage;

    void Update()
    {
        Heal(health.passiveValue * Time.deltaTime);

        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Die()
    {
        Debug.Log("Player is dead");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }
}