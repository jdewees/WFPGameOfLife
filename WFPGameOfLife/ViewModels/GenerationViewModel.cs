using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WFPGameOfLife.Models;
using WFPGameOfLife.Utils;

namespace WFPGameOfLife.ViewModels
{
    internal class GenerationViewModel : ObservableBase
    {
        private LifeEngine _lifeEngine;

        public int GameSize
        {
            get
            {
                return _lifeEngine.GetUniverseSize();
            }
        }

        private int populationCount;

        public int PopulationCount
        {
            get { return populationCount; }
            set
            {
                populationCount = value;
                OnPropertyChanged();
            }
        }

        private int generationNumber;

        public int Epoch
        {
            get { return generationNumber; }
            set
            {
                generationNumber = value;
                OnPropertyChanged();
            }

        }

        private bool evolutionEnded;
        public bool EvolutionEnded
        {
            get { return evolutionEnded; }
            set
            {
                evolutionEnded = value;
                OnPropertyChanged();
            }
        }

        private bool _isAnimating = false;
        public bool IsAnimating
        {
            get { return _isAnimating; }
            set
            {
                _isAnimating = value; OnPropertyChanged();
            }
        }
        public GenerationViewModel(int universeSize)
        {

            _lifeEngine = new LifeEngine(new Models.Generation(universeSize));

            EvolveCommand = new Command<object>(_ => Evolve(), _ => CanEvolve());

            ResetCommand = new Command<object>(_ => ResetGame(), _ => CanReset());

            SwitchIsAliveCommand = new Command<string>((rowColOfCell) => SwitchCellLife(rowColOfCell), _ => CanSwitchCellIsAlive());

            AnimateCommand = new Command<object>(_ => Animate(), _ => CanAnimate());

            Epoch = _lifeEngine.Epoch;

        }
        public Cell GetCell(int row, int col)
        {
            return _lifeEngine.GetCell(row, col);
        }

        private bool CanAnimate()
        {
            return !_isAnimating && CanEvolve();
        }

        private void Animate()
        {
            if (CanAnimate()) {
                _isAnimating = true;
                while (CanEvolve())
                {
                    Evolve();
                }
            
            }
            _isAnimating = false;
        }

        private void Evolve()
        {
            LifeEngineActionResult res = _lifeEngine.Evolve();
            Epoch = res.GenerationNumber;
            EvolutionEnded = res.EvolutionEnded;
        }

        private bool CanEvolve()
        {
            return !EvolutionEnded;
        }

        private void ResetGame()
        {
            LifeEngineActionResult res = _lifeEngine.ResetGeneration();

            Epoch = res.GenerationNumber;
            EvolutionEnded = res.EvolutionEnded;
        }

        private bool CanReset()
        {
            return Epoch > 1 || EvolutionEnded;
        }

        private void SwitchCellLife(string rowColOfCell)
        {
            string[] splits = rowColOfCell.Split(new char[] { ',' });
            int row = int.Parse(splits[0]);
            int col = int.Parse(splits[1]);

            _lifeEngine.SwitchCellIsAlive(row, col);
        }

        private bool CanSwitchCellIsAlive()
        {
            return Epoch == 1 && !EvolutionEnded;
        }
        public Command<object> EvolveCommand { get; set; }

        public Command<object> ResetCommand { get; set; }

        public Command<string> SwitchIsAliveCommand { get; set; }

        public Command<object> AnimateCommand { get; set; }

    }
}