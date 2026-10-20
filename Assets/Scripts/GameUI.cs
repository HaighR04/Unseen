using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float messageDuration = 3f;
    [SerializeField] private GameObject completionPanel;
    [SerializeField] private TMP_Text completionText;

    private Coroutine hideRoutine;

    private void Start() {
        messageText.text = "";
        completionPanel.SetActive(false);
    }

    public void ShowMessage(string text) {
        messageText.text = text;
        if (hideRoutine != null) StopCoroutine(hideRoutine);
        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay() {
        yield return new WaitForSeconds(messageDuration);
        messageText.text = "";
        hideRoutine = null;
    }

    public void ShowCompletion(string text) {
        completionText.text = text;
        completionPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
