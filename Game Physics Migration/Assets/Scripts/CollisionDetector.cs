using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public static bool CheckForCollision(Particle2D obj1, Particle2D obj2)
    {
        if(obj1!=null && obj2!= null)
        {
            return (Vector3.Distance(obj1.transform.position, obj2.transform.position) - obj1.transform.localScale.y) <= 0.0f;
        }
        return false;
    }
}
