using System;
using System.IO;
using System.Reflection;
using cleanCore.D3D;
using cleanPattern;

namespace cleanCore
{
    
    public static class Offsets
    {
        public static event EventHandler OnOffsetsLoaded;

        public static int DescriptorOffset = 0x8;

        public static uint EnumVisibleObjects;
        public static uint GetObjectByGuid;
        public static uint GetLocalPlayerGuid;
        public static uint GetObjectName;
        public static uint IsLoading;
        public static uint GetObjectLocation;
        public static uint ClickToMove;
        public static uint PerformanceCounter;
        public static uint SelectObject;
        public static uint SetFacing;
        public static uint IsClickMoving;
        public static uint HasAuraBySpellId;
        public static uint CastSpell;
        public static uint Interact;
        public static uint UseItem;
        public static uint GetBag;
        public static uint StopCTM;
        public static uint Traceline;
        public static uint UnitReaction;

        public static uint EventVictim;
        public static uint LuaSetTop;
        public static uint LuaGetTop;
        public static uint LuaState;
        public static uint LuaLoadBuffer;
        public static uint LuaPCall;
        public static uint LuaType;
        public static uint LuaToNumber;
        public static uint LuaToLString;
        public static uint LuaToBoolean;

        public static uint PartyArray;
        public static uint CorpsePosition;
        public static uint LastHardwareAction;

        public static uint CurrentMapId;

        public static class AuctionHouse
        {
            public static uint ListBase;
            public static uint ListCount;
            public static uint OwnerBase;
            public static uint OwnerCount;
            public static uint BidderBase;
            public static uint BidderCount;
            public static uint AuctionSize;
            public static uint ExpireOffset;
        }

        public static class Teleport
        {
            public static uint OpcodeStart;
            public static uint OpcodeStop;
            public static uint SendMovementPacket;
            public static uint SendMoveSplineDone;
            public static uint UnitMovementData;
            public static uint MovementDataPosition;
            public static uint Singleton;
            public static uint CurMgrOffset;
            public static uint CurMgrGlobalMovement;
            public static uint GlobalMovementHeartbeat;
        }

        public static void Initialize()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf('\\') + 1);

