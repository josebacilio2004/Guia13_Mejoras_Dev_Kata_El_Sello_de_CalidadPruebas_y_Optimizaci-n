using UnityEngine;

public class SystemDoor : MonoBehaviour
{
    public bool doorOpen = false; //abierto o cerrado
    public float doorOpenAngle = 95f;   //Angulo de puerta abierta
    public float doorCloseAngle = 0.0f;   //Angulo cerrado
    public float smooth = 3.0f;              //velocidad de cerrar la puerta
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(doorOpen)
        {
            Quaternion targetRotation = Quaternion.Euler(0,doorOpenAngle,0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);

        }
        else
        {
            Quaternion targetRotation2 = Quaternion.Euler(0,doorCloseAngle,0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, smooth * Time.deltaTime);
        }
    }
}
