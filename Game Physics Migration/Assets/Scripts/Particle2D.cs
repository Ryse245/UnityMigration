using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    [SerializeField]
    float mass, damping;

    [SerializeField]
    Vector3 velocity, acceleration, accumulatedForces;

    [SerializeField]
    bool shouldIgnoreForces = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Getters/Setters

    /// <summary>
    /// Return a reference to the particle's velocity variable, should work like a C++ pointer.
    /// This allows us to get the variable AND set it at the same time, in theory
    /// </summary>
    /// <returns></returns>
    public ref Vector3 getVelocityRef()
    {
        return ref velocity;
    }

    public Vector3 getAcceleration()
    {
        return acceleration;
    }

    public Vector3 getAccumulatedForces()
    {
        return accumulatedForces;
    }

    public float getMass()
    {
        return mass;
    }

    public float getInverseMass()
    {
        return 1.0f / mass;
    }

    public float getDamping()
    {
        return damping;
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