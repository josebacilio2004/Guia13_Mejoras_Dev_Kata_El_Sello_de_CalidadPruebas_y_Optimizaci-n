using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [System.Serializable]
    public class Objective
    {
        public string id;
        public string description;
        public bool isCompleted;
    }

    [Header("Objectives")]
    [SerializeField] private List<Objective> objectives = new List<Objective>
    {
        new Objective { id = "explore", description = "Explora el área", isCompleted = false },
        new Objective { id = "open_chest", description = "Abre el cofre", isCompleted = false },
        new Objective { id = "defeat_enemy", description = "Derrota al enemigo", isCompleted = false }
    };

    public static ObjectiveManager Instance { get; private set; }
    public event Action<int, int> OnObjectivesProgressChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("✅ ObjectiveManager iniciado");
        
        // Registrar eventos
        var enemy = FindFirstObjectByType<AIController>();
        if (enemy != null)
        {
            enemy.OnDeath += () => CompleteObjective("defeat_enemy");
        }

        // Simular otros objetivos
        StartCoroutine(SimulateObjectiveCompletion());
    }

    private IEnumerator SimulateObjectiveCompletion()
    {
        yield return new WaitForSeconds(5f);
        CompleteObjective("explore");
        
        yield return new WaitForSeconds(3f);
        CompleteObjective("open_chest");
    }

    public void CompleteObjective(string id)
    {
        var objective = objectives.Find(o => o.id == id);
        if (objective != null && !objective.isCompleted)
        {
            objective.isCompleted = true;
            Debug.Log($"✅ Objetivo completado: {objective.description}");
            
            int completed = GetCompletedCount();
            int total = objectives.Count;
            
            OnObjectivesProgressChanged?.Invoke(completed, total);
            GameEvents.TriggerObjectiveActivated(); // ✅ Usar método existente
            
            if (completed >= total)
            {
                Debug.Log("🎉 ¡Todos los objetivos completados!");
                // El GameManager se encargará de la victoria
            }
        }
    }

    public int GetCompletedCount()
    {
        return objectives.FindAll(o => o.isCompleted).Count;
    }

    public int GetTotalCount()
    {
        return objectives.Count;
    }
}