﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackWeb.Models
{
    public class GameCreator : IGameCreator
    {
        private static int DECK_SIZE = 52;

        private List<Card> _deck { get; }
        private int _hiddenCardValue;

        public List<Card> PlayerHand { get; set; }
        public List<Card> DealerHand { get; set; }

        public int PlayerScore { get; set; }
        public int DealerScore { get; set; }

        public int PlayerChips { get; set; }


        private int _bet;
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage =" Please enter a number")]
        //[GreaterThan(100)]
        public int Bet
        {
            get { return _bet; }
            set
            {
                if (value <= PlayerChips)
                    _bet = value;
                else
                    _bet = PlayerChips;
            }
        }

        private int _playerAces;
        private int _dealerAces;

        public bool Stand = false;

        public enum Results { Bet, Playing, Bust, PlayerWins, DealerWins, Shuffle, Broke }
        public Results GameMode { get; set; }

        public string GameId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameCreator()
        {
            _deck = new List<Card>(DECK_SIZE);
        }

        public void NewGame()
        {
            Shuffle();
            Stand = false;
            GameMode = Results.Bet;
            PlayerHand = new List<Card>();
            DealerHand = new List<Card>();
            PlayerScore = 0;
            DealerScore = 0;
            PlayerChips = 100;
            _playerAces = 0;
            _dealerAces = 0;

            PullPlayerCard();
            PullDealerCard();
            PullDealerCard();
        }

        public void MakeBet(int bet)
        {
            Bet = bet;
            GameMode = Results.Playing;
        }

        /// <summary>
        /// Returns a random card and places it in the player's hand
        /// </summary>
        public void PullPlayerCard()
        {
            Card tempCard;
            int randNum;
            Random random = new Random();

            if (_deck.Count == 0)
            {
                GameMode = Results.Shuffle;
                Shuffle();
            }
            else
            {
                randNum = random.Next(0, _deck.Count());
                tempCard = _deck[randNum];
                _deck.Remove(_deck[randNum]);

                PlayerHand.Add(tempCard);

                //Update player score
                if (tempCard.Rank == 1)
                {
                    _playerAces++;
                    PlayerScore += 11;
                }
                else if (tempCard.Rank < 11)
                {
                    PlayerScore += tempCard.Rank;
                }
                else if (tempCard.Rank >= 11)
                {
                    PlayerScore += 10;
                }

                //Changes Aces from 11 to 1 if hand is over 21
                if (_playerAces > 0)
                {
                    int count = 0;

                    while (PlayerScore > 21 && count < _playerAces)
                    {
                        PlayerScore -= 10;
                        count++;
                        _playerAces--;
                    }
                }

                //If player busts
                if (PlayerScore > 21)
                {
                    Stand = true;
                    GameMode = Results.Bust;
                    PlayerChips -= Bet;
                }
                else if (PlayerScore == 21)
                {
                    PlayerStand();
                }
            }
        }

        /// <summary>
        /// Returns a random card and places it in the dealer's hand
        /// </summary>
        public void PullDealerCard()
        {
            Card tempCard;
            int randNum;
            int newValue = 0;
            Random random = new Random();

            if (_deck.Count == 0)
            {
                GameMode = Results.Shuffle;
                Shuffle();
            }
            else
            {
                randNum = random.Next(0, _deck.Count());

                tempCard = _deck[randNum];
                _deck.Remove(_deck[randNum]);

                DealerHand.Add(tempCard);

                //Update dealer score
                if (tempCard.Rank == 1)
                {
                    _dealerAces++;
                    newValue += 11;
                }
                else if (tempCard.Rank < 11)
                {
                    newValue += tempCard.Rank;
                }
                else if (tempCard.Rank >= 11)
                {
                    newValue += 10;
                }

                //Hold the value of the first card a secret
                if (DealerHand.Count == 1)
                    _hiddenCardValue = newValue;
                else
                    DealerScore += newValue;

                //Changes Aces from 11 to 1 if hand is over 21
                if (_dealerAces > 0)
                {
                    int count = 0;

                    while (DealerScore > 21 && count < _dealerAces)
                    {
                        DealerScore -= 10;
                        count++;
                        _dealerAces--;
                    }
                }
            }
        }

        public void Double()
        {
            Bet = Bet * 2;
            PullPlayerCard();

            if (PlayerScore <= 21)
                PlayerStand();
            else
            {
                Stand = true;
                GameMode = Results.Bust;
                PlayerChips -= Bet;
            }
        }

        public void PlayerStand()
        {
            Stand = true;
            DealerScore += _hiddenCardValue;

            while (DealerScore < 21)
            {
                if (DealerScore > PlayerScore)
                {
                    break;
                }

                PullDealerCard();
            }

            EndResults();
        }

        private void Shuffle()
        {
            _deck.Clear();
            foreach (var suit in new[] { "Spades", "Hearts", "Clubs", "Diamonds", })
            {
                for (var rank = 1; rank <= (DECK_SIZE / 4); rank++)
                {
                    _deck.Add(new Card
                    {
                        Rank = rank,
                        Suit = suit,
                        ImageName = $@"images/{rank}_of_{suit}.jpg"
                    });
                }
            }
        }

        public void NextGame()
        {
            Stand = false;
            GameMode = Results.Bet;
            PlayerHand.Clear();
            DealerHand.Clear();
            PlayerScore = 0;
            DealerScore = 0;
            _playerAces = 0;
            _dealerAces = 0;
            Bet = 0;

            PullPlayerCard();
            PullDealerCard();
            PullDealerCard();
        }

        private void EndResults()
        {
            if (DealerScore > 21)
            {
                GameMode = Results.PlayerWins;
                PlayerChips += Bet;
            }
            else if (DealerScore <= 21 && DealerScore >= PlayerScore)
            {
                GameMode = Results.DealerWins;
                PlayerChips -= Bet;
            }

            if (PlayerChips <= 0)
            {
                GameMode = Results.Broke;
            }
        }

        public void Continue()
        {
            throw new NotImplementedException();
        }
    }
}
