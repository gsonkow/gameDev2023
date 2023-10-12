using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGrappler : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform grappler, playerCamera, player;
    public float maxDistance;
    private SpringJoint joint;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);


            joint.maxDistance = distanceFromPoint * 0.4f;
            joint.minDistance = distanceFromPoint * 0.1f;

            joint.spring = 8f;
            joint.damper = 3.5f;
            joint.massScale = 8f;

            lr.positionCount = 2;
        }

    }

    void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, grappler.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }
}
