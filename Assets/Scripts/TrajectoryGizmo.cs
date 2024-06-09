using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryGizmo : MonoBehaviour
{
    public Transform startPoint; // Reference to the start point Transform
    public Transform endPoint;   // Reference to the end point Transform
    public int numberOfSegments = 20; // Number of segments to display
    public float offsetDistance = 1.0f; // Distance to offset from the startPoint

    void OnDrawGizmos()
    {
        if (startPoint == null || endPoint == null)
        {
            return;
        }

        // Calculate direction from startPoint to endPoint
        Vector3 direction = (endPoint.position - startPoint.position).normalized;

        // Calculate a perpendicular vector to the direction
        Vector3 perpendicularDirection = Vector3.Cross(direction, Vector3.up).normalized;

        // Calculate the control point for the Bezier curve
        Vector3 controlPoint = startPoint.position + perpendicularDirection * offsetDistance;

        // Set the color of the gizmos
        Gizmos.color = Color.red;

        // Draw spheres along the Bezier curve trajectory
        for (int i = 0; i <= numberOfSegments; i++)
        {
            float t = (float)i / numberOfSegments;
            Vector3 point = CalculateQuadraticBezierPoint(t, startPoint.position, controlPoint, endPoint.position);
            Gizmos.DrawSphere(point, 0.5f); // Draw sphere with radius 0.1
        }
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
}