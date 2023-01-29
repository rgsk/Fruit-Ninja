using UnityEngine;

public class Blade : MonoBehaviour {

    public Collider bladeCollider;
    public TrailRenderer bladeTrail;
    private bool slicing = false;
    public Vector3 direction { get; private set; }
    public float minSliceVelocity = 0.01f;
    public float sliceForce = 5f;

    private void OnEnable() {
        StopSlicing();
    }
    private void OnDisable() {
        StopSlicing();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartSlicing();

        } else if (Input.GetMouseButtonUp(0)) {
            // when we release the left click
            StopSlicing();
        } else if (slicing) {
            ContinueSlicing();
        }
    }
    private void StartSlicing() {
        // when we start slicing we move the blade position to where the cursor is
        var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        transform.position = newPosition;

        slicing = true;
        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear();
    }
    private void StopSlicing() {
        slicing = false;
        bladeCollider.enabled = false;
        bladeTrail.enabled = false;
    }
    private void ContinueSlicing() {
        var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        direction = newPosition - transform.position;
        var velocity = direction.magnitude / Time.deltaTime;
        bladeCollider.enabled = velocity > minSliceVelocity;
        transform.position = newPosition;
    }
}
