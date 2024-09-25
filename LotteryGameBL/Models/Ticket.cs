namespace LotteryGame.Classes
{
    public class Ticket
    {
        /// <summary>
        /// Gets or sets the unique ticket id used for the prize draw.
        /// </summary>
        public int TicketId { get; set; }
        /// <summary>
        /// Gets or sets the name of the player who purchased the ticket.
        /// </summary>
        public string Player { get; set; }
    }
}
