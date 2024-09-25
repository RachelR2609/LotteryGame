using LotteryGame.Classes;
using LotteryGameBL.Enums;
using LotteryGameBL.Models;

namespace LotteryGameBL
{
    public interface IApplicationService
    {
        Player PromptPlayerOne();
        PlayerInputValidation IsValidPlayerInput(string input);
        List<Ticket> CreateTicket(int ticketCount, List<int> existingTicketIds, string player);
        List<Player> GeneratePlayers(List<int> ticketIds);
        PrizeDrawResult GetGrandPrizeWinner(List<Ticket> tickets, decimal totalRevenue);
        PrizeDrawResult GetSecondOrThirdTierWinners(List<Ticket> tickets, decimal totalRevenue, PrizeType type);
        List<Ticket> ShuffleTickets(List<Ticket> tickets);
    }
}
