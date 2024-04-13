using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    public Vector2 direction = Vector2.zero;
    public Vector2 target = Vector2.zero;
    public float weight = 0;
    LayerMask collisionMask;
    LayerMask targetMask;
    public void checkCollisions(Vector2 pos, LayerMask collisionMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, direction, 10f, collisionMask);
        if (hit.collider != null && hit.collider.GetComponent<Player>() == null)
        {
            weight = -1;
        }
    }

    float DotAngle(Vector2 a, Vector2 b)
    {
        return Vector2.Dot(a.normalized, b.normalized);
    }

    public float setInterest()
    {
        return DotAngle(target.normalized, direction.normalized);
    }

    public float setDanger(Vector2 pos, LayerMask collisionMask)
    {
        float danger = 0;
        RaycastHit2D hit = Physics2D.Raycast(pos, direction, 10f, collisionMask);
        if (hit.collider != null && hit.collider.GetComponent<Player>() == null)
        {
            danger = 1;
        }
        return danger;
    }

    public void debugRays(Vector2 pos, LayerMask collisionMask)
    {
        float danger = setDanger(pos, collisionMask);

        if (danger == 1)
            Debug.DrawRay(pos, direction * 5f, Color.red);
        else if(setInterest()>= .9f)
            Debug.DrawRay(pos, direction * 5f, Color.blue);
        else
            Debug.DrawRay(pos, direction * 5f, Color.green);
    }       

}
