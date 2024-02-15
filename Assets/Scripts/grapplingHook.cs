using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplingHook : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float var_GrapplingStrength = 2;

    [Header("Components")]
    [SerializeField] LineRenderer comp_LineRenderer;

    [Header("Game Objects")]
    [SerializeField] Transform obj_Grappling_Hook;
    [SerializeField] Camera obj_PlayerCamera;

    [Header("Layers")]
    [SerializeField] LayerMask layer_Ground;
    private Vector3 pvar_WorldPosition;
    private RaycastHit2D pvar_Hit;

    [Header("Scripts")]
    [SerializeField] playerMovement script_PlayerMovement;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = obj_PlayerCamera.nearClipPlane;
        pvar_WorldPosition = obj_PlayerCamera.ScreenToWorldPoint(mousePosition);

        float rotation = Mathf.Rad2Deg * Mathf.Atan2((pvar_WorldPosition.y - transform.position.y), (pvar_WorldPosition.x - transform.position.x));
        obj_Grappling_Hook.localEulerAngles = new Vector3(0, 0, rotation);
        
        comp_LineRenderer.SetPosition(0, transform.position);
        pvar_Hit = Physics2D.Raycast(transform.position, new Vector2((pvar_WorldPosition.x - transform.position.x), (pvar_WorldPosition.y - transform.position.y)), Mathf.Infinity, layer_Ground);

        if (Input.GetMouseButtonDown(0))
        {
            script_PlayerMovement.var_GrapplingVelocityOverride = new Vector2(var_GrapplingStrength * (pvar_Hit.point.x - transform.position.x), var_GrapplingStrength * (pvar_Hit.point.y - transform.position.y));
            comp_LineRenderer.SetPosition(1, pvar_Hit.point);
        }

        if (Input.GetMouseButton(0))
        {
            
            comp_LineRenderer.enabled = true; 
        }
        else
        {
            script_PlayerMovement.var_GrapplingVelocityOverride = Vector2.zero;
            comp_LineRenderer.enabled = false;
            comp_LineRenderer.SetPosition(1, transform.position);
        }

        
    }
}
