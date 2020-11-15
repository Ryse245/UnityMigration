using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLifetime : MonoBehaviour
{
    public float maxLifetime = 5.0f;
    float currentTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (currentTime >= maxLifetime)
        {
            Destroy(this.gameObject);
        }
        else currentTime += Time.deltaTime;
    }
}
