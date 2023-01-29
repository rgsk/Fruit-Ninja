
using UnityEngine;

public class Fruit : MonoBehaviour {
    public GameObject whole;
    public GameObject sliced;
    private Rigidbody fruitRigidbody;
    private ParticleSystem juiceParticleEffect;
    private bool fruitIsCut = false;
    private Blade blade;
    private void Awake() {
        fruitRigidbody = GetComponent<Rigidbody>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
        var player = GameObject.FindWithTag("Player");
        if (player != null) {
            blade = player.GetComponent<Blade>();
        }
    }

    private void Slice(Vector3 direction, Vector3 position, float force) {
        whole.SetActive(false);
        sliced.SetActive(true);
        juiceParticleEffect.Play();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();
        foreach (var slice in slices) {
            slice.velocity = fruitRigidbody.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }

    private void Update() {
        if (!fruitIsCut) {
            CheckBladeNearTheFruit();
        }
    }

    private void CheckBladeNearTheFruit() {
        if (blade.slicing) {
            if (blade.bladeTrailPositionsCount > 0) {
                for (int i = 1; i < blade.bladeTrailPositionsCount; i++) {
                    var position = blade.bladeTrailPositions[i];
                    bool bladeNearTheFruit =
                    Mathf.Abs(fruitRigidbody.position.y - position.y) < whole.transform.lossyScale.y
                    &&
                    Mathf.Abs(fruitRigidbody.position.x - position.x) < whole.transform.lossyScale.x;
                    if (bladeNearTheFruit) {
                        var prevPosition = blade.bladeTrailPositions[i - 1];
                        var direction = position - prevPosition;
                        Slice(direction, position, blade.sliceForce);
                        fruitIsCut = true;
                        return;
                    }
                }

            }
        }
    }
}