using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    [SerializeField]
    float mMass, mDamping;

    [SerializeField]
    Vector3 mVelocity, mAcceleration, mAccumulatedForces;

    [SerializeField]
    bool shouldIgnoreForces = false;

    public void CreateParticle2D(float mass, float damping, float speed, Vector3 direction, Vector3 gravity)
    {
        mMass = mass;
        mDamping = damping;
        mVelocity = direction * speed;
        mAcceleration = gravity;

        GameManager.instance.particleArray.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(!shouldIgnoreForces)
        {
            ForceManager.instance.ApplyAllForces();
            Integrate();
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.particleArray.Remove(this);
    }

    public void AddForce(Vector3 force)
    {
        mAccumulatedForces += force;
    }

    public void ClearAccumulated()
    {
        mAccumulatedForces = Vector3.zero;
    }

    void Integrate()
    {
        ParticleIntegrator.Integrate(this.GetComponent<Particle2D>(), Time.deltaTime);
    }

    #region Getters/Setters

    /// <summary>
    /// Return a reference to the particle's velocity variable, should work like a C++ pointer.
    /// This allows us to get the variable AND set it at the same time, in theory
    /// </summary>
    /// <returns></returns>
    public ref Vector3 getVelocityRef()
    {
        return ref mVelocity;
    }

    public Vector3 getVelocity()
    {
        return mVelocity;
    }

    public void setVelocity(Vector3 vel)
    {
        mVelocity = vel;
    }

    public Vector3 getAcceleration()
    {
        return mAcceleration;
    }

    public Vector3 getAccumulatedForces()
    {
        return mAccumulatedForces;
    }

    public float getMass()
    {
        return mMass;
    }

    public float getInverseMass()
    {
        return 1.0f / mMass;
    }

    public float getDamping()
    {
        return mDamping;
    }

    public void setShouldIgnoreForces(bool shouldIgnore)
    {
        shouldIgnoreForces = shouldIgnore;
    }

    public bool getShouldIgnoreForces()
    {
        return shouldIgnoreForces;
    }
    #endregion
}