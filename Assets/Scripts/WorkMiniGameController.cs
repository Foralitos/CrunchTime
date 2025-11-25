using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Controlador para el minijuego de trabajo tipo typing
/// El jugador debe presionar teclas que aparecen en pantalla
/// </summary>
public class WorkMiniGameController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Texto que muestra la letra actual a presionar")]
    public TextMeshProUGUI letterText;

    [Tooltip("Texto que muestra el tiempo restante")]
    public TextMeshProUGUI timerText;

    [Tooltip("Texto que muestra el puntaje")]
    public TextMeshProUGUI scoreText;

    [Tooltip("Texto de instrucciones")]
    public TextMeshProUGUI instructionsText;

    [Header("Game Settings")]
    [Tooltip("Duración del minijuego en segundos")]
    public float gameTime = 60f;

    [Tooltip("Teclas posibles para el minijuego")]
    public string[] possibleKeys = { "A", "S", "D", "F", "J", "K", "L" };

    [Tooltip("Nombre de la escena a la que regresar")]
    public string returnSceneName = "Tutorial Level";

    [Header("Debug")]
    public bool showDebugInfo = false;

    // Estado del juego
    private float timeRemaining;
    private KeyCode currentKey;
    private int score = 0;
    private bool gameActive = true;

    void Start()
    {
        timeRemaining = gameTime;
        score = 0;

        // Mostrar instrucciones iniciales
        if (instructionsText != null)
        {
            instructionsText.text = "¡Presiona las teclas que aparecen!\nESC para salir";
        }

        // Generar primera tecla
        GenerateNewKey();

        // Actualizar UI inicial
        UpdateUI();

        DebugLog("Minijuego iniciado");
    }

    void Update()
    {
        if (!gameActive)
            return;

        // Actualizar timer
        timeRemaining -= Time.deltaTime;

        // Verificar si se acabó el tiempo
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            EndGame();
            return;
        }

        // Actualizar UI del timer
        UpdateUI();

        // Verificar input de la tecla correcta
        if (Input.GetKeyDown(currentKey))
        {
            OnCorrectKeyPressed();
        }

        // ESC para salir anticipadamente
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DebugLog("Jugador presionó ESC - saliendo del minijuego");
            EndGame();
        }
    }

    /// <summary>
    /// Genera una nueva tecla aleatoria para que el jugador presione
    /// </summary>
    private void GenerateNewKey()
    {
        if (possibleKeys == null || possibleKeys.Length == 0)
        {
            Debug.LogError("[WorkMiniGameController] possibleKeys está vacío!");
            return;
        }

        // Seleccionar una letra aleatoria
        string randomLetter = possibleKeys[Random.Range(0, possibleKeys.Length)];

        // Convertir a KeyCode
        try
        {
            currentKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), randomLetter);

            // Mostrar en UI
            if (letterText != null)
            {
                letterText.text = randomLetter;
            }

            DebugLog($"Nueva tecla generada: {randomLetter}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[WorkMiniGameController] Error al parsear KeyCode: {e.Message}");
        }
    }

    /// <summary>
    /// Llamado cuando el jugador presiona la tecla correcta
    /// </summary>
    private void OnCorrectKeyPressed()
    {
        score += 10;
        DebugLog($"¡Correcto! Puntaje: {score}");

        // Generar nueva tecla
        GenerateNewKey();

        // Actualizar UI
        UpdateUI();
    }

    /// <summary>
    /// Actualiza todos los elementos de UI
    /// </summary>
    private void UpdateUI()
    {
        // Actualizar timer
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = $"Tiempo: {seconds}s";
        }

        // Actualizar score
        if (scoreText != null)
        {
            scoreText.text = $"Puntaje: {score}";
        }
    }

    /// <summary>
    /// Termina el minijuego y regresa a la escena principal
    /// </summary>
    private void EndGame()
    {
        gameActive = false;

        DebugLog($"Minijuego terminado - Puntaje final: {score}");

        // Mostrar resultado final
        if (letterText != null)
        {
            letterText.text = "¡Terminado!";
        }

        if (instructionsText != null)
        {
            instructionsText.text = $"Puntaje Final: {score}\nRegresando...";
        }

        // Regresar a la escena principal después de 1.5 segundos
        Invoke(nameof(ReturnToMainScene), 1.5f);
    }

    /// <summary>
    /// Regresa a la escena principal
    /// </summary>
    private void ReturnToMainScene()
    {
        DebugLog($"Cargando escena: {returnSceneName}");
        SceneManager.LoadScene(returnSceneName);
    }

    /// <summary>
    /// Log de debug condicional
    /// </summary>
    private void DebugLog(string message)
    {
        if (showDebugInfo)
        {
            Debug.Log($"[WorkMiniGameController] {message}");
        }
    }

    // Getters públicos
    public int GetScore() => score;
    public float GetTimeRemaining() => timeRemaining;
    public bool IsGameActive() => gameActive;
}
