using LotteryGame.Classes;
using LotteryGameBL.Enums;

namespace LotteryGameBL.Models
{
    public class PrizeDrawResult
    {
        /// <summary>
        /// Gets or sets the prize type: grand, second tier or third tier.
        /// </summary>
        public PrizeType Type { get; set; }
        /// <summary>
        /// Gets or sets all the remaining tickets after the prize draw.
        /// </summary>
        public List<Ticket> RemainingTickets { get; set; }
        /// <summary>
        /// Gets or sets the message to print to the console after the prize draw.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the total prize amount.
        /// </summary>
        public double Total { get; set; }
    }
}
