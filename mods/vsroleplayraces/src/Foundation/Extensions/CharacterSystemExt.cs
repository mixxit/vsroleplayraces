using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace vsroleplayraces.src.Foundation.Extensions
{
    public static class CharacterSystemExt
    {
        public static void ClientSelectionDone(this CharacterSystem modSys, IInventory characterInv, string characterClass, bool didSelect)
        {
            MethodInfo dynMethod = modSys.GetType().GetMethod("ClientSelectionDone",
    BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(modSys, new object[] { characterInv, characterClass, didSelect });
        }

        public static void ClientRaceSelectionDone(this CharacterSystem modSys, ICoreClientAPI coreClientApi, string raceName, int idealid, int trait1id, int trait2id, int flawid, int  bondid, string roleplayForename, string roleplaySurname)
        {
            coreClientApi.Network.GetChannel("raceselection").SendPacket(new RaceSelectionPacket()
            {
                RaceName = raceName,
                IdealId = idealid,
                Trait1Id = trait1id,
                Trait2Id = trait2id,
                FlawId = flawid,
                BondId = bondid,
                RoleplayForename = roleplayForename,
                RoleplaySurname = roleplaySurname,
            }) ;
        }
    }
}
