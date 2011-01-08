using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace cleanCore
{
    
    public static class Manager
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint EnumVisibleObjectsDelegate(IntPtr callback, int filter);
        private static EnumVisibleObjectsDelegate _enumVisibleObjects;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int EnumVisibleObjectsCallback(ulong guid, uint filter);
        private static IntPtr _ourCallback;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr GetObjectByGuidDelegate(ulong guid, int filter);
        private static GetObjectByGuidDelegate _getObjectByGuid;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong GetLocalPlayerDelegate();
        private static GetLocalPlayerDelegate _getLocalPlayer;

        private static readonly EnumVisibleObjectsCallback _callback = Callback;
        private static readonly Dictionary<ulong, WoWObject> _objects = new Dictionary<ulong, WoWObject>();

        public static WoWLocalPlayer LocalPlayer { get; private set; }
        public static List<WoWObject> Objects { get; private set; }

        private unsafe static IntPtr CurMgr
        {
            get
            {
                uint singleton = *(uint*)Offsets.Teleport.Singleton;
                if (singleton == 0)
                    return IntPtr.Zero;
                uint* ret = *(uint**)(singleton + Offsets.Teleport.CurMgrOffset);
                return new IntPtr(ret);
            }
        }

        public static unsafe void SetHeartbeatTime(uint next)
        {
            *(uint*)(GetGlobalMovement() + Offsets.Teleport.GlobalMovementHeartbeat) = next;
        }

        private static unsafe uint GetGlobalMovement()
        {
            return *(uint*)((uint)CurMgr + Offsets.Teleport.CurMgrGlobalMovement);
        }

        public static void Initialize()
        {
            _enumVisibleObjects = Helper.Magic.RegisterDelegate<EnumVisibleObjectsDelegate>(Offsets.EnumVisibleObjects);
            _getObjectByGuid = Helper.Magic.RegisterDelegate<GetObjectByGuidDelegate>(Offsets.GetObjectByGuid);
            _getLocalPlayer = Helper.Magic.RegisterDelegate<GetLocalPlayerDelegate>(Offsets.GetLocalPlayerGuid);
            _ourCallback = Marshal.GetFunctionPointerForDelegate(_callback);
        }

        public static WoWObject GetObjectByGuid(ulong guid)
        {
            if (_objects.ContainsKey(guid))
                return _objects[guid];
            return null;
        }
        
        public static bool IsInGame
        {
            get
            {
                return LocalPlayer != null;
            }
        }

        public static void Pulse()
        {
            var localPlayerGuid = _getLocalPlayer();
            if (localPlayerGuid == 0)
                return;
            var localPlayerPointer = _getObjectByGuid(localPlayerGuid, -1);
            if (localPlayerPointer == IntPtr.Zero)
                return;
            LocalPlayer = new WoWLocalPlayer(localPlayerPointer);

            foreach (var obj in _objects.Values)
                obj.Pointer = IntPtr.Zero;

            _enumVisibleObjects(_ourCallback, 0);

            foreach (var pair in _objects.Where(p => p.Value.Pointer == IntPtr.Zero).ToList())
                _objects.Remove(pair.Key);

            Objects = _objects.Values.ToList();
        }

        private static int Callback(ulong guid, uint filter)
        {
            var pointer = _getObjectByGuid(guid, -1);
            if (pointer == IntPtr.Zero)
                return 1;

            if (_objects.ContainsKey(guid))
                _objects[guid].Pointer = pointer;
            else
            {
                var obj = new WoWObject(pointer);
                var type = obj.Type;

                if (type.HasFlag(WoWObjectType.Player))
                    _objects.Add(guid, new WoWPlayer(pointer));
                else if (type.HasFlag(WoWObjectType.Unit))
                    _objects.Add(guid, new WoWUnit(pointer));
                else if (type.HasFlag(WoWObjectType.Container))
                    _objects.Add(guid, new WoWContainer(pointer));
                else if (type.HasFlag(WoWObjectType.Item))
                    _objects.Add(guid, new WoWItem(pointer));
                else if (type.HasFlag(WoWObjectType.Corpse))
                    _objects.Add(guid, new WoWCorpse(pointer));
                else if (type.HasFlag(WoWObjectType.GameObject))
                    _objects.Add(guid, new WoWGameObject(pointer));
                else if (type.HasFlag(WoWObjectType.DynamicObject))
                    _objects.Add(guid, new WoWDynamicObject(pointer));
                else
                    _objects.Add(guid, obj);
            }
            return 1;
        }
    }

}