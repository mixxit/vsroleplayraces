using System;
using System.Text.RegularExpressions;

namespace vsroleplayraces.src
{
    internal class PlayerNameUtils
    {
        internal static string CleanupRoleplayName(string playerName)
        {
            if (string.IsNullOrEmpty(playerName))
                return playerName;

            playerName = playerName.ToLower();
            if (playerName.Length > 8)
                playerName = playerName.Substring(0, 8);

            Regex rgx = new Regex("[^a-z]");
            playerName = rgx.Replace(playerName, "");

            if (playerName.Length < 1)
                return "unknown";

            return playerName;
        }
    }
}