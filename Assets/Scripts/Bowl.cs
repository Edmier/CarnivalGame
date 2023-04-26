using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{

    [SerializeField] private GameObject particles;
    public static List<Bowl> bowls = new List<Bowl>();
    public bool Selected = false;
    private Vector3 startPosition;
    private float offset = 0;

    // Start is called before the first frame update
    void Start() {
        bowls.Add(this);
        startPosition = transform.position;
        offset = Random.Range(0, 1000);

        if (bowls.Count == 1) {
            StartCoroutine(SelectBowls());
        }
    }

    void Update() {
        // Move bowl up and down with applied offset and amplitude and speed
        transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * 2 + offset) * 0.05f, 0);

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
}
