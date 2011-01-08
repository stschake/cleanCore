using System;

namespace cleanCore
{

    public enum UnitReaction
    {
        ExceptionallyHostile,
        VeryHostile,
        Hostile,
        Neutral,
        Friendly,
        VeryFriendly,
        ExceptionallyFriendly,
        Exalted
    }

    public enum WoWClass
    {
        Warrior = 1,
        Paladin,
        Hunter,
        Rogue,
        Priest,
        DeathKnight,
        Shaman,
        Mage,
        Warlock,
        Druid = 11
    }

    public enum WoWRace
    {
        Human = 1,
        Orc,
        Dwarf,
        NightElf,
        Undead,
        Tauren,
        Gnome,
        Troll,
        Goblin,
        BloodElf,
        Draenei,
        FelOrc,
        Naga,
        Broken,
        Skeleton,
        Vrykul,
        Tuskarr,
        ForestTroll,
        Taunka,
        NorthrendSkeleton,
        IceTroll,
        Worgen,
        Gilnean
    }

    [Flags]
    public enum WoWObjectType
    {
        Object = 0x1,
        Item = 0x2,
        Container = 0x4,
        Unit = 0x8,
        Player = 0x10,
        GameObject = 0x20,
        DynamicObject = 0x40,
        Corpse = 0x80,
        AiGroup = 0x100,
        AreaTrigger = 0x200,
    }

    [Flags]
    public enum GameObjectFlags : ushort
    {
        None = 0x00000000,
        InUse = 0x00000001,
        Locked = 0x00000002,
        InteractionCondition = 0x00000004,
        Transport = 0x00000008,
        Unknown1 = 0x00000010,
        NeverDespawn = 0x00000020,
        Triggered = 0x00000040,
        Unknown2 = 0x00000080,
        Unknown3 = 0x00000100,
        Damaged = 0x00000200,
        Destroyed = 0x00000400
    }

    [Flags]
    public enum ItemFlags : uint
    {
        None = 0x00000000,
        Unknown1 = 0x00000001,
        Conjured = 0x00000002,
        Openable = 0x00000004,
        Heroic = 0x00000008,
        Deprecated = 0x00000010,
        Indestructible = 0x00000020,
        Consumable = 0x00000040,
        NoEquipCooldown = 0x00000080,
        Unknown2 = 0x00000100,
        Wrapper = 0x00000200,
        Unknown3 = 0x00000400,
        PartyLoot = 0x00000800,
        Refundable = 0x00001000,
        Charter = 0x00002000,
        Unknown4 = 0x00004000,
        Unknown5 = 0x00008000,
        Unknown6 = 0x00010000,
        Unknown7 = 0x00020000,
        Prospectable = 0x00040000,
        UniqueEquip = 0x00080000,
        Unknown8 = 0x00100000,
        UsableInArena = 0x00200000,
        Throwable = 0x00400000,
        UsableWhenShapeshifted = 0x00800000,
        Unknown9 = 0x01000000,
        SmartLoot = 0x02000000,
        UnusableInArena = 0x04000000,
        BindToAccount = 0x08000000,
        TriggeredCast = 0x10000000,
        Millable = 0x20000000,
        Unknown10 = 0x40000000,
        Unknown11 = 0x80000000,
    }

}