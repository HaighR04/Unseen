using UnityEngine;

public class BodyHUD : MonoBehaviour
{
    [SerializeField] private PlayerAbilities abilities;
    [SerializeField] private GameObject eyesIcon;
    [SerializeField] private GameObject legsIcon;
    [SerializeField] private GameObject armsIcon;

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

    private void Refresh() {
        eyesIcon.SetActive(abilities.HasEyes);
        legsIcon.SetActive(abilities.HasLegs);
        armsIcon.SetActive(abilities.HasArms);
    }
}
