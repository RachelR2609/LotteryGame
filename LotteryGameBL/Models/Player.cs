namespace LotteryGame.Classes
{
    public class Player
    {
        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets tickets to the player.
        /// </summary>
        public List<Ticket> Tickets { get; set; }
    }
}
