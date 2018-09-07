using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Models
{
    class Balancement
    {
        //public Estimateur estimateur; 
        //public DataFeed data;
        public PortefeuilleModel portefolio;
        public Pricer priceDelta;
        private IDataFeedProvider dataFeedProvider;

        public Balancement(DataFeed data)
        {
            DateTime dateDebut = DateTime.Now;
            VanillaCall option = new VanillaCall("Vanilla Call", new Share("VanillaShare", "1"), new DateTime(2019,12,04), 8);
            var priceList = dataFeedProvider.GetDataFeed(option, dateDebut);
            priceDelta = new Pricer();
            //priceDelta.PriceCall(option, dateDebut, dataFeedProvider.NumberOfDaysPerYear, priceList[0], 0.25);

        }

    }
}
