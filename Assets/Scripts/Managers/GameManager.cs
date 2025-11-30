using System.Collections;
using UnityEngine;

/// <summary>
/// Gestiona el estado principal del juego, como jugar, ganar o perder
/// Implementa el patrón Singleton para un acceso global único
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton Pattern
    public static GameManager Instance { get; private set; }

    // Estado del Juego
    public enum GameState { Playing, Victory, Loss }
    private GameState _currentState;

    [Header("Gameplay Settings")]

    [SerializeField] private int _objectivesToWin = 3;
    
    public GameLogic Logic {get; private set;}

    private void Awake()
    {
        // Configuración del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // opcional: 
        // DontDestroyOnLoad(gameObject); 
        // si necesitas que persista entre escenas
        Logic = new GameLogic(_objectivesToWin);
    }

    private void OnEnable()
    {
        GameEvents.OnObjectiveActivated += HandleObjectiveActivated;
    }

    private void OnDisable()
    {
        GameEvents.OnObjectiveActivated -= HandleObjectiveActivated;
    }

    private void HandleObjectiveActivated()
    {
        if(_currentState != GameState.Playing) return;

        Logic.CompleteObjective();
        Debug.Log($"Objetivo Completado. Progreso: {Logic.ObjectivesCompleted}/{Logic.ObjectivesToWin}");
        if(Logic.IsVictoryConditionMet)
        {
            ChangeState(GameState.Victory);
        }
    }
    private void Start()
    {
        // estado inicial del juego
        ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        Debug.Log($"Nuevo estado de juego: {_currentState}");

        switch (_currentState)
        {
            case GameState.Playing:
                // logica para cuando empieza el juego
                break;
            case GameState.Victory:
                StartCoroutine(VictorySequence());
                break;
            case GameState.Loss:
                // logica para cuando se pierde
                break;
        }
    }

    /// <summary>
    /// Corrutina que gestiona la secuencia de eventos cuando el jugador gana.
    /// </summary>
    private IEnumerator VictorySequence()
    {
        Debug.Log("SECUENCIA DE VICTORIA INICIADA");

        // desactivar el control del jugador (opcional, pero buena práctica)
        FindFirstObjectByType<FirstPersonController>().enabled = false;

        // 1: espera 1 segundo
        yield return new WaitForSeconds(1f);

        // 2: muestra un panel de victoria en la UI
        Debug.Log("mostrando UI de Victoria...");

        // suponiendo que UIManager tiene una referencia a este panel
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowVictoryPanel();
        }

        // 3. espera 3 segundos más
        yield return new WaitForSeconds(3f);

        // 4. carga la escena del Menú Principal
        Debug.Log("Volviendo al Menú Principal...");
        //SceneManager.LoadScene("MainMenuScene"); // Si tienes esta escena

        // o cambiar el Estado al MainMenu
        UIManager.Instance.ChangeState(UIManager.Instance.MainMenuState);
    }
}