using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject timeChangePrefab;
    public static float TimeRemaining = 90f;
    public static GameTimer instance;
    public static bool IsRunning = false;

    private bool firstRun = true;
    // Start is called before the first frame update

    void Start() {
        instance = this;
    }

    public void StartGame() {
        if (IsRunning) return;

        if (!firstRun) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            return;
        }

        firstRun = false;

        TimeRemaining = 20f;
        StartCoroutine(StartTimer());
        IsRunning = true;
    }

    public void AddSeconds(int time) {
        if (!IsRunning) return;

        TimeRemaining += time;

        GameObject timeChange = Instantiate(timeChangePrefab, transform.position, transform.rotation);
        timeChange.GetComponent<TimeChange>().SetChangeTime(time);
    }

    private IEnumerator StartTimer() {
        while (TimeRemaining > 0) {
            TimeRemaining -= Time.deltaTime;
            
            int minutes = Mathf.FloorToInt(TimeRemaining / 60f);
            int seconds = Mathf.FloorToInt(TimeRemaining - minutes * 60);

            text.text = string.Format("{0:0}:{1:00}", minutes, seconds);

            yield return null;
        }
        IsRunning = false;
        text.text = "0:00";
    }
}
