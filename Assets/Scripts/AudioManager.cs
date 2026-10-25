using UnityEngine;

// Attached to AudioManager object itself rather than other gameObjects as they destroy/disable themselves, which would stop the sound.
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 0.8f;

    private AudioSource source;

    private void Awake() {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    // Function for playing audio, PlayOneShot plays the audio on top of whatever else is playing.
    public void Play(AudioClip clip) {
        if (clip == null) return;
        source.PlayOneShot(clip, sfxVolume);
    }
}
