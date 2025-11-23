using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace GameUtch.SearchSystem
{
    /// <summary>
    /// Maneja la UI del sistema de búsqueda
    /// Muestra mensajes de resultado y prompts de interacción
    /// </summary>
    public class SearchUI : MonoBehaviour
    {
        public static SearchUI Instance { get; private set; }

        [Header("Referencias UI - Resultado")]
        [Tooltip("Panel que contiene el mensaje de resultado")]
        public GameObject resultPanel;

        [Tooltip("Texto del mensaje de resultado")]
        public TextMeshProUGUI resultText;

        [Tooltip("Imagen de fondo del panel de resultado (opcional)")]
        public Image resultBackground;

        [Header("Referencias UI - Prompt de Interacción")]
        [Tooltip("Panel que muestra el prompt de interacción")]
        public GameObject promptPanel;

        [Tooltip("Texto del prompt de interacción")]
        public TextMeshProUGUI promptText;

        [Header("Configuración de Animación")]
        [Tooltip("Duración de la animación de fade in/out")]
        public float fadeDuration = 0.3f;

        [Tooltip("Tiempo que se muestra el resultado antes de desaparecer")]
        public float resultDisplayTime = 2.5f;

        [Header("Configuración de Colores")]
        public Color successColor = new Color(0.2f, 0.8f, 0.2f);
        public Color failColor = Color.white;
        public Color promptColor = new Color(1f, 1f, 0.8f);

        [Header("Audio")]
        public AudioClip messageSound;

        private CanvasGroup resultCanvasGroup;
        private CanvasGroup promptCanvasGroup;
        private AudioSource audioSource;
        private Coroutine currentResultCoroutine;
        private Coroutine currentPromptCoroutine;

        void Awake()
        {
            // Implementación del patrón Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            // Configurar CanvasGroups para fade
            SetupCanvasGroups();

            // Configurar AudioSource
            if (messageSound != null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }

            // Ocultar panels inicialmente
            if (resultPanel != null)
                resultPanel.SetActive(false);

            if (promptPanel != null)
                promptPanel.SetActive(false);
        }

        /// <summary>
        /// Configura los CanvasGroups para animaciones
        /// </summary>
        private void SetupCanvasGroups()
        {
            // Result panel
            if (resultPanel != null)
            {
                resultCanvasGroup = resultPanel.GetComponent<CanvasGroup>();
                if (resultCanvasGroup == null)
                {
                    resultCanvasGroup = resultPanel.AddComponent<CanvasGroup>();
                }
                resultCanvasGroup.alpha = 0f;
            }

            // Prompt panel
            if (promptPanel != null)
            {
                promptCanvasGroup = promptPanel.GetComponent<CanvasGroup>();
                if (promptCanvasGroup == null)
                {
                    promptCanvasGroup = promptPanel.AddComponent<CanvasGroup>();
                }
                promptCanvasGroup.alpha = 0f;
            }
        }

        /// <summary>
        /// Muestra el resultado de una búsqueda
        /// </summary>
        public void ShowSearchResult(string message, Color color)
        {
            if (resultPanel == null || resultText == null)
            {
                Debug.LogWarning("[SearchUI] Result panel o text no configurado");
                return;
            }

            // Detener corrutina anterior si existe
            if (currentResultCoroutine != null)
            {
                StopCoroutine(currentResultCoroutine);
            }

            // Configurar texto y color
            resultText.text = message;
            resultText.color = color;

            if (resultBackground != null)
            {
                resultBackground.color = new Color(color.r, color.g, color.b, 0.3f);
            }

            // Reproducir sonido
            if (audioSource != null && messageSound != null)
            {
                audioSource.PlayOneShot(messageSound);
            }

            // Mostrar con animación
            currentResultCoroutine = StartCoroutine(ShowResultCoroutine());
        }

        /// <summary>
        /// Sobrecarga para mostrar resultado sin color personalizado
        /// </summary>
        public void ShowSearchResult(string message)
        {
            ShowSearchResult(message, failColor);
        }

        /// <summary>
        /// Corrutina para mostrar y ocultar el resultado
        /// </summary>
        private IEnumerator ShowResultCoroutine()
        {
            resultPanel.SetActive(true);

            // Fade in
            yield return FadeCanvasGroup(resultCanvasGroup, 0f, 1f, fadeDuration);

            // Esperar
            yield return new WaitForSeconds(resultDisplayTime);

            // Fade out
            yield return FadeCanvasGroup(resultCanvasGroup, 1f, 0f, fadeDuration);

            resultPanel.SetActive(false);
            currentResultCoroutine = null;
        }

        /// <summary>
        /// Muestra el prompt de interacción
        /// </summary>
        public void ShowInteractionPrompt(string message)
        {
            if (promptPanel == null || promptText == null)
            {
                Debug.LogWarning("[SearchUI] Prompt panel o text no configurado");
                return;
            }

            // Detener corrutina anterior si existe
            if (currentPromptCoroutine != null)
            {
                StopCoroutine(currentPromptCoroutine);
            }

            // Configurar texto
            promptText.text = message;
            promptText.color = promptColor;

            // Mostrar con animación
            currentPromptCoroutine = StartCoroutine(ShowPromptCoroutine());
        }

        /// <summary>
        /// Oculta el prompt de interacción
        /// </summary>
        public void HideInteractionPrompt()
        {
            if (promptPanel == null)
                return;

            // Detener corrutina anterior si existe
            if (currentPromptCoroutine != null)
            {
                StopCoroutine(currentPromptCoroutine);
            }

            // Ocultar con animación
            currentPromptCoroutine = StartCoroutine(HidePromptCoroutine());
        }

        /// <summary>
        /// Corrutina para mostrar el prompt
        /// </summary>
        private IEnumerator ShowPromptCoroutine()
        {
            promptPanel.SetActive(true);
            yield return FadeCanvasGroup(promptCanvasGroup, 0f, 1f, fadeDuration);
            currentPromptCoroutine = null;
        }

        /// <summary>
        /// Corrutina para ocultar el prompt
        /// </summary>
        private IEnumerator HidePromptCoroutine()
        {
            yield return FadeCanvasGroup(promptCanvasGroup, 1f, 0f, fadeDuration);
            promptPanel.SetActive(false);
            currentPromptCoroutine = null;
        }

        /// <summary>
        /// Anima el alpha de un CanvasGroup
        /// </summary>
        private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
        {
            if (group == null)
                yield break;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                group.alpha = Mathf.Lerp(from, to, t);
                yield return null;
            }

            group.alpha = to;
        }

        /// <summary>
        /// Muestra un mensaje de error
        /// </summary>
        public void ShowError(string message)
        {
            ShowSearchResult(message, Color.red);
        }

        /// <summary>
        /// Muestra un mensaje de éxito
        /// </summary>
        public void ShowSuccess(string message)
        {
            ShowSearchResult(message, successColor);
        }
    }
}
