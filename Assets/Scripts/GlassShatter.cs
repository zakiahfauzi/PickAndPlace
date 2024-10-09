using System.Collections;
using UnityEngine;

[SelectionBase]
public class GlassShatterAndReturn : MonoBehaviour
{
    [SerializeField] GameObject intactCube;
    [SerializeField] GameObject brokenCube;

    [Tooltip("Time to return the cube to its start position after breaking.")]
    [SerializeField] float returnTime = 2f;

    Vector3 _startPOS;
    Rigidbody _rigidBody;
    WaitForSeconds _returnTimer;
    bool _waitingForReturn;

    BoxCollider bc;

    private void Awake()
    {
        intactCube.SetActive(true);
        brokenCube.SetActive(false);

        bc = GetComponent<BoxCollider>();
        _rigidBody = GetComponent<Rigidbody>();

        _startPOS = transform.position;
        _returnTimer = new WaitForSeconds(returnTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.transform.name); // Debug statement
        if (collision.transform.CompareTag("Floor") && !_waitingForReturn)
        {
            Break();
        }
    }

    private void Break()
    {
        intactCube.SetActive(false);
        brokenCube.SetActive(true);

        bc.enabled = false; // Disable the BoxCollider to prevent further interaction
        _rigidBody.isKinematic = true; // Disable physics on the object after breaking

        Return(); // Initiates the return process after the cube breaks
    }

    public void Return()
    {
        StartCoroutine(ReturnRoutine());
    }

    IEnumerator ReturnRoutine()
    {
        _waitingForReturn = true;
        yield return _returnTimer;

        // Reset position and enable intact cube
        _rigidBody.velocity = Vector3.zero; // Stop any remaining motion
        _rigidBody.angularVelocity = Vector3.zero; // Stop any remaining rotation
        _rigidBody.position = _startPOS; // Move the object back to the starting position

        // Optionally reset the cube to its intact state after returning
        intactCube.SetActive(true);
        brokenCube.SetActive(false);
        bc.enabled = true; // Re-enable the BoxCollider
        _rigidBody.isKinematic = false; // Enable physics on the object

        _waitingForReturn = false; // Allow future returns
    }
}
