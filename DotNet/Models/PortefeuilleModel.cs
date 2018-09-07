using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Models
{
    internal class PortefeuilleModel
    {
        #region Public fieds
        public Dictionary<string, decimal> priceAsset;
        public Dictionary<string, double> composition;
        public decimal liquidity;
        #endregion

        #region Public constructors
        public PortefeuilleModel(Dictionary<string, decimal> priceAsset, Dictionary<string, double> composition, List<decimal> initialPrice)
        {
            this.priceAsset = priceAsset;
            this.composition = composition;
            this.liquidity = 0;
            int i = 1;
            foreach (string element in composition.Keys)
            {
                liquidity += Convert.ToDecimal(composition[element]) * initialPrice[i];
                i += 1;
            }
        }
        #endregion
        #region public methods
        public void UpdateValue()
        {

        }

        public void UpdateCompo()
        {

        }
        #endregion
    }
}
