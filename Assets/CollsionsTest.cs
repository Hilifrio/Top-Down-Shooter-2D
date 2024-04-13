using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CollsionsTest : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask collisionMask;
    public LayerMask targetMask;
    public int moveSpeed = 0;
    public int nbRay;

    public int lookAhead = 100;

    GameObject player;
    Direction[] directions;

    float[] interestMap;
    float[] dangerMap;

    private Rigidbody2D rb;

    Vector2 currentDirection;

    // Update is called once per frame
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        DrawRays();

    }
    void Update()
    {
        DrawRays();
        setDanger();
        setInterest();
        ChooseDirection();
        Move();
    }
    void DrawRays()
    {
        Vector2 targetPositon = player.transform.position;
        var directionToPlayer = player.transform.position - transform.position;

        Debug.DrawRay(transform.position, directionToPlayer * 6f, Color.yellow);

        directions = new Direction[nbRay];
        interestMap = new float[nbRay];
        dangerMap = new float[nbRay];

        for (int i = 0; i < nbRay; i++)
        {
            Direction d = new Direction();
            var radians = i * 2 * Mathf.PI / nbRay;
            var x = Mathf.Sin(radians);
            var y = Mathf.Cos(radians);
            Vector2 dir = new Vector2(x, y);

            d.direction = dir;
            d.target = directionToPlayer;
            d.debugRays(transform.position, collisionMask);
            dangerMap[i] = d.setDanger(transform.position, collisionMask);
            interestMap[i] = d.setInterest();

            directions[i] = d;
        }
    }

    void ChooseDirection()
    {
        //h is for the lowest value in the danger map
        currentDirection = Vector2.zero;

        for (int i = 0; i < nbRay; i++)
        {
            currentDirection += directions[i].direction * interestMap[i];
        }
        currentDirection.Normalize();
    }

    void setDanger()
    {
        for (int i = 0; i < nbRay; i++)
        {
            var result = directions[i].setDanger(transform.position, collisionMask);
            dangerMap[i] = result;
        }
    }

    void setInterest()
    {
        for(int i = 0; i<nbRay; i++)
        {
            var d = directions[i].setInterest();
            if (dangerMap[i] == 1)
                d = 0;
            interestMap[i] = Mathf.Max(0, d);
        }
    }

    void Move()
    { 
        rb.MovePosition(rb.position + currentDirection * moveSpeed * Time.fixedDeltaTime);
        Debug.Log("Moving to: " + currentDirection);
    }
}
