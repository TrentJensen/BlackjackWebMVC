using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackWeb.Models
{
    public class GameCreator : IGameCreator
    {
        private static int DECK_SIZE = 52;

        private List<Card> _deck { get; }

        public List<Card> PlayerHand { get; set; }
        public List<Card> DealerHand { get; set; }

        public int PlayerScore { get; set; }
        public int DealerScore { get; set; }

        public bool Hold = false;

        public string GameId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameCreator()
        {
            _deck = new List<Card>(DECK_SIZE);
            foreach (var suit in new[] { "Spades", "Hearts", "Clubs", "Diamonds", })
            {
                for (var rank = 1; rank <= (DECK_SIZE / 4); rank++)
                {
                    _deck.Add(new Card
                    {
                        Rank = rank,
                        Suit = suit,
                        ImageName = $@"images\{rank}_of_{suit}.jpg"
                    });
                }
            }

            NewGame();
        }

        private void NewGame()
        {
            Hold = false;
            PlayerHand = new List<Card>();
            DealerHand = new List<Card>();
            PlayerScore = 0;
            DealerScore = 0;

            PullPlayerCard();
            PullDealerCard();
            PullDealerCard();
        }

        //public void GetGame(IServiceProvider services)
        //{
        //    ISession session = services.GetRequiredService<IHttpContextAccessor>()?
        //        .HttpContext.Session;

        //    var context = services.GetService<AppDbContext>();

        //    PlayerHand.Clear();
        //    DealerHand.Clear();
        //}

        //Returns a random card and places it in the player's hand
        public void PullPlayerCard()
        {
            Card tempCard;
            int randNum;
            Random random = new Random();

            randNum = random.Next(0, _deck.Count());

            tempCard = _deck[randNum];
            _deck.Remove(_deck[randNum]);

            PlayerHand.Add(tempCard);

            //Update player score
            if (tempCard.Rank == 1)
            {
                PlayerScore += 11;
                if (PlayerScore > 21)
                {
                    PlayerScore -= 10;
                }
            }
            else if (tempCard.Rank < 11)
            {
                PlayerScore += tempCard.Rank;
            }
            else if (tempCard.Rank >= 11)
            {
                PlayerScore += 10;
            }
        }

        //Returns a random card and places it in the dealer's hand
        public void PullDealerCard()
        {
            Card tempCard;
            int randNum;
            Random random = new Random();

            randNum = random.Next(0, _deck.Count());

            tempCard = _deck[randNum];
            _deck.Remove(_deck[randNum]);

            DealerHand.Add(tempCard);

            //Update dealer score
            if (tempCard.Rank == 1)
            {
                DealerScore += 11;
                if (DealerScore > 21)
                {
                    DealerScore -= 10;
                }
            }
            else if (tempCard.Rank < 11)
            {
                DealerScore += tempCard.Rank;
            }
            else if (tempCard.Rank >= 11)
            {
                DealerScore += 10;
            }
        }

        public void PlayerHold()
        {
            Hold = true;

            while (DealerScore < 21)
            {
                if (DealerScore > PlayerScore)
                {
                    break;
                }

                PullDealerCard();
            }
        }
    }
}
