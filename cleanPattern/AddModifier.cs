namespace cleanPattern
{

    public class AddModifier : IModifier
    {
        public uint Offset { get; private set; }

        public AddModifier()
        {
            
        }

        public AddModifier(uint val)
        {
            Offset = val;
        }

        public uint Apply(uint addr)
        {
            return (addr + Offset);
        }
    }

}