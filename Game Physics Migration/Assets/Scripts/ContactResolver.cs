using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ContactResolver : MonoBehaviour
{
    public static ContactResolver instance;
    [SerializeField]
    int maxIterations;

    int numIterationsUsed;

    public void resolveContacts(ref List<Particle2DContact> contacts, float dt)
    {
        numIterationsUsed = 0;

        while(numIterationsUsed < maxIterations)
        {
            float max = float.MaxValue;
            int numContacts = contacts.Count;
            int maxIndex = numContacts;

            for(int i = 0; i < numContacts; i++)
            {
                float sepVel = contacts[i].calculateSeparatingVelocity();
                if(sepVel < max && (sepVel < 0.0f || contacts[i].penetration > 0.0f))
                {
                    max = sepVel;
                    maxIndex = i;
                }
            }

            if (maxIndex == numContacts)
                break;


            contacts[maxIndex].resolve(dt);

            for(int i = 0; i < numContacts; i++)
            {
                if(contacts[i].firstContact == contacts[maxIndex].firstContact)
                {
                    contacts[i].penetration -= Vector3.Dot(contacts[maxIndex].firstMove, contacts[i].contactNormal);
                }
                else if (contacts[i].firstContact == contacts[maxIndex].secondContact)
                {
                    contacts[i].penetration -= Vector3.Dot(contacts[maxIndex].secondMove, contacts[i].contactNormal);
                }

                if(contacts[i].secondContact)
                {
                    if (contacts[i].secondContact == contacts[maxIndex].firstContact)
                    {
                        contacts[i].penetration += Vector3.Dot(contacts[maxIndex].firstMove, contacts[i].contactNormal);
                    }
                    else if (contacts[i].secondContact == contacts[maxIndex].secondContact)
                    {
                        contacts[i].penetration -= Vector3.Dot(contacts[maxIndex].secondMove, contacts[i].contactNormal);
                    }
                }
            }

            numIterationsUsed++;
        }


    }
}
