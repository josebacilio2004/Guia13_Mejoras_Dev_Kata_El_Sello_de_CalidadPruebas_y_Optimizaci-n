using UnityEngine;
using System.Collections;

public class StunState : AIState
{
    private float _stunDuration;
    private Coroutine _stunCoroutine;

    public StunState(AIController controller, float stunDuration) : base(controller)
    {
        _stunDuration = stunDuration;
    }

    public override void OnEnter()
    {
        Debug.Log("Entrando en estado de Aturdimiento.");
        
        // Detener el NavMeshAgent
        m_agent.isStopped = true;
        m_agent.velocity = Vector3.zero;
        
        // Iniciar corrutina de aturdimiento
        _stunCoroutine = m_controller.StartCoroutine(StunCoroutine());
    }

    public override void UpdateState()
    {
        // No hacer nada durante el aturdimiento
        // El agente permanece inmóvil
    }

    public override void OnExit()
    {
        // Reanudar el movimiento
        m_agent.isStopped = false;
        
        // Detener la corrutina si está activa
        if (_stunCoroutine != null)
        {
            m_controller.StopCoroutine(_stunCoroutine);
        }
    }

    private IEnumerator StunCoroutine()
    {
        // Esperar la duración del aturdimiento
        yield return new WaitForSeconds(_stunDuration);
        
        // Volver al estado de patrulla
        m_controller.ChangeState(new PatrolState(m_controller));
    }
}