using System.Reflection;
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
    }
}
