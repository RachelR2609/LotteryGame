using LotteryGame.Classes;
using LotteryGameBL;
using LotteryGameBL.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace LotteryGame
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var services = CreateServices();
            var app = services.GetRequiredService<IApplicationService>();

            List<Ticket> tickets = new List<Ticket>();
            List<Player> players = new List<Player>();

            var player1 = app.PromptPlayerOne();
            players.Add(player1);
            tickets.AddRange(player1.Tickets);

            var cpuPlayers = app.GeneratePlayers(tickets.Select(p => p.TicketId).ToList());
            players.AddRange(cpuPlayers);
            foreach (var player in cpuPlayers)
            {
                tickets.AddRange(player.Tickets);
            }
            Console.WriteLine("13 other CPU players have also purchased tickets." + System.Environment.NewLine);

            var totalRevenue = tickets.Count;

            var grandPrizeDraw = app.GetGrandPrizeWinner(tickets, totalRevenue);
            Console.WriteLine(grandPrizeDraw.Message);

            var secondTierDraw =
                app.GetSecondOrThirdTierWinners(
                grandPrizeDraw.RemainingTickets,
                totalRevenue,
                PrizeType.SecondTier);
            Console.WriteLine(secondTierDraw.Message);

            var thirdTierDraw =
                app.GetSecondOrThirdTierWinners(
                secondTierDraw.RemainingTickets,
                totalRevenue,
                PrizeType.ThirdTier);
            Console.WriteLine(thirdTierDraw.Message);

            Console.WriteLine(
                $"{System.Environment.NewLine}" +
                $"House Revenue: " +
                $"${Math.Round(totalRevenue - (grandPrizeDraw.Total + secondTierDraw.Total + thirdTierDraw.Total), 2)}");
        }

        public static ServiceProvider CreateServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IApplicationService, ApplicationService>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
