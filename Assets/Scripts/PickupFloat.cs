using UnityEngine;

public class PickupFloat : MonoBehaviour
{
    // Fields for customizing the pickups, glow, and bobbing
    [SerializeField] private SpriteRenderer glow;
    [SerializeField] private float bobHeight = 0.06f;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float glowMinAlpha = 0.2f;
    [SerializeField] private float glowMaxAlpha = 0.5f;
    [SerializeField] private float pulseSpeed = 3f;

    private Vector3 startLocalPos;
    private float phase;

    // When game starts, gives each pickup a random phase to randomize teh starting point of the wave for update
    private void Awake() {
        startLocalPos = transform.localPosition;
        phase = Random.value * Mathf.PI * 2f;
    }

    private void Update() {
        float t = Time.time + phase;
        // Sin goes from -1 to 1 as time goes on and creates the bobbing movement
        transform.localPosition = startLocalPos + Vector3.up * (Mathf.Sin(t * bobSpeed) * bobHeight);

        // Manages the glow using Sin combined with set pulseSpeed, as well as Alpha min and max
        if (glow != null) {
            Color c = glow.color;
            float wave = (Mathf.Sin(t * pulseSpeed) + 1f) * 0.5f; 
            c.a = Mathf.Lerp(glowMinAlpha, glowMaxAlpha, wave);
            glow.color = c;
        }
    }
}
