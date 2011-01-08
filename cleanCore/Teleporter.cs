using System;
using System.Runtime.InteropServices;
using WhiteMagic.Internals;

namespace cleanCore
{

    public static class Teleporter
    {
        private const int StepTimeWait = 25; /*ms*/

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate uint SendMovementPacketDelegate(
            IntPtr instance, uint timestamp, uint opcode, uint zero, uint zero2, uint zero3, uint zero4, uint ff);
        private static SendMovementPacketDelegate _sendMovementPacket;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void SendSplineMoveDone(IntPtr instance, uint timestamp, uint unknown);
        private static SendSplineMoveDone _sendSplineMoveDone;

        private static Detour _sendSplineMoveDoneDetour;
        private static Detour _sendMovementPacketDetour;
        private static uint _timeAdvance;
        private static DateTime _lastFrame;
        private static bool _firstTeleport = true;
        public static Location? Destination { get; private set; }

        public static void Initialize()
        {
            _sendMovementPacket = Helper.Magic.RegisterDelegate<SendMovementPacketDelegate>(Offsets.Teleport.SendMovementPacket);
            _sendSplineMoveDone = Helper.Magic.RegisterDelegate<SendSplineMoveDone>(Offsets.Teleport.SendMoveSplineDone);
            _sendSplineMoveDoneDetour = Helper.Magic.Detours.Create(_sendSplineMoveDone,
                                                                      new SendSplineMoveDone(HandleSendSplineMoveDone),
                                                                      "TeleporterSendSplineMoveDone");
            _sendMovementPacketDetour = Helper.Magic.Detours.Create(_sendMovementPacket,
                                                                      new SendMovementPacketDelegate(
                                                                          HandleSendMovementPacket),
                                                                      "TeleporterSendMovementPacket");
        }

        private static void HandleSendSplineMoveDone(IntPtr instance, uint timestamp, uint unknown)
        {
            _sendSplineMoveDoneDetour.CallOriginal(new object[] { instance, timestamp + _timeAdvance, unknown });
        }

        private static uint HandleSendMovementPacket(IntPtr instance, uint timestamp, uint opcode, uint zero, uint zero2, uint zero3, uint zero4, uint ff)
        {
            var result = (uint)
                         _sendMovementPacketDetour.CallOriginal(new object[]
                                                                    {
                                                                        instance, timestamp + _timeAdvance, opcode,
                                                                        zero, zero2,
                                                                        zero3, zero4, ff
                                                                    });
            if (instance == Manager.LocalPlayer.Pointer)
                Manager.SetHeartbeatTime(timestamp + 500);

            return result;
        }

        public static void ClearDestination()
        {
            Destination = null;
        }

        public static void SetDestination(Location dest)
        {
            if (_firstTeleport)
            {
                _sendMovementPacketDetour.Apply();
                _sendSplineMoveDoneDetour.Apply();
                _firstTeleport = false;
            }
            _lastFrame = DateTime.Now - TimeSpan.FromMilliseconds(StepTimeWait);
            Destination = dest;
        }

        public static bool Pulse()
        {
            if (Destination == null)
                return false;

            if (Manager.LocalPlayer == null)
                return false;

            if ((_lastFrame + TimeSpan.FromMilliseconds(StepTimeWait)) > DateTime.Now)
                return true;

            _lastFrame = DateTime.Now;

            if (TeleportStep(Destination.Value))
            {
                ClearDestination();
                return true;
            }

            return false;
        }

        private static void AdvanceTime(uint amount)
        {
            _timeAdvance += amount;
        }

        private static void SendMovementPacket(uint timestamp, uint opcode, uint hidden)
        {
            if (Manager.LocalPlayer != null)
            {
                _sendMovementPacket(Manager.LocalPlayer.Pointer, timestamp, opcode, hidden, 0, 0, 0, 0xFF);
            }
        }

        private static float CalculateMaxMovement(uint ms, float limit)
        {
            return limit / 500 * ms;
        }

        private static bool TeleportStep(Location dest)
        {
            var local = Manager.LocalPlayer.Location;
            var direction = new Location(dest.X - local.X, dest.Y - local.Y, dest.Z - local.Z);

            var distance = direction.Length;
            if (distance > 3.2f)
            {
                var normal = direction.Normalize();
                var scalar = CalculateMaxMovement(450, 3.2f);
                direction = new Location(normal.X * scalar, normal.Y * scalar, normal.Z * scalar);
            }
            else if (distance < 0.5f)
                return true;

            MovePlayer(direction.X, direction.Y, direction.Z);
            return false;
        }

        private static void MovePlayer(float x, float y, float z)
        {
            if (Manager.LocalPlayer == null)
                return;

            uint lastTime = Helper.PerformanceCount;
            SendMovementPacket(lastTime, Offsets.Teleport.OpcodeStart, 0);

            var local = Manager.LocalPlayer.Location;
            local.X += x;
            local.Y += y;
            local.Z += z;
            Manager.LocalPlayer.SetLocation(local);

            AdvanceTime(450);
            SendMovementPacket(lastTime, Offsets.Teleport.OpcodeStop, 0);
        }
    }
}