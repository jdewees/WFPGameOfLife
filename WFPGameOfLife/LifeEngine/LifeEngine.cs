using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using WFPGameOfLife.Models;

namespace WFPGameOfLife
{
    internal class LifeEngine
    {

        /// <summary>
        /// Gets and Sets the current generation number
        /// </summary>
        public int Epoch { get; set; }
        public Generation CurrentGeneration { get; set; }



        public LifeEngine(Generation currentGeneration)
        {
            CurrentGeneration = currentGeneration;
            Epoch = 1;

        }

        public LifeEngineActionResult Evolve()
        {
            const int UnderThereshold = 2;
            const int OverThreshold = 3;
            const int ReproThreshold = 3;

            var cellLifeChangedList = new List<Tuple<int, int, bool>>();
            for (int row = 0; row < CurrentGeneration.GameSize; row++)
            {
                for (int col = 0; col < CurrentGeneration.GameSize; col++)
                {
                    var c = CurrentGeneration.GetCell(row, col);
                    int numNeighboursAlive = NeighboursAlive(CurrentGeneration, c);

                    if (c.IsAlive && (numNeighboursAlive < UnderThereshold || numNeighboursAlive > OverThreshold))
                    {
                        cellLifeChangedList.Add(new Tuple<int, int, bool>(row, col, false));
                    }
                    else if (!c.IsAlive && numNeighboursAlive == ReproThreshold)
                    {
                        cellLifeChangedList.Add(new Tuple<int, int, bool>(row, col, true));
                    }
                }
            }

            if (cellLifeChangedList.Any())
            {
                Epoch++;
                Parallel.ForEach(cellLifeChangedList, tuple => CurrentGeneration.SetCell(tuple.Item1, tuple.Item2, tuple.Item3));
            }

            return new LifeEngineActionResult(evolutionEnded: !cellLifeChangedList.Any(), generationNumber: Epoch);

        }

        /// <summary>
        /// Given a generation and a cell, returns the number of neighbours surrounding a cell that are alive.
        /// TODO: Implement a cache so we dont' have to recompute the list of each cell neighbours each time.
        /// </summary>
        /// <param name="gen"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        private int NeighboursAlive(Generation gen, Cell cell)
        {
            int numberofNeighboursAlive = 0;
            var neighbours = new List<Cell>
            {
                gen.GetCell(cell.Row-1, cell.Column-1),
                gen.GetCell(cell.Row-1, cell.Column),
                gen.GetCell(cell.Row-1, cell.Column+1),
                gen.GetCell(cell.Row, cell.Column-1),
                gen.GetCell(cell.Row, cell.Column+1),
                gen.GetCell(cell.Row+1, cell.Column),
                gen.GetCell(cell.Row+1, cell.Column-1),
                gen.GetCell(cell.Row+1, cell.Column+1)

            };

            neighbours.ForEach(neighbouringCell => numberofNeighboursAlive += (neighbouringCell != null && neighbouringCell.IsAlive) ? 1 : 0);
            return numberofNeighboursAlive;
        }

        public int GetUniverseSize()
        {
            return CurrentGeneration.GameSize;
        }

        public void SetCell(int row, int col, bool alive)
        {
            CurrentGeneration.SetCell(row, col, alive);
        }

        public Cell GetCell(int row, int col)
        {
            return CurrentGeneration.GetCell(row, col);
        }

        public LifeEngineActionResult ResetGeneration()
        {
            CurrentGeneration.Reset();
            Epoch = 1;
            return new LifeEngineActionResult(evolutionEnded: true, generationNumber: Epoch);
        }

        public void SwitchCellIsAlive(int row, int col)
        {
            CurrentGeneration.SwitchCellIsAlive(row, col);
        }
    }
}
