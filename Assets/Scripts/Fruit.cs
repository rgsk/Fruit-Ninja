
using UnityEngine;

public class Fruit : MonoBehaviour {
    public GameObject whole;
    public GameObject sliced;
    private Rigidbody fruitRigidbody;
    private ParticleSystem juiceParticleEffect;
    private bool fruitIsCut = false;
    private Blade blade;
    private void Awake() {
        Debug.Log(whole.transform.lossyScale.x);
        fruitRigidbody = GetComponent<Rigidbody>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
        var player = GameObject.FindWithTag("Player");
        if (player != null) {
            blade = player.GetComponent<Blade>();
        }
    }

    private void Slice(Vector3 direction, Vector3 position, float force) {
        if (fruitIsCut) return;
        fruitIsCut = true;
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
        Debug.Log(blade.bladeTrailPositionsCount);
        if (blade.slicing) {
            if (blade.bladeTrailPositionsCount > 0) {
                for (int i = 1; i < blade.bladeTrailPositionsCount; i++) {
                    var position = blade.bladeTrailPositions[i];
                    var prevPosition = blade.bladeTrailPositions[i - 1];
                    var direction = position - prevPosition;
                    var velocity = direction.magnitude / Time.deltaTime;
                    Debug.Log("Velocity: " + velocity / 50);
                    if (Mathf.Abs(fruitRigidbody.position.y - position.y) < whole.transform.lossyScale.y
                    &&
                     Mathf.Abs(fruitRigidbody.position.x - position.x) < whole.transform.lossyScale.x
                     ) {
                        Slice(direction, position, velocity / 50);
                    }
                }

            }
        }

    }

}
