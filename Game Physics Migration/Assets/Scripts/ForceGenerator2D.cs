using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2D))]
public class ForceGenerator2D : MonoBehaviour
{
    protected bool shouldAffectAll = true;

    public bool getShouldAffectAll() { return shouldAffectAll; }
    public void setShouldAffectAll(bool should) { shouldAffectAll = should; }

    public virtual void updateForces(float dt)
    {

    }
}

public class PointForceGenerator : ForceGenerator2D
{
    GameObject target;
    Vector3 point;
    float magnitude;

    public override void updateForces(float dt)
    {
        Vector3 diff = point - target.transform.position;
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
            target.GetComponent<Particle2D>().AddForce(diff);
        }
    }
}

public class SpringForceGenerator : ForceGenerator2D
{
    [SerializeField]
    GameObject firstObj, secondObj; //God I hate naming things

    [SerializeField]
    float springConstant, springLength;

    public SpringForceGenerator(GameObject first, GameObject second, float springConst, float springLen)
    {
        firstObj = first;
        secondObj = second;
        springConstant = springConst;
        springLength = springLen;
    }

    public override void updateForces(float dt)
    {
        if (firstObj == null || secondObj == null) return;  //Patch job, need to actually remove force from list

        Vector3 pos1 = firstObj.transform.position;
        Vector3 pos2 = secondObj.transform.position;

        Vector3 diff = pos1 - pos2;
        float dist = diff.magnitude;

        float magnitude = dist - springLength;

        magnitude *= springConstant;

        diff.Normalize();
        diff *= magnitude;

        //add negative force "diff" to firstObj
        firstObj.GetComponent<Particle2D>().AddForce(-diff);
        //add diff to secondObj
        secondObj.GetComponent<Particle2D>().AddForce(diff);
    }
}
public class BuoyancyForceGenerator : ForceGenerator2D
{
    [SerializeField]
    GameObject mTarget;

    [SerializeField]
    float mMaximumDepth, mVolume, mWaterHeight, mLiquidDensity;

    public BuoyancyForceGenerator(GameObject target, float maxDepth, float volume, float waterHeight, float density)
    {
        mTarget = target;
        mMaximumDepth = maxDepth;
        mVolume = volume;
        mWaterHeight = waterHeight;
        mLiquidDensity = density;
    }

    public override void updateForces(float dt)
    {
        if (mTarget == null) return;  //Patch job, need to actually remove force from list
        Vector3 force = Vector3.zero;
        float depth = mTarget.transform.position.y;

        // check if out of water
        if(depth >= mWaterHeight) //+ mMaximumDepth)
        {
            return;
        }

        if(depth <= mWaterHeight - mMaximumDepth)
        {
            force.y = mLiquidDensity * mVolume;
            //add negative(?) force to target (that's what i have for dean)
            mTarget.GetComponent<Particle2D>().AddForce(force);
            return;
        }
        //else if(depth <= mWaterHeight)
        //{
            float buoyancy = mLiquidDensity * mVolume * (depth - mMaximumDepth - mWaterHeight) / (2 * mMaximumDepth);
            force.y = buoyancy;
            //add force to target
            mTarget.GetComponent<Particle2D>().AddForce(force);
            return;
        //}
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

    public override void updateForces(float dt)
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