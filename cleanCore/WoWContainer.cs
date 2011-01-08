using System;
using System.Collections.Generic;

namespace cleanCore
{

    public class WoWContainer : WoWItem
    {
        public WoWContainer(IntPtr pointer)
            : base(pointer)
        {
            
        }

        public uint Slots
        {
            get
            {
                return GetDescriptor<uint>((int) ContainerField.CONTAINER_FIELD_NUM_SLOTS);
            }
        }

        public ulong GetItemGuid(int index)
        {
            if (index > 35 || index >= Slots || index <= 0)
                return 0;

            return GetDescriptor<ulong>((int) ContainerField.CONTAINER_FIELD_SLOT_1 + (index*8));
        }

        public WoWItem GetItem(int index)
        {
            return Manager.GetObjectByGuid(GetItemGuid(index)) as WoWItem;
        }

        public List<WoWItem> Items
        {
            get
            {
                var ret = new List<WoWItem>((int)Slots);
                for (int i = 0; i < Slots; i++)
                {
                    var guid = GetItemGuid(i);
                    if (guid != 0)
                    {
                        var obj = Manager.GetObjectByGuid(guid);
                        if (obj == null || !obj.IsValid || !obj.IsItem)
                            continue;
                        ret.Add(obj as WoWItem);
                    }
                }
                return ret;
            }
        }
    }

}