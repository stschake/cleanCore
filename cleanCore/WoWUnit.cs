using System;
using System.Runtime.InteropServices;

namespace cleanCore
{
    
    public class WoWUnit : WoWObject
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate bool HasAuraDelegate(IntPtr thisObj, int spellId);
        private static HasAuraDelegate _hasAura;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UnitReactionDelegate(IntPtr thisObj, IntPtr unitToCompare);
        private static UnitReactionDelegate _unitReaction;

        public WoWUnit(IntPtr pointer)
            : base(pointer)
        {

        }

        public UnitReaction Reaction
        {
            get
            {
                if (_unitReaction == null)
                    _unitReaction = Helper.Magic.RegisterDelegate<UnitReactionDelegate>(Offsets.UnitReaction);
                return (UnitReaction) _unitReaction(Pointer, Manager.LocalPlayer.Pointer);
            }
        }

        public bool IsFriendly
        {
            get { return (int) Reaction > (int) UnitReaction.Neutral; }
        }

        public bool IsNeutral
        {
            get { return Reaction == UnitReaction.Neutral; }
        }

        public bool IsHostile
        {
            get { return (int) Reaction < (int) UnitReaction.Neutral; }
        }

        public bool HasAura(int spellId)
        {
            if (_hasAura == null)
                _hasAura = Helper.Magic.RegisterDelegate<HasAuraDelegate>(Offsets.HasAuraBySpellId);
            return _hasAura(Pointer, spellId);
        }

        private unsafe uint MovementData
        {
            get
            {
                return *(uint*)((uint)Pointer + Offsets.Teleport.UnitMovementData);
            }
        }

        public void SetLocation(Location newLoc)
        {
            var loc = new Location { X = newLoc.X, Y = newLoc.Y, Z = newLoc.Z };
            Helper.Magic.WriteStruct(new IntPtr(MovementData + Offsets.Teleport.MovementDataPosition), loc);
        }

        public ulong TargetGuid
        {
            get
            {
                return GetDescriptor<ulong>((int) UnitField.UNIT_FIELD_TARGET);
            }
        }

        public WoWObject Target
        {
            get
            {
                return Manager.GetObjectByGuid(TargetGuid);
            }
        }

        public bool IsDead
        {
            get { return Health <= 0; }
        }

        public WoWRace Race
        {
            get { return (WoWRace) GetDescriptor<byte>((int) UnitField.UNIT_FIELD_BYTES_0); }
        }

        public WoWClass Class
        {
            get { return (WoWClass)GetDescriptor<byte>((int) UnitField.UNIT_FIELD_BYTES_0 + 1); }
        }

        public bool IsLootable
        {
            get { return (DynamicFlags & 1) != 0; }
        }

        public bool IsTapped
        {
            get { return (DynamicFlags & 4) != 0; }
        }

        public bool IsTappedByMe
        {
            get { return (DynamicFlags & 8) != 0; }
        }

        public uint Health
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_HEALTH);
            }
        }

        public uint MaxHealth
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_MAXHEALTH);
            }
        }

        public double HealthPercentage
        {
            get
            {
                return (Health/(double) MaxHealth)*100; 
            }
        }

        public uint Level
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_LEVEL);
            }
        }

        public uint Flags
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_FLAGS);
            }
        }

        public uint Flags2
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_FLAGS_2);
            }
        }

        public uint NpcFlags
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_NPC_FLAGS);
            }
        }

        public uint DynamicFlags
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_DYNAMIC_FLAGS);
            }
        }

        public uint Faction
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_FACTIONTEMPLATE);
            }
        }

        public uint BaseAttackTime
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_BASEATTACKTIME);
            }
        }

        public uint RangedAttackTime
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_RANGEDATTACKTIME);
            }
        }

        public float BoundingRadius
        {
            get
            {
                return GetDescriptor<float>((int) UnitField.UNIT_FIELD_BOUNDINGRADIUS);
            }
        }

        public float CombatReach
        {
            get
            {
                return GetDescriptor<float>((int) UnitField.UNIT_FIELD_COMBATREACH);
            }
        }

        public uint DisplayId
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_DISPLAYID);
            }
        }

        public uint MountDisplayId
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_MOUNTDISPLAYID);
            }
        }

        public uint NativeDisplayId
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_NATIVEDISPLAYID);
            }
        }

        public uint MinDamage
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_MINDAMAGE);
            }
        }

        public uint MaxDamage
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_MAXDAMAGE);
            }
        }

        public uint MinOffhandDamage
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_MINOFFHANDDAMAGE);
            }
        }

        public uint MaxOffhandDamage
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_MAXOFFHANDDAMAGE);
            }
        }

        public uint PetExperience
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_PETEXPERIENCE);
            }
        }

        public uint PetNextLevelExperience
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_PETNEXTLEVELEXP);
            }
        }

        public uint BaseMana
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_BASE_MANA);
            }
        }

        public uint BaseHealth
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_BASE_HEALTH);
            }
        }

        public uint AttackPower
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_ATTACK_POWER);
            }
        }

        public uint RangedAttackPower
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_RANGED_ATTACK_POWER);
            }
        }

        public uint MinRangedDamage
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_MINRANGEDDAMAGE);
            }
        }

        public uint MaxRangedDamage
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_MAXRANGEDDAMAGE);
            }
        }

        public uint MaxItemLevel
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_MAXITEMLEVEL);
            }
        }

        public uint Mana
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_POWER1);
            }
        }

        public uint Rage
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_POWER2);
            }
        }

        public uint Focus
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_POWER3);
            }
        }

        public uint Happiness
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_POWER4);
            }
        }

        public uint Runes
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_POWER5);
            }
        }

        public uint RunicPower
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_POWER6);
            }
        }

        public uint SoulShards
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_POWER7);
            }
        }

        public uint Eclipse
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_POWER8);
            }
        }

        public uint HolyPower
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_POWER9);
            }
        }

        public uint Alternate
        {
            get
            {
                return GetDescriptor<uint>((int) UnitField.UNIT_FIELD_POWER10);
            }
        }

        public uint MaxMana
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER1);
            }
        }

        public uint MaxRage
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER2);
            }
        }

        public uint MaxFocus
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER3);
            }
        }

        public uint MaxHappiness
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER4);
            }
        }

        public uint MaxRunes
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER5);
            }
        }

        public uint MaxRunicPower
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER6);
            }
        }

        public uint MaxSoulShards
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER7);
            }
        }

        public uint MaxEclipse
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER8);
            }
        }

        public uint MaxHolyPower
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER9);
            }
        }

        public uint MaxAlternate
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER10);
            }
        }

        public override string ToString()
        {
            return "[\"" + Name + "\", Distance = " + (int)Distance + ", Type = " + Type + ", React = " + Reaction + "]";
        }
    }

}