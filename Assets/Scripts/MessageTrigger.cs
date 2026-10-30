using UnityEngine;

// Shows a message when the player walks into the selected object's trigger collider.
// Used for signs and memory lines along the path, requires a Collider2D with IsTrigger checked on
public class MessageTrigger : MonoBehaviour
{
    [SerializeField, TextArea] private string message;
    [SerializeField] private bool once = true; // If true, only shows first time

    private bool used;
    
    // Called by Unity when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;
        if (other.GetComponent<PlayerAbilities>() == null) return; // Only the player triggers messages
        if (once) used = true;

        GameUI ui = FindAnyObjectByType<GameUI>();
        if (ui != null) ui.ShowMessage(message);

    }
}
