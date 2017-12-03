using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SpellParticlesMotion : MonoBehaviour {

    ParticleSystem particleSystem;

    [SerializeField]
    Vector3 spellTarget;

    [SerializeField]
    float targetForce = 1.0f;

    static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

    // Use this for initialization
    void Start () {
        particleSystem = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        var count = particleSystem.GetParticles(particles);
        Vector3 target = spellTarget;

        for (int i = 0; i < count; i++)
        {
            var particle = particles[i];
            Vector3 dp = target - particle.position;
            float distance = dp.magnitude;
            if (distance > 0.1f)
            {
                float speed = particle.velocity.magnitude;
                particle.velocity += dp * (targetForce / distance);
                particle.velocity *= speed / (particle.velocity.magnitude);

            }
            if (distance < 2.0f)
            {
                particle.velocity *= 0.9f;
            }
            particles[i] = particle;
        }

        particleSystem.SetParticles(particles, count);
    }
}
