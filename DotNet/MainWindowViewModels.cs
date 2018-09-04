using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;

namespace DotNet
{
    internal class MainWindowViewModels : BindableBase
    {
        #region public fields
        public ObservableCollection<IOption> AvailableOptions { get; private set; }
        public double strike { get; set; }
        public DateTime maturity { get; set; }
        public decimal price { get; set; }
        //public ObservableCollection<>
        #endregion

        #region Public Constructors
        public MainWindowViewModels()
        {
            IOption call = new VanillaCall("Vanilla", new Share("VanillaShare", "1"), new DateTime(2019, 1, 6), 10);
            List<IOption> myOptionsList = new List<IOption>() { call };
            AvailableOptions = new ObservableCollection<IOption>(myOptionsList);
            Console.Write(myOptionsList[0].Name);
        }
        #endregion
    }
}
