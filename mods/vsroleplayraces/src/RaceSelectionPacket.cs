using ProtoBuf;

namespace vsroleplayraces.src
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class RaceSelectionPacket
    {
        public string RaceName;
        public int IdealId;
        public int Trait1Id;
        public int Trait2Id;
        public int FlawId;
        public int BondId;
    }
}