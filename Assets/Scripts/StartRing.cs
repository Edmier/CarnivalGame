using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRing : MonoBehaviour
{
    
    [SerializeField] private AudioClip startSound;

    void OnTriggerEnter(Collider other) {
        if (GameTimer.IsRunning) return;

        GameTimer.instance.StartGame();

        AudioSource.PlayClipAtPoint(startSound, transform.position);
    }
}
