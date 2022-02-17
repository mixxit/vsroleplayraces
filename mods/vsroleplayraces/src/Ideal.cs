namespace vsroleplayraces.src
{
	public class Ideal
	{
		public int id;
		public string ideal;
		public string description;
		public AlignmentType alignmentType;
		public Ideal(int id, string ideal, string description, AlignmentType alignmentType)
		{
			this.id = id;
			this.ideal = ideal;
			this.description = description;
			this.alignmentType = alignmentType;
		}
	}
}