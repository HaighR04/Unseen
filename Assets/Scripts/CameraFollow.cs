using UnityEngine;

// Helps with the "lagging camera" view by taking a little while to catch up to the player
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.25f;

    private Vector3 velocity;
    
    // LateUpdate runs after every Update in the frame, so camera moves after the player moved
    private void LateUpdate() {
        Vector3 goal = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);
    }
    // Smooth damp makes the camera feel more like a spring, larger smoothTime means slower camera
}
