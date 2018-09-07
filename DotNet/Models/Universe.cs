using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Models
{
    internal class Universe
    {
        #region Private Fields

        private SimulationModel simulation;
        private Graph graph;
        //private IDataType dataType;

        #endregion Private Fields

        #region Public Constructors

        public Universe(SimulationModel simulation, Graph graph)
        {
            this.simulation = simulation;
            this.graph = graph;
            graph.setSimulation(simulation);
        }

        #endregion Public Constructors

        #region Public Properties


        public SimulationModel Simulation
        {
            get { return simulation; }
            set
            {
                simulation = value;
                //RaisePropertyChanged(nameof(simulation));
            }
        }

        public Graph Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                graph.setSimulation(simulation);
                //RaisePropertyChanged(nameof(graph);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void InitializeUniverse()
        {
            Graph.setSimulation(simulation);
        }
        /* 
       public void UpdateField()
        {
            int[,] newField = new int[Field.GetLength(0), Field.GetLength(1)];
            for (int i = 0; i < Field.GetLength(0); i++)
            {
                for (int j = 0; j < Field.GetLength(1); j++)
                {
                    newField[i, j] = GameOfLifeRule(i, j);
                }
            }
            Field = newField;
        }
        */
        #endregion Public Methods
        
    }
}
