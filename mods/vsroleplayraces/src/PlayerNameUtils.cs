using System;
using System.Linq;
using System.Text.RegularExpressions;
using Vintagestory.API.Common;

namespace vsroleplayraces.src
{
    public class PlayerNameUtils
    {
        public static string CleanupRoleplayName(string playerName)
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

        public static string GetFullRoleplayNameAsDisplayFormat(EntityPlayer player)
        {
            var foreName = FirstCharToUpper(PlayerNameUtils.CleanupRoleplayName(player.WatchedAttributes.GetString("roleplayForename", "")).TrimEnd());
            var lastName = FirstCharToUpper(PlayerNameUtils.CleanupRoleplayName(player.WatchedAttributes.GetString("roleplaySurname", "")).TrimEnd());

            if (String.IsNullOrEmpty(foreName + lastName))
                return FirstCharToUpper(PlayerNameUtils.CleanupRoleplayName(player.GetName()).TrimEnd());

            if ((foreName + lastName).Length > 16)
                return FirstCharToUpper(PlayerNameUtils.CleanupRoleplayName(player.GetName()).TrimEnd());

            return foreName + lastName;
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;

            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }
    }
}