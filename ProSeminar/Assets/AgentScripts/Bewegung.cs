using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Reynolds proposed the use of multiple simple behaviors to define the building blocks of a more complex steering agent.
//Each of these steering behaviors observes the agent's local environment and returns VELOCITIES, consisting of a DIRECTION VECTOR and a MAGNITUDE, to react to the current situation.
public class Bewegung
{
    public Vector3 direction;
    public float speed;
}
