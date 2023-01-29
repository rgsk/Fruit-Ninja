using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour {
    public TMP_Text scoreText;
    private int score = 0;

    private void Start() {
        NewGame();
    }

    private void NewGame() {
        score = 0;
        scoreText.text = score.ToString();
    }

    public void IncreaseScore(int points) {
        score += points;
        scoreText.text = score.ToString();
    }
}
