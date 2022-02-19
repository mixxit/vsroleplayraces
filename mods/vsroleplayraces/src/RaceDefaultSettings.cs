namespace vsroleplayraces.src
{
    internal class RaceDefaultSettings
    {
        public string raceCode { get; set; }
        public string bodyCode { get; set; }
        public string hairBase { get; set; }
        public string mustache { get; set; }
        public string beard { get; set; }
        public string hairExtra { get; set; }
        public int strength { get; set; }
        public int stamina { get; set; }
        public int agility { get; set; }
        public int dexterity { get; set; }
        public int intelligence { get; set; }
        public int wisdom { get; set; }
        public int charisma { get; set; }
        public AlignmentType alignment { get; set; }
        public string description { get; set; }
        public int[] allowedHairColors { get; set; }
        public int defaultHairColor { get; set; }
    }
}