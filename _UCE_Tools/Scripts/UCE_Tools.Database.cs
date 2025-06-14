// =======================================================================================
// Maintained by bobatea#9400 on Discord
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: 

// * Leave a star on my Github Repo.....: https://github.com/breehuynh/Bree-mmorpg-tools
// * Instructions.......................: https://indie-mmo.net/knowledge-base/
// =======================================================================================
#if MYSQL
using MySql.Data.MySqlClient;
#endif
using SQLite;
using UnityEngine;

// DATABASE CLASSES

public partial class Database
{
    // -----------------------------------------------------------------------------------
    // UCE_connection
    // @ workaround because uMMORPGs default database connection is private.
    // -----------------------------------------------------------------------------------
    public SQLiteConnection UCE_connection {
		get {
#if _SQLITE
        return connection;
#else
        return null;
#endif
		}
    }
}
