using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float rotationSpeed = 15.0f;
    Vector3 projGravity = new Vector3(0.0f, -20.0f, 0.0f);

    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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

        if(Input.GetKeyDown(KeyCode.Return))
        {
            GameObject currentProj = Instantiate(projectilePrefab);
            currentProj.transform.position = transform.position;
            currentProj.GetComponent<Particle2D>().CreateParticle2D(10.0f, 0.99f, 50.0f, transform.right, projGravity);
        }
    }
}
