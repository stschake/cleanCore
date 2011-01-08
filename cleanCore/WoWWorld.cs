using System.Runtime.InteropServices;

namespace cleanCore
{
    
    public enum TracelineResult
    {
        Collided,
        NoCollision
    }

    public static class WoWWorld
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool TracelineDelegate(
            ref Location start, ref Location end, out Location result, ref float distanceTravelled, uint flags,
            uint zero);
        private static TracelineDelegate _traceline;

        public static void Initialize()
        {
            _traceline = Helper.Magic.RegisterDelegate<TracelineDelegate>(Offsets.Traceline);
        }

        public static TracelineResult Traceline(Location start, Location end, uint flags)
        {
            float dist = 1.0f;
            Location result;
            return _traceline(ref start, ref end, out result, ref dist, flags, 0)
                       ? TracelineResult.Collided
                       : TracelineResult.NoCollision;
        }

        public static TracelineResult Traceline(Location start, Location end)
        {
            return Traceline(start, end, 0x120171);
        }

        public static TracelineResult LineOfSightTest(Location start, Location end)
        {
            start.Z += 2;
            end.Z += 2;
            return Traceline(start, end, 0x1000024);
        }

        public static uint CurrentMapId
        {
            get { return Helper.Magic.Read<uint>(Offsets.CurrentMapId); }
        }

        public static string CurrentMap
        {
            get
            {
                // we should do this properly using Map.dbc
                // this can presumably also be used for phases, but its rather useless because its our current zone, and we need to know all phases beforehand
                switch (CurrentMapId)
                {
                    case 0:
                        return "Azeroth";
                    case 1:
                        return "Kalimdor";
                    case 571:
                        return "Northrend";
                    case 530:
                        return "Expansion01";
                    case 34:
                        return "StormwindJail";
                    case 43:
                        return "WailingCaverns";
                    case 47:
                        return "RazorfenKraulInstance";
                    case 48:
                        return "Blackfathom";
                    case 70:
                        return "Uldaman";
                    case 90:
                        return "GnomeragonInstance";
                    case 109:
                        return "SunkenTemple";
                    case 129:
                        return "RazorfenDowns";
                    case 189:
                        return "MonasteryInstances";
                    case 209:
                        return "TanarisInstance";
                    case 389:
                        return "OrgrimmarInstance";
                }
                return null;
            }
        }

    }

}