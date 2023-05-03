using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeChange : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);   
    }

    // Update is called once per frame
    void Update()
    {
        // Move up slowly
        transform.position += new Vector3(0, 0.5f, 0) * Time.deltaTime;
    }

    public void SetChangeTime(int time) {
        if (time < 0) {
            text.color = Color.red;
            text.text = "- " + time + "s";
        } else {
            text.color = Color.green;
            text.text = "+ " + time + "s";
        }
    }
}
