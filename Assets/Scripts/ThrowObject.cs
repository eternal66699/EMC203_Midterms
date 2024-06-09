using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    public Transform startPoint; // Start point of the throw
    public Transform endPoint;   // End point of the throw
    public float throwDuration = 1f; // Duration of the throw in seconds
    public float offsetDistance = 1.0f; // Distance to offset from the startPoint

    private float throwTimer = 0f;
    private bool isThrowing = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isThrowing)
        {
            StartThrow();
        }

        if (isThrowing)
        {
            ThrowObjectAlongPath();
        }
    }

    void StartThrow()
    {
        isThrowing = true;
        throwTimer = 0f;
    }

    void ThrowObjectAlongPath()
    {
        throwTimer += Time.deltaTime;

        // Calculate interpolation factor based on throw duration
        float t = Mathf.Clamp01(throwTimer / throwDuration);

        // Calculate the current position on the loop
        Vector3 bezierPosition;
        if (t <= 0.5f)
        {
            // First half of the loop: startPoint to endPoint
            float t1 = t * 2;
            bezierPosition = CalculateBezierLoopPoint(t1, startPoint.position, endPoint.position, true);
        }
        else
        {
            // Second half of the loop: endPoint back to startPoint
            float t2 = (t - 0.5f) * 2;
            bezierPosition = CalculateBezierLoopPoint(t2, endPoint.position, startPoint.position, false);
        }

        // Move the object to the Bezier curve position
        transform.position = bezierPosition;

        // Check if throw is finished
        if (throwTimer >= throwDuration)
        {
            isThrowing = false;
        }
    }

    // Function to calculate a point on a quadratic Bezier curve forming a loop
    Vector3 CalculateBezierLoopPoint(float t, Vector3 p0, Vector3 p2, bool forward)
    {
        // Calculate direction from p0 to p2
        Vector3 direction = (p2 - p0).normalized;

        // Calculate a perpendicular vector to the direction
        Vector3 perpendicularDirection = Vector3.Cross(direction, Vector3.up).normalized;

        // Calculate the control point for the Bezier curve
        Vector3 controlPoint;
        if (forward)
        {
            controlPoint = p0 + perpendicularDirection * offsetDistance;
        }
        else
        {
            controlPoint = p2 + perpendicularDirection * offsetDistance;
        }

        // Calculate the position on the Bezier curve
        return CalculateQuadraticBezierPoint(t, p0, controlPoint, p2);
    }

    // Function to calculate a point on a quadratic Bezier curve
    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; // (1-t)^2 * P0
        p += 2 * u * t * p1; // 2(1-t)t * P1
        p += tt * p2;        // t^2 * P2

        return p;
    }

    /*void OnDrawGizmos()
    {
        if (startPoint != null && endPoint != null)
        {
            Gizmos.color = Color.green;
            Vector3 previousPoint = startPoint.position;
            for (int i = 1; i <= 40; i++) // Increased steps for a smoother loop
            {
                float t = i / 40f;
                Vector3 currentPoint;
                if (t <= 0.5f)
                {
                    float t1 = t * 2;
                    currentPoint = CalculateBezierLoopPoint(t1, startPoint.position, endPoint.position, true);
                }
                else
                {
                    float t2 = (t - 0.5f) * 2;
                    currentPoint = CalculateBezierLoopPoint(t2, endPoint.position, startPoint.position, false);
                }
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }
        }
    }*/
}