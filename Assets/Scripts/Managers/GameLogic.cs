using UnityEngine;

/// <summary>
/// Clase de lógica pura que gestiona el estado de los objetivos
/// NO hereda de MonoBehaviour para ser fácilmente testeable
/// </summary>
[System.Serializable]
public class GameLogic
{
    [SerializeField] private int objectivesToWin;
    [SerializeField] private int objectivesCompleted;
    
    public int ObjectivesToWin => objectivesToWin;
    public int ObjectivesCompleted => objectivesCompleted;
    public bool IsVictoryConditionMet => objectivesCompleted >= objectivesToWin;

    // Constructor para testing
    public GameLogic(int objectivesToWin)
    {
        this.objectivesToWin = objectivesToWin > 0 ? objectivesToWin : 1;
        this.objectivesCompleted = 0;
    }
    
    // Constructor sin parámetros para Unity
    public GameLogic() : this(1) { }

    public void CompleteObjective()
    {
        if (!IsVictoryConditionMet)
        {
            objectivesCompleted++;
        }
    }
    
    public void ResetObjectives()
    {
        objectivesCompleted = 0;
    }
    
    // Método para forzar estado (útil para testing)
    public void SetObjectivesCompleted(int count)
    {
        objectivesCompleted = Mathf.Clamp(count, 0, int.MaxValue);
    }
}