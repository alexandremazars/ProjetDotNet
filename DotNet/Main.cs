using DotNet.Models;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet
{
    class Main
    {
        /*SimulatedDataFeedProvider simulatedData;
        VanillaCall vanillaCall;
        
        public Main()
        {
            int strike = 8;
  
            simulatedData = new SimulatedDataFeedProvider();
            var dateMaturite = new DateTime(2019, 6, 6);
            vanillaCall = new VanillaCall("Vanilla", new Share("VanillaShare", "1"), dateMaturite, strike);
            var priceList = simulatedData.GetDataFeed(vanillaCall, DateTime.Now);
            var jour0 = priceList[0];


            RebalancementModel couverture = new RebalancementModel(vanillaCall, jour0.Date, (double) jour0.PriceList["1"]);
            couverture.nbActifSsJacents = couverture.Delta();
            couverture.valeurPortefeuille = couverture.prixOption();
            double optionInitial = couverture.valeurPortefeuille;
            couverture.liquidite = couverture.Liquidite();

            for (var i=1; i<priceList.Count; i++)
            {
                var element = priceList[i];
                decimal spotPrice = element.PriceList["1"];
                RebalancementModel NewCouverture = new RebalancementModel(vanillaCall, element.Date, (double)spotPrice, couverture);
                couverture = NewCouverture;
                Console.WriteLine("Payoff de l'option: " + (spotPrice - strike));
                Console.WriteLine("Valeur portefeuille: " + NewCouverture.ValeurPortefeuille());
                Console.WriteLine("Pourcentage: " + (NewCouverture.ValeurPortefeuille() - NewCouverture.prixOption())/optionInitial);
            }
        }*/
    }
}
