using DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DotNet.Visualization
{
    /// <summary>
    /// Logique d'interaction pour GraphVisualization.xaml
    /// </summary>
    public partial class GraphVisualization: Window
    {
        public GraphVisualization()
        {
            InitializeComponent();
            this.DataContext = MainWindowViewModels.graphTest;
        }
    }
}