            using (var log = new StreamWriter(path + "Offsets.log"))
            {

                {
                    var p = Pattern.FromTextstyle("EnumVisibleObjects",
                                                  "55 8b ec a1 ?? ?? ?? ?? 64 8b 0d ?? ?? ?? ?? 53 56 57 8b 3c 81 8b 87 ?? ?? ?? ?? 05 ?? ?? ?? ?? 8b 40 ?? a8 ??");
                    EnumVisibleObjects = p.Find();
                    log.WriteLine("EnumVisibleObjects: 0x" + EnumVisibleObjects.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("GetObjectByGuid",
                                                  "55 8b ec 64 8b 0d ?? ?? ?? ?? a1 ?? ?? ?? ?? 8b 14 81 8b 8a ?? ?? ?? ?? 83 ec ?? 85 c9 74 ?? 8b 45 ?? 8b 55 ?? 56 8b f0");
                    GetObjectByGuid = p.Find();
                    log.WriteLine("GetObjectByGuid: 0x" + GetObjectByGuid.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("GetLocalPlayerGuid",
                                                  "64 8b 0d ?? ?? ?? ?? a1 ?? ?? ?? ?? 8b 14 81 8b 8a ?? ?? ?? ?? 85 c9 75 ?? 33 c0 33 d2 c3 8b 81 ?? ?? ?? ?? 8b 91 ?? ?? ?? ?? c3");
                    GetLocalPlayerGuid = p.Find();
                    log.WriteLine("GetLocalPlayerGuid: 0x" + GetLocalPlayerGuid.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("GetObjectName",
                                                  "dd 5c 24 ?? d9 45 ?? dd 5c 24 ?? d9 45 ?? dd 1c 24 50 8b 06 8b 90 ?? ?? ?? ?? 51 8b ce ff d2 50 8b 45 ?? 50");
                    p.Modifiers.Add(new AddModifier(22));
                    p.Modifiers.Add(new LeaModifier());
                    GetObjectName = p.Find();
                    log.WriteLine("GetObjectName: 0x" + GetObjectName.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("IsLoading",
                                                  "52 8d 85 ?? ?? ?? ?? 50 e8 ?? ?? ?? ?? 83 c4 ?? 85 c0 8d 4d ?? a3 ?? ?? ?? ?? 0f 95 c3");
                    p.Modifiers.Add(new AddModifier(22));
                    p.Modifiers.Add(new LeaModifier());
                    IsLoading = p.Find();
                    log.WriteLine("IsLoading: 0x" + IsLoading.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("GetObjectLocation",
                                                  "85 c0 0f 84 ?? ?? ?? ?? 8b 06 8b 50 ?? 8d 4d ?? 51 8b ce ff d2 d9 45 ?? 8b 46 ??");
                    p.Modifiers.Add(new AddModifier(12));
                    p.Modifiers.Add(new LeaModifier(LeaType.Byte));
                    GetObjectLocation = p.Find();
                    log.WriteLine("GetObjectLocation: 0x" + GetObjectLocation.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("LuaState",
                                                  "8b 35 ?? ?? ?? ?? 57 8b f9 8b 47 ?? 85 c0 7e ?? 83 c0 ?? 89 47 ?? 75 ?? 8b 47 ?? 50 68 ?? ?? ?? ?? 56 e8 ?? ?? ?? ?? d9 ee 83 c4");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier());
                    LuaState = p.Find();
                    log.WriteLine("LuaState: 0x" + LuaState.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("FrameScript_GetTop",
                                                  "55 8b ec 8b 4d ?? 8b 41 ?? 2b 41 ?? c1 f8 ?? 5d c3");
                    LuaGetTop = p.Find();
                    log.WriteLine("LuaGetTop: 0x" + LuaGetTop.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("FrameScript_LoadBuffer",
                                                  "55 8b ec 8b 4d ?? 8b 45 ?? 83 ec ?? 83 f9 ?? 72 ?? 80 38 ?? 75 ?? 80 78 ?? ?? 75 ?? 80 78 ?? ?? 75 ?? 83 c0 ?? 83 e9");
                    LuaLoadBuffer = p.Find();
                    log.WriteLine("LuaLoadBuffer: 0x" + LuaLoadBuffer.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("FrameScript_PCall",
                                                  "55 8b ec 8b 45 ?? 83 ec ?? 56 8b 75 ?? 85 c0 75 ?? 33 c9 eb ?? 8b ce e8 ?? ?? ?? ?? 2b 46 ?? 8b c8 8b 45 ?? 40 c1 e0");
                    LuaPCall = p.Find();
                    log.WriteLine("LuaPCall: 0x" + LuaPCall.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("FrameScript_Type",
                                                  "55 8b ec 8b 45 ?? 8b 4d ?? e8 ?? ?? ?? ?? 3d ?? ?? ?? ?? 75 ?? 83 c8 ?? 5d c3 8b 40 ?? 5d c3");
                    LuaType = p.Find();
                    log.WriteLine("LuaType: 0x" + LuaType.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("FrameScript_ToNumber",
                                                  "55 8b ec 8b 45 ?? 8b 4d ?? 83 ec ?? e8 ?? ?? ?? ?? 83 78 ?? ?? 74 ?? 8d 4d ?? 51 50 e8 ?? ?? ?? ?? 83 c4 ?? 85 c0 75 ?? d9 ee 8b e5 5d c3 dd 00 8b e5 5d c3");
                    LuaToNumber = p.Find();
                    log.WriteLine("LuaToNumber: 0x" + LuaToNumber.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("FrameScript_ToLString",
                                                  "55 8b ec 56 8b 75 ?? 57 8b 7d ?? 8b c7 8b ce e8 ?? ?? ?? ?? 83 78 ?? ?? 74 ?? 50 56 e8 ?? ?? ?? ?? 83 c4 ?? 85 c0 75 ?? 8b 45 ?? 85 c0 74 ?? c7 00 ?? ?? ?? ?? 5f 33 c0 5e 5d c3");
                    LuaToLString = p.Find();
                    log.WriteLine("LuaToLString: 0x" + LuaToLString.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("FrameScript_ToBoolean",
                                                  "55 8b ec 8b 45 ?? 8b 4d ?? e8 ?? ?? ?? ?? 8b 48 ?? 85 c9 74 ?? 83 f9 ?? 75 ?? 83 38 ?? 74 ?? b8 ?? ?? ?? ?? 5d c3");
                    LuaToBoolean = p.Find();
                    log.WriteLine("LuaToBoolean: 0x" + LuaToBoolean.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("FrameScript__SetTop",
                                                  "55 8b ec 8b 4d ?? 8b 45 ?? 85 c9 7c ?? c1 e1 ?? 8b d1 8b 48 ?? 03 ca 39 48 ?? 73 ?? 57 8d 49 ?? 8b 48 ?? 8d 79 ?? 89 78 ?? 8b 3d ?? ?? ?? ?? 89 79 ?? c7 41");
                    LuaSetTop = p.Find();
                    log.WriteLine("LuaSetTop: 0x" + LuaSetTop.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("PartyArray",
                                                  "55 8b ec 51 8b 0d ?? ?? ?? ?? 33 c0 0b 0d ?? ?? ?? ?? 74 ?? b8 ?? ?? ?? ?? 8b 15 ?? ?? ?? ?? 0b 15 ?? ?? ?? ?? 74 ?? 40 8b 0d ?? ?? ?? ?? 0b 0d ?? ?? ?? ?? 74 ?? 40");
                    p.Modifiers.Add(new AddModifier(6));
                    p.Modifiers.Add(new LeaModifier());
                    PartyArray = p.Find();
                    log.WriteLine("PartyArray: 0x" + PartyArray.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("lua_GetBillingTimeRested",
                                                  "55 8b ec 51 e8 ?? ?? ?? ?? 8b 80 ?? ?? ?? ?? 89 45 ?? db 45 ?? 85 c0 7d ?? dc 05 ?? ?? ?? ??");
                    EventVictim = p.Find();
                    log.WriteLine("EventVictim: 0x" + EventVictim.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("CorpsePosition",
                                                  "8b 75 ?? 89 35 ?? ?? ?? ?? 8b 08 89 0d ?? ?? ?? ?? 8b 50 ?? 8b 4d ?? 89 15 ?? ?? ?? ?? 8b 40 ?? a3 ?? ?? ?? ?? 8b 45 ?? 89 0d ?? ?? ?? ?? 33 c9 3b c1 74 ?? 99 81 ca");
                    p.Modifiers.Add(new AddModifier(13));
                    p.Modifiers.Add(new LeaModifier());
                    CorpsePosition = p.Find();
                    log.WriteLine("CorpsePosition: 0x" + CorpsePosition.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("CGPlayer_C__ClickToMove",
                                                  "55 8b ec 83 ec ?? 53 56 8b f1 8b 46 ?? 8b 08 57 3b 0d ?? ?? ?? ?? 75 ?? 8b 50 ?? 3b 15 ?? ?? ?? ?? 75 ?? a1 ?? ?? ?? ?? 83 f8 ?? 74 ?? 33 ff 83 f8 ?? 75 ?? 57 68");
                    ClickToMove = p.Find();
                    log.WriteLine("ClickToMove: 0x" + ClickToMove.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("PerformanceCounter",
                                                  "8d 8d ?? ?? ?? ?? 51 68 ?? ?? ?? ?? ff 15 ?? ?? ?? ?? 85 c0 0f 84 ?? ?? ?? ?? e8 ?? ?? ?? ?? 50 8d 95 ?? ?? ?? ?? 52 68 ?? ?? ?? ?? 8d 95 ?? ?? ?? ?? e8 ?? ?? ?? ?? 8d 85 ?? ?? ?? ?? 68 ?? ?? ?? ?? 50 e8 ?? ?? ?? ?? 83 c4");
                    p.Modifiers.Add(new AddModifier(27));
                    uint relStart = p.Find() - 1;
                    p.Modifiers.Add(new LeaModifier());
                    uint offset = p.Find();
                    PerformanceCounter = offset + relStart + 5;
                    log.WriteLine("PerformanceCounter: 0x" + PerformanceCounter.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("LastHardwareAction",
                                                  "8b 1d ?? ?? ?? ?? 57 8d be ?? ?? ?? ?? 7e ?? 8b 86 ?? ?? ?? ?? 8b 80 ?? ?? ?? ?? 85 c0 74 ?? f7 40 ?? ?? ?? ?? ?? 74 ?? 8b ce e8 ?? ?? ?? ?? 85 c0 75 ??");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier());
                    LastHardwareAction = p.Find();
                    log.WriteLine("LastHardwareAction: 0x" + LastHardwareAction.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("SelectObject",
                                                  "55 8b ec 81 ec ?? ?? ?? ?? e8 ?? ?? ?? ?? 85 c0 74 ?? 80 3d ?? ?? ?? ?? ?? 75 ?? 33 c0");
                    SelectObject = p.Find();
                    log.WriteLine("SelectObject: 0x" + SelectObject.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("SetFacing",
                                                  "55 8b ec 56 8b f1 56 e8 ?? ?? ?? ?? 8b c8 e8 ?? ?? ?? ?? 85 c0 74 ?? d9 45 ?? 83 0d ?? ?? ?? ?? ?? 51 8b 8e ?? ?? ?? ?? d9 1c 24 e8 ?? ?? ?? ?? 8b 45 ?? 51 d9 1c 24 50 8b ce e8 ?? ?? ?? ?? 83 25");
                    SetFacing = p.Find();
                    log.WriteLine("SetFacing: 0x" + SetFacing.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("lua_GetInstanceInfo",
                                                  "8b 1d ?? ?? ?? ?? 3b d8 56 0f 8c ?? ?? ?? ?? 3b 1d ?? ?? ?? ?? 0f 8f ?? ?? ?? ?? 8b 15 ?? ?? ?? ?? 8b cb 2b c8 8b 34 8a");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier());
                    CurrentMapId = p.Find();
                    log.WriteLine("CurrentMapId: 0x" + CurrentMapId.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("GetNumAuctionItems",
                                                  "8b 0d ?? ?? ?? ?? db 05 ?? ?? ?? ?? 85 c9 7d ?? dc 05 ?? ?? ?? ?? 83 ec ?? dd 1c 24 56 e8");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier());
                    AuctionHouse.ListCount = p.Find();
                    AuctionHouse.ListBase = AuctionHouse.ListCount + 4;
                    AuctionHouse.OwnerCount = AuctionHouse.ListBase + 12;
                    AuctionHouse.OwnerBase = AuctionHouse.OwnerCount + 4;
                    AuctionHouse.BidderCount = AuctionHouse.OwnerBase + 12;
                    AuctionHouse.BidderBase = AuctionHouse.BidderCount + 4;
                    log.WriteLine("AuctionHouse:");
                    log.WriteLine("\tListCount: 0x" + AuctionHouse.ListCount.ToString("X"));
                    log.WriteLine("\tListBase: 0x" + AuctionHouse.ListBase.ToString("X"));
                    log.WriteLine("\tOwnerCount: 0x" + AuctionHouse.OwnerBase.ToString("X"));
                    log.WriteLine("\tOwnerBase: 0x" + AuctionHouse.OwnerCount.ToString("X"));
                    log.WriteLine("\tBidderCount: 0x" + AuctionHouse.BidderBase.ToString("X"));
                    log.WriteLine("\tBidderBase: 0x" + AuctionHouse.BidderCount.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("AuctionSize",
                                                  "69 f6 ?? ?? ?? ?? 03 35 ?? ?? ?? ?? eb ?? 68 ?? ?? ?? ?? 57 e8 ?? ?? ?? ?? 83 c4");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier());
                    AuctionHouse.AuctionSize = p.Find();
                    log.WriteLine("\tAuctionSize: 0x" + AuctionHouse.AuctionSize.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("ExpireOffset", "8b 8e ?? ?? ?? ?? 8b f8 2b c1 78 ?? d9 e8 83 ec");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier());
                    AuctionHouse.ExpireOffset = p.Find();
                    log.WriteLine("\tExpireOffset: 0x" + AuctionHouse.ExpireOffset.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("OpcodeStop", "05 ?? ?? ?? ?? 5b 5d c2 ?? ?? b8");
                    p.Modifiers.Add(new AddModifier(11));
                    p.Modifiers.Add(new LeaModifier());
                    Teleport.OpcodeStop = p.Find();
                    log.WriteLine("Teleport:");
                    log.WriteLine("\tOpcodeStop: 0x" + Teleport.OpcodeStop.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("OpcodeStart", "33 5d ?? f6 c3 ?? 74 ?? f6 c2 ?? 74 ?? b8");
                    p.Modifiers.Add(new AddModifier(14));
                    p.Modifiers.Add(new LeaModifier());
                    Teleport.OpcodeStart = p.Find();
                    log.WriteLine("\tOpcodeStart: 0x" + Teleport.OpcodeStart.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("CGUnit_C__WriteMovementPacketWithTransport",
                                                  "55 8b ec 83 ec ?? 53 56 8b f1 8b 06 8b 50 ?? 57 33 ff c7 45 ?? ?? ?? ?? ?? 89 7d");
                    Teleport.SendMovementPacket = p.Find();
                    log.WriteLine("\tSendMovementPacket: 0x" + Teleport.SendMovementPacket.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("CGUnit_C__Send_CMSG_MOVE_SPLINE_DONE",
                                                  "55 8b ec 83 ec ?? d9 ee 53 56 57 8b 7d ?? 33 db");
                    Teleport.SendMoveSplineDone = p.Find();
                    log.WriteLine("\tSendMoveSplineDone: 0x" + Teleport.SendMoveSplineDone.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("UnitMovementInfo",
                                                  "8b 89 ?? ?? ?? ?? 8b 51 ?? 8b 45 ?? 89 10 8b 51 ?? 8b 49");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier());
                    Teleport.UnitMovementData = p.Find();
                    log.WriteLine("\tUnitMovementData: 0x" + Teleport.UnitMovementData.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("MovementDataPosition",
                                                  "8b 51 ?? 8b 45 ?? 89 10 8b 51 ?? 8b 49 ?? 89 50 ?? 89 48 ?? 5d");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier(LeaType.Byte));
                    Teleport.MovementDataPosition = p.Find();
                    log.WriteLine("\tMovementDataPosition: 0x" + Teleport.MovementDataPosition.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("MovementHeartbeat", "8d 8f ?? ?? ?? ?? a8 ?? 75 ?? 85 c0 75 ?? 8b c1");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier());
                    Teleport.GlobalMovementHeartbeat = p.Find();
                    log.WriteLine("\tGlobalMovementHeartbeat: 0x" + Teleport.GlobalMovementHeartbeat.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("GlobalMovement",
                                                  "64 8b 0d ?? ?? ?? ?? 8b 14 81 8b 82 ?? ?? ?? ?? 85 c0 74 ?? 8b 4d ?? 89 88 ?? ?? ?? ??");
                    p.Modifiers.Add(new AddModifier(25));
                    p.Modifiers.Add(new LeaModifier());
                    Teleport.CurMgrGlobalMovement = p.Find();
                    log.WriteLine("\tCurMgrGlobalMovement: 0x" + Teleport.CurMgrGlobalMovement.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("Singleton2",
                                                  "8b 15 ?? ?? ?? ?? 89 82 ?? ?? ?? ?? 89 81 ?? ?? ?? ?? 8b 0d ?? ?? ?? ?? 89 88 ?? ?? ?? ?? c7 81 ?? ?? ?? ?? ?? ?? ?? ?? c7 81 ?? ?? ?? ?? ?? ?? ?? ?? 8b 88");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier());
                    Teleport.Singleton = p.Find();
                    p.Modifiers.Clear();
                    p.Modifiers.Add(new AddModifier(8));
                    p.Modifiers.Add(new LeaModifier());
                    Teleport.CurMgrOffset = p.Find();
                    log.WriteLine("\tSingleton: 0x" + Teleport.Singleton.ToString("X"));
                    log.WriteLine("\tCurMgrOffset: 0x" + Teleport.CurMgrOffset.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("CGPlayer_C__IsClickMoving",
                                                  "8b 41 ?? 8b 08 3b 0d ?? ?? ?? ?? 75 ?? 8b 50 ?? 3b 15 ?? ?? ?? ?? 75 ?? 83 3d ?? ?? ?? ?? ?? 74 ?? b8");
                    IsClickMoving = p.Find();
                    log.WriteLine("IsClickMoving: 0x" + IsClickMoving.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("CGUnit_C__HasAuraBySpellId",
                                                  "55 8b ec 53 56 57 8b f1 33 ff e8 ?? ?? ?? ?? 85 c0 76 ?? 8b 5d ?? 33 d2");
                    HasAuraBySpellId = p.Find();
                    log.WriteLine("HasAuraBySpellId: 0x" + HasAuraBySpellId.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("Spell_C_CastSpell",
                                                  "55 8b ec e8 ?? ?? ?? ?? 68 ?? ?? ?? ?? 68 ?? ?? ?? ?? 6a ?? 52 50 e8 ?? ?? ?? ?? 8b 4d ?? 8b 55");
                    CastSpell = p.Find();
                    log.WriteLine("CastSpell: 0x" + CastSpell.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("Interact",
                                                  "8b 82 ?? ?? ?? ?? 8b ce ff d0 5f 5e b8 ?? ?? ?? ?? 5b 5d c3");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier());
                    Interact = p.Find();
                    log.WriteLine("Interact: 0x" + Interact.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("UseItem",
                                                  "55 8b ec 83 ec ?? 57 8b f9 8b 47 ?? 0f bf 48 ?? c1 e9 ?? f6 c1 ?? 0f 85 ?? ?? ?? ?? f6 87 ?? ?? ?? ?? ?? 0f 85");
                    UseItem = p.Find();
                    log.WriteLine("UseItem: 0x" + UseItem.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("CGPlayer_C__AutoEquipItem",
                                                  "8b 42 ?? 8b ce ff d0 85 c0 74 ?? 8b 16 8b 42 ?? 53 8b ce ff d0");
                    p.Modifiers.Add(new AddModifier(2));
                    p.Modifiers.Add(new LeaModifier(LeaType.Byte));
                    GetBag = p.Find();
                    log.WriteLine("GetBag: 0x" + GetBag.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("CGPlayer_C__ClickToMoveStop",
                                                  "a1 ?? ?? ?? ?? 83 c0 ?? 56 8b f1 83 f8 ?? 0f 87 ?? ?? ?? ?? ff 24 85 ?? ?? ?? ?? 8b 0d");
                    StopCTM = p.Find();
                    log.WriteLine("StopCTM: 0x" + StopCTM.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("TracelineWrapper",
                                                  "55 8b ec a1 ?? ?? ?? ?? 8b 0c 85 ?? ?? ?? ?? 85 c9 75 ?? 32 c0 5d c3 8b 55 ?? 8b 45 ?? 52 8b 55 ?? 50 8b 45 ?? 52");
                    Traceline = p.Find();
                    log.WriteLine("Traceline: 0x" + Traceline.ToString("X"));
                }

                {
                    var p = Pattern.FromTextstyle("CGUnit_C__UnitReaction",
                                                  "55 8b ec 83 ec 14 53 57 8b 7d 08 8b d9 85 ff 75 ?? 8d 47 01 5f 5b 8b e5 5d c2 04 00 3b fb 75 ??");
                    UnitReaction = p.Find();
                    log.WriteLine("UnitReaction: 0x" + UnitReaction.ToString("X"));
                }

                WoWLocalPlayer.Initialize();
                LuaInterface.Initialize();
                Manager.Initialize();
                Helper.Initialize();
                Pulse.Initialize();
                Events.Initialize();
                Teleporter.Initialize();
                WoWWorld.Initialize();

                log.Flush();
            }

            if (OnOffsetsLoaded != null)
                OnOffsetsLoaded(null, new EventArgs());
        }
    }

}