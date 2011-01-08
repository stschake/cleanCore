using System;

namespace cleanCore
{

    public class WoWGameObject : WoWObject
    {
        public WoWGameObject(IntPtr pointer)
            : base(pointer)
        {

        }

        public uint DisplayId
        {
            get
            {
                return GetDescriptor<uint>((int) GameObjectField.GAMEOBJECT_DISPLAYID);
            }
        }

        public uint Flags
        {
            get
            {
                return GetDescriptor<uint>((int) GameObjectField.GAMEOBJECT_FLAGS);
            }
        }

        public uint Level
        {
            get
            {
                return GetDescriptor<uint>((int) GameObjectField.GAMEOBJECT_LEVEL);
            }
        }

        public uint Faction
        {
            get
            {
                return GetDescriptor<uint>((int) GameObjectField.GAMEOBJECT_FACTION);
            }
        }

        public bool Locked
        {
            get
            {
                return (Flags & (uint)GameObjectFlags.Locked) > 0;
            }
        }

        public bool InUse
        {
            get
            {
                return (Flags & (uint) GameObjectFlags.InUse) > 0;
            }
        }

        public bool IsTransport
        {
            get
            {
                return (Flags & (uint)GameObjectFlags.Transport) > 0;
            }
        }
    }

}