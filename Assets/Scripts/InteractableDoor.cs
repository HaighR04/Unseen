using UnityEngine;
using UnityEngine.InputSystem;


public class InteractableDoor : MonoBehaviour
{
    [SerializeField, TextArea] private string lockedMessage = "It won't budge, I need my arms";
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip lockedSound;

    private PlayerAbilities playerInRange;
    private GameUI ui;
    private AudioManager audioManager;

    // Look for the UI and audio
    private void Start() {
        ui = FindAnyObjectByType<GameUI>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }
    
    // When another collider enters this collider, checks if collider is player
    private void OnTriggerEnter2D(Collider2D other) {
        PlayerAbilities a = other.GetComponent<PlayerAbilities>();
        if (a != null) playerInRange = a;
    }

    // Same logic but leaving the collider range
    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<PlayerAbilities>() != null) playerInRange = null;
    }

    private void Update() {
        if (playerInRange == null) return;

        Keyboard kb = Keyboard.current;
        if (kb == null || !kb.eKey.wasPressedThisFrame) return;

        // Logic for if the player already has the arms body part when opening a door
        if (playerInRange.HasArms) {
            if (audioManager != null) audioManager.Play(openSound);
            gameObject.SetActive(false);
        }
        // Logic if not, plays closed door sound
        else {
            if (audioManager != null) audioManager.Play(lockedSound);
            if (ui != null) ui.ShowMessage(lockedMessage);
        }
    }
}
