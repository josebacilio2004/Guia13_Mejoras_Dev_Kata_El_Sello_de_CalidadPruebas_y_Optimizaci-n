using UnityEngine;

public class AttackState : AIState
{
    private float _attackRange;
    
    public AttackState(AIController controller) : base(controller)
    {
        _attackRange = controller.attackRange;
    }
    
    public override void OnEnter()
    {
        Debug.Log("⚔️ Entrando en estado de Ataque");
        m_agent.isStopped = true; // Detener movimiento para atacar
    }

    public override void UpdateState()
    {
        // 1. Verificar si el jugador sigue dentro del rango de ataque
        float distanceToPlayer = Vector3.Distance(m_controller.transform.position, m_playerTransform.position);
        
        if (distanceToPlayer <= _attackRange)
        {
            // Lógica de ataque
            Debug.Log("⚔️ Atacando al Jugador");
            // Aquí puedes añadir: animaciones, reducir salud del jugador, etc.
        }
        // 2. Si el jugador se aleja pero sigue visible, perseguirlo
        else if (distanceToPlayer <= m_controller.loseSightRadius)
        {
            Debug.Log("🎯 Jugador se alejó - Persiguiendo");
            m_controller.ChangeState(new ChaseState(m_controller));
            return;
        }
        // 3. Si el jugador se sale del radio de detección, volver a patrullar
        else
        {
            Debug.Log("👀 Jugador perdido - Volviendo a patrulla");
            m_controller.ChangeState(new PatrolState(m_controller));
            return;
        }
    }

    public override void OnExit()
    {
        Debug.Log("🛑 Saliendo del estado de Ataque");
        m_agent.isStopped = false; // Reanudar movimiento
    }
}