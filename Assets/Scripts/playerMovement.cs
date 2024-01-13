using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    [SerializeField] float playerSpeed = 10;
    [SerializeField] float jumpHeight = 10;

    public GameObject player;

    private CapsuleCollider2D collider;
    private Rigidbody2D rbody;

    private Vector2 playerMove;
    private Vector2 height;
    
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider2D>();
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    
    private void FixedUpdate()
    {
        
        if (Mathf.Abs(Input.GetAxis("walk")) > 0.1)
        {
            playerMove = new Vector2(Input.GetAxis("walk") * playerSpeed, 0);
        }
        else
        {
            playerMove = Vector2.zero;
        }
        print(collider.IsTouchingLayers(ground));

        if (Input.GetAxis("jump") > 0.1 && collider.IsTouchingLayers(ground))
        {
            rbody.AddForce(new Vector2(0, jumpHeight));
        }
        
        if (Input.GetAxis("jump") < -0.1)
        {
            collider.size = new Vector2(1, 1);
        }
        else
        {
            collider.size = new Vector2(1, 2);
        }
        
        rbody.velocity = playerMove;
    }

    
}
