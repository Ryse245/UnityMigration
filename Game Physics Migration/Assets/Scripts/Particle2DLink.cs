using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Particle2DLink : MonoBehaviour
{
    [SerializeField]
    protected Particle2D mObject1, mObject2;

    public bool stillExists()
    {
        if (mObject1.Equals(null) || mObject2.Equals(null)) return false;
        else return true;
    }

    protected float getCurrentLength()
    {
        return Vector3.Distance(mObject1.transform.position, mObject2.transform.position);
    }


    public virtual void createContacts(ref List<Particle2DContact> contacts)
    {
    }
}

public class ParticleCable : Particle2DLink
{
    [SerializeField]
    private float mMaxLength, mRestitution;

    public override void createContacts(ref List<Particle2DContact> contacts)
    {
        if (mObject1 == null || mObject2 == null) return;

        float length = getCurrentLength();
        if (length < mMaxLength)
            return;

        Vector3 normal = mObject2.transform.position - mObject2.transform.position;
        normal.Normalize();

        float penetration = length - mMaxLength;

        Particle2DContact contact = new Particle2DContact(mObject1.gameObject, mObject2.gameObject, normal, mRestitution, penetration);

        contacts.Add(contact);
    }
}

public class ParticleRod : Particle2DLink
{
    [SerializeField]
    private float mLength;

    public void InstantiateVariables(Particle2D obj1, Particle2D obj2, float length)
    {
        mObject1 = obj1;
        mObject2 = obj2;
        mLength = length;
    }

    public override void createContacts(ref List<Particle2DContact> contacts)
    {
        if (mObject1 == null || mObject2 == null) return;    //Patch job, need to actually remove contact from list

        float currentLength = getCurrentLength();
        if (currentLength == mLength) return;

        Vector3 normal = mObject2.transform.position - mObject1.transform.position;
        normal.Normalize();

        float penetration = (currentLength - mLength)/10.0f;

        if(currentLength > mLength)
        {
            Particle2DContact newContact = new Particle2DContact(mObject1.gameObject, mObject2.gameObject, normal, 0.0f, penetration);
            contacts.Add(newContact);
        }
        else if(currentLength < mLength)
        {
            Particle2DContact newContact = new Particle2DContact(mObject1.gameObject, mObject2.gameObject, (normal*-1.0f), 0.0f, -penetration);
            contacts.Add(newContact);
        }
    }
}