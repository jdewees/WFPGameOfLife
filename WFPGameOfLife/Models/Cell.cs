using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WFPGameOfLife.Models
{
    internal class Cell : ObservableBase
    {
        public int Row { get; set; }
        public int Column { get; set; }
        private bool isAlive;
        public bool IsAlive
        {
            get { return isAlive; }
            set
            {
                isAlive = value;
                OnPropertyChanged();

            }
        }

        public Cell(int row, int column, bool alive)
        {
            Row = row;
            Column = column;
            IsAlive = alive;
        }


    }
}
