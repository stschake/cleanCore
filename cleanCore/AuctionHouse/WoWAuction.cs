using System;

namespace cleanCore.AuctionHouse
{

    public class WoWAuction
    {
        public IntPtr Pointer { get; private set; }

        public bool IsValid
        {
            get { return Pointer != IntPtr.Zero; }
        }

        public uint ExpireTime
        {
            get { return Helper.Magic.Read<uint>((uint)Pointer + Offsets.AuctionHouse.ExpireOffset); }
        }

        public TimeSpan TimeLeft
        {
            get { return TimeSpan.FromSeconds(ExpireTime - Helper.PerformanceCount); }
        }

        public WoWAuction(AuctionListType type, int index)
        {
            uint listBase = Offsets.AuctionHouse.ListBase;
            uint listCount = Offsets.AuctionHouse.ListCount;
            switch (type)
            {
                case AuctionListType.Bidder:
                    listCount = Offsets.AuctionHouse.BidderCount;
                    listBase = Offsets.AuctionHouse.BidderBase;
                    break;

                case AuctionListType.Owner:
                    listCount = Offsets.AuctionHouse.OwnerCount;
                    listBase = Offsets.AuctionHouse.OwnerBase;
                    break;
            }

            var count = Helper.Magic.Read<uint>(listCount);
            if (count <= index)
                Pointer = IntPtr.Zero;
            else
            {
                var b = Helper.Magic.Read<uint>(listBase);
                Pointer = new IntPtr(Helper.Magic.Read<uint>((uint) (b + (Offsets.AuctionHouse.AuctionSize*index))));
            }
        }

        public static int GetAuctionCount(AuctionListType type)
        {
            switch (type)
            {
                case AuctionListType.Bidder:
                    return Helper.Magic.Read<int>(Offsets.AuctionHouse.BidderCount);
                case AuctionListType.List:
                    return Helper.Magic.Read<int>(Offsets.AuctionHouse.ListCount);
                case AuctionListType.Owner:
                    return Helper.Magic.Read<int>(Offsets.AuctionHouse.OwnerCount);
                default:
                    return 0;
            }
        }

        public static int BidderCount
        {
            get { return GetAuctionCount(AuctionListType.Bidder); }
        }

        public static int ListCount
        {
            get { return GetAuctionCount(AuctionListType.List); }
        }

        public static int OwnerCount
        {
           get { return GetAuctionCount(AuctionListType.Owner); }
        }
    }

}