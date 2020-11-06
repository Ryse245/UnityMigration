using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator2D : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void updateForces()
    {

    }
}

public class SpringForceGenerator : ForceGenerator2D
{
    [SerializeField]
    GameObject firstObj, secondObj; //God I hate naming things

    [SerializeField]
    float springConstant, springLength;

    public override void updateForces()
    {

    }
}
public class BuoyancyForceGenerator : ForceGenerator2D
{
    [SerializeField]
    GameObject mTarget;

    [SerializeField]
    float mMaximumDepth, mVolume, mWaterHeight, mLiquidDensity;

    public override void updateForces()
    {

    }
}