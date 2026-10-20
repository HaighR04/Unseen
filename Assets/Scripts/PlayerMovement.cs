using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAbilities))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float legSpeedMultiplier = 1.75f;
    
    private Rigidbody2D body;
    private PlayerAbilities abilities;
    private Vector2 moveInput;
    
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbilities>();
    }   

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

    private void FixedUpdate() {
        float speed = abilities.HasLegs ? baseSpeed * legSpeedMultiplier : baseSpeed;
        body.linearVelocity = moveInput * speed;
    }
}
