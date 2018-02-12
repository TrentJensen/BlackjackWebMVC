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
            _gameCreator.NewGame();
            return View();
        }
    }
}