using System;

namespace cleanCore
{

    public class WoWDynamicObject : WoWObject
    {
        public WoWDynamicObject(IntPtr pointer)
            : base(pointer)
        {

        }

        public uint SpellId
        {
            get
            {
                return GetDescriptor<uint>((int)DynamicObjectField.DYNAMICOBJECT_SPELLID);
            }
        }

        public ulong CasterGuid
        {
            get
            {
                return GetDescriptor<ulong>((int) DynamicObjectField.DYNAMICOBJECT_CASTER);
            }
        }

        public uint CastTime
        {
            get
            {
                return GetDescriptor<uint>((int) DynamicObjectField.DYNAMICOBJECT_CASTTIME);
            }
        }

        public float Radius
        {
            get
            {
                return GetDescriptor<float>((int) DynamicObjectField.DYNAMICOBJECT_RADIUS);
            }
        }
    }

}