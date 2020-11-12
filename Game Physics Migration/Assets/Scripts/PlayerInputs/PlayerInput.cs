﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    enum Projectiles
    {
        PISTOL,
        SPRINGSHOT,
        RODSHOT,
        NUM_PROJTYPES
    }

    public float rotationSpeed = 15.0f;
    Vector3 projGravity = new Vector3(0.0f, -20.0f, 0.0f);

    Projectiles currentProjectileType;
    public GameObject projectilePrefab;

    List<Particle2DLink> particleLinks = new List<Particle2DLink>();

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        HandleContacts();
    }

    void HandleInputs()
    {
        float zAngle = transform.rotation.z;
        //Player rotation is around the Z-Axis
        if(Input.GetKey(KeyCode.Alpha1))
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.Alpha2))
        {
            transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            currentProjectileType++;
            if(currentProjectileType==Projectiles.NUM_PROJTYPES)
            {
                currentProjectileType = 0;
            }
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            GameObject currentProj = Instantiate(projectilePrefab);
            CreateProjectile(currentProjectileType, currentProj);
        }
    }

    void HandleContacts()
    {
        List<Particle2DContact> contacts = new List<Particle2DContact>();
        
        foreach(Particle2DLink link in particleLinks)
        {
            /*if (!link.stillExists())particleLinks.Remove(link);
            else*/ link.createContacts(ref contacts);
        }
        
        ContactResolver.instance.resolveContacts(ref contacts, Time.deltaTime);
    }

    void CreateProjectile(Projectiles projectileType, GameObject projectile)
    {
        switch(projectileType)
        {
            case Projectiles.PISTOL:
                projectile.transform.position = transform.position;
                projectile.GetComponent<Particle2D>().CreateParticle2D(2.0f, 0.99f, 10.0f, transform.right, projGravity);
                projectile.GetComponent<Particle2D>().setShouldIgnoreForces(false);

                BuoyancyForceGenerator buoyancy = new BuoyancyForceGenerator(projectile, projectile.transform.localScale.y / 2.0f, 1.0f, WaterObject.waterTopPos.y, 80.0f);
                ForceManager.instance.AddForceGen(buoyancy);

                projectile.GetComponent<Particle2D>().setParticleInstantiated(true);
                Debug.Log(currentProjectileType);
                break;

            case Projectiles.SPRINGSHOT:
                projectile.transform.position = transform.position;
                GameObject secondShot = Instantiate(projectile);
                Vector3 newPos = new Vector3(0.0f, 2.0f, 0.0f);
                secondShot.transform.position = newPos;

                projectile.GetComponent<Particle2D>().CreateParticle2D(2.0f, 0.99f, 10.0f, transform.right, projGravity);
                secondShot.GetComponent<Particle2D>().CreateParticle2D(2.0f, 0.99f, 25.0f, transform.right, projGravity);
                projectile.GetComponent<Particle2D>().setShouldIgnoreForces(false);
                secondShot.GetComponent<Particle2D>().setShouldIgnoreForces(false);

                SpringForceGenerator springGen = new SpringForceGenerator(projectile, secondShot, 2.0f, 0.5f);
                ForceManager.instance.AddForceGen(springGen);

                BuoyancyForceGenerator springBuoy1 = new BuoyancyForceGenerator(projectile, projectile.transform.localScale.y / 2.0f, 1.0f, WaterObject.waterTopPos.y, 80.0f);
                ForceManager.instance.AddForceGen(springBuoy1);

                BuoyancyForceGenerator springBuoy2 = new BuoyancyForceGenerator(projectile, projectile.transform.localScale.y / 2.0f, 1.0f, WaterObject.waterTopPos.y, 80.0f);
                ForceManager.instance.AddForceGen(springBuoy2);

                projectile.GetComponent<Particle2D>().setParticleInstantiated(true);
                secondShot.GetComponent<Particle2D>().setParticleInstantiated(true);

                Debug.Log(currentProjectileType);
                break;

            case Projectiles.RODSHOT:
                projectile.transform.position = transform.position;
                GameObject connectedShot = Instantiate(projectile);
                Vector3 changePos = new Vector3(0.0f, 2.0f, 0.0f);
                connectedShot.transform.position = changePos;
                projectile.GetComponent<Particle2D>().CreateParticle2D(2.0f, 0.99f, 10.0f, transform.right, projGravity);
                connectedShot.GetComponent<Particle2D>().CreateParticle2D(2.0f, 0.99f, 25.0f, transform.right, projGravity);
                projectile.GetComponent<Particle2D>().setShouldIgnoreForces(false);
                connectedShot.GetComponent<Particle2D>().setShouldIgnoreForces(false);

                ParticleRod newRod = new ParticleRod();
                newRod.InstantiateVariables(projectile.GetComponent<Particle2D>(), connectedShot.GetComponent<Particle2D>(), 10.0f);
                particleLinks.Add(newRod);
                
                BuoyancyForceGenerator rodBuoy1 = new BuoyancyForceGenerator(projectile, projectile.transform.localScale.y / 2.0f, 1.0f, WaterObject.waterTopPos.y, 80.0f);
                ForceManager.instance.AddForceGen(rodBuoy1);

                BuoyancyForceGenerator rodBuoy2 = new BuoyancyForceGenerator(projectile, projectile.transform.localScale.y / 2.0f, 1.0f, WaterObject.waterTopPos.y, 80.0f);
                ForceManager.instance.AddForceGen(rodBuoy2);

                projectile.GetComponent<Particle2D>().setParticleInstantiated(true);
                connectedShot.GetComponent<Particle2D>().setParticleInstantiated(true);
                Debug.Log(currentProjectileType);
                break;
        }
    }
}
