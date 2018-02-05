using BlackJackWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackWeb.ViewModels
{
    public class HomeViewModel
    {
        private static int DECK_SIZE = 52;

        private List<Card> _deck { get; }

        public List<Card> PlayerHand { get; set; }
        public List<Card> DealerHand { get; set; }

        public int PlayerScore { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HomeViewModel()
        {
            PlayerHand = new List<Card>();
            DealerHand = new List<Card>();

            _deck = new List<Card>(DECK_SIZE);
            foreach (var suit in new[] { "Spades", "Hearts", "Clubs", "Diamonds", })
            {
                for (var rank = 1; rank <= (DECK_SIZE / 4); rank++)
                {
                    _deck.Add(new Card{
                        Rank = rank,
                        Suit = suit,
                        ImageName = $@"images\{rank}_of_{suit}.jpg"
                });
                }
            }

            PlayerHand.Add(PullCard());
            DealerHand.Add(PullCard());
        }

        //Returns a random card and removes it from the deck
        public Card PullCard()
        {
            Card tempCard;
            int randNum;
            Random random = new Random();

            randNum = random.Next(0, _deck.Count());

            tempCard = _deck[randNum];
            _deck.Remove(_deck[randNum]);

            UpdateScores();

            return tempCard;
        }

        public void UpdateScores()
        {
            PlayerScore = 0;

            foreach (Card card in PlayerHand)
            {
                PlayerScore += card.Rank;
            }
        }
    }
}
