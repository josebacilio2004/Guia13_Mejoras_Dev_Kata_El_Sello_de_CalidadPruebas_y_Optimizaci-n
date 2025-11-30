public interface IDamageable
{
    /// <summary>
    /// Recibe daño de un tipo específico
    /// </summary>
    /// <param name="amount">Cantidad de daño</param>
    /// <param name="damageType">Tipo de daño ("Stun", "Fire", "Physical", etc.)</param>
    void TakeDamage(float amount, string damageType);
}