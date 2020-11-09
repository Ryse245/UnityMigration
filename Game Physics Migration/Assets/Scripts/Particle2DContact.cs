using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2DContact
{
    public GameObject firstContact, secondContact; //AAAAAAAAAAAAA NAMING
    public Vector3 contactNormal, firstMove, secondMove;   //Not sure about the "moves" tbh, but Dean says we'd need them
    public float restitutionCoefficient, penetration;  //( ͡° ͜ʖ ͡°)


    public Particle2DContact(GameObject first, GameObject second, Vector3 normal, float rest, float pen)
    {
        firstContact = first;
        secondContact = second;
        contactNormal = normal;
        restitutionCoefficient = rest;
        penetration = pen;
    }

    public float calculateSeparatingVelocity()
    {
        Vector3 relativeVel = firstContact.GetComponent<Particle2D>().getVelocity();

        if(secondContact)
        {
            relativeVel -= secondContact.GetComponent<Particle2D>().getVelocity();
        }

        return Vector3.Dot(relativeVel, contactNormal);
    }

    public void resolve(float dt)
    {
        resolveVelocity(dt);
        resolveInterpenetration(dt);
    }

    public void resolveVelocity(float dt)
    {
        Particle2D firstParticle = firstContact.GetComponent<Particle2D>();
        Particle2D secondParticle = secondContact.GetComponent<Particle2D>();

        float separatingVel = calculateSeparatingVelocity();
        if (separatingVel > 0.0f)
            return;

        float newSepVel = -separatingVel * restitutionCoefficient;

        Vector3 velFromAcc = firstParticle.getAcceleration();
        if (secondParticle)
            velFromAcc -= secondParticle.getAcceleration();

        float accCausedSepVelocity = Vector3.Dot(velFromAcc, contactNormal) * dt;

        if(accCausedSepVelocity < 0.0f)
        {
            newSepVel += restitutionCoefficient * accCausedSepVelocity;
            if (newSepVel < 0.0f)
                newSepVel = 0.0f;
        }

        float deltaVel = newSepVel - separatingVel;

        float totalInverseMass = firstParticle.getInverseMass();
        if (secondParticle)
            totalInverseMass += secondParticle.getInverseMass();

        if (totalInverseMass <= 0)  //infinite mass objects
            return;

        float impulse = deltaVel / totalInverseMass;
        Vector3 impulsePerIMass = contactNormal * impulse;

        Vector3 newVelocity = firstParticle.getVelocity() + impulsePerIMass * firstParticle.getInverseMass();
        firstParticle.setVelocity(newVelocity);

        if(secondParticle)
        {
            newVelocity = secondParticle.getVelocity() + impulsePerIMass * -secondParticle.getInverseMass();
            secondParticle.setVelocity(newVelocity);
        }

    }

    public void resolveInterpenetration(float dt)
    {
        Particle2D firstParticle = firstContact.GetComponent<Particle2D>();
        Particle2D secondParticle = secondContact.GetComponent<Particle2D>();

        if (penetration <= 0.0f)
            return;

        float totalInverseMass = firstParticle.getInverseMass();
        if (secondParticle)
            totalInverseMass += secondParticle.getInverseMass();

        if (totalInverseMass <= 0)  //infinite mass objects
            return;

        Vector3 movePerIMass = contactNormal * (penetration / totalInverseMass);

        firstMove = movePerIMass * firstParticle.getInverseMass();
        if (secondParticle)
            secondMove = movePerIMass * -secondParticle.getInverseMass();
        else
            secondMove = Vector3.zero;

        Vector3 newPos = firstContact.transform.position + firstMove;
        firstContact.transform.position = newPos;
        if(secondParticle)
        {
            newPos = secondContact.transform.position + secondMove;
            secondContact.transform.position = newPos;
        }
    }
}
