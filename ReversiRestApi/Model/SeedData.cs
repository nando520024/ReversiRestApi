using ReversiRestApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public static class SeedData
    {
        public static void Initialize(DbGameContext context)
        {
            context.Database.EnsureCreated();

            AddGame(context, "abcdef", null, "game1token", "Potje snel reveri, dus niet lang nadenken", "0000000000000000000000000001200000021000000000000000000000000000", Color.Black);
            AddGame(context, "ghijkl", "mnopqr", null, "Ik zoek een gevorderde tegenspeler!", "0000000000000000000000000001200000021000000000000000000000000000", Color.Black);
            AddGame(context, "stuvwx", null, null, "Na dit spel wil ik er nog een paar spelen tegen zelfde tegenstander", "0000000000000000000000000001200000021000000000000000000000000000", Color.Black);
            context.SaveChanges();
        }

        private static void AddGame(DbGameContext context, string player1Token, string player2Token, string token, string description, string board, Color color)
        {
            DbGame dbGame = context.DbGames.FirstOrDefault(x => x.Player1Token.Equals(player1Token));
            if (dbGame == null)
            {
                context.DbGames.Add(new DbGame
                {
                    Player1Token = player1Token,
                    Player2Token = player2Token,
                    Token = token,
                    Description = description,
                    Board = board,
                    Turn = color
                });
            }
        }
    }
}
