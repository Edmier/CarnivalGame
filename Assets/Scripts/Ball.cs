using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ball : MonoBehaviour
{

    [SerializeField] private AudioClip ballHitSound;
    [SerializeField] private AudioClip ballMissSound;
    [SerializeField] private AudioClip ballScoreSound;

    Vector3 startPosition;
    AudioSource audioSource;
    public bool isTrown = false;
    private bool hitTarget = false;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();

        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(DropBall);

        startPosition = transform.position;

        StartCoroutine(ResetBallTimer());
    }

    void DropBall(SelectExitEventArgs args)
    {
        if (args.isCanceled) return;

        isTrown = true;
        StartCoroutine(ResetBallTimer());
    }

    private IEnumerator ResetBallTimer(float time = 7f)
    {
        yield return new WaitForSeconds(time);
        
        if (isTrown) {
            ResetBall();
        }
    }

    private void ResetBall() {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.useGravity = false;

        transform.position = startPosition;

        if (!hitTarget) {
            audioSource.PlayOneShot(ballMissSound);
        }

        isTrown = false;
        hitTarget = false;

        StopAllCoroutines();
    }

    void OnCollisionEnter(Collision other)
    {
        // Debug.Log("Collision");
        // Debug.Log(other.gameObject.name);

        audioSource.PlayOneShot(ballHitSound);
        StartCoroutine(ResetBallTimer(3f));
    }

    void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("BowlArea") || hitTarget) return;

        Bowl bowl = other.gameObject.transform.parent.GetComponent<Bowl>();

        if (bowl?.Selected == true) {
            audioSource.PlayOneShot(ballScoreSound, 0.75f);
            GameTimer.instance.AddSeconds(15);

            bowl.Selected = false;
        } else if (bowl?.fishCaught == true) {
            GameTimer.instance.AddSeconds(2);
        } else {
            GameTimer.instance.AddSeconds(5);
        }

        bowl?.CatchFish();

        hitTarget = true;
        audioSource.PlayOneShot(ballScoreSound, 0.25f);
        StartCoroutine(ResetBallTimer(3f));
    }
}
