using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool Collided { get; private set; } = false;
    void OnCollisionEnter(Collision collisionInfo)
    {
        Collided = true;
    }
}
