using UnityEngine;

public class LootChestController : MonoBehaviour, IInteractable
{
    public bool IsOpened { get ; private set; }
    
    private bool _costlyFunctionWasCalled = false;
    
    public event System.Action OnChestOpened;

    public void Interact()
    {
        if(IsOpened)
        {
            Debug.Log("Haz ABIERTO este cofre.");
            // ❌ BUG INTENCIONAL: Comenta esta línea temporalmente para el Dev Kata
            return; // ❌ COMENTA ESTO PARA INTRODUCIR EL BUG
            
            PlayCostlyFunction(); // Esto NO debería ejecutarse
        }

        IsOpened = true;
        
        // ✅ CORREGIDO: Usar el método Trigger en lugar de asignar directamente
        OnChestOpened?.Invoke();
        GameEvents.TriggerObjectiveActivated(); // ✅ CORREGIDO
        
        Debug.Log("Haz ABIERTO el cofre y encontrado un tesoro de oro en el interior del cofre. Felicidades!");
    }

    private void PlayCostlyFunction()
    {
        _costlyFunctionWasCalled = true;
        Debug.LogError("❌ FUNCIÓN COSTOSA EJECUTADA - Esto impacta el rendimiento!");
        
        for (int i = 0; i < 1000; i++)
        {
            Vector3 expensiveCalculation = transform.position * i;
        }
    }

    public bool WasCostlyFunctionCalled()
    {
        return _costlyFunctionWasCalled;
    }

    public void ResetForTesting()
    {
        IsOpened = false;
        _costlyFunctionWasCalled = false;
    }
}                                                                                   