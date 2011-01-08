using System;
using System.Runtime.InteropServices;

namespace cleanCore
{

    public class WoWItem : WoWObject
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void UseItemDelegate(IntPtr thisObj, ref ulong guid, int unkZero);
        private static UseItemDelegate _useItem;

        public WoWItem(IntPtr pointer)
            : base(pointer)
        {

        }

        public ulong OwnerGuid
        {
            get
            {
                return GetDescriptor<ulong>((int) ItemField.ITEM_FIELD_OWNER);
            }
        }

        public ulong CreatorGuid
        {
            get
            {
                return GetDescriptor<ulong>((int) ItemField.ITEM_FIELD_CREATOR);
            }
        }

        public uint StackCount
        {
            get
            {
                return GetDescriptor<uint>((int) ItemField.ITEM_FIELD_STACK_COUNT);
            }
        }

        public ItemFlags Flags
        {
            get
            {
                return (ItemFlags)GetDescriptor<uint>((int) ItemField.ITEM_FIELD_FLAGS);
            }
        }

        public uint RandomPropertiesId
        {
            get
            {
                return GetDescriptor<uint>((int) ItemField.ITEM_FIELD_RANDOM_PROPERTIES_ID);
            }
        }

        public uint Durability
        {
            get
            {
                return GetDescriptor<uint>((int) ItemField.ITEM_FIELD_DURABILITY);
            }
        }

        public uint MaxDurability
        {
            get
            {
                return GetDescriptor<uint>((int) ItemField.ITEM_FIELD_MAXDURABILITY);
            }
        }

        public void Use()
        {
            Use(Manager.LocalPlayer);
        }

        public void Use(WoWObject target)
        {
            var guid = target.Guid;
            if (_useItem == null)
                _useItem = Helper.Magic.RegisterDelegate<UseItemDelegate>(Offsets.UseItem);
            _useItem(Manager.LocalPlayer.Pointer, ref guid, 0);
        }
    }

}