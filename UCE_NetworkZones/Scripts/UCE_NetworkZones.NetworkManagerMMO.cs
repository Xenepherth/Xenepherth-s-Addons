// =======================================================================================
// Maintained by bobatea#9400 on Discord
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: 
 
// * Leave a star on my Github Repo.....: https://github.com/breehuynh/Bree-mmorpg-tools
// * Instructions.......................: https://indie-mmo.net/knowledge-base/
// =======================================================================================
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public struct SwitchServerMsg : NetworkMessage
{
    public string sceneName;
    public string characterName;
}

// NETWORK MANAGER MMO

public partial class NetworkManagerMMO
{
    // -----------------------------------------------------------------------------------
    // OnClientConnect_UCE_NetworkZones
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("OnClientConnect")]
    public void OnClientConnect_UCE_NetworkZones(NetworkConnection conn)
    {
        NetworkClient.RegisterHandler<SwitchServerMsg>(GetComponent<UCE_NetworkZone>().OnClientSwitchServerRequested);
    }

    // -----------------------------------------------------------------------------------
    // OnStartServer_UCE_NetworkZones
    // spawn instance processes (if any)
    // @Server
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("OnStartServer")]
    public void OnStartServer_UCE_NetworkZones()
    {
#if !UNITY_EDITOR
		if (GetComponent<UCE_NetworkZone>() != null)
    		GetComponent<UCE_NetworkZone>().SpawnProcesses();
#endif
    }

    // -----------------------------------------------------------------------------------
    // OnClientCharactersAvailable_UCE_NetworkZones
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("OnClientCharactersAvailable")]
    public void OnClientCharactersAvailable_UCE_NetworkZones(CharactersAvailableMsg message)
    {
        int index = message.characters.ToList().FindIndex(c => c.name == UCE_NetworkZone.autoSelectCharacter);

        if (index != -1 && isNetworkActive && changingCharacters == false && OnLogout == false)
        {
            // send character select message
            print("[Zones]: autoselect " + UCE_NetworkZone.autoSelectCharacter + "(" + index + ")");
            selection = index;
            NetworkClient.Ready();
            NetworkClient.connection.Send(new CharacterSelectMsg { index = selection });
            ClearPreviews();
        }
        
        OnLogout = false;
    }

    // -----------------------------------------------------------------------------------
    // OnServerServerCharacterSelect_UCE_NetworkZones
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("OnServerCharacterSelect")]
    public void OnServerCharacterSelect_UCE_NetworkZones(string account, GameObject player, NetworkConnection conn, CharacterSelectMsg message)
    {
        // where was the player saved the last time?
        string lastScene = Database.singleton.GetCharacterScene(player.name);

        if (lastScene != "" && lastScene != SceneManager.GetActiveScene().name && isNetworkActive)
        {
            print("[Zones]: " + player.name + " was last saved on another scene, transferring to: " + lastScene);

            // ask client to switch server
            conn.Send(
                new SwitchServerMsg
                {
                    sceneName = lastScene,
                    characterName = player.name
                }
            );

            // immediately destroy so nothing messes with the new
            // position and so it's not saved again etc.
            player.SetActive(false);
        }
    }

    // -----------------------------------------------------------------------------------
    // OnServerCharacterCreate_UCE_NetworkZone
    // Save starting scene of the player only when that player is created
    // @Server
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("OnServerCharacterCreate")]
    public void OnServerCharacterCreate_UCE_NetworkZone(CharacterCreateMsg message, Player player)
    {
        if (player.startingScene == null) return;
        Database.singleton.SaveCharacterScene(player.name, player.startingScene.Name);
    }

    // -----------------------------------------------------------------------------------
    // UCE_NetworkSpawn
    // -----------------------------------------------------------------------------------
    public void UCE_NetworkSpawn(GameObject gob)
    {
        NetworkServer.Spawn(gob);
    }

    // -----------------------------------------------------------------------------------
}
