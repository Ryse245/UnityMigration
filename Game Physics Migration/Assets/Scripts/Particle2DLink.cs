using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2DLink : MonoBehaviour
{
    [SerializeField]
    protected Particle2D mObject1, mObject2;

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
    private float mMaxLength;

    public override void createContacts(ref List<Particle2DContact> contacts)
    {
        // ben my rod code is bad please help
    }
}