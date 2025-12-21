using UnityEngine;

public interface IHealthSystem
{
    void TakeDamage(float damage);
    float GetCurrentHealth();
    float GetMaxHealth();
}
