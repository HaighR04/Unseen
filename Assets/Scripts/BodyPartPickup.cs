using UnityEngine;

public class BodyPartPickup : MonoBehaviour
{
    [SerializeField] private BodyPartType part;
    [SerializeField, TextArea] private string message;

    private bool collected;

    private void OnTriggerEnter2D(Collider2D other) {
        if (collected) return;

        PlayerAbilities abilities = other.GetComponent<PlayerAbilities>();
        if (abilities == null) return;

        collected = true;
        abilities.Unlock(part);

        GameUI ui = FindAnyObjectByType<GameUI>();
        if (ui != null) ui.ShowMessage(message);

        Destroy(gameObject);
    }
}
