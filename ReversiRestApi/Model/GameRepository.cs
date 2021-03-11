using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public class GameRepository : IGameRepository
    {
        // Lijst met tijdelijke spellen
        public List<Game> Games { get; set; }

        public GameRepository()
        {
            Game game1 = new Game();
            Game game2 = new Game();
            Game game3 = new Game();
            game1.Player1Token = "abcdef";
            game1.Token = "game1token";
            game1.Description = "Potje snel reveri, dus niet lang nadenken";
            game2.Player1Token = "ghijkl";
            game2.Player2Token = "mnopqr";
            game2.Description = "Ik zoek een gevorderde tegenspeler!";
            game3.Player1Token = "stuvwx";
            game3.Description = "Na dit spel wil ik er nog een paar spelen tegen zelfde tegenstander";

            Games = new List<Game> { game1, game2, game3 };
        }

        public void AddGame(Game game)
        {
            Games.Add(game);
        }

        public Game GetGame(string token)
        {
            return Games.FirstOrDefault(x => x.Token == token);
        }

        public List<Game> GetGames()
        {
            return Games;
        }
    }
}
