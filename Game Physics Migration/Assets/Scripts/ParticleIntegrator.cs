using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleIntegrator
{
    public static void Integrate(Particle2D particle, float dt)
    {
        particle.gameObject.transform.position += particle.getVelocityRef() * dt;

        Vector3 resultingAccel = particle.getAcceleration();

        if(!particle.getShouldIgnoreForces())
        {
            resultingAccel += particle.getAccumulatedForces() * particle.getInverseMass();
        }

        particle.getVelocityRef() += resultingAccel * dt;

        float damping = Mathf.Pow(particle.getDamping(), dt);

        particle.getVelocityRef() *= damping;
    }
}
