using UnityEngine;

public class TerminalController : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact()
    {
        Debug.Log("Terminal activado. Disparando evento OnObjectiveActivated.");
        GameEvents.TriggerObjectiveActivated();

        //gameObject.GetComponent<Collider>().enabled=false;
    }

}
