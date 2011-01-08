using System;
using System.Runtime.InteropServices;

namespace cleanCore
{
    
    public class WoWObject
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetObjectNameDelegate(IntPtr thisPointer);
        private readonly GetObjectNameDelegate _getObjectName;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void GetObjectLocationDelegate(IntPtr thisPointer, out Location loc);
        private readonly GetObjectLocationDelegate _getObjectLocation;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void InteractDelegate(IntPtr thisPointer);
        private readonly InteractDelegate _interact;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetBagDelegate(IntPtr thisObj);
        private readonly GetBagDelegate _getBag;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SelectObjectDelegate(ulong guid);
        private static SelectObjectDelegate _selectObject;

        public IntPtr Pointer { get; set; }

        public static void Initialize()
        {
            _selectObject = Helper.Magic.RegisterDelegate<SelectObjectDelegate>(Offsets.SelectObject);
        }
        
        public WoWObject(IntPtr pointer)
        {
            Pointer = pointer;

            if (IsValid)
            {
                _getBag = RegisterVirtualFunction<GetBagDelegate>(Offsets.GetBag);
                _getObjectName = RegisterVirtualFunction<GetObjectNameDelegate>(Offsets.GetObjectName);
                _getObjectLocation = RegisterVirtualFunction<GetObjectLocationDelegate>(Offsets.GetObjectLocation);
                _interact = RegisterVirtualFunction<InteractDelegate>(Offsets.Interact);
            }
        }

        protected T RegisterVirtualFunction<T>(uint offset) where T : class
        {
            var pointer = Helper.Magic.GetObjectVtableFunction(Pointer, offset / 4);
            if (pointer == IntPtr.Zero)
                return null;
            return Helper.Magic.RegisterDelegate<T>(pointer);
        }

        public WoWBag GetBag()
        {
            var ptr = _getBag(Pointer);
            if (ptr == IntPtr.Zero)
                return null;
            return new WoWBag(ptr);
        }

        public void Select()
        {
            _selectObject(Guid);
        }

        public void Interact()
        {
            _interact(Pointer);
        }

        public string Name
        {
            get
            {
                var pointer = _getObjectName(Pointer);
                if (pointer == IntPtr.Zero)
                    return "UNKNOWN";
                return Marshal.PtrToStringAnsi(pointer);
            }
        }

        public Location Location
        {
            get
            {
                Location ret;
                _getObjectLocation(Pointer, out ret);
                return ret;
            }
        }

        public bool InLoS
        {
            get { return WoWWorld.LineOfSightTest(Location, Manager.LocalPlayer.Location) == TracelineResult.NoCollision; }
        }

        public bool IsValid
        {
            get
            {
                return Pointer != IntPtr.Zero;
            }
        }

        public WoWObjectType Type
        {
            get
            {
                return (WoWObjectType)GetDescriptor<uint>((int)ObjectField.OBJECT_FIELD_TYPE);
            }
        }

        public ulong Guid
        {
            get
            {
                return GetDescriptor<ulong>((int) ObjectField.OBJECT_FIELD_GUID);                
            }
        }

        public ulong GuildId
        {
            get
            {
                return GetDescriptor<ulong>((int) ObjectField.OBJECT_FIELD_DATA);
            }
        }

        public uint Data
        {
            get
            {
                return GetDescriptor<uint>((int) ObjectField.OBJECT_FIELD_ENTRY);
            }
        }

        public float Distance
        {
            get
            { 
                var local = Manager.LocalPlayer;
                if (local == null || !local.IsValid)
                    return float.NaN;
                return (float)local.Location.DistanceTo(Location);
            }
        }

        protected unsafe T GetDescriptor<T>(int offset)

        {
            uint descriptorArray = *(uint*) (Pointer + Offsets.DescriptorOffset);
            int size = Marshal.SizeOf(typeof (T));
            object ret = null;
            switch (size)
            {
                case 1:
                    ret = *(byte*) (descriptorArray + offset);
                    break;

                case 2:
                    ret = *(short*) (descriptorArray + offset);
                    break;

                case 4:
                    ret = *(uint*) (descriptorArray + offset);
                    break;

                case 8:
                    ret = *(ulong*) (descriptorArray + offset);
                    break;
            }
            return (T) ret;
        }

        public bool IsContainer { get { return Type.HasFlag(WoWObjectType.Container); } }
        public bool IsCorpse { get { return Type.HasFlag(WoWObjectType.Corpse); } }
        public bool IsGameObject { get { return Type.HasFlag(WoWObjectType.GameObject); } }
        public bool IsDynamicObject { get { return Type.HasFlag(WoWObjectType.DynamicObject); } }
        public bool IsUnit { get { return Type.HasFlag(WoWObjectType.Unit); } }
        public bool IsPlayer { get { return Type.HasFlag(WoWObjectType.Player); } }
        public bool IsItem { get { return Type.HasFlag(WoWObjectType.Item); } }

        public override string ToString()
        {
            return "[\"" + Name + "\", Distance = " + (int) Distance + ", Type = " + Type + "]";
        }
    }

}