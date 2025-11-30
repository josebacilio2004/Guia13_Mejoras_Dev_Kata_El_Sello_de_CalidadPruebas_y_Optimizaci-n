using UnityEngine;

public class EnemyInteractable : MonoBehaviour, IInteractable
{
    private AIController _aiController;
    
    void Start()
    {
        _aiController = GetComponent<AIController>();
        if (_aiController == null)
        {
            Debug.LogError("AIController no encontrado en el enemigo!");
        }
    }
    
    // Implementación del método de la interfaz IInteractable
    public void Interact()
    {
        _aiController?.Stun();
        Debug.Log($"Enemigo {gameObject.name} aturdido por {_aiController.stunDuration} segundos");
    }
}