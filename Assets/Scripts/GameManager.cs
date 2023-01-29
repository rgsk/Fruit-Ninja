using UnityEngine;
using TMPro;
using System.Collections;
public class GameManager : MonoBehaviour {
    public TMP_Text scoreText;
    private int score = 0;
    private Blade blade;
    public UnityEngine.UI.Image fadeImage;
    private Spawner spawner;

    private void Start() {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
        NewGame();
    }

    private void NewGame() {
        blade.enabled = true;
        spawner.enabled = true;
        score = 0;
        scoreText.text = score.ToString();
        ClearScene();
    }

    private void ClearScene() {
        var fruits = FindObjectsOfType<Fruit>();
        foreach (var fruit in fruits) {
            Destroy(fruit.gameObject);
        }
        var bombs = FindObjectsOfType<Bomb>();
        foreach (var bomb in bombs) {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int points) {
        score += points;
        scoreText.text = score.ToString();
    }

    public void GameOver() {
        Debug.Log("GameOver");
        spawner.enabled = false;
        StartCoroutine(ExplodeSequence());
    }
    private IEnumerator ExplodeSequence() {
        float elapsed = 0f;
        float duration = 1f;
        while (elapsed < duration) {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);
            Time.timeScale = 1 - t;
            // we use unscaledDeltaTime instead of deltaTime because Time.timeScale is distorted
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        // below is not used because Time.timeScale is distorted
        // yield return new WaitForSeconds(1f);
        yield return new WaitForSecondsRealtime(1f);
        NewGame();
        Time.timeScale = 1f;
        elapsed = 0f;
        while (elapsed < duration) {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
