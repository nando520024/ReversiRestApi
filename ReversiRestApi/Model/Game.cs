using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public class Game : IGame
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }
        public string Player1Token { get; set; }
        public string Player2Token { get; set; }
        public Color[,] Board { get; set; }
        public Color Turn { get; set; }

        public string Winner { get; set; }

        private List<Direction> directions;
        private int row2, column2;
        private Color enemy;

        public Game()
        {
            Board = new Color[8, 8];
            Array.Clear(Board, 0, Board.Length);
            int turn = new Random().Next(1, 3);
            Turn = (Color)turn;
            Board[3, 3] = Board[4, 4] = (Color)1;
            Board[4, 3] = Board[3, 4] = (Color)2;
        }

        public bool Finished()
        {
            if (Pass() && Pass())
            {
                return true;
            }

            return false;
        }

        public bool DoMove(int row, int column)
        {
            if (!MovePossible(row, column))
            {
                return false;
            }

            Board[row, column] = Turn;

            // Up
            if (directions.Contains(Direction.Up))
            {
                row2 = row - 1;
                column2 = column;
                while (Board[row2, column2] == enemy)
                {
                    Board[row2, column2] = Turn;
                    row2--;
                }
            }

            // Down
            if (directions.Contains(Direction.Down))
            {
                row2 = row + 1;
                column2 = column;
                while (Board[row2, column2] == enemy)
                {
                    Board[row2, column2] = Turn;
                    row2++;
                }
            }

            // Left 
            if (directions.Contains(Direction.Left))
            {
                row2 = row;
                column2 = column - 1;
                while (Board[row2, column2] == enemy)
                {
                    Board[row2, column2] = Turn;
                    column2--;
                }
            }

            // Right
            if (directions.Contains(Direction.Right))
            {
                row2 = row;
                column2 = column + 1;
                while (Board[row2, column2] == enemy)
                {
                    Board[row2, column2] = Turn;
                    column2++;
                }
            }

            // Up left
            if (directions.Contains(Direction.UpLeft))
            {
                row2 = row - 1;
                column2 = column - 1;
                while (Board[row2, column2] == enemy)
                {
                    Board[row2, column2] = Turn;
                    row2--;
                    column2--;
                }
            }

            // Up right
            if (directions.Contains(Direction.UpRight))
            {
                row2 = row - 1;
                column2 = column + 1;
                while (Board[row2, column2] == enemy)
                {
                    Board[row2, column2] = Turn;
                    row2--;
                    column2++;
                }
            }

            // Down left
            if (directions.Contains(Direction.DownLeft))
            {
                row2 = row + 1;
                column2 = column - 1;
                while (Board[row2, column2] == enemy)
                {
                    Board[row2, column2] = Turn;
                    row2++;
                    column2--;
                }
            }

            // Down right
            if (directions.Contains(Direction.DownRight))
            {
                row2 = row + 1;
                column2 = column + 1;
                while (Board[row2, column2] == enemy)
                {
                    Board[row2, column2] = Turn;
                    row2++;
                    column2++;
                }
            }

            Turn = enemy;
            return true;
        }

        public Color PredominantColor()
        {
            int black = 0;
            int white = 0;
            int none = 0;

            foreach (var item in Board)
            {
                switch (item)
                {
                    case Color.None:
                        none++;
                        break;
                    case Color.White:
                        white++;
                        break;
                    case Color.Black:
                        black++;
                        break;
                }
            }

            if (black > white)
            {
                return Color.Black;
            }
            else if (white > black)
            {
                return Color.White;
            }
            else
            {
                return Color.None;
            }
        }

        public bool Pass()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] == Color.None)
                    {
                        if (MovePossible(i, j))
                        {
                            return false;
                        }
                    }
                }
            }

            Turn = enemy;
            return true;
        }

        // Returns true if the distance between two positions is bigger than 1
        public bool Distance(int i1, int j1, int i2, int j2)
        {
            if (Math.Abs(i1 - i2) > 0)
            {
                return (Math.Abs(i1 - i2) > 1);
            } 
            else
            {
                return (Math.Abs(j1 - j2) > 1);
            }
        }

        public bool MovePossible(int row, int column)
        {
            directions = new List<Direction>();
            bool valid = false;

            // Check if the position is within the 8x8
            Func<int, int, bool> ValidPosition = (i, j) => (i >= 0 && i < 8 && j >= 0 && j < 8);

            if (!ValidPosition(row, column) || Board[row, column] != 0)
            {
                return valid;
            }

            // Set the enemy color
            if (Turn == Color.Black)
            {
                enemy = Color.White;
            } 
            else
            {
                enemy = Color.Black;
            }

            // Up
            row2 = row - 1;
            column2 = column;

            while (ValidPosition(row2, column2) && Board[row2, column2] == enemy)
            {
                row2--;
            }

            if (ValidPosition(row2, column2) && Distance(row, column, row2, column2) && Board[row2, column2] == Turn)
            {
                valid = true;
                directions.Add(Direction.Up);
            }

            // Down
            row2 = row + 1;
            column2 = column;

            while (ValidPosition(row2, column2) && Board[row2, column2] == enemy)
            {
                row2++;
            }

            if (ValidPosition(row2, column2) && Distance(row, column, row2, column2) && Board[row2, column2] == Turn)
            {
                valid = true;
                directions.Add(Direction.Down);
            }

            // Left
            row2 = row;
            column2 = column - 1;

            while (ValidPosition(row2, column2) && Board[row2, column2] == enemy)
            {
                column2--;
            }

            if (ValidPosition(row2, column2) && Distance(row, column, row2, column2) && Board[row2, column2] == Turn)
            {
                valid = true;
                directions.Add(Direction.Left);
            }

            // Right
            row2 = row;
            column2 = column + 1;

            while (ValidPosition(row2, column2) && Board[row2, column2] == enemy)
            {
                column2++;
            }

            if (ValidPosition(row2, column2) && Distance(row, column, row2, column2) && Board[row2, column2] == Turn)
            {
                valid = true;
                directions.Add(Direction.Right);
            }

            // Up left
            row2 = row - 1;
            column2 = column - 1;

            while (ValidPosition(row2, column2) && Board[row2, column2] == enemy)
            {
                row2--;
                column2--;
            }

            if (ValidPosition(row2, column2) && Distance(row, column, row2, column2) && Board[row2, column2] == Turn)
            {
                valid = true;
                directions.Add(Direction.UpLeft);
            }

            // Up right
            row2 = row - 1;
            column2 = column + 1;

            while (ValidPosition(row2, column2) && Board[row2, column2] == enemy)
            {
                row2--;
                column2++;
            }

            if (ValidPosition(row2, column2) && Distance(row, column, row2, column2) && Board[row2, column2] == Turn)
            {
                valid = true;
                directions.Add(Direction.UpRight);
            }

            // Down left
            row2 = row + 1;
            column2 = column - 1;

            while (ValidPosition(row2, column2) && Board[row2, column2] == enemy)
            {
                row2++;
                column2--;
            }

            if (ValidPosition(row2, column2) && Distance(row, column, row2, column2) && Board[row2, column2] == Turn)
            {
                valid = true;
                directions.Add(Direction.DownLeft);
            }

            // Down right
            row2 = row + 1;
            column2 = column + 1;

            while (ValidPosition(row2, column2) && Board[row2, column2] == enemy)
            {
                row2++;
                column2++;
            }

            if (ValidPosition(row2, column2) && Distance(row, column, row2, column2) && Board[row2, column2] == Turn)
            {
                valid = true;
                directions.Add(Direction.DownRight);
            }

            return valid;
        }
        
    }

}
