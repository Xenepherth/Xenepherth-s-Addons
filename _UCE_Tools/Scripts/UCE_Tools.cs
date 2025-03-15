﻿// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/iMMOban
// =======================================================================================
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

// =======================================================================================
// UCE TOOLS
// =======================================================================================
public partial class UCE_Tools
{
    private const char CONST_DELIMITER = ';';

    public static int joy_tIdx = -1;
    public static bool pointerDown;

    // -----------------------------------------------------------------------------------
    // GetZoomUniversal
    // -----------------------------------------------------------------------------------
    // universal zoom: mouse scroll if mouse, two finger pinching otherwise
    public static float GetZoomUniversal()
    {
        if (Input.mousePresent)
            return Utils.GetAxisRawScrollUniversal();
        else if (Input.touchSupported)
            return Utils.GetPinch();
        return 0;
    }

    // -----------------------------------------------------------------------------------
    // GetDeterministicHashCode
    // -----------------------------------------------------------------------------------
    public static int GetDeterministicHashCode(string str)
    {
        int hash1 = (5381 << 16) + 5381;
        int hash2 = hash1;

        for (int i = 0; i < str.Length; i += 2)
        {
            hash1 = ((hash1 << 5) + hash1) ^ str[i];
            if (i == str.Length - 1)
                break;
            hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
        }

        return hash1 + (hash2 * 1566083941);
    }

    // -----------------------------------------------------------------------------------
    // GetTouchDown
    // @Client
    // -----------------------------------------------------------------------------------
    public static bool GetTouchDown
    {
        get { return (joy_tIdx == -1 && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began); }
    }

    // -----------------------------------------------------------------------------------
    // GetTouchUp
    // @Client
    // -----------------------------------------------------------------------------------
    public static bool GetTouchUp
    {
        get { return (joy_tIdx == -1 && Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)); }
    }

    // -----------------------------------------------------------------------------------
    // ReachableRandomUnitCircleOnNavMesh
    // -----------------------------------------------------------------------------------
    public static Vector2 ReachableRandomUnitCircleOnNavMesh(Vector2 position, float radiusMultiplier, int solverAttempts = 3)
    {
        for (int i = 0; i < solverAttempts; ++i)
        {
            Vector2 candidate = RandomUnitCircleOnNavMesh(position, radiusMultiplier);

            NavMeshHit hit;
            if (!NavMesh.Raycast(position, candidate, out hit, NavMesh.AllAreas))
                return candidate;
        }

        return position;
    }

    // -----------------------------------------------------------------------------------
    // RandomUnitCircleOnNavMesh
    // -----------------------------------------------------------------------------------
    public static Vector2 RandomUnitCircleOnNavMesh(Vector2 position, float radiusMultiplier)
    {
        Vector2 r = UnityEngine.Random.insideUnitCircle * radiusMultiplier;

        Vector2 randomPosition = new Vector2(position.x + r.x, position.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, radiusMultiplier * 2, NavMesh.AllAreas))
            return hit.position;
        return position;
    }

    // -----------------------------------------------------------------------------------
    // UCE_SelectionHandling
    // Checks if the client has clicked on any clickable object in the scene (not UI)
    // @Client
    // -----------------------------------------------------------------------------------
    public static bool UCE_SelectionHandling(GameObject target)
    {
        Player player = Player.localPlayer;
        if (!player) return false;

        if (player.isAlive &&
            Input.GetMouseButtonDown(0) && !Utils.IsCursorOverUserInterface() && Input.touchCount <= 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 pointOrigin = new Vector2(ray.origin.x, ray.origin.y);
            Vector2 pointDirection = new Vector2(ray.direction.x, ray.direction.y);

            RaycastHit2D hit;

            bool cast = player.localPlayerClickThrough ? hit = Utils.Raycast2DWithout(ray, player.gameObject) : hit = Physics2D.Raycast(pointOrigin, pointDirection);

            if (cast && hit.transform.gameObject == target)
            {
                return true;
            }
        }

        return false;
    }

    // -----------------------------------------------------------------------------------
    // UCE_CheckSelectionHandling
    // Validates the interaction range, the player's state and if the player is alive or not
    // @Client OR @Server
    // -----------------------------------------------------------------------------------
    public static bool UCE_CheckSelectionHandling(GameObject target, Player localPlayer = null)
    {
        if (localPlayer == null)
            localPlayer = Player.localPlayer;

        if (!localPlayer || !target) return false;

        return localPlayer.isAlive &&
                (
                (target.GetComponent<Collider2D>() && Utils.ClosestDistance(localPlayer.collider, target.GetComponent<Collider2D>()) <= localPlayer.interactionRange) ||
                (target.GetComponent<Entity>() && Utils.ClosestDistance(localPlayer.collider, target.GetComponent<Entity>().collider) <= localPlayer.interactionRange)
                );
    }

    // -----------------------------------------------------------------------------------
    // IntArrayToString
    // -----------------------------------------------------------------------------------
    public static string IntArrayToString(int[] array)
    {
        if (array == null || array.Length == 0) return null;
        string arrayString = "";
        for (int i = 0; i < array.Length; i++)
        {
            arrayString += array[i].ToString();
            if (i < array.Length - 1)
                arrayString += CONST_DELIMITER;
        }
        return arrayString;
    }

    // -----------------------------------------------------------------------------------
    // IntStringToArray
    // -----------------------------------------------------------------------------------
    public static int[] IntStringToArray(string array)
    {
        if (string.IsNullOrWhiteSpace(array)) return null;
        string[] tokens = array.Split(CONST_DELIMITER);
        int[] arrayInt = Array.ConvertAll<string, int>(tokens, int.Parse);
        return arrayInt;
    }

    // -----------------------------------------------------------------------------------
    // FindOnlinePlayerByName
    // -----------------------------------------------------------------------------------
    public static Player FindOnlinePlayerByName(string playerName)
    {
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            if (Player.onlinePlayers.ContainsKey(playerName))
            {
                return Player.onlinePlayers[playerName].GetComponent<Player>();
            }
        }
        return null;
    }

    // -------------------------------------------------------------------------------
    // ArrayContains
    // -------------------------------------------------------------------------------
    public static bool ArrayContains(int[] defines, int define)
    {
        foreach (int def in defines)
        {
            if (def == define)
                return true;
        }
        return false;
    }

    // -------------------------------------------------------------------------------
    // ArrayContains
    // -------------------------------------------------------------------------------
    public static bool ArrayContains(string[] defines, string define)
    {
        foreach (string def in defines)
        {
            if (def == define)
                return true;
        }
        return false;
    }

    // -------------------------------------------------------------------------------
    // RemoveFromArray
    // -------------------------------------------------------------------------------
    public static string[] RemoveFromArray(string[] defines, string define)
    {
        return defines.Where(x => x != define).ToArray();
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================