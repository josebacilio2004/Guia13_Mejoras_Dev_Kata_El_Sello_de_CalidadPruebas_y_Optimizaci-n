using System;

public static class GameEvents
{
    // Eventos existentes
    public static event Action OnObjectiveActivated;
    
    // Nuevos eventos necesarios
    public static event Action<string> OnSpecificObjectiveCompleted;
    public static event Action OnAllObjectivesCompleted;
    public static event Action OnEnemyDefeated;

    // Métodos para disparar eventos
    public static void TriggerObjectiveActivated()
    {
        OnObjectiveActivated?.Invoke();
    }
    
    public static void TriggerSpecificObjectiveCompleted(string objectiveId)
    {
        OnSpecificObjectiveCompleted?.Invoke(objectiveId);
    }
    
    public static void TriggerAllObjectivesCompleted()
    {
        OnAllObjectivesCompleted?.Invoke();
    }
    
    public static void TriggerEnemyDefeated()
    {
        OnEnemyDefeated?.Invoke();
    }
}