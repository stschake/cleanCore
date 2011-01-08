using System;

namespace cleanCore
{
    
    public class WoWBag
    {
        public IntPtr Pointer { get; set; }

        // see CGPlayer_C__AutoEquipItem
        public int Slots
        {
            get { return Helper.Magic.Read<int>(Pointer); }
        }

        public ulong GetItemGuid(int index)
        {
            var array = Helper.Magic.Read<uint>(Pointer + 0x4);
            return Helper.Magic.Read<ulong>((uint)(array + (index*0x8)));
        }
        
        public WoWBag(IntPtr pointer)
        {
            Pointer = pointer;
        }
    }

}