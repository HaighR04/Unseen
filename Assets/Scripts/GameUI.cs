using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

// Handles the messages on screen, typed out letter by letter, as well as the ending screen
public class GameUI : MonoBehaviour
{
    // Customization fields, completion fields for ending scene and text
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float messageDuration = 3f;
    [SerializeField] private float charDelay = 0.03f;
    [SerializeField] private GameObject completionPanel;
    [SerializeField] private TMP_Text completionText;

    // A coroutine is a function that can pause and resume without stopping the rest of the game
    private Coroutine messageRoutine;

    private void Start() {
        completionPanel.SetActive(false);
        ShowMessage("WASD / Arrows: move\nE: interact"); // Controls message
    }

    // Displays provided message for set duration time using the coroutine, any new message replaces the message still showing
    public void ShowMessage(string text) {
        if (messageRoutine != null) StopCoroutine(messageRoutine);
        messageRoutine = StartCoroutine(ShowMessageRoutine(text));
    }

    // Types the message one character at a time, maxVisibleCharacters controls how many are shown, ForceMeshUpdate makes TextMeshPro calculate the layout immediately, so characterCount isn't old/stale
    // Unscaled time is used so that the messages keep working even if Time.timeScale is 0
    private IEnumerator ShowMessageRoutine(string text) {
        messageText.text = text;
        messageText.maxVisibleCharacters = 0;
        messageText.ForceMeshUpdate();
        int total = messageText.textInfo.characterCount;

        float timer = 0f;
        int shown = 0;
        while (shown < total) {
            if (charDelay <= 0f) {
                shown = total; // No delay means show everything right away
            } else {
                timer += Time.unscaledDeltaTime;
                while (timer >= charDelay && shown < total) {
                    timer -= charDelay;
                    shown++;
                }
            }

            messageText.maxVisibleCharacters = shown;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(messageDuration);
        messageText.text = "";
        messageText.maxVisibleCharacters = int.MaxValue;
        messageRoutine = null;
    }

    // Display completion text and set timeScale to 0f to stop in-game time progression
    public void ShowCompletion(string text) {
        completionText.text = text;
        completionPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Restarts everything by loading current scene, resets timeScale (abilities, pickups, doors, and fog as well), hooked to Play Again button
    // skips intro
    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
