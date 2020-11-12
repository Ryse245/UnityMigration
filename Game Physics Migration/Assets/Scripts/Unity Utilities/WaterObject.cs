using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObject : MonoBehaviour
{
    public static Vector3 waterTopPos;

    void Start()
    {
        Vector3 offset = transform.up * (transform.localScale.y / 2f);
        waterTopPos = transform.position + offset; //This is the position of the top of the water (from center), which means we can use the y as waterheight
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(waterTopPos, 0.5f);
    }
}
