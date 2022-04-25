using HarmonyLib;
using System;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace vsroleplayraces.src
{
    [HarmonyPatch]
    public sealed class CharacterSystemCharSelectReplacerMod : ModSystem
    {
        private readonly Harmony harmony;
        public CharacterSystemCharSelectReplacerMod()
        {
            harmony = new Harmony("CharacterSystemCharSelectReplacerMod");
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            base.StartClientSide(api);
            harmony.PatchAll();
        }
        public override double ExecuteOrder()
        {
            /// Worldgen:
            /// - GenTerra: 0 
            /// - RockStrata: 0.1
            /// - Deposits: 0.2
            /// - Caves: 0.3
            /// - Blocklayers: 0.4
            /// Asset Loading
            /// - Json Overrides loader: 0.05
            /// - Load hardcoded mantle block: 0.1
            /// - Block and Item Loader: 0.2
            /// - Recipes (Smithing, Knapping, Clayforming, Grid recipes, Alloys) Loader: 1
            /// 
            return 1.1;
        }
    }

    [HarmonyPatch(typeof(CharacterSystem), "onCharSelCmd")]
    public class CharacterSystemPatch_onCharSelCmd
    {
        [HarmonyPrefix]
        public static bool Prefix(CharacterSystem __instance, int groupId, CmdArgs args)
        {
            Console.WriteLine("Skipping");
            return false;
        }
    }

    [HarmonyPatch(typeof(CharacterSystem), "Event_PlayerJoin")]
    public class CharacterSystemPatch_Event_PlayerJoin
    {
        // Original of this can be found here when updating vintagestory
        // https://github.com/anegostudios/vssurvivalmod/blob/master/Systems/Character/Character.cs
        // Point of this is to replace the GuiDialogCreateCharacter with a new one
        [HarmonyPrefix]
        public static bool Prefix(CharacterSystem __instance, IClientPlayer byPlayer)
        {
            if (byPlayer.Entity != null)
            if (byPlayer.Entity.Api is ICoreClientAPI)
            {
                FieldInfo fieldInfo = typeof(CharacterSystem).GetField("didSelect", BindingFlags.NonPublic |
                                              BindingFlags.Instance);

                if (!(bool)fieldInfo.GetValue(__instance) && byPlayer.PlayerUID == ((ICoreClientAPI)byPlayer.Entity.Api).World.Player.PlayerUID)
                {
                    var createCharDlg = new GuiDialogCreateCharacterExtendedRoleplay(((ICoreClientAPI)byPlayer.Entity.Api), __instance);
                    createCharDlg.PrepAndOpen();
                }
            }

            return false;
        }
    }
}
