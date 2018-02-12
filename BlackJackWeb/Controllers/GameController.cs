using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJackWeb.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlackJackWeb.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameCreator _gameCreator;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameController(IGameCreator gameCreator)
        {
            _gameCreator = gameCreator;
        }

        public IActionResult Index()
        {
            return View(_gameCreator);
        }

        [HttpPost]
        public IActionResult Index(int bet)
        {
            if (ModelState.IsValid)
            {
                _gameCreator.MakeBet(bet);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult Hit()
        {
            _gameCreator.PullPlayerCard();
            return RedirectToAction("Index", "Game");
        }

        public RedirectToActionResult Stand()
        {
            _gameCreator.PlayerStand();
            return RedirectToAction("Index");
        }

        public RedirectToActionResult Double()
        {
            _gameCreator.Double();
            return RedirectToAction("Index");
        }

        public RedirectToActionResult NewGame()
        {
            _gameCreator.NewGame();
            return RedirectToAction("Index");
        }

        public RedirectToActionResult Continue()
        {
            _gameCreator.Continue();
            return RedirectToAction("Index");
        }

        public RedirectToActionResult NextGame()
        {
            _gameCreator.NextGame();
            return RedirectToAction("Index");
        }
    }
}
