using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2D))]
public class GravityGenerator : ForceGenerator2D
{
    public int maxInfluences = 1;
    public float maxDistance = 1000;

    Particle2D particle;
    List<GameObject> influences;

    float G = 6.67f * Mathf.Pow(10, -5);    //Changed power from -11 to speed up

    void Start()
    {
        particle = GetComponent<Particle2D>();
        influences = new List<GameObject>();

        ForceManager.instance.AddForceGen(this);
    }

    void Update()
    {
        GetInfluences();
    }

    public override void updateForces(float dt)
    {
        foreach(GameObject g in influences)
        {
            particle.AddForce(CalculateGravForce(g));
        }
    }

    private Vector3 CalculateGravForce(GameObject obj)
    {
        Vector3 result = Vector3.zero;

        float dist = Vector3.Distance(transform.position, obj.transform.position);
        float distSq = dist * dist;

        float m1 = particle.getMass();
        float m2 = obj.GetComponent<Particle2D>().getMass();

        float forceMag = (G * m1 * m2) / distSq;

        result = Vector3.Normalize((obj.transform.position - transform.position)) * forceMag;

        return result;
    }

    private void GetInfluences()
    {
        List<Particle2D> list = GameManager.instance.particleArray;

        influences.Clear();

        foreach(Particle2D p in list)
        {
            if(Vector3.Distance(gameObject.transform.position, p.transform.position) <= maxDistance)
            {
                if(influences.Count <= maxInfluences && p.gameObject != gameObject)
                {
                    influences.Add(p.gameObject);
                }
            }
        }
    }
}
