using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerAbilities))]

// Class for managing body part sprites and stages
public class Player : MonoBehaviour
{
    [SerializeField] private Sprite[] stages; // 0 = none, 1 += arms, 2 += legs, 3 += eyes

    private SpriteRenderer sr;
    private PlayerAbilities abilities;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        abilities = GetComponent<PlayerAbilities>();
    }

    private void OnEnable() {
        abilities.OnUnlocked += HandleUnlocked;
        Refresh();
    }

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
