using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2D))]
public class GravityGenerator : ForceGenerator2D
{
    public int maxInfluences = 1;
    public float maxDistance = 1000;
    public GameObject orbitBody = null;

    Particle2D particle;
    List<GameObject> influences;
    bool firstRun = true;

    float G = 6.67f * Mathf.Pow(10, -5);    //Changed power from -11 to speed up

    Vector3 g1, g2;

    void Start()
    {
        particle = GetComponent<Particle2D>();
        influences = new List<GameObject>();

        ForceManager.instance.AddForceGen(this);
    }

    void Update()
    {
        if(firstRun)
        {
            orbitBody = GetOrbitBody();

            particle.setVelocity(CalcOrbitVelocity());

            firstRun = false;
        }

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

    private GameObject GetOrbitBody()
    {
        GameObject obj = null;

        List<Particle2D> particles = new List<Particle2D>();

        int closestIndex = -1, heaviestIndex = -2;
        float minDist = float.MaxValue;
        float maxMass = float.MinValue;

        particles = GameManager.instance.particleArray;

        if (particles.Count == 2)
        {
            if(particle.getMass() >= particles[1].getMass())
                return null;
            else
            {
                return particles[1].gameObject;
            }
        }

        //while(closestIndex != heaviestIndex)
        //{
        //    int i;
        //    for (i = 0; i < particles.Count; i++)
        //    {
        //        float dist = Vector3.Distance(transform.position, particles[i].transform.position);
        //        float otherMass = particles[i].getMass();

        //        if (dist < minDist)
        //        {
        //            closestIndex = i;
        //        }

        //        if (otherMass > maxMass)
        //        {
        //            heaviestIndex = i;
        //        }
        //    }

        //    if (i == 0)
        //        continue;

        //    if(closestIndex != heaviestIndex)
        //    {
        //        particles.RemoveAt(closestIndex);
        //    }

        //}


        //obj = particles[closestIndex].gameObject;

        return obj;
    }

    private Vector3 CalcOrbitVelocity()
    {
        Vector3 result = Vector3.zero;

        Vector3 toOrbitBody = orbitBody.transform.position - transform.position;
        g1 = toOrbitBody;

        Vector3 tangentialDirection = Vector3.Cross(toOrbitBody, transform.up);

        float rVel = Mathf.Sqrt((G * particle.getMass() + orbitBody.GetComponent<Particle2D>().getMass()) / Vector3.Distance(transform.position, orbitBody.transform.position));

        rVel /= 90.0f;

        tangentialDirection.Normalize();
        g2 = tangentialDirection;

        result = tangentialDirection * rVel;

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(orbitBody.transform.position, this.transform.position);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + g2 * 10000);
    }
}
