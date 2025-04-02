using System;
using UnityEngine;

public interface IDamageable
{
    void TakePhysicalDamage(float damage);
}

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    Condition farmingProficiency { get { return uiCondition.farmingProficiency; } }
    Condition fishingProficiency { get { return uiCondition.fishingProficiency; } }

    public event Action onTakeDamage;

    void Update()
    {
        Heal(health.passiveValue * Time.deltaTime);

        if (health.curValue == 0f)
        {
            Die();
        }
    }
    
    // Health 관련
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Die()
    {
        Debug.Log("Player is dead");
    }

    public void TakePhysicalDamage(float damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }


    // Stamina 관련
    public void UseStamina(float amount)
    {
        stamina.Subtract(amount);
    }


    // Proficiency 관련
    public void GetFarmingProficiency(float amount)
    {
        farmingProficiency.Add(amount);
    }

    public void GetFishingProficiency(float amount)
    {
        fishingProficiency.Add(amount);
    }
}