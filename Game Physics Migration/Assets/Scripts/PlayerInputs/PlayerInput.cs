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

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
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

    void CreateProjectile(Projectiles projectileType, GameObject projectile)
    {
        switch(projectileType)
        {
            case Projectiles.PISTOL:
                projectile.transform.position = transform.position;
                projectile.GetComponent<Particle2D>().CreateParticle2D(10.0f, 0.99f, 50.0f, transform.right, projGravity);
                Debug.Log(currentProjectileType);
                break;

            case Projectiles.SPRINGSHOT:
                projectile.transform.position = transform.position;
                GameObject secondShot = Instantiate(projectile);
                projectile.GetComponent<Particle2D>().CreateParticle2D(10.0f, 0.99f, 50.0f, transform.right, projGravity);
                secondShot.GetComponent<Particle2D>().CreateParticle2D(10.0f, 0.99f, 75.0f, transform.right, projGravity);
                Debug.Log(currentProjectileType);
                break;

            case Projectiles.RODSHOT:
                Debug.Log(currentProjectileType);
                break;
        }
    }
}
