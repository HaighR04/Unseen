using UnityEngine;

public class BodyPartPickup : MonoBehaviour
{
    [SerializeField] private BodyPartType part;
    [SerializeField, TextArea] private string message;
    [SerializeField] private AudioClip pickupSound;

    private bool collected;

    // This is called by Unity when something enters the trigger collider
    // and gets the other objects collider as "other"
    private void OnTriggerEnter2D(Collider2D other) {
        if (collected) return;

        // Only player can collect pickups, any other objects would not have abilities
        // and would return null
        PlayerAbilities abilities = other.GetComponent<PlayerAbilities>();
        if (abilities == null) return;

        collected = true;
        abilities.Unlock(part);

        // Plays the provided audio for pickups
        AudioManager audio = FindAnyObjectByType<AudioManager>();
        if (audio != null) audio.Play(pickupSound);

        // Shows the provided message for each unique pickup
        GameUI ui = FindAnyObjectByType<GameUI>();
        if (ui != null) ui.ShowMessage(message);

        // Removes the objectfrom the map
        Destroy(gameObject);
    }
}
