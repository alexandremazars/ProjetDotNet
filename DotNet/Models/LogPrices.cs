using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Models
{
    class CovMatrix
    {
        /* Bien faire attention à ne fournir la liste de dataFeed que pour les dates souhaitées (limité à la fenêtre d'estimation par exemple) */

        List<DataFeed> dataFeedList;

        public CovMatrix(List<DataFeed> dataFeedList)
        {
            this.dataFeedList = dataFeedList;
        }


        /*Retourns la matrice des differences de prix, passés au log*/

        public decimal[,] LogReturns()
        {
            decimal[,] returns = new decimal[dataFeedList.Count(),dataFeedList.First().PriceList.Count()];
            
            for (int i = 1; i < dataFeedList.Count(); i++)
            {
                for (int j = 0; j < dataFeedList.First().PriceList.Count(); j ++)
                {
                    decimal element1 = dataFeedList[i-1].PriceList.ElementAt(j).Value;
                    decimal element2 = dataFeedList[i].PriceList.ElementAt(j).Value;
                    returns[i - 1, j] = (decimal)Math.Log((double)element2) - (decimal)Math.Log((double)element1);

                }
            }
            return returns;
        }

        
    }
}
