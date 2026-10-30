using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAbilities))]

// Top-down movement, Unity adds the required componets automatically if the player doesn't have them
public class PlayerMovement : MonoBehaviour
{
    // Customizable fields for movement
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float legSpeedMultiplier = 1.75f;
    
    private Rigidbody2D body;
    private PlayerAbilities abilities;
    private Vector2 moveInput;
    
    // Gets components for abilities and body of the player
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbilities>();
    }   

    // Reads input each frame, WASD and arrow keys both work, vector is normalized so that diagonal movement is the same speed as horizontal or vertical
    private void Update() {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        float x = 0f;
        float y = 0f;

        // Easy way of doing movement input for both WASD and arrows
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) x -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) x += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) y -= 1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) y += 1f;
  
        moveInput = new Vector2(x, y).normalized;
    }

    // Checks whether the player has legs to update the movement speed.
    private void FixedUpdate() {
        float speed = abilities.HasLegs ? baseSpeed * legSpeedMultiplier : baseSpeed;
        body.linearVelocity = moveInput * speed;
    }
}
