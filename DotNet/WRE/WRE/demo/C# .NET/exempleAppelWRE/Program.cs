/*
 *  Copyright (c) 2014 Raise Partner
 *  26, rue Gustave Eifel, 38000 Grenoble, France
 *  All rights reserved.
 *
 *  This software is the confidential and proprietary information
 *  of Raise Partner. You shall not disclose such Confidential
 *  Information and shall use it only in accordance with the terms
 *  of the licence agreement you entered into with Raise Partner.
 *  
 * */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace exempleAppelWRE
{
    class Program
    {
        // import WRE dlls
        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingCov", CallingConvention=CallingConvention.Cdecl)]
        
	// declaration
        public static extern int WREmodelingCov(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
            ref int info
        );
	
        public static double[,] computeCovarianceMatrix(double[,] returns) {
            int dataSize = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            double[,] covMatrix = new double[nbAssets, nbAssets];
            int info = 0;
            int res;
            res = WREmodelingCov(ref dataSize, ref nbAssets, returns, covMatrix, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREmodelingCov encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREmodelingCov encountred a problem. See info parameter for more details");
            }
            return covMatrix;
        }

	public static void dispMatrix(double[,] myCovMatrix){
            int n = myCovMatrix.GetLength(0);
		
            Console.WriteLine("Covariance matrix:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) 
                {
                    Console.Write(myCovMatrix[i, j] + "\t");
                }
                Console.Write("\n");
            }
	}

        static void Main(string[] args)
        {
            // header
            Console.WriteLine("******************************");
            Console.WriteLine("*    WREmodelingCov in C#   *");
            Console.WriteLine("******************************");

            // sample data
            double[,] returns = { {0.05, -0.1, 0.6}, {-0.001, -0.4, 0.56}, {0.7, 0.001, 0.12}, {-0.3, 0.2, -0.1},
                                {0.1, 0.2, 0.3}};
            
            // call WRE via computeCovarianceMatrix encapsulation
            double[,] myCovMatrix = computeCovarianceMatrix(returns);
            
            // display result
            dispMatrix(myCovMatrix);
            
            // ending the program            
            Console.WriteLine("\nType enter to exit");
            Console.ReadKey(true);
        }
    }
}
