// =======================================================================================
// Maintained by bobatea#9400 on Discord
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: 
 
// * Leave a star on my Github Repo.....: https://github.com/breehuynh/Bree-mmorpg-tools
// * Instructions.......................: https://indie-mmo.net/knowledge-base/
// =======================================================================================
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;

#if _MYSQL
using MySql.Data;
using MySql.Data.MySqlClient;
using SqlParameter = MySql.Data.MySqlClient.MySqlParameter;
#elif _SQLITE
using SQLite;
#endif

// DATABASE (SQLite / mySQL Hybrid)

public partial class Database
{

#if _SQLITE
	// -----------------------------------------------------------------------------------
    // Character Scene
    // -----------------------------------------------------------------------------------
    class character_scene
    {
        [PrimaryKey] // important for performance: O(log n) instead of O(n)
        public string character { get; set; }
        public string scene { get; set; }
    }

    // -----------------------------------------------------------------------------------
    // Zones Online
    // -----------------------------------------------------------------------------------
    class zones_online
    {
        public string online { get; set; }
    }
#endif
    
    // -----------------------------------------------------------------------------------
    // Connect_UCE_NetworkZone
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("Connect")]
    public void Connect_UCE_NetworkZone()
    {
#if _MYSQL
		ExecuteNonQueryMySql(@"
        CREATE TABLE IF NOT EXISTS character_scene (
            `character` VARCHAR(32) NOT NULL,
            scene VARCHAR(64) NOT NULL,
            PRIMARY KEY(`character`)
            ) CHARACTER SET=utf8mb4");

        ExecuteNonQueryMySql(@"
        CREATE TABLE IF NOT EXISTS zones_online (
            id INT NOT NULL AUTO_INCREMENT,
            PRIMARY KEY(id),
            online TIMESTAMP NOT NULL
        ) CHARACTER SET=utf8mb4");
#elif _SQLITE
        connection.CreateTable<character_scene>();
        connection.CreateTable<zones_online>();
#endif
    }

    // -----------------------------------------------------------------------------------
    // IsCharacterOnlineAnywhere
    // a character is online on any of the servers if the online string is not
    // empty and if the time difference is less than the save interval * 2
    // (* 2 to have some tolerance)
    // -----------------------------------------------------------------------------------
    public bool IsCharacterOnlineAnywhere(string characterName)
    {
        float saveInterval = ((NetworkManagerMMO)NetworkManager.singleton).saveInterval;

#if _MYSQL
	    int obj = (int)ExecuteScalarMySql("SELECT online FROM characters WHERE name=@name", new SqlParameter("@name", characterName));
		if (obj == 1)
        {
             var time = ExecuteScalarMySql("SELECT lastsaved FROM characters WHERE name=@name", new SqlParameter("@name", characterName));
             double elapsedSeconds = (DateTime.UtcNow - (DateTime)time).TotalSeconds;
             return elapsedSeconds < saveInterval * 2;
        }
#elif _SQLITE
        characters character = connection.FindWithQuery<characters>("SELECT * FROM characters WHERE name=?", characterName);
        if (character != null)
        {
            if (character.online == true)
            {
                var lastsaved = character.lastsaved;
                double elapsedSeconds = (DateTime.UtcNow - lastsaved).TotalSeconds;
                float SaveInterval = ((NetworkManagerMMO)NetworkManager.singleton).saveInterval;

                // online if 1 and last saved recently (it's possible that online is
                // still 1 after a server crash, hence last saved detection)
                return character.online == true && elapsedSeconds < SaveInterval * 2;
            }
            else
            {
                character.online = false;
                return false;
            }
        }
#endif
        return false;
    }

    // -----------------------------------------------------------------------------------
    // AnyAccountCharacterOnline
    // -----------------------------------------------------------------------------------
    public bool AnyAccountCharacterOnline(string account)
    {
#if _SERVER
        if (NetworkServer.active)
        { 
            List<string> characters = CharactersForAccount(account);
            return characters.Any(IsCharacterOnlineAnywhere);
        }
#endif
        return false;
    }

    // -----------------------------------------------------------------------------------
    // GetCharacterScene
    // -----------------------------------------------------------------------------------
    public string GetCharacterScene(string characterName)
    {
#if _MYSQL

        var obj = ExecuteScalarMySql("SELECT scene FROM character_scene WHERE `character`=@character", new SqlParameter("@character", characterName));
        if (obj != null)
        return (string)obj;

#elif _SQLITE
        character_scene characterScene = connection.FindWithQuery<character_scene>("SELECT scene FROM character_scene WHERE character=?", characterName);
        if (characterScene != null)
            return characterScene.scene;
#endif
        return "";
    }

    // -----------------------------------------------------------------------------------
    // SaveCharacterScene
    // -----------------------------------------------------------------------------------
    public void SaveCharacterScene(string characterName, string sceneName)
    {
#if _MYSQL
		var query = @"
            INSERT INTO character_scene
            SET
                `character`=@character,
                scene=@scene
            ON DUPLICATE KEY UPDATE
                scene=@scene";

        ExecuteNonQueryMySql(query,
                             new SqlParameter("@character", characterName),
                             new SqlParameter("@scene", sceneName));
#elif _SQLITE
        connection.InsertOrReplace(new character_scene
        {
            character = characterName,
            scene = sceneName
        });
#endif
    }

    // -----------------------------------------------------------------------------------
    // TimeElapsedSinceMainZoneOnline
    // a zone is online if the online string is not empty and if the time
    // difference is less than the write interval * multiplier
    // (* multiplier to have some tolerance)
    // -----------------------------------------------------------------------------------
    public double TimeElapsedSinceMainZoneOnline()
    {
#if _MYSQL
		var obj = ExecuteScalarMySql("SELECT online FROM zones_online");
        if (obj != null)
        {
            var time = (DateTime)obj;
            return (DateTime.Now - time).TotalSeconds;
        }
#elif _SQLITE
        zones_online onlineInfo = connection.FindWithQuery<zones_online>("SELECT online FROM zones_online");
        if (onlineInfo != null && !string.IsNullOrEmpty(onlineInfo.online))
        {
            DateTime time = DateTime.Parse(onlineInfo.online);
            return (DateTime.UtcNow - time).TotalSeconds;
        }
#endif
        return Mathf.Infinity;
    }

    // -----------------------------------------------------------------------------------
    // SaveMainZoneOnlineTime
    // Note: should only be called by main zone
    // online status:
    //   '' if offline (if just logging out etc.)
    //   current time otherwise
    // -> it uses the ISO 8601 standard format
    // -----------------------------------------------------------------------------------
    public void SaveMainZoneOnlineTime()
    {
#if _MYSQL
		var query = @"
            INSERT INTO zones_online
            SET
                id=@id,
                online=@online
            ON DUPLICATE KEY UPDATE
                online=@online";

        ExecuteNonQueryMySql(query,
                             new SqlParameter("@id", 1),
                             new SqlParameter("@online", DateTime.Now));
#elif _SQLITE
        string onlineString = DateTime.UtcNow.ToString("s");
        connection.Execute("DELETE FROM zones_online");
        connection.InsertOrReplace(new zones_online
        {
            online = onlineString
        });
#endif
    }

    // -----------------------------------------------------------------------------------
}
