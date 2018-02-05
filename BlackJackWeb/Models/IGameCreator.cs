using System.Collections.Generic;

namespace BlackJackWeb.Models
{
    public interface IGameCreator
    {
        List<Card> DealerHand { get; set; }
        List<Card> PlayerHand { get; set; }
        int PlayerScore { get; set; }

        Card PullCard();
        void PullDealerCard();
        void PullPlayerCard();
        void UpdateScores();
    }
}