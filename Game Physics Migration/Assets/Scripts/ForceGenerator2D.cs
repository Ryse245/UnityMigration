using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator2D : MonoBehaviour
{
    protected bool shouldAffectAll = true;

    public bool getShouldAffectAll() { return shouldAffectAll; }

    public virtual void updateForces(Particle2D particle, float dt)
    {

    }
}

public class PointForceGenerator : ForceGenerator2D
{
    Vector3 point;
    float magnitude;

    public override void updateForces(Particle2D particle, float dt)
    {
        Vector3 diff = point - particle.transform.position;
        float range = 1000;
        float rangeSQ = 1000 * 1000;
        float distSQ = diff.sqrMagnitude;

        if(distSQ < rangeSQ)
        {
            float dist = diff.magnitude;
            float proportionAway = dist / range;
            proportionAway = 1 - proportionAway;
            diff.Normalize();

            //add force
        }
    }
}

public class SpringForceGenerator : ForceGenerator2D
{
    [SerializeField]
    GameObject firstObj, secondObj; //God I hate naming things

    [SerializeField]
    float springConstant, springLength;

    public override void updateForces(Particle2D particle, float dt)
    {
        Vector3 pos1 = firstObj.transform.position;
        Vector3 pos2 = secondObj.transform.position;

        Vector3 diff = pos1 - pos2;
        float dist = diff.magnitude;

        float magnitude = dist - springLength;

        magnitude *= springConstant;

        diff.Normalize();
        diff *= magnitude;

        //add force "diff" to firstObj
        firstObj.GetComponent<Particle2D>().AddForce(diff);
        //add negative diff to secondObj
        firstObj.GetComponent<Particle2D>().AddForce(-diff);
    }
}
public class BuoyancyForceGenerator : ForceGenerator2D
{
    [SerializeField]
    GameObject mTarget;

    [SerializeField]
    float mMaximumDepth, mVolume, mWaterHeight, mLiquidDensity;

    public override void updateForces(Particle2D particle, float dt)
    {
        Vector3 force;
        float depth = mTarget.transform.position.y;

        // check if out of water
        if(depth <= mWaterHeight + mMaximumDepth)
        {
            return;
        }

        if(depth >= mWaterHeight + mMaximumDepth)
        {
            force.y = mLiquidDensity * mVolume;
            //add negative(?) force to target (that's what i have for dean)
            return;
        }
        else if(depth >= mWaterHeight)
        {
            float buoyancy = mLiquidDensity * mVolume * (depth - mMaximumDepth - mWaterHeight) / (2 * mMaximumDepth);
            force.y = buoyancy;
            //add force to target
            return;
        }
    }
}

public class AnchoredBungieForceGenerator : ForceGenerator2D
{
    [SerializeField]
    GameObject mTarget;

    [SerializeField]
    Vector3 mAnchor;

    [SerializeField]
    float mRestLength, mSpringConstant;

    public override void updateForces(Particle2D particle, float dt)
    {
        Vector3 force;

        force = mAnchor - mTarget.transform.position;

        float magnitude = force.magnitude;

        if(magnitude <= mRestLength)
        {
            return;
        }

        magnitude = mSpringConstant * (mRestLength - magnitude);

        force.Normalize();
        force *= -magnitude;

        //add force to target
    }
}