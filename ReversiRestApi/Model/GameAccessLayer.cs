using ReversiRestApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public class GameAccessLayer : IGameRepository
    {
        private readonly DbGameContext dbGameContext;

        public GameAccessLayer(DbGameContext dbGameContext)
        {
            this.dbGameContext = dbGameContext;
        }

        public void AddGame(Game game)
        {
            dbGameContext.DbGames.Add(ConvertGameToDbGame(game));

            // Commit to the database
            dbGameContext.SaveChanges();
        }

        public Game GetGame(string token)
        {
            try
            {
                return ConvertDbGameToGame(dbGameContext.DbGames.Single(dbGame => dbGame.Token.Equals(token)));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DeleteGame(string token)
        {
            dbGameContext.Remove(dbGameContext.DbGames.Single(dbGame => dbGame.Token.Equals(token)));
            dbGameContext.SaveChanges();
        }

        public List<Game> GetGames()
        {
            IEnumerable<DbGame> dbGames = dbGameContext.DbGames;
            List<Game> Games = new List<Game>();

            if (dbGames.Count() == 0)
            {
                return Games;
            } 

            foreach (DbGame dbGame in dbGames)
            {
                Games.Add(ConvertDbGameToGame(dbGame));
            }

            return Games;
        }

        public void UpdateGame(Game game)
        {
            var dbGame = dbGameContext.DbGames.FirstOrDefault(x => x.Token == game.Token);
            dbGameContext.Entry(dbGame).CurrentValues.SetValues(ConvertGameToDbGame(game));
            dbGameContext.SaveChanges();
        }

        // Converts DbGame (what we get out the database) to Game
        private Game ConvertDbGameToGame(DbGame dbGame)
        {
            Color[,] board = new Color[8,8];
            int counter = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = (Color)char.GetNumericValue(dbGame.Board[counter]);
                    counter++;
                }
            }

            return new Game
            {
                ID = dbGame.ID,
                Description = dbGame.Description,
                Token = dbGame.Token,
                Player1Token = dbGame.Player1Token,
                Player2Token = dbGame.Player2Token,
                Board = board,
                Turn = dbGame.Turn,
                Winner = dbGame.Winner
            };
        }

        // Converts Game to DbGame so that it can go into the database
        private DbGame ConvertGameToDbGame(Game game)
        {
            string board = "";

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board += ((int)game.Board[i, j]).ToString();
                }
            }

            var dbGame = new DbGame
            {
                ID = game.ID,
                Description = game.Description,
                Token = game.Token,
                Player1Token = game.Player1Token,
                Player2Token = game.Player2Token,
                Board = board,
                Turn = game.Turn,
                Winner = game.Winner
            };

            return dbGame;
        }
    }
}
