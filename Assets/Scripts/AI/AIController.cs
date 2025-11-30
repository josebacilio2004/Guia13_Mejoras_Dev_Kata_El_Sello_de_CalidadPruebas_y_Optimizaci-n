using UnityEngine;

public class AIController : MonoBehaviour, IDamageable
{
    [Header("AI Settings")]
    public Transform[] waypoints;
    public float patrolSpeed = 5f;
    public float chaseSpeed = 5f;
    public float detectionRadius = 10f;
    public float loseSightRadius = 15f;
    public float attackRange = 3f;

    [Header("Stun Settings")]
    [SerializeField] public float stunDuration = 3f;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private AIState _currentState;
    
    // ✅ EVENTO AGREGADO
    public event System.Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        ChangeState(new PatrolState(this));
    }    
 
    void Update()
    {
        _currentState?.UpdateState();
    }

    public void ChangeState(AIState newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    }

    public void TakeDamage(float amount, string damageType)
    {
        Debug.Log($"💥 AIController recibió {amount} de daño tipo: {damageType}");
        
        switch (damageType)
        {
            case "Stun":
                Stun();
                break;
            case "Physical":
                currentHealth -= amount;
                Debug.Log($"❤️ Salud actual: {currentHealth}/{maxHealth}");
                
                if (currentHealth <= 0)
                {
                    Die();
                }
                break;
            case "Fire":
                currentHealth -= amount * 1.5f;
                Debug.Log($"🔥 Daño por fuego! Salud: {currentHealth}/{maxHealth}");
                
                if (currentHealth <= 0)
                {
                    Die();
                }
                break;
            default:
                currentHealth -= amount;
                Debug.Log($"⚔️ Daño genérico. Salud: {currentHealth}/{maxHealth}");
                break;
        }
    }

    private void Die()
    {
        Debug.Log("💀 ENEMIGO DERROTADO");
        
        // ✅ CORREGIDO: Usar el método Trigger
        OnDeath?.Invoke();
        GameEvents.TriggerEnemyDefeated(); // ✅ CORREGIDO
        
        gameObject.SetActive(false);
    }

    public void Stun()
    {
        if (_currentState is StunState) 
        {
            Debug.Log("⏳ Ya está aturdido, ignorando...");
            return;
        }
        
        ChangeState(new StunState(this, stunDuration));
    }

    public bool IsStunned()
    {
        return _currentState is StunState;
    }

    public float AttackRange => attackRange;
}