using LotteryGame.Classes;
using LotteryGameBL;
using LotteryGameBL.Enums;
using LotteryGameBL.Models;

namespace LotteryTicketTests
{
    public class LotteryTicketTests
    {
        #region Tests
        [Theory]
        [InlineData("1")]
        [InlineData("10")]
        [InlineData("Abc")]
        public void CheckInputIsValidAndPrintAppropriateMessage(string input)
        {
            var app = new ApplicationService();
            PlayerInputValidation result = app.IsValidPlayerInput(input);
            Assert.True(result != null);
        }

        [Theory]
        [MemberData(nameof(CreateAndAssignTicketsToPlayersData))]
        public void CreateAndAssignTicketsToPlayers(int ticketCount, List<int> existingTicketIds, string player)
        {
            var app = new ApplicationService();
            List<Ticket> result = app.CreateTicket(ticketCount, existingTicketIds, player);
            Assert.True(result != null && result.Count() > 0);
        }

        [Theory]
        [MemberData(nameof(Generate13PlayersData))]
        public void Generate13Players(List<int> ticketIds)
        {
            var app = new ApplicationService();
            List<Player> result = app.GeneratePlayers(ticketIds);
            Assert.True(result != null && result.Count() == 13);
        }

        [Theory]
        [MemberData(nameof(GrandPrizeDrawerData))]
        public void GetGrandPrizeWinner(List<Ticket> tickets, decimal totalRevenue)
        {
            var app = new ApplicationService();
            PrizeDrawResult result = app.GetGrandPrizeWinner(tickets, totalRevenue);
            Assert.True(result != null);
        }

        [Theory]
        [MemberData(nameof(SecondAndThirdPrizeDrawerData))]
        public void GetSecondTierWinners(List<Ticket> tickets, decimal totalRevenue, PrizeType type)
        {
            var app = new ApplicationService();
            PrizeDrawResult result = app.GetSecondOrThirdTierWinners(tickets, totalRevenue, type);
            Assert.True(result != null);
        }

        [Theory]
        [MemberData(nameof(ShuffleTicketsBeforeDrawData))]
        public void ShuffleTicketsBeforeDraw(List<Ticket> tickets)
        {
            var app = new ApplicationService();
            List<Ticket> result = app.ShuffleTickets(tickets);
            Assert.True(result != null && result.Count == tickets.Count);
        }
        #endregion

        #region Test Data
        public static IEnumerable<object[]> CreateAndAssignTicketsToPlayersData()
        {
            yield return new object[] { 1, new List<int> { 1, 2, 3 }, "Player 1" };
        }
        public static IEnumerable<object[]> Generate13PlayersData()
        {
            yield return new object[] { new List<int> { 1, 2, 3 } };
        }
        public static IEnumerable<object[]> GrandPrizeDrawerData()
        {
            yield return new object[] {
                new List<Ticket>(){
                new Ticket
                {
                    TicketId = 1,
                    Player = "Player 1"
                },
                 new Ticket
                {
                    TicketId = 2,
                    Player = "Player 1"
                },
                  new Ticket
                {
                    TicketId = 3,
                    Player = "Player 2"
                },
            },
                30.00
            };
        }
        public static IEnumerable<object[]> SecondAndThirdPrizeDrawerData()
        {
            yield return new object[] {
                new List<Ticket>(){
                new Ticket
                {
                    TicketId = 1,
                    Player = "Player 1"
                },
                 new Ticket
                {
                    TicketId = 2,
                    Player = "Player 1"
                },
                  new Ticket
                {
                    TicketId = 3,
                    Player = "Player 2"
                },
            },
                30.00,
                PrizeType.SecondTier
            };
        }
        public static IEnumerable<object[]> ShuffleTicketsBeforeDrawData()
        {
            yield return new object[] {
                new List<Ticket>(){
                new Ticket
                {
                    TicketId = 1,
                    Player = "Player 1"
                },
                 new Ticket
                {
                    TicketId = 2,
                    Player = "Player 1"
                },
                  new Ticket
                {
                    TicketId = 3,
                    Player = "Player 2"
                },
                    new Ticket
                {
                    TicketId = 4,
                    Player = "Player 2"
                },
                     new Ticket
                {
                    TicketId = 5,
                    Player = "Player 2"
                },
                       new Ticket
                {
                    TicketId = 6,
                    Player = "Player 3"
                },
            },
            };
        }
        #endregion

    }
}