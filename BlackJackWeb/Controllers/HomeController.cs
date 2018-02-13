using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJackWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlackJackWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly IGameCreator _gameCreator;

        public HomeController(IGameCreator gameCreator)
        {
            _gameCreator = gameCreator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Game game)
        {
            _gameCreator.SetGame(game);
            _gameCreator.NewGame();
            return RedirectToAction("Index", "Game");
        }
    }
}