using UnityEngine;

[SelectionBase]
public class GlassShatter : MonoBehaviour
{
    [SerializeField] GameObject intactCube;
    [SerializeField] GameObject brokenCube;

    [Tooltip("The minimum force required to break the cube.")]
    [SerializeField] float breakForceThreshold = 5f;

    BoxCollider bc;
    Rigidbody rb;

    private void Awake()
    {
        intactCube.SetActive(true);
        brokenCube.SetActive(false);

        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>(); // Ensures the object has a Rigidbody for physics interaction
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Calculate the magnitude of the collision force
        float impactForce = collision.relativeVelocity.magnitude;

        // If the impact force exceeds the threshold, break the cube
        if (impactForce >= breakForceThreshold)
        {
            Break();
        }
    }

    private void Break()
    {
        intactCube.SetActive(false);
        brokenCube.SetActive(true);

        bc.enabled = false; // Disable the BoxCollider to prevent further interaction
        rb.isKinematic = true; // Optionally disable physics on the object after breaking
    }
}
