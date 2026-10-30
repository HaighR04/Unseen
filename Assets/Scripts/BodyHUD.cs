using UnityEngine;
// Side panel to show the icons for each body part the player collected
public class BodyHUD : MonoBehaviour
{
    // Creates editable fields for list of PlayerAbilities and the icons for each body part 
    [SerializeField] private PlayerAbilities abilities;
    [SerializeField] private GameObject eyesIcon;
    [SerializeField] private GameObject legsIcon;
    [SerializeField] private GameObject armsIcon;

    // Enable and Disable are Unity calls for when an object is turned on or off
    // Subscribe to the unlock event, unsubscribe in OnDisable
    private void OnEnable() {
        abilities.OnUnlocked += HandleUnlocked;
        Refresh();
    }

    private void OnDisable() {
        abilities.OnUnlocked -= HandleUnlocked;
    }
    // Shows or hides the object
    private void HandleUnlocked(BodyPartType part) {
        Refresh();
    }

    // Shows each icon only if the matching body part was picked up
    private void Refresh() {
        eyesIcon.SetActive(abilities.HasEyes);
        legsIcon.SetActive(abilities.HasLegs);
        armsIcon.SetActive(abilities.HasArms);
    }
}
