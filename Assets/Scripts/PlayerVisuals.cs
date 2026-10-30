using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerAbilities))]

/* Swaps the players sprite as the body parts get picked up, sprite is chosen by how many parts the player has collected
Stages only match the art if parts are collected in correct order

*/
public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private Sprite[] stages; // 0 = none, 1 += arms, 2 += legs, 3 += eyes

    private SpriteRenderer sr;
    private PlayerAbilities abilities;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        abilities = GetComponent<PlayerAbilities>();
    }
    
    // Connect to the unlock event so the sprite can update right as a part gets collected
    private void OnEnable() {
        abilities.OnUnlocked += HandleUnlocked;
        Refresh();
    }

    // Always unsubscribe when disabled so the scene reloads do not leave dangling event handlers
    private void OnDisable() {
        abilities.OnUnlocked -= HandleUnlocked;
    }

    private void HandleUnlocked(BodyPartType part) {
        Refresh();
    }

    // Used to switch between sprites based on how many abilities the player has
    private void Refresh() {
        int count = (abilities.HasLegs ? 1 : 0) + (abilities.HasEyes ? 1 : 0) + (abilities.HasArms ? 1 : 0);

        if (stages == null || stages.Length == 0) return;
        sr.sprite = stages[Mathf.Min(count, stages.Length - 1)];
    }

}
