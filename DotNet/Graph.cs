using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization;
using System.Windows.Media;
using DotNet.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace DotNet
{
    internal class Graph 
    {
        #region public field
        public string Name { get { return "HedgeVsPorfolio"; } }
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public SimulationModel GraphSimulation;
        #endregion
  
        public Graph() {

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Price Option",
                    Values = new ChartValues<decimal>{}
                },
                new LineSeries
                {
                    Title = "Hedge portfolio",
                    Values = new ChartValues<decimal> {}
                }
            };
            if (GraphSimulation != null){
                foreach (var item in GraphSimulation.GetRebalancement())
                {
                    SeriesCollection[0].Values.Add(item.prixOption());
                    SeriesCollection[1].Values.Add(item.ValeurPortefeuille);
                    Labels.Add(item.Date.ToString());
                }
            }
            
            
            
        }

    }
}