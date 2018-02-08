using BlackJackWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackWeb.Components
{
    public class Results :ViewComponent
    {
        private readonly IGameCreator _gameCreator;

        public Results(IGameCreator gameCreator)
        {
            _gameCreator = gameCreator;
        }

        public IViewComponentResult Invoke()
        {
            return View(_gameCreator);
        }
    }
}
