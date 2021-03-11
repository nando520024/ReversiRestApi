using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public interface IGame
    {
        int ID { get; set; }

        string Description { get; set; }

        // het unieke token van het spel
        string Token { get; set; }

        string Player1Token { get; set; }

        string Player2Token { get; set; }

        Color[,] Board { get; set; }

        Color Turn { get; set; }

        bool Pass();

        bool Finished();

        // which color is most common on the game board
        Color PredominantColor();

        // check whether a move is possible at a certain position
        bool MovePossible(int row, int column);

        bool DoMove(int row, int column);
    }

    public enum Color { None, White, Black };

}


