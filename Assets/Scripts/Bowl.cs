using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{

    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject[] fishPrefabs;
    private GameObject fish;
    public static List<Bowl> bowls = new List<Bowl>();
    public bool Selected = false;
    private Vector3 startPosition;
    private float offset = 0;
    private float fishOffset = 0;
    private float fishSpeed = 0.5f;
    private bool fishCaught = false;

    // Start is called before the first frame update
    void Start() {
        bowls.Add(this);
        startPosition = transform.position;
        offset = Random.Range(0, 1000);

        if (bowls.Count == 1) {
            StartCoroutine(SelectBowls());
        }

        // Spawn random fish
        int randomIndex = Random.Range(0, fishPrefabs.Length);
        fish = Instantiate(fishPrefabs[randomIndex], transform.position, Quaternion.identity, transform);
    }

    void Update() {
        // Move bowl up and down with applied offset and amplitude and speed
        transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * 2 + offset) * 0.05f, 0);

        if (fishCaught) return;
        // Move fish up and down with applied offset and amplitude and speed
        fish.transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * fishSpeed + fishOffset) * 0.05f, 0);
    }

    private IEnumerator SelectBowls(float time = 20f) {
        while (true) {
            yield return new WaitForSeconds(time);

            if (bowls.Count > 0) {
                int randomIndex = Random.Range(0, bowls.Count);
                Bowl bowl = bowls[randomIndex];
                bowl.Select();
            }
        }
    }

    public void CatchFish() {
        if (fishCaught || !GameTimer.IsRunning) return;

        fishCaught = true;
        StartCoroutine(VanishFish());
    }

    private void Select() {
        Selected = true;
        particles.SetActive(true);

        StartCoroutine(ResetBowlTimer());
    }

    private IEnumerator ResetBowlTimer(float time = 10f)
    {
        yield return new WaitForSeconds(time);

        particles.SetActive(false);
        Selected = false;
    }

    private IEnumerator VanishFish() {
        // Move fish upwards and fade out
        while (fish.transform.position.y < 1f) {
            fish.transform.position += new Vector3(0, 0.5f, 0) * Time.deltaTime;
            yield return null;
        }

        // Destroy fish
        fish.SetActive(false);
    }
}
