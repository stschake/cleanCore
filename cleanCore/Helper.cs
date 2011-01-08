using System.Collections.Generic;
using System.Runtime.InteropServices;
using WhiteMagic;

namespace cleanCore
{
    
    public static class Helper
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint PerformanceCounterDelegate();
        private static PerformanceCounterDelegate _performanceCounter;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool CastSpellDelegate(int spellId, int unk, ulong targetGuid, int unk1, int unk2);
        private static CastSpellDelegate _castSpell;

        public static Magic Magic = new Magic();
        public static bool InCombat { get; private set; }

        public static void Initialize()
        {
            _performanceCounter = Magic.RegisterDelegate<PerformanceCounterDelegate>(Offsets.PerformanceCounter);
            _castSpell = Magic.RegisterDelegate<CastSpellDelegate>(Offsets.CastSpell);

            Events.Register("PLAYER_REGEN_DISABLED", SetInCombat);
            Events.Register("PLAYER_REGEN_ENABLED", UnsetInCombat);
        }

        public static void CastSpell(int spellId, WoWObject target)
        {
            target.Select();
            WoWScript.ExecuteNoResults("CastSpellByID(" + spellId + ")");
        }

        private static void SetInCombat(string ev, List<string> args)
        {
            InCombat = true;
        }

        private static void UnsetInCombat(string ev, List<string> args)
        {
            InCombat = false;
        }

        public static void ResetHardwareAction()
        {
            Magic.Write(Offsets.LastHardwareAction, PerformanceCount);
        }

        public static uint PerformanceCount
        {
            get
            {
                return _performanceCounter();
            }
        }
    }

}