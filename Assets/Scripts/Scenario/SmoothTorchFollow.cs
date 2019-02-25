using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTorchFollow : MonoBehaviour 
{
    public Transform target;

    public float smoothSpeed = 10f;
    public Vector3 offset;
    public Vector3 offsetback;
    public Vector3 offsetfront;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed * Time.deltaTime); //* Time.deltaTime
        transform.position = smoothedPosition;

        if (!OnePlayerMovement.playerdeath)
        {
            float x = Input.GetAxis("Horizontal");
            if (x < 0)
            {
                offset = offsetback;
            }
            if (x > 0)
            {
                offset = offsetfront;
            }

        }    
    }



}
