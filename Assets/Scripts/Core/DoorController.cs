using UnityEngine;

public class DoorController : MonoBehaviour,IInteractable
{
    private bool _isOpen = false;

    public void Interact()
    {
        _isOpen = !_isOpen;
        Debug.Log(_isOpen ? "Haz ABIERTO la puerta." : "Haz CERRADO la puerta.");

        //Aquí activarás una animacion o rotarías el objeto.
    }
    
}
