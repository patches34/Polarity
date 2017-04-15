using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Polarity
{
    Positive = 1,
    Negative = -1
}
public class MagnetConstantForce : MonoBehaviour
{
    public float force;
    public Polarity polarity;
    Vector3 direction;

    private void Update()
    {
        direction = transform.up;
    }

    public Vector3 GetForce()
    {
        return direction * (force * (int)polarity);
    }
}
