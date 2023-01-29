using UnityEngine;

public class Blade : MonoBehaviour {

    public TrailRenderer bladeTrail;
    public bool slicing = false;
    public float sliceForce = 5f;
    private Vector3[] bladeTrailPositions = new Vector3[100];
    private int bladeTrailPositionsCount = 0;
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
        bladeTrailPositionsCount = bladeTrail.GetPositions(bladeTrailPositions);
    }
    private void StartSlicing() {
        // when we start slicing we move the blade position to where the cursor is
        var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        transform.position = newPosition;
        slicing = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear();
    }
    private void StopSlicing() {
        slicing = false;
        bladeTrail.enabled = false;
    }
    private void ContinueSlicing() {
        var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        transform.position = newPosition;
    }
    public (Vector3 direction, Vector3 position)? CheckObjectIsNear(Rigidbody body) {
        if (slicing) {
            if (bladeTrailPositionsCount > 0) {
                for (int i = 1; i < bladeTrailPositionsCount; i++) {
                    var position = bladeTrailPositions[i];
                    bool bladeNearTheFruit =
                    Mathf.Abs(body.position.y - position.y) < body.transform.lossyScale.y
                    &&
                    Mathf.Abs(body.position.x - position.x) < body.transform.lossyScale.x;
                    if (bladeNearTheFruit) {
                        var prevPosition = bladeTrailPositions[i - 1];
                        var direction = position - prevPosition;
                        return (direction, position);
                    }
                }

            }
        }
        return null;
    }

}
