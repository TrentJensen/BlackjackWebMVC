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

        public string GameId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameCreator()
        {
            PlayerHand = new List<Card>();
            DealerHand = new List<Card>();

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

            PullPlayerCard();
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
            UpdateScores();
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

            //UpdateScores();

            DealerHand.Add(tempCard);
        }

        public void UpdateScores()
        {
            PlayerScore = 0;
            int count = 0;
            bool[] isAce = new bool[PlayerHand.Count];

            foreach (Card card in PlayerHand)
            {
                if (card.Rank == 1)
                { 
                    PlayerScore += 11;
                    isAce[count] = true;
                }
                else if (card.Rank < 11)
                {
                    PlayerScore += card.Rank;
                }
                else if (card.Rank >= 11)
                {
                    PlayerScore += 10;
                }
                count++;
            }

            //Double-check aces if score is over 21
            while (PlayerScore > 21 && isAce != null)
            {
                for (int i = 0; i < isAce.Count(); i++)
                {
                    if (isAce[i] == true)
                    {
                        PlayerScore -= 10;
                        isAce[i] = false;
                    }
                }
            }
        }

        public Card PullCard()
        {
            throw new NotImplementedException();
        }
    }
}
