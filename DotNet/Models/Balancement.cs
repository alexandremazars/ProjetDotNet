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
        public Portfolio portfolio;
        public Pricer pricer;
        private IDataFeedProvider dataFeedProvider;
        public int periodeRebalancement;
        #endregion
        public Balancement(DataFeed data)
        {
            DateTime dateDebut = DateTime.Now;
            VanillaCall option = new VanillaCall("Vanilla Call", new Share("VanillaShare", "1"), new DateTime(2019, 12, 04), 8);
            var priceList = dataFeedProvider.GetDataFeed(option, dateDebut);
            //Je pense en fait que la fonction Pricer c'est la fonction doit être un fonction qui renvoit pricedelta et du 
            //coup c'est la bas qu'on differencie les baskets des vanilles
            pricer = new Pricer();
            PricingResults priceDelta = pricer.PriceCall(option, dateDebut, dataFeedProvider.NumberOfDaysPerYear, Convert.ToDouble(priceList[0].PriceList[option.UnderlyingShareIds[1]]), 0.25);
            Dictionary<string, double> compo = new Dictionary<string, double> { };
            int i = 1;
            foreach (string id in option.UnderlyingShareIds)
            {
                compo[id] = priceDelta.Deltas[i];
                i += 1;
            }
            // Le premier argument de portefeuille doit être le prix de l'option en 0; 
            //Le deuxième la compo
            //le troisième le prix des actifs en 0 
            List<decimal> priceOption = new List<decimal> { Convert.ToDecimal(priceDelta.Price) };
            Portfolio portfolio = new Portfolio(priceOption, compo, priceList[0].PriceList);
            //Traitement des données 

            foreach (DataFeed priceAsset_t in priceList.Skip(1))
            {
                priceDelta = pricer.PriceCall(option, dateDebut, dataFeedProvider.NumberOfDaysPerYear, Convert.ToDouble(priceAsset_t.PriceList[option.UnderlyingShareIds[1]]), 0.25);
                //updateCompo
                foreach (string id in option.UnderlyingShareIds)
                {
                    compo[id] = priceDelta.Deltas[i];
                    i += 1;
                }
                portfolio.UpdatePortfolioValue(priceAsset_t, dataFeedProvider.NumberOfDaysPerYear, periodeRebalancement);
                portfolio.UpdateCompo(compo);
            }
        }
    }
}
