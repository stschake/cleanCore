using System;

namespace  cleanCore
{

    public class WoWPlayer : WoWUnit
    {
         public WoWPlayer(IntPtr pointer)
             : base(pointer)
         {
             
         }

         public uint Experience
         {
             get
             {
                 return GetDescriptor<uint>((int)PlayerField.PLAYER_XP);
             }
         }

         public uint NextLevelExperience
         {
             get
             {
                 return GetDescriptor<uint>((int)PlayerField.PLAYER_NEXT_LEVEL_XP);
             }
         }
        
        public uint GuildRank
        {
            get
            {
                return GetDescriptor<uint>((int) PlayerField.PLAYER_GUILDRANK);
            }
        }

        public uint GuildLevel
        {
            get
            {
                return GetDescriptor<uint>((int) PlayerField.PLAYER_GUILDLEVEL);
            }
        }

        public float BlockPercentage
        {
            get
            {
                return GetDescriptor<float>((int) PlayerField.PLAYER_BLOCK_PERCENTAGE);
            }
        }

        public float DodgePercentage
        {
            get
            {
                return GetDescriptor<float>((int) PlayerField.PLAYER_DODGE_PERCENTAGE);
            }
        }

        public float ParryPercentage
        {
            get
            {
                return GetDescriptor<float>((int) PlayerField.PLAYER_PARRY_PERCENTAGE);
            }
        }

        public uint Expertise
        {
            get
            {
                return GetDescriptor<uint>((int) PlayerField.PLAYER_EXPERTISE);
            }
        }

        public uint OffhandExpertise
        {
            get
            {
                return GetDescriptor<uint>((int) PlayerField.PLAYER_OFFHAND_EXPERTISE);
            }
        }

        public float CritPercentage
        {
            get
            {
                return GetDescriptor<float>((int) PlayerField.PLAYER_CRIT_PERCENTAGE);
            }
        }

        public float RangedCritPercentage
        {
            get
            {
                return GetDescriptor<float>((int) PlayerField.PLAYER_RANGED_CRIT_PERCENTAGE);
            }
        }

        public float OffhandCritPercentage
        {
            get
            {
                return GetDescriptor<float>((int) PlayerField.PLAYER_OFFHAND_CRIT_PERCENTAGE);
            }
        }

        public uint Mastery
        {
            get
            {
                return GetDescriptor<uint>((int) PlayerField.PLAYER_MASTERY);
            }
        }

        public uint RestedExperience
        {
            get
            {
                return GetDescriptor<uint>((int) PlayerField.PLAYER_REST_STATE_EXPERIENCE);
            }
        }

        public ulong Coinage
        {
            get
            {
                return GetDescriptor<ulong>((int) PlayerField.PLAYER_FIELD_COINAGE);
            }
        }

        public uint PlayerFlags
        {
            get { return GetDescriptor<uint>((int) PlayerField.PLAYER_FLAGS); }
        }

        public bool IsGhost
        {
            get { return (PlayerFlags & (1 << 4)) > 0; }
        }

    }

}