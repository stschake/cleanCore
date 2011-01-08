using System.IO;

namespace cleanPattern
{
    public enum LeaType
    {
        Byte,
        Word,
        Dword
    }

    public class LeaModifier : IModifier
    {
        public LeaType Type { get; private set; }

        public LeaModifier()
        {
            Type = LeaType.Dword;
        }

        public LeaModifier(LeaType type)
        {
            Type = type;
        }

        public unsafe uint Apply(uint address)
        {
            switch (Type)
            {
                case LeaType.Byte:
                    return *(byte*) (address);
                case LeaType.Word:
                    return *(ushort*) (address);
                case LeaType.Dword:
                    return *(uint*) (address);
            }
            throw new InvalidDataException("Unknown LeaType");
        }
    }
}