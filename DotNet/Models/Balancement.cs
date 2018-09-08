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
        #region public fields
        //public Estimateur estimateur; 
        //public DataFeed data;
        public PortefeuilleModel portfolio;
        public Pricer priceDelta;
        private IDataFeedProvider dataFeedProvider;
        #endregion
        public Balancement(DataFeed data)
        {
            DateTime dateDebut = DateTime.Now;
            VanillaCall option = new VanillaCall("Vanilla Call", new Share("VanillaShare", "1"), new DateTime(2019,12,04), 8);
            var priceList = dataFeedProvider.GetDataFeed(option, dateDebut);
            priceDelta = new Pricer();
            priceDelta.PriceCall(option, dateDebut, dataFeedProvider.NumberOfDaysPerYear, Convert.ToDouble(priceList[0].PriceList[option.UnderlyingShareIds[1]]), 0.25);

        }

    }
}
