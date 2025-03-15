﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// ===================================================================================
// HOLD MOUSE TO WALK - PLAYER
// ===================================================================================
public partial class Player : Entity
{
    //change this if you need to use different timing
    private float secondsBetweenMovementRequests = 0.5f;

    //shouldn't have to change anything below this
    private bool isMouseDown = false;
    private float nextMovement = 0f;

    // -----------------------------------------------------------------------------------
    // UpdateClient_HoldMouseToWalk
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("UpdateClient")]
    private void UpdateClient_HoldMouseToWalk()
    {
        //if mouse up, reset everything
        if (Input.GetMouseButtonUp(0))
        {
            //reset
            isMouseDown = false;
        }

        if (isLocalPlayer)
        {
            if (state == "IDLE" || state == "MOVING" || state == "CASTING")
            {
                //if mouse is down
                if (isMouseDown || (Input.GetMouseButtonDown(0) && !Utils.IsCursorOverUserInterface() && Input.touchCount <= 1))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    // raycast with local player ignore option
                    RaycastHit2D hit = localPlayerClickThrough ? Utils.Raycast2DWithout(ray, gameObject) : Physics2D.GetRayIntersection(ray);

                    //if hit something and if mouse is held down
                    Entity entity = hit.transform != null ? hit.transform.GetComponent<Entity>() : null;
                    if (entity && isMouseDown)
                    {
                        //subtract the delta time
                        nextMovement -= Time.deltaTime;
                        //if it is time to move, initiate move and reset timer
                        if (nextMovement <= 0f)
                        {
                            nextMovement = secondsBetweenMovementRequests;

                            // set indicator and navigate to the nearest walkable
                            // destination. this prevents twitching when destination is
                            // accidentally in a room without a door etc.
                            Vector2 bestDestination = movement.NearestValidDestination(hit.point);
                            indicator.SetViaPosition(bestDestination);

                            movement.Navigate(bestDestination, 0);
                        }
                    }
                    else
                    {
                        // set indicator and navigate to the nearest walkable
                        // destination. this prevents twitching when destination is
                        // accidentally in a room without a door etc.
                        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector2 bestDestination = movement.NearestValidDestination(worldPos);
                        indicator.SetViaPosition(bestDestination);

                        // casting? then set pending destination
                        if (state == "CASTING")
                        {
                            pendingDestination = bestDestination;
                            pendingDestinationValid = true;
                        }
                        else
                        {
                            movement.Navigate(bestDestination, 0);
                        }

                        //it's a movement target
                        //set mouseDown to true
                        isMouseDown = true;
                        //set the timer for next movement
                        nextMovement = secondsBetweenMovementRequests;
                    }
                }
            }
        }
    }
}