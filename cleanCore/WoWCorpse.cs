using System;

namespace cleanCore
{

    public class WoWCorpse : WoWObject
    {
        public WoWCorpse(IntPtr pointer)
            : base(pointer)
        {
            
        }

        public ulong OwnerGuid
        {
            get
            {
                return GetDescriptor<ulong>((int) CorpseField.CORPSE_FIELD_OWNER);
            }
        }

        public uint DisplayId
        {
            get
            {
                return GetDescriptor<uint>((int) CorpseField.CORPSE_FIELD_DISPLAY_ID);
            }
        }

        public uint Flags
        {
            get
            {
                return GetDescriptor<uint>((int) CorpseField.CORPSE_FIELD_FLAGS);
            }
        }

        public uint DynamicFlags
        {
            get
            {
                return GetDescriptor<uint>((int) CorpseField.CORPSE_FIELD_DYNAMIC_FLAGS);
            }
        }
    }

}