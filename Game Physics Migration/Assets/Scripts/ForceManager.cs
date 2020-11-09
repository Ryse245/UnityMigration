using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ForceManager : MonoBehaviour
{
    public static ForceManager instance;

    [SerializeField] List<ForceGenerator2D> forces;

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
            forces = new List<ForceGenerator2D>();
        }
    }

    public List<ForceGenerator2D> GetForces() { return forces; }

    public void AddForceGen(ForceGenerator2D force)
    {
        forces.Add(force);
    }

    public void RemoveForceGen(ForceGenerator2D force)
    {
        forces.Remove(force);
    }

    public void ApplyAllForces()
    {
        for(int i = 0; i < forces.Count; i++)
        {
            forces[i].updateForces(Time.deltaTime);
        }
    }
}
