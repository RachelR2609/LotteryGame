using LotteryGame.Classes;
using LotteryGameBL.Enums;
using LotteryGameBL.Models;
using System.Text;

namespace LotteryGameBL
{
    public class ApplicationService : IApplicationService
    {
        /// <summary>
        /// This method will ask player 1 how many tickets they want to purchase and then capture their input.
        /// </summary>
        /// <returns>
        /// Returns a LotteryGameBL.Models.Player object with the name of the player and the tickets they have purchased (will be an empty Ticket list if player's input was invalid).
        /// </returns>
        public Player PromptPlayerOne()
        {
            Console.WriteLine("Welcome to the Bede Lottery Player 1!");
            Console.WriteLine("* Your balance: $10");
            Console.WriteLine("* Price per ticket: $1");
            Console.WriteLine("......................................");
            Console.WriteLine("How many tickets do you want to buy?");

            var ticketNumStr = Console.ReadLine();
            PlayerInputValidation val = IsValidPlayerInput(ticketNumStr);
            if (val.IsValid)
            {
                Console.WriteLine(val.Message);
                return new Player()
                {
                    Name = "Player 1",
                    Tickets = CreateTicket(val.Value, new List<int>() { 0 }, "Player 1"),
                };
            }
            else
            {
                Console.WriteLine(val.Message);
                return new Player()
                {
                    Name = "Player 1",
                    Tickets = new List<Ticket>(),
                };
            }
        }

        /// <summary>
        /// Checks to see if player's input was a valid integer value.
        /// </summary>
        /// <param name="input">
        /// System.String instance holding the player's entered value for the number of desired tickets.
        /// </param>
        /// <returns> 
        /// Returns a LotteryGameBL.Models.PlayerInputValidation object holding whether the check was valid or not, the validation message and the converted input value.
        /// </returns>
        public PlayerInputValidation IsValidPlayerInput(string input)
        {
            int value;
            if (Int32.TryParse(input, out value))
            {
                if (value <= 10)
                    return new PlayerInputValidation(
                        true,
                        $"{System.Environment.NewLine}" + $"You have successfully purchased {value} ticket(s).",
                        value);
                else
                    return new PlayerInputValidation(
                        true,
                        $"{System.Environment.NewLine}" +
                        $"You cannot purchase more than your balance allows." +
                        $"{System.Environment.NewLine}" + $"Successfully purchsed 10 tickets.",
                        10);
            }
            else return new PlayerInputValidation(false, "Please provide a valid input...");
        }

        /// <summary>
        /// Creates tickets for a user based on how many tickets they purchased.
        /// </summary>
        /// <param name="ticketCount"> System.Int32 instance holding the count of the desired number of tickets.</param>
        /// <param name="existingTicketIds">System.Collection.Generic.List<int> holding the ids of previously created tickets.</param>
        /// <param name="player">System.String instance holding the name of the player to create tickets for.</param>
        /// <returns>System.Collection.Generic.List<Ticket> of newly created tickets.</returns>
        public List<Ticket> CreateTicket(int ticketCount, List<int> existingTicketIds, string player)
        {
            List<Ticket> tickets = new List<Ticket>();
            var id = existingTicketIds[existingTicketIds.Count - 1] + 1;
            for (int i = 0; i < ticketCount; i++)
            {
                tickets.Add(new Ticket()
                {
                    TicketId = id++,
                    Player = player
                });
            }
            return tickets;
        }

        /// <summary>
        /// Generates CPU players having a random number of tickets between 1 and 10 each.
        /// </summary>
        /// <param name="player1TicketIds">System.Collection.Generic.List<int> holding the ids of previously created tickets (Player 1 tickets).</param>
        /// <returns>System.Collection.Generic.List<Player> holding the newly created players and thier tickets.</returns>
        public List<Player> GeneratePlayers(List<int> player1TicketIds)
        {
            List<Player> cpuPlayers = new List<Player>();
            var count = 13;
            List<int> ticketIds = player1TicketIds;
            while (count > 0)
            {
                Random rnd = new Random();
                var ticketCount = rnd.Next(0, 11);
                var player = $"Player {count}";
                var tickets = CreateTicket(ticketCount, ticketIds, player);
                cpuPlayers.Add(new Player()
                {
                    Name = player,
                    Tickets = tickets,
                });

                ticketIds.AddRange(tickets.ToList().Select(t => t.TicketId));
                count--;
            }
            return cpuPlayers;
        }

