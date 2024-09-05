using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiruclarSpectrumScript : MonoBehaviour
{
    public float rotationSpeedX = 10f;
    public float rotationSpeedY = 10f;
    public float rotationSpeedZ = 10f;

    public float rotationLimitX = 22.5f;
    public float rotationLimitY = 22.5f;
    public float rotationLimitZ = 22.5f;

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private float currentRotationZ = 0f;

    private int directionX = 1;
    private int directionY = 1;
    private int directionZ = 1;

    private const float rotationLimit = 22.5f;

    // Update is called once per frame
    void Update()
    {
        // Calculate the new rotation for each axis
        float deltaRotationX = rotationSpeedX * Time.deltaTime * directionX;
        float deltaRotationY = rotationSpeedY * Time.deltaTime * directionY;
        float deltaRotationZ = rotationSpeedZ * Time.deltaTime * directionZ;

        // Apply the rotation to the object
        transform.Rotate(deltaRotationX, deltaRotationY, deltaRotationZ);

        // Update the current rotation
        currentRotationX += deltaRotationX;
        currentRotationY += deltaRotationY;
        currentRotationZ += deltaRotationZ;

        // Check if the rotation exceeds 22.5 degrees and reverse the direction if necessary
        if (Mathf.Abs(currentRotationX) >= rotationLimitX)
        {
            directionX *= -1; // Reverse direction
            currentRotationX = Mathf.Clamp(currentRotationX, -rotationLimitX, rotationLimitX); // Clamp to avoid overshooting
        }

        // Disable for ring rotation
        // if (Mathf.Abs(currentRotationY) >= rotationLimitY)
        // {
        //     directionY *= -1; // Reverse direction
        //     currentRotationY = Mathf.Clamp(currentRotationY, -rotationLimitY, rotationLimitY); // Clamp to avoid overshooting
        // }

        if (Mathf.Abs(currentRotationZ) >= rotationLimitZ)
        {
            directionZ *= -1; // Reverse direction
            currentRotationZ = Mathf.Clamp(currentRotationZ, -rotationLimitZ, rotationLimitZ); // Clamp to avoid overshooting
        }
    }
}
