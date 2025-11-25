using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class SceneTransitionDoor : MonoBehaviour
{
    [Header("Scene Transition")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private bool savePlayerState = true;

    [Header("UI Prompt")]
    [SerializeField] private GameObject interactPrompt; // "Press E to Enter" UI
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInRange = false;
    private GameObject player;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            TransitionToScene();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;

            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }

    void TransitionToScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Target scene name not set!");
            return;
        }

        // Save player state before transition
        if (savePlayerState && player != null && GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SavePlayerState(player.transform);
        }

        // Load target scene
        SceneManager.LoadScene(targetSceneName);
    }
}