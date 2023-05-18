using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFPGameOfLife
{
    internal class LifeEngineActionResult
    {
        public bool EvolutionEnded { get; private set; }
        public int GenerationNumber { get; private set; }

        public LifeEngineActionResult(bool evolutionEnded, int generationNumber)
        {
            EvolutionEnded = evolutionEnded;
            GenerationNumber = generationNumber;
        }
    }
}
