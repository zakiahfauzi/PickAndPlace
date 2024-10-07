// Create a Particle System
// Set a 5 second start delay for the system, and a 2 second lifetime for each particle
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ExampleClass : MonoBehaviour
{
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var main = ps.main;

        main.startDelay = 5.0f;
        main.startLifetime = 2.0f;
    }
}
