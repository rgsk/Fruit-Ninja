
using UnityEngine;

public class Fruit : MonoBehaviour {
    public GameObject whole;
    public GameObject sliced;
    private Rigidbody fruitRigidbody;
    private ParticleSystem juiceParticleEffect;
    private bool fruitIsCut = false;
    private bool fruitIsFallen = false;
    private bool fruitIsOnScreen = false;
    private Blade blade;
    private GameManager gameManager;
    public int points = 1;
    private void Awake() {
        fruitRigidbody = GetComponent<Rigidbody>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
        blade = FindObjectOfType<Blade>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Slice(Vector3 direction, Vector3 position, float force) {
        gameManager.IncreaseScore(points);
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
        if (!fruitIsCut && !fruitIsFallen) {
            CheckBladeNearTheFruit();
            CheckFallen();
        }
    }

    private void CheckBladeNearTheFruit() {
        var details = blade.CheckObjectIsNear(fruitRigidbody);
        if (details.HasValue) {
            Slice(details.Value.direction, details.Value.position, blade.sliceForce);
            fruitIsCut = true;
        }
    }



    private void CheckFallen() {
        var fruitPosition = fruitRigidbody.position;
        var position = Camera.main.WorldToViewportPoint(fruitPosition);
        if (position.y > 0 && !fruitIsOnScreen) {
            fruitIsOnScreen = true;
            // Debug.Log("Fruit Entered");
        }
        if (position.y < 0 && fruitIsOnScreen) {
            fruitIsFallen = true;
            fruitIsOnScreen = false;
            // Debug.Log("Fruit Fallen");
            OnFall();
        }
    }
    private void OnFall() {
        gameManager.GameOver();
    }
}