        /// <summary>
        /// Picks one random winner for the grand prize.
        /// </summary>
        /// <param name="tickets">System.Collection.Generic.List<Ticket> holding all tickets purchased.</param>
        /// <param name="totalRevenue">System.Decimal instance holding the total revenue (number of tickets)</param>
        /// <returns>
        /// LotteryGameBL.Models.PrizeDrawResult containg the prize type, remaining tickets after draw, message to display to the console and the total amount won.
        /// </returns>
        public PrizeDrawResult GetGrandPrizeWinner(List<Ticket> tickets, decimal totalRevenue)
        {
            Random rand = new Random();
            var ticketId = rand.Next(1, tickets.Count + 1);
            var winner = tickets.Where(t => t.TicketId == ticketId).FirstOrDefault().Player;
            var amount = (int)totalRevenue * 0.5;

            return new PrizeDrawResult()
            {
                Type = PrizeType.GrandPrize,
                RemainingTickets = tickets.Where(t => t.TicketId != ticketId).ToList(),
                Message = $"Grand Prize: {winner} wins ${amount}!",
                Total = amount
            };
        }

        /// <summary>
        /// Gets the second tier winners or third tier winners based on the type specified.
        /// </summary>
        /// <param name="tickets">
        /// System.Collection.Generic.List<Ticket> holding remaing tickets after the grand or second prize draws.
        /// </param>
        /// <param name="totalRevenue">System.Decimal instance holding the total revenue (number of tickets)</param>
        /// <param name="type">
        /// LotteryGameBL.Enums.PrizeType holding the type of prize (grand, second tier or third tier).
        /// </param>
        /// <returns>
        /// LotteryGameBL.Models.PrizeDrawResult containg the prize type, remaining tickets after draw, message to display to the console and the total amount won.
        /// </returns>
        public PrizeDrawResult GetSecondOrThirdTierWinners(List<Ticket> tickets, decimal totalRevenue, PrizeType type)
        {
            var countPer = type == PrizeType.SecondTier ? 0.1 : 0.2;
            var amountPer = type == PrizeType.SecondTier ? 0.3 : 0.1;

            var winnerCount = (int)(Math.Round(tickets.Count * countPer, 0, MidpointRounding.AwayFromZero));

            var typeLabel = type == PrizeType.SecondTier ? "Second Tier" : "Third Tier";
            if (winnerCount != 0)
            {
                var shuffledTicketList = ShuffleTickets(tickets);
                var distinctTicketList = shuffledTicketList.DistinctBy(t => t.Player);
                var winners = distinctTicketList.Skip(0).Take(winnerCount).ToList();
                var amountEach = Math.Round(((int)totalRevenue * amountPer) / winnerCount, 2);

                StringBuilder sb = new StringBuilder();
                sb.Append($"{typeLabel}: players ");
                var last = winners.Last();
                foreach (var winner in winners)
                {
                    var playerNum = winner.Player.Split(" ")[1];
                    if (winner == last)
                        sb.Append($"and {playerNum} have each won ${amountEach}!");
                    else sb.Append($"{playerNum}, ");

                    tickets.Remove(winner);
                }

                return new PrizeDrawResult()
                {
                    Type = PrizeType.SecondTier,
                    RemainingTickets = tickets,
                    Message = sb.ToString(),
                    Total = amountEach * winnerCount,
                };
            }
            else
            {
                return new PrizeDrawResult()
                {
                    Type = PrizeType.SecondTier,
                    RemainingTickets = tickets,
                    Message = $"{typeLabel}: no winner as not enough players.",
                    Total = 0
                };
            }
        }

        /// <summary>
        /// Takes in a list of tickets and performs the Fisher-Yates-Shuffle on them.
        /// </summary>
        /// <param name="tickets">System.Collection.Generic.List<Ticket> of tickets to be shuffled.</param>
        /// <returns>System.Collection.Generic.List<Ticket> of the newly shuffled ticket list.</returns>
        public List<Ticket> ShuffleTickets(List<Ticket> tickets)
        {
            int n = tickets.Count;
            Random rng = new Random();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Ticket value = tickets[k];
                tickets[k] = tickets[n];
                tickets[n] = value;
            }
            return tickets;
        }
    }
}
