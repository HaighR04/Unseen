using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    // Customization fields, completion fields for ending scene and text
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float messageDuration = 3f;
    [SerializeField] private GameObject completionPanel;
    [SerializeField] private TMP_Text completionText;

    // A coroutine is a function that can pause and resume without stopping the rest of the game
    private Coroutine hideRoutine;

    private void Start() {
        completionPanel.SetActive(false);
        ShowMessage("WASD / Arrows: move\nE: interact");
    }

    // Displays provided message for set duration time using the coroutine
    public void ShowMessage(string text) {
        messageText.text = text;
        if (hideRoutine != null) StopCoroutine(hideRoutine);
        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    // After duration of seconds has been waited, set coroutine to null and text to ""
    private IEnumerator HideAfterDelay() {
        yield return new WaitForSeconds(messageDuration);
        messageText.text = "";
        hideRoutine = null;
    }

    // Display completion text and set timeScale to 0f to stop in-game time progression
    public void ShowCompletion(string text) {
        completionText.text = text;
        completionPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Restarts everything by loading first scene, resets timeScale
    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
