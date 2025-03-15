// =======================================================================================
// Maintained by bobatea#9400 on Discord
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: 
 
// * Leave a star on my Github Repo.....: https://github.com/breehuynh/Bree-mmorpg-tools
// * Instructions.......................: https://indie-mmo.net/knowledge-base/
// =======================================================================================
using UnityEngine;
using Mirror;
using Eflatun.SceneReference;

// PLAYER

public partial class Player
{
    [Header("[-=-=- UCE NETWORK ZONES -=-=-]")]
    public SceneReference startingScene;


    // -----------------------------------------------------------------------------------
    // UCE_OnPortal
    // @Server
    // -----------------------------------------------------------------------------------
    [ServerCallback]
    public void UCE_OnPortal(SceneReference targetScene, Vector2 targetPosition, Player player)
    {
        player.transform.position = targetPosition;
        Database.singleton.CharacterSave(player, false);
        Database.singleton.SaveCharacterScene(player.name, targetScene.Name);

        // ask client to switch server
        player.connectionToClient.Send(new SwitchServerMsg { sceneName = targetScene.Name, characterName = player.name });

        // immediately destroy so nothing messes with the new
        // position and so it's not saved again etc.
        player.gameObject.SetActive(false);
    }

#if _iMMOBINDPOINT

    // -----------------------------------------------------------------------------------
    // UCE_OnPortal
    // @Server
    // -----------------------------------------------------------------------------------
    [ServerCallback]
    public void UCE_OnPortal(UCE_BindPoint bindpoint)
    {
        this.transform.position = bindpoint.position;
        Database.singleton.CharacterSave(this, false);
        Database.singleton.SaveCharacterScene(this.name, bindpoint.SceneName);

        // ask client to switch server
        this.connectionToClient.Send(
            new SwitchServerMsg
            {
                sceneName = bindpoint.SceneName,
                characterName = this.name
            }
        );

        // immediately destroy so nothing messes with the new
        // position and so it's not saved again etc.
        NetworkServer.Destroy(this.gameObject);
    }

#endif

    // -----------------------------------------------------------------------------------
}
