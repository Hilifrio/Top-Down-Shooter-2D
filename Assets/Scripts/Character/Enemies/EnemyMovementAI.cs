using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyMovementAI : MonoBehaviour
{
    public Transform target;

    //how many time each second we will updated our path
    public float updateRate = 2f;
    Vector2[] Rays = new Vector2[3];

    //caching 
    private Seeker seeker;
    private Rigidbody2D rb;

    //the calculated path;
    public Path path;

    //Tha aI speed per second
    public float speed = 3f;
    private float baseSpeed;

    [HideInInspector]
    public bool pathIsEnded = false;

    public bool canMove = true;
    [Header("Avoidance")]
    public bool localAvoidance = false;
    public bool isAvoiding = false;
    public LayerMask avoidingMask;
    private Vector3 offset;

    //the max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    //The way Point we are currently moving toward
    private int currentWayPoint = 0;
    private Vector2 dir;
    private bool searchingForPlayer = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        baseSpeed = speed;
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        //Start a next path to the target position and return the result to the OnPathComplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            searchingForPlayer = false;
            target = sResult.transform;
            StartCoroutine(UpdatePath());
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            //TODO: insert a player search here.
        }
        if(canMove)
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            
        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    void LocalAvoidance()
    {
        UpdateRays();
        foreach(Vector2 r in Rays)
        {
            Color c = Color.green;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, r, 1f, avoidingMask);
            if (hit.collider != null)
            {
                c = Color.red;
                isAvoiding = true;
                //dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
                if (r == Rays[0])
                    speed /= 3;
                else if (r == Rays[1])
                    dir = Rays[2];
                else
                    dir = Rays[1];
            }
            else
            {
                dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
                speed = baseSpeed;
                isAvoiding = false;
            }
                
            Debug.DrawRay(transform.position + offset, r * 1f, c);
        }
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
            foreach (GraphNode n in p.path)
            {
                //Set all the nodes with higher penalty, causes agents to spread out
                if(canMove)
                    n.Penalty = 1500;
            }
        }
    }

    void FixedUpdate()
    {

        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        //TODO: Always look at player ?
        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //Direction to the next Waypoint
        if(canMove)
        {
            Vector2 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
            dir *= speed * Time.fixedDeltaTime;

            //Move the AI
            //rb.AddForce(dir, fMode);
            rb.MovePosition(rb.position + dir);

            float dist = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);
            if (dist < nextWaypointDistance)
            {
                currentWayPoint++;
                return;
            }

        }

    }

    void UpdateRays()
    {
        Vector2 moveDir = dir.normalized;
        float alpha = Mathf.Deg2Rad * 45;
        Rays[0] = moveDir;
        Rays[1] = new Vector2(moveDir.x * Mathf.Cos(alpha) + moveDir.y * Mathf.Sin(alpha), -moveDir.x * Mathf.Sin(alpha) + moveDir.y * Mathf.Cos(alpha));
        Rays[2] = new Vector2(moveDir.x * Mathf.Cos(alpha) - moveDir.y * Mathf.Sin(alpha),  moveDir.x * Mathf.Sin(alpha) + moveDir.y * Mathf.Cos(alpha));

        //DEBUG SECTION
        //new Vector3(moveDir.x, moveDir.y, 0)
        offset = new Vector3(moveDir.x / 1.5f, moveDir.y / 1.5f, 0);

        /*
        Debug.DrawRay(transform.position + offset, moveDir * 1f, Color.green);
        Debug.DrawRay(transform.position + offset, new Vector2(moveDir.x * Mathf.Cos(alpha) + moveDir.y * Mathf.Sin(alpha),
                                                     -moveDir.x * Mathf.Sin(alpha) + moveDir.y * Mathf.Cos(alpha)) * 1f, Color.red);
        Debug.DrawRay(transform.position + offset, new Vector2(moveDir.x * Mathf.Cos(alpha) - moveDir.y * Mathf.Sin(alpha),
                                                      moveDir.x * Mathf.Sin(alpha) + moveDir.y * Mathf.Cos(alpha)) * 1f, Color.red);
        */
    }
}