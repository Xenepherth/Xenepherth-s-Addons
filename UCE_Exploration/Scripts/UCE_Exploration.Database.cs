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

#if _MYSQL
using MySql.Data;
using MySql.Data.MySqlClient;
#elif _SQLITE
using SQLite;
#endif

// DATABASE (SQLite / mySQL Hybrid)

public partial class Database
{

#if _SQLITE
	// -----------------------------------------------------------------------------------
    // Character Exploration
    // -----------------------------------------------------------------------------------
    class character_exploration
    {
        public string character { get; set; }
        public string exploredArea { get; set; }
    }
#endif

    // -----------------------------------------------------------------------------------
    // Connect_UCE_Exploration
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("Connect")]
    public void Connect_UCE_Exploration()
    {
#if _MYSQL
		ExecuteNonQueryMySql(@"CREATE TABLE IF NOT EXISTS character_exploration (`character` VARCHAR(32) NOT NULL, exploredArea VARCHAR(32) NOT NULL) CHARACTER SET=utf8mb4");
#elif _SQLITE
        connection.CreateTable<character_exploration>();
#endif
    }

    // -----------------------------------------------------------------------------------
    // CharacterLoad_UCE_Exploration
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("CharacterLoad")]
    public void CharacterLoad_UCE_Exploration(Player player)
    {
#if _MYSQL
		var table = ExecuteReaderMySql("SELECT exploredArea FROM character_exploration WHERE `character`=@character",
						new MySqlParameter("@character", player.name)
						);
		foreach (var row in table) {
			player.UCE_exploredAreas.Add((string)row[0]);
		}
#elif _SQLITE
        var table = connection.Query<character_exploration>("SELECT exploredArea FROM character_exploration WHERE character=?", player.name);
        foreach (var row in table)
        {
            player.UCE_exploredAreas.Add(row.exploredArea);
        }
#endif
    }

    // -----------------------------------------------------------------------------------
    // CharacterSave_UCE_Exploration
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("CharacterSave")]
    public void CharacterSave_UCE_Exploration(Player player)
    {
#if _MYSQL
		ExecuteNonQueryMySql("DELETE FROM character_exploration WHERE `character`=@character", new MySqlParameter("@character", player.name));
        for (int i = 0; i < player.UCE_exploredAreas.Count; ++i)
        {
            ExecuteNonQueryMySql("INSERT INTO character_exploration VALUES (@character, @exploredArea)",
                 new MySqlParameter("@character", player.name),
                 new MySqlParameter("@exploredArea", player.UCE_exploredAreas[i])
                 );
        }
#elif _SQLITE
        connection.Execute("DELETE FROM character_exploration WHERE character=?", player.name);
        for (int i = 0; i < player.UCE_exploredAreas.Count; i++)
        {
            connection.Insert(new character_exploration
            {
                character = player.name,
                exploredArea = player.UCE_exploredAreas[i]
            });
        }
#endif
    }

    // -----------------------------------------------------------------------------------
}
