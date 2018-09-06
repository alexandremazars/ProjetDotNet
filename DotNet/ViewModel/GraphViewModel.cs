using DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.ViewModel
{
    class GraphViewModel 
    {
        private Graph graph;
        public string Name { get { return "HedgeVsPorfolio"; } }
        public GraphViewModel(SimulationModel simulation)
        {
            graph = new Graph();
            graph.setSimulation(simulation);

        }
    }
}
