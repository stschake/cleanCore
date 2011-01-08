using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace cleanPattern
{
    public class Pattern
    {
        public string Name { get; private set; }
        public byte[] Bytes { get; private set; }
        public bool[] Mask { get; private set; }
        public List<IModifier> Modifiers = new List<IModifier>();

        private unsafe bool DataCompare(long offset)
        {
            return !Mask.Where((t, i) => t && Bytes[i] != *(byte*) (offset + i)).Any();
        }

        private uint FindStart()
        {
            var mainModule = Process.GetCurrentProcess().MainModule;

            var start = mainModule.BaseAddress.ToInt32();
            var size = mainModule.ModuleMemorySize;
            for (uint i = 0; i < size; i++)
            {
                if (DataCompare(start + i))
                    return (uint)(start + i);
            }
            throw new InvalidDataException("Pattern not found");
        }

        public uint Find()
        {
            var start = FindStart();
            foreach (var mod in Modifiers)
                start = mod.Apply(start);
            return start;
        }

        public static Pattern FromTextstyle(string name, string pattern)
        {
            var ret = new Pattern {Name = name};
            var split = pattern.Split(' ');
            int index = 0;
            ret.Bytes = new byte[split.Length];
            ret.Mask = new bool[split.Length];
            foreach (var token in split)
            {
                if (token.Length > 2)
                    throw new InvalidDataException("Invalid token: " + token);
                if (token.Contains("?"))
                    ret.Mask[index++] = false;
                else
                {
                    byte data = byte.Parse(token, NumberStyles.HexNumber);
                    ret.Bytes[index] = data;
                    ret.Mask[index] = true;
                    index++;
                }
            }
            return ret;
        }
    }
}
