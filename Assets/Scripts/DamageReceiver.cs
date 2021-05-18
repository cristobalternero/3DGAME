using UnityEngine;

class DamageReceiver : MonoBehaviour
{
    [SerializeField]
    private TakeDoDamage m_owner;
    [SerializeField]
    private float m_multiplier = 1;

    public void ApplyDamage(float bulletDamage)
    {
        int totalDamage = (int)(m_multiplier * bulletDamage);
        m_owner.TakeDamage(totalDamage);
    }

}