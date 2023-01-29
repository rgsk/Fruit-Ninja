using UnityEngine;

public class Bomb : MonoBehaviour {
    private Blade blade;
    private Rigidbody bombRigidbody;

    private bool bombIsHit = false;


    private void Awake() {
        bombRigidbody = GetComponent<Rigidbody>();
        var player = GameObject.FindWithTag("Player");
        if (player != null) {
            blade = player.GetComponent<Blade>();
        }
    }

    private void Update() {
        if (!bombIsHit)
            CheckBladeNearTheBomb();
    }

    private void CheckBladeNearTheBomb() {
        var details = blade.CheckObjectIsNear(bombRigidbody);
        if (details != null) {
            Debug.Log("Bomb hit");
            bombIsHit = true;
        }
    }
}
