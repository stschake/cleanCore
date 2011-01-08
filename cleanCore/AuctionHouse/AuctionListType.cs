namespace cleanCore.AuctionHouse
{

    public enum AuctionListType
    {
        /// <summary>
        /// An item up for auction, the "Browse" tab in the dialog
        /// </summary>
        List,

        /// <summary>
        /// An item the player has bid on, the "Bids" tab in the dialog
        /// </summary>
        Bidder,

        /// <summary>
        /// An item the player has up for auction, the "Auctions" tab in the dialog
        /// </summary>
        Owner
    }

}