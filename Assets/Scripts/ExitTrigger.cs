using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [SerializeField, TextArea] private string exitMessage = "I remember why I ran.";

    private bool finished;

    private void OnTriggerEnter2D(Collider2D other) {
        if (finished) return;
        if (other.GetComponent<PlayerAbilities>() == null) return;

        finished = true;
        GameUI ui = FindAnyObjectByType<GameUI>();
        if (ui != null) ui.ShowCompletion(exitMessage);
    }
}
