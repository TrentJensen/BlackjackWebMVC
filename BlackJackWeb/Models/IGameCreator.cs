using System.Collections.Generic;

namespace BlackJackWeb.Models
{
    public interface IGameCreator
    {
        List<Card> DealerHand { get; set; }
        List<Card> PlayerHand { get; set; }
        int PlayerScore { get; set; }

        void NewGame();
        void MakeBet(int bet);
        void PullDealerCard();
        void PullPlayerCard();
        void PlayerStand();
        void Double();
        void NextGame();
        void Continue();
    }
}