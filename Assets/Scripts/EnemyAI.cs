using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Pathfinding")]
    public Transform target;
    public float activationDistance = 50f;
    public float pathUpdateTime = .5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpHeight = .8f;
    public float jumpModifier = .3f;
    public float jumpCheckOffset = .1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;


    private Path path;
    private int currentWaypoint = 0;
    bool isGrounded =  false;
    Seeker seeker;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateTime);
    }

    private bool targetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activationDistance;
    }

    private void onPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void PathFollow()
    {
        // check for existence of path
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }


        //check if on ground
        Vector3 startOffset = transform.position - new Vector3(0, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);
        print(currentWaypoint );
        print(path.vectorPath.Count);

        //get position and direction
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //check if it needs to jump
        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpHeight)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }

        //move
        rb.AddForce(force);

        //sets next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        /*
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new ve
            }
        }*/
    }

    private void FixedUpdate()
    {
        if (targetInDistance() && followEnabled)
            PathFollow();
    }

    private void UpdatePath()
    {
        if (targetInDistance() && followEnabled && seeker.IsDone())
            seeker.StartPath(rb.position, target.position, onPathComplete);
    }
}
