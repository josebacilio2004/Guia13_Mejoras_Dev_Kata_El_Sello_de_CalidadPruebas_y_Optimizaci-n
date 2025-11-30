using UnityEngine;

public class ChaseState : AIState
{
    private float _attackRange;
    
    public ChaseState(AIController controller) : base(controller) 
    {
        _attackRange = controller.attackRange;
    }
    
    public override void OnEnter()
    {
        Debug.Log("🎯 Entrando en estado de Persecución");
        m_agent.speed = m_controller.chaseSpeed;
    }

    public override void UpdateState()
    {
        // 1. Condicion de transición
        if(Vector3.Distance(m_controller.transform.position, m_playerTransform.position) > m_controller.loseSightRadius)
        {
            m_controller.ChangeState(new PatrolState(m_controller));
            return;
        }

        // 2. Lógica del estado: perseguir al jugador
        m_agent.destination = m_playerTransform.position;
    }

    public override void OnExit() 
    {
        Debug.Log("🛑 Saliendo del estado de Persecución");
    }
}