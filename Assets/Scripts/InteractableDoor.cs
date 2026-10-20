using UnityEngine;
using UnityEngine.InputSystem;


public class InteractableDoor : MonoBehaviour
{
    [SerializeField, TextArea] private string lockedMessage = "It won't budge, I need my arms";

    private PlayerAbilities playerInRange;
    private GameUI ui;

    private void Start() {
        ui = FindAnyObjectByType<GameUI>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerAbilities a = other.GetComponent<PlayerAbilities>();
        if (a != null) playerInRange = a;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<PlayerAbilities>() != null) playerInRange = null;
    }

    private void Update() {
        if (playerInRange == null) return;

        Keyboard kb = Keyboard.current;
        if (kb == null || !kb.eKey.wasPressedThisFrame) return;

        if (playerInRange.HasArms) 
            gameObject.SetActive(false);
        else if (ui != null)
            ui.ShowMessage(lockedMessage);
    }



}
