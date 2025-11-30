using UnityEngine;
using UnityEngine.InputSystem;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Settings")]
    [SerializeField] private float shootRange = 20f;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private float damageAmount = 25f;
    [SerializeField] private string damageType = "Stun";
    
    [Header("Visual Feedback")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float lineDisplayTime = 0.1f;

    void Start()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
        
        Debug.Log("🎯 Rifle INICIALIZADO - Esperando input...");
        
        // Verificar dispositivos de input
        Debug.Log($"🖱️ Mouse disponible: {Mouse.current != null}");
        Debug.Log($"⌨️ Teclado disponible: {Keyboard.current != null}");
    }
    
    void Update()
    {
        // DEBUG TEMPORAL - Verificar input cada frame
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("🔴🔴🔴 CLICK IZQUIERDO DETECTADO por Input System!");
                Shoot();
            }
            
            // También verificar click derecho para testing
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Debug.Log("🔵 CLICK DERECHO DETECTADO");
            }
        }
        
        if (Keyboard.current != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("🟢 ESPACIO DETECTADO");
                Shoot();
            }
            
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                Debug.Log("🟡 TECLA F DETECTADA");
                Shoot();
            }
        }
    }
    
    private void Shoot()
    {
        Debug.Log("🎯 MÉTODO SHOOT() EJECUTADO");
        
        RaycastHit hit;
        Vector3 startPosition = transform.position;
        Vector3 direction = transform.forward;

        Debug.Log($"📍 Posición del rifle: {startPosition}");
        Debug.Log($"🎯 Dirección: {direction}");

        if (Physics.Raycast(startPosition, direction, out hit, shootRange, enemyLayerMask))
        {
            Debug.Log($"✅✅✅ RAYCAST GOLPEÓ: {hit.collider.gameObject.name}");
            
            // Buscar IDamageable
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Debug.Log($"💥💥💥 IDamageable ENCONTRADO - Aplicando daño!");
                damageable.TakeDamage(damageAmount, damageType);
            }
            else
            {
                Debug.LogError($"❌❌❌ NO SE ENCONTRÓ IDamageable en {hit.collider.gameObject.name}");
            }
        }
        else
        {
            Debug.Log($"❌ RAYCAST FALLÓ - No golpeó nada");
        }
    }
    private void ShowShotLine(Vector3 start, Vector3 end)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            lineRenderer.enabled = true;
            Invoke(nameof(HideShotLine), lineDisplayTime);
        }
    }
    
    private void HideShotLine()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    // MÉTODOS NUEVOS PARA CONFIGURAR EL DAÑO DINÁMICAMENTE
    public void SetDamage(float newDamage, string newDamageType)
    {
        damageAmount = newDamage;
        damageType = newDamageType;
        Debug.Log($"🔄 Rifle configurado - Daño: {damageAmount} Tipo: {damageType}");
    }

    public void SetStunDamage()
    {
        SetDamage(25f, "Stun");
        Debug.Log("🌀 Rifle configurado para daño ATURDIDOR");
    }

    public void SetPhysicalDamage()
    {
        SetDamage(35f, "Physical");
        Debug.Log("⚔️ Rifle configurado para daño FÍSICO");
    }

    public void SetFireDamage()
    {
        SetDamage(50f, "Fire");
        Debug.Log("🔥 Rifle configurado para daño de FUEGO");
    }
    
    // Método para debug visual en el Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * shootRange);
    }
}