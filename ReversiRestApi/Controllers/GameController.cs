using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReversiRestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class GameController : ControllerBase
    {

        private readonly IGameRepository iRepository;

        public GameController(IGameRepository repository)
        {
            iRepository = repository;
        }

        // GET api/game
        [HttpGet("game")]
        public ActionResult<IEnumerable<string>> GetGameDescriptionsFromGamesWithWaitingPlayer()
        {
            // Get games that have not speler2token
            var games = iRepository.GetGames().FindAll(x => x.Player2Token == null || x.Player2Token == "");
            
            if (games != null)
            {
                List<string> descriptions = new List<string>();

                // Add descriptions to List<string>
                foreach (Game game in games)
                {
                    descriptions.Add(game.Description);
                }

                return Ok(descriptions);
            }

            return NotFound();
        }

        // GET api/game/4
        [HttpGet("game/{token}")]
        public ActionResult<JsonGame> GetGameByToken(string token)
        {
            var result = iRepository.GetGame(token);

            if (result != null)
            {
                return Ok(ConvertGameToJsonGame(result));
            } 
            else
            {
                return NotFound();
            }
        }

        // GET api/gameplayer/4
        [HttpGet("gameplayer/{playertoken}")]
        public ActionResult<JsonGame> GetGameByPlayerToken(string playerToken)
        {
            var result = iRepository.GetGames().FirstOrDefault(x => x.Player1Token == playerToken || x.Player2Token == playerToken);
            if (result != null)
            {
                return Ok(ConvertGameToJsonGame(result));
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/game/turn/4
        [HttpGet("game/turn/{token}")]
        public ActionResult<string> GetTurnByToken(string token)
        {
            var result = iRepository.GetGame(token);

            if (result != null)
            {
                return Ok(result.Turn.ToString());
            }
            else
            {
                return NotFound();
            }
        }

        // PUT api/game/move
        [HttpPut("game/move")]
        public IActionResult PutMove([FromBody] TokenGame tokenGame)
        {
            var result = iRepository.GetGame(tokenGame.Token);

            if (result != null)
            {
                if (result.Player1Token.Equals(tokenGame.PlayerToken) || result.Player2Token.Equals(tokenGame.PlayerToken))
                {
                    if (tokenGame.Pass)
                    {
                        return Ok(result.Pass());
                    }
                    else
                    {
                        return Ok(result.DoMove(tokenGame.Row, tokenGame.Column));
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // PUT api/game/move/pass
        [HttpPut("game/move/pass")]
        public IActionResult PutMove([FromBody] PassGame passGame)
        {
            var result = iRepository.GetGame(passGame.Token);

            if (result != null)
            {
                if (result.Player1Token.Equals(passGame.PlayerToken) || result.Player2Token.Equals(passGame.PlayerToken))
                {
                        return Ok(result.Pass());
                }
                return NotFound();
            }
            return NotFound();
        }

        // TODO PUT api/game/surrender 

        // POST api/game
        [HttpPost("game")]
        public IActionResult PostSpel([FromBody] PostGame postGame)
        {
            Game game = new Game();
            game.Player1Token = postGame.Player1Token;
            game.Description = postGame.Description;
            game.Token = Guid.NewGuid().ToString();

            iRepository.AddGame(game);

            return Ok();
        }

        private JsonGame ConvertGameToJsonGame(Game game)
        {
            string board = "";

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board += ((int)game.Board[i, j]).ToString();
                }
            }

            var jsonGame = new JsonGame
            {
                ID = game.ID,
                Description = game.Description,
                Token = game.Token,
                Player1Token = game.Player1Token,
                Player2Token = game.Player2Token,
                Board = board,
                Turn = game.Turn.ToString()
            };

            return jsonGame;
        }
    }

    public class PostGame
    {
        public string Player1Token { get; set; }
        public string Description { get; set; }
    }

    public class TokenGame
    {
        public string Token { get; set; }
        public string PlayerToken { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Pass { get; set; }
    }

    public class PassGame
    {
        public string Token { get; set; }
        public string PlayerToken { get; set; }
    }

    public class JsonGame
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }
        public string Player1Token { get; set; }
        public string Player2Token { get; set; }
        public string Board { get; set; }
        public string Turn { get; set; }
    }
}
