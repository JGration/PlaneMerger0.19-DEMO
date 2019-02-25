using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour 
{
    public Transform target;

    private float smoothSpeed = 10f;
    public Vector3 offset;
    public Vector3 offsetback;
    public Vector3 offsetfront;
    private Vector3 velocity = Vector3.zero;
    private float PlayerHSpeed;

    void Update()
    {
        PlayerHSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().checkRbVelocityY;
        StartCoroutine(CameraSpeed());
    }

    IEnumerator CameraSpeed()
    {
        if (PlayerHSpeed < -15)
            while (smoothSpeed != 0)
            {
                smoothSpeed--;
                yield return new WaitForSeconds(1f);
            }
        if (PlayerHSpeed > -15)
            while (smoothSpeed != 10)
            {
                smoothSpeed++;
                yield return new WaitForSeconds(0.2f);
            }     
        yield return 0;
    }

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
