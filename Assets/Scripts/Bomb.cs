using UnityEngine;

public class Bomb : MonoBehaviour {
    private Blade blade;
    private Rigidbody bombRigidbody;

    private bool bombIsHit = false;

    private GameManager gameManager;
    private void Awake() {
        bombRigidbody = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        blade = FindObjectOfType<Blade>();
    }

    private void Update() {
        if (!bombIsHit)
            CheckBladeNearTheBomb();
    }

    private void CheckBladeNearTheBomb() {
        var details = blade.CheckObjectIsNear(bombRigidbody);
        if (details != null) {
            gameManager.GameOver();
            bombIsHit = true;
        }
    }
}
