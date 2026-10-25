using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [SerializeField, TextArea] private string exitMessage = "I remember why I ran.";
    [SerializeField] private AudioClip victorySound;

    private bool finished;
    // Same thing as BodyPartPickup, runs when another collider enters this collider
    private void OnTriggerEnter2D(Collider2D other) {
        if (finished) return;
        if (other.GetComponent<PlayerAbilities>() == null) return;

        finished = true;
        
        // Plays victory audio when end is reached
        AudioManager audio = FindAnyObjectByType<AudioManager>();
        if (audio != null) audio.Play(victorySound);

        // Shows victory/end scene once the final door is reached
        GameUI ui = FindAnyObjectByType<GameUI>();
        if (ui != null) ui.ShowCompletion(exitMessage);
    }
}
