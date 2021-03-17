using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReversiRestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {

        private readonly IGameRepository iRepository;

        public GameController(IGameRepository repository)
        {
            iRepository = repository;
        }

        // GET api/game
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetGameDescriptionsFromGamesWithWaitingPlayer()
        {
            // Get games that have no Player2Token
            var games = iRepository.GetGames().FindAll(x => x.Player2Token == null || x.Player2Token == "").Select(x => new { x.Description, x.Token, x.Player1Token });

            if (games != null)
            {
                return new ObjectResult(games);
            }

            return NotFound();

        }

        // GET api/game/{token}
        [HttpGet("{token}")]
        public ActionResult<JsonGame> GetGameByToken(string token)
        {
            var result = iRepository.GetGame(token);

            if (result != null)
            {
                return new ObjectResult(ConvertGameToJsonGame(result));
            }

            return NotFound();
        }

        // GET api/game/player/{playertoken}
        [HttpGet("player/{playertoken}")]
        public ActionResult<JsonGame> GetGameByPlayerToken(string playerToken)
        {
            if (!string.IsNullOrWhiteSpace(playerToken))
            {
                var result = iRepository.GetGames().FirstOrDefault(x => x.Player1Token == playerToken || x.Player2Token == playerToken);

                if (result != null)
                {
                    return new ObjectResult(ConvertGameToJsonGame(result));
                }

                return NotFound();
            }

            return NotFound();
        }

        // GET api/game/turn/{token}
        [HttpGet("turn/{token}")]
        public ActionResult<Color> GetTurnByToken(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var result = iRepository.GetGame(token);

                if (result != null)
                {
                    return new ObjectResult(result.Turn);
                }

                return NotFound();
            }

            return NotFound();
            
        }

        // PUT api/game/move
        [HttpPut("move")]
        public ActionResult<bool> PutMove([FromBody] TokenGame tokenGame)
        {
            var result = iRepository.GetGame(tokenGame.Token);

            if (result != null)
            {
                // Check it the given playertoken has its turn 
                if ((result.Turn == Color.Black && result.Player1Token.Equals(tokenGame.PlayerToken)) || (result.Turn == Color.Black && result.Player1Token.Equals(tokenGame.PlayerToken)))
                {
                    // Check if Pass is true and if pass is possible
                    if (tokenGame.Pass)
                    {
                        if (result.Pass())
                        {
                            iRepository.UpdateGame(result);
                            return Ok(true);
                        }

                        return Ok(false);
                    }
                    // Check if move is possible if yes then return true else return false
                    else if (result.DoMove(tokenGame.Row, tokenGame.Column))
                    {
                        iRepository.UpdateGame(result);
                        return Ok(true);
                    }
                    return Ok(false);
                }
                return Ok(false);
            }
            return NotFound();
        }

        // PUT api/game/surrender 
        [HttpPut("surrender")]
        public IActionResult Surrender([FromBody] SurrenderGame surrenderGame)
        {
            var result = iRepository.GetGame(surrenderGame.Token);

            if (result != null)
            {
                if (result.Player1Token.Equals(surrenderGame.PlayerToken))
                {
                    result.Winner = result.Player2Token;
                }
                else
                {
                    result.Winner = result.Player1Token;
                }
                return Ok(result.Winner);
            }
            return NotFound();
        }

        // POST api/game
        [HttpPost]
        public IActionResult PostSpel([FromBody] PostGame postGame)
        {
            Game game = new Game
            {
                Player1Token = postGame.Player1Token,
                Description = postGame.Description,
                Token = Guid.NewGuid().ToString()
            };

            iRepository.AddGame(game);

            return Ok(game.Token);
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
                Turn = game.Turn
            };

            return jsonGame;
        }
    }

    public class PostGame
    {
        public string Player1Token { get; set; }
        public string Description { get; set; }
    }

    public class SurrenderGame
    {
        public string PlayerToken { get; set; }
        public string Token { get; set; }
    }

    public class TokenGame
    {
        public string Token { get; set; }
        public string PlayerToken { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Pass { get; set; }
    }

    public class JsonGame
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }
        public string Player1Token { get; set; }
        public string Player2Token { get; set; }
        public string Board { get; set; }
        public Color Turn { get; set; }
    }
}
