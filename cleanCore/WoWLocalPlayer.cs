using System;
using System.Runtime.InteropServices;

namespace cleanCore
{
    
    public class WoWLocalPlayer : WoWPlayer
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int ClickToMoveDelegate(
            IntPtr thisPointer, int clickType, ref ulong interactGuid, ref Location clickLocation, float precision);
        public static ClickToMoveDelegate ClickToMoveFunction;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void SetFacingDelegate(IntPtr thisObj, uint time, float facing);
        private static SetFacingDelegate _setFacing;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate bool IsClickMovingDelegate(IntPtr thisObj);
        private static IsClickMovingDelegate _isClickMoving;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void StopCTMDelegate(IntPtr thisObj);
        private static StopCTMDelegate _stopCTM;

        public new static void Initialize()
        {
            WoWObject.Initialize();

            ClickToMoveFunction = Helper.Magic.RegisterDelegate<ClickToMoveDelegate>(Offsets.ClickToMove);
            _setFacing = Helper.Magic.RegisterDelegate<SetFacingDelegate>(Offsets.SetFacing);
            _isClickMoving = Helper.Magic.RegisterDelegate<IsClickMovingDelegate>(Offsets.IsClickMoving);
            _stopCTM = Helper.Magic.RegisterDelegate<StopCTMDelegate>(Offsets.StopCTM);
        }

        public WoWLocalPlayer(IntPtr pointer)
            : base(pointer)
        {
            
        }

        public void ClickToMove(Location target)
        {
            Helper.ResetHardwareAction();
            ulong guid = 0;
            ClickToMoveFunction(Pointer, 0x4, ref guid, ref target, 0.1f);
        }

        public void StopCTM()
        {
            _stopCTM(Pointer);
        }

        public void LookAt(Location loc)
        {
            var local = Location;
            var diffVector = new Location(loc.X - local.X, loc.Y - local.Y, loc.Z - local.Z);
            SetFacing(diffVector.Angle);
        }

        public void SetFacing(float angle)
        {
            const float pi2 = (float)(Math.PI * 2);
            if (angle < 0.0f)
                angle += pi2;
            if (angle > pi2)
                angle -= pi2;
            _setFacing(Pointer, Helper.PerformanceCount, angle);
        }

        public bool IsClickMoving
        {
            get { return _isClickMoving(Pointer); }
        }

        public Location Corpse
        {
            get
            {
                return Helper.Magic.ReadStruct<Location>(new IntPtr(Offsets.CorpsePosition));
            }
        }
    }

}