using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] private GameObject titleObject;
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private GameObject hintObject;
    [SerializeField] private float charDelay = 0.05f;
    [SerializeField] private string nextScene = "Main";
    [SerializeField, TextArea] private string[] lines;

    private void Start() {
        StartCoroutine(Run());
    }

    private IEnumerator Run() {
        storyText.text = "";
        titleObject.SetActive(true);
        hintObject.SetActive(true);
        yield return WaitForKey();
        titleObject.SetActive(false);

        foreach (string line in lines) {
            hintObject.SetActive(false);
            yield return TypeLine(line);
            hintObject.SetActive(true);
            yield return WaitForKey();
        }
        SceneManager.LoadScene(nextScene);
    }

    private IEnumerator WaitForKey() {
        // Ignore the key that was pressed to get to the function
        while (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
        {
            yield return null;
        }

        while (!KeyPressed()) yield return null;

        while (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
        {
            yield return null;
        }
    }

    // Function for actually tracking and typing the lines, based on charDelay
    private IEnumerator TypeLine(string line) {
        storyText.text = line;
        storyText.maxVisibleCharacters = 0;
        storyText.ForceMeshUpdate();
        int total = storyText.textInfo.characterCount;

        float timer = 0f;
        int shown = 0;
        while (shown < total) {

            if (charDelay <= 0f)
            {
                shown = total;
            }
            else
            {
                timer += Time.deltaTime;
                while (timer >= charDelay && shown < total) {
                    timer -= charDelay;
                    shown++;
                } 
            }
            storyText.maxVisibleCharacters = shown;
            yield return null;
        }

        storyText.maxVisibleCharacters = total;
    }

    private bool KeyPressed() {
        Keyboard kb = Keyboard.current;
        return kb != null && kb.anyKey.wasPressedThisFrame;
    }

}
