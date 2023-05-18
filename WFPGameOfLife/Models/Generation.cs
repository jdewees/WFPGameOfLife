using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WFPGameOfLife.Models
{
    internal class Generation 
    {
        private readonly Cell[,] universe;
        public int GameSize { get; set; }

        public Generation(int gameSize)
        {
            universe = new Cell[gameSize, gameSize];
            GameSize = gameSize;
            Init();
        }

        private void Init()
        {
            var rd = new Random();
            for (int row = 0; row < GameSize; row++)
            {
                for (int col = 0; col < GameSize; col++)
                {
                    universe[row, col] = new Cell(row, col, rd.Next(4)==1);
                }

            }
        }

        public void Reset()
        {
            var rd = new Random();
            for (int row = 0; row < GameSize; row++)
            {
                for (int col = 0; col < GameSize; col++)
                {
                    SetCell(row, col, rd.Next(4) == 1);
                }

            }
        }

        public void SetCell(int row, int col, bool alive)
        {
            var cell = GetCell(row, col);
            if (cell == null)
            {
                throw new ArgumentNullException(string.Format("no cell exists for the specified row{0} and column{1}",row,col));
            }

            cell.IsAlive = alive;
        }
        public Cell GetCell(int row, int col)
        {
            if (row < 0) row += GameSize;
            else if (row >= GameSize) row -= GameSize;
            if (col < 0) col += GameSize;
            else if (col >= GameSize) col -= GameSize;
                
            return universe[row, col];
        }

        public void SwitchCellIsAlive(int row, int column)
        {
            var c  = GetCell(row, column);
            c.IsAlive = !c.IsAlive;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int row = 0; row < GameSize; row++)
            {
                for (int col = 0; col < GameSize; col++)
                {
                   sb.Append(string.Format("Cell at [{0}, {1}] is ",row, col, GetCell(row,col).IsAlive));
                }

            }
            return sb.ToString();
        }
    }
}
