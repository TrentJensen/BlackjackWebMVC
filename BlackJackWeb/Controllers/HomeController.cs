using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJackWeb.Models;
using BlackJackWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlackJackWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameCreator _gameCreator;

        /// <summary>
        /// Constructor
        /// </summary>
        public HomeController(IGameCreator gameCreator)
        {
            _gameCreator = gameCreator;
        }

        public IActionResult Index()
        { 
            return View(_gameCreator);
        }

        public IActionResult Results()
        {
            return View(_gameCreator);
        }

        public RedirectToActionResult Hit()
        {
            _gameCreator.PullPlayerCard();

            if (_gameCreator.PlayerScore < 21)
            {
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Results");
        }

        public RedirectToActionResult Hold()
        {
            _gameCreator.PlayerHold();

            return RedirectToAction("Index");
        }
    }
}