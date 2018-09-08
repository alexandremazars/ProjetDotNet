/*=======================================================================

	Wall Risk Engine - ENSIMAG - ENSIMAG module
	C/C++ header file	version 4.1

	Copyright (c) 2018 Raise Partner
	26 rue Gustave Eiffel, 38000 Grenoble, FRANCE
	All rights reserved.

	This software is the confidential and proprietary information
	of Raise Partner. You shall not disclose such Confidential
	Information and shall use it only in accordance with the terms
	of the licence agreement you entered into with Raise Partner.

==========================================================================*/

/*
	All parameters are passed by reference. 

	1. For the following types the argument is passed as follows:

		int a;
		double b;
		int[ROWS*COLUMNS] M;
		double[ROWS*COLUMNS] N; 

		in the function call we declare: 

		function(....,&a,&b,M,N,...)

	2. For pointers:

		int *a;
		double *b;

		a = malloc (ROWS*COLUMNS*sizeof(int));
		b = malloc (ROWS*COLUMNS*sizeof(double));

		in the function call we declare: 

		function(....,a,b,...)

	WARNING: The array sizes must be equal to the specified dimensions
			given here after.

*/

#ifdef __cplusplus
# define CONST const
#else
# define CONST 
#endif


# define F77_ADD_UNDERSCORE 1
#ifndef F77_ADD_UNDERSCORE
# define F77_ADD_UNDERSCORE 1
#endif

#if F77_ADD_UNDERSCORE
# define F77_FUNCTION(f) f##_
# define F77_FUNCTION2(f) f##__
#else
# define F77_FUNCTION(f) f
# define F77_FUNCTION2(f) f
#endif

/**********************************************************************************
								WREallocIT
							
	Index Tracking Minimization
	This component computes the optimal portfolio which minimizing the tracking error vs. a benchmark.

***********************************************************************************

INPUTS
	nbAssets: integer,
		number of asset(s) (>1)
	cov: double array of dimension *nbAssets by *nbAssets,
		covariance matrix
	expectedReturns: double array of dimension *nbAssets,
		mean return(s)
	benchmarkCov: double array of dimension *nbAssets,
		covariance of benchmark
	benchmarkExpectedReturn: double,
		benchmark mean return supplied
	nbEqConst: integer,
		number of equality constraints (>=0)
	nbIneqConst: integer,
		number of inequality constraints (>=0)
	C: double array of dimension *nbAssets by *nbEqConst+*nbIneqConst,
		matrix of constraints. If no constraints are given, C=0
	b: double array of dimension *nbEqConst+*nbIneqConst,
		vector of constraints. If no constraints are given, b=0
	minWeights: double array of dimension *nbAssets,
		lower bounds
	maxWeights: double array of dimension *nbAssets,
		upper bounds
	relativeTargetReturn: double,
		target performance relative to the benchmark

OUTPUTS
	optimalWeights: double array of dimension *nbAssets,
		optimal portfolio
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREallocIT(
	CONST int * nbAssets, CONST double * cov, CONST double * expectedReturns, 
	CONST double * benchmarkCov, CONST double * benchmarkExpectedReturn, CONST int * nbEqConst, 
	CONST int * nbIneqConst, CONST double * C, CONST double * b, 
	CONST double * minWeights, CONST double * maxWeights, CONST double * relativeTargetReturn, 
	 double * optimalWeights,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREallocMV
							
	Mean-Variance (Markowitz)
	This component computes the optimal portfolio with minimal volatility under a performance constraint (target return) and various linear constraints.

***********************************************************************************

INPUTS
	nbAssets: integer,
		number of asset(s) (>1)
	cov: double array of dimension *nbAssets by *nbAssets,
		covariance matrix
	expectedReturns: double array of dimension *nbAssets,
		mean return(s)
	nbEqConst: integer,
		number of equality constraints (>=0)
	nbIneqConst: integer,
		number of inequality constraints (>=0)
	C: double array of dimension *nbAssets by *nbEqConst+*nbIneqConst,
		matrix of constraints. If no constraints are given, C=0
	b: double array of dimension *nbEqConst+*nbIneqConst,
		vector of constraints. If no constraints are given, b=0
	minWeights: double array of dimension *nbAssets,
		lower bounds
	maxWeights: double array of dimension *nbAssets,
		upper bounds
	targetReturn: double,
		target return

OUTPUTS
	optimalWeights: double array of dimension *nbAssets,
		optimal portfolio
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREallocMV(
	CONST int * nbAssets, CONST double * cov, CONST double * expectedReturns, 
	CONST int * nbEqConst, CONST int * nbIneqConst, CONST double * C, 
	CONST double * b, CONST double * minWeights, CONST double * maxWeights, 
	CONST double * targetReturn,  double * optimalWeights,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREallocSharpeRatio
							
	Maximum Sharpe Ratio
	This component computes the optimal portfolio with maximum Sharpe Ratio under various linear constraints.

***********************************************************************************

INPUTS
	nbAssets: integer,
		number of asset(s) (>1)
	cov: double array of dimension *nbAssets by *nbAssets,
		covariance matrix
	expectedReturns: double array of dimension *nbAssets,
		mean return(s)
	nbEqConst: integer,
		number of equality constraints (>=0)
	nbIneqConst: integer,
		number of inequality constraints (>=0)
	C: double array of dimension *nbAssets by *nbEqConst+*nbIneqConst,
		matrix of constraints. If no constraints are given, C=0
	b: double array of dimension *nbEqConst+*nbIneqConst,
		vector of constraints. If no constraints are given, b=0
	minWeights: double array of dimension *nbAssets,
		lower bounds
	maxWeights: double array of dimension *nbAssets,
		upper bounds
	riskFreeRate: double,
		the risk-free rate (with the same frequency as expected returns)

OUTPUTS
	optimalWeights: double array of dimension *nbAssets,
		optimal portfolio
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREallocSharpeRatio(
	CONST int * nbAssets, CONST double * cov, CONST double * expectedReturns, 
	CONST int * nbEqConst, CONST int * nbIneqConst, CONST double * C, 
	CONST double * b, CONST double * minWeights, CONST double * maxWeights, 
	CONST double * riskFreeRate,  double * optimalWeights,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREanalysisExanteModifiedVaR
							
	Ex-ante Modified Value-at-Risk
	The modified Value-at-Risk (VaR) adjusts the traditional Gaussian VaR with the Skewness and Kurtosis of the distribution.

***********************************************************************************

INPUTS
	nbValues: integer,
		number of values (>=4)
	nbAssets: integer,
		number of asset(s) (>=1)
	weights: double array of dimension *nbAssets,
		portfolio's weight(s)
	assetsReturns: double array of dimension *nbValues by *nbAssets,
		asset(s) returns
	cov: double array of dimension *nbAssets by *nbAssets,
		covariance matrix
	probabilityLevel: double,
		probability level (>0 and <1) - if exanteModifiedValueAtRisk < 0: probability that the loss will be lower to exanteModifiedValueAtRisk - if exanteModifiedValueAtRisk > 0: probability that the gain will be upper to exanteModifiedValueAtRisk

OUTPUTS
	exanteModifiedValueAtRisk: double array of dimension 1,
		portfolio's ex-ante modified Value-at-Risk
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREanalysisExanteModifiedVaR(
	CONST int * nbValues, CONST int * nbAssets, CONST double * weights, 
	CONST double * assetsReturns, CONST double * cov, CONST double * probabilityLevel, 
	 double * exanteModifiedValueAtRisk,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREanalysisExanteNormalVaR
							
	Ex-ante Value-at-Risk (Gaussian)
	This component computes the Gaussian ex-ante Value-at-Risk (VaR) with the parametric Variance-Covariance method.

***********************************************************************************

INPUTS
	nbAssets: integer,
		covariance matrix size (>=1)
	cov: double array of dimension *nbAssets by *nbAssets,
		covariance matrix
	expectedReturns: double array of dimension *nbAssets,
		mean return(s)
	weights: double array of dimension *nbAssets,
		portfolio's weight(s)
	probabilityLevel: double,
		probability level (>0 and <1) - if exanteNormalValueAtRisk < 0: probability that the loss will be lower to exanteNormalValueAtRisk - if exanteNormalValueAtRisk > 0: probability that the gain will be upper to exanteNormalValueAtRisk

OUTPUTS
	exanteNormalValueAtRisk: double array of dimension 1,
		portfolio's ex-ante Normal Value-at-Risk
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREanalysisExanteNormalVaR(
	CONST int * nbAssets, CONST double * cov, CONST double * expectedReturns, 
	CONST double * weights, CONST double * probabilityLevel,  double * exanteNormalValueAtRisk, 
	 int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREanalysisExanteReturn
							
	Ex-ante Mean Return
	This function computes the ex-ante mean return (performance) of a given portfolio.

***********************************************************************************

INPUTS
	nbAssets: integer,
		number of asset(s) (>=1)
	expectedReturns: double array of dimension *nbAssets,
		mean return(s)
	weights: double array of dimension *nbAssets,
		weight(s)

OUTPUTS
	exanteReturn: double array of dimension 1,
		portfolio's mean return
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREanalysisExanteReturn(
	CONST int * nbAssets, CONST double * expectedReturns, CONST double * weights, 
	 double * exanteReturn,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREanalysisExanteVolatility
							
	Ex-ante Volatility
	This component computes the ex-ante volatility (standard deviation) of a portfolio.

***********************************************************************************

INPUTS
	nbAssets: integer,
		covariance matrix size (>=1)
	cov: double array of dimension *nbAssets by *nbAssets,
		covariance matrix
	weights: double array of dimension *nbAssets,
		weight(s)

OUTPUTS
	exanteVolatility: double array of dimension 1,
		portfolio's ex-ante volatility
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREanalysisExanteVolatility(
	CONST int * nbAssets, CONST double * cov, CONST double * weights, 
	 double * exanteVolatility,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREanalysisExpostReturn
							
	Ex-post Mean Return
	This function computes the ex-post mean return (performance).

***********************************************************************************

INPUTS
	nbValues: integer,
		number of points (>=1)
	portfolioReturns: double array of dimension *nbValues,
		portfolio's return(s)

OUTPUTS
	expostReturn: double array of dimension 1,
		ex-post mean return
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREanalysisExpostReturn(
	CONST int * nbValues, CONST double * portfolioReturns,  double * expostReturn, 
	 int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREanalysisExpostVolatility
							
	Ex-post Volatility
	The volatility (or standard deviation) is a statistical measure of the historical returns. The ex-post volatility is usually computed using daily or monthly returns.

***********************************************************************************

INPUTS
	nbValues: integer,
		number of points (>1)
	portfolioReturns: double array of dimension *nbValues,
		portfolio's return(s)

OUTPUTS
	expostVolatility: double array of dimension 1,
		ex-post volatility
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREanalysisExpostVolatility(
	CONST int * nbValues, CONST double * portfolioReturns,  double * expostVolatility, 
	 int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREanalysisGaussianKernel
							
	Density Estimation by Gaussian Kernel Method
	

***********************************************************************************

INPUTS
	n: integer,
		number of return(s) (>=2)
	m: integer,
		number of grid point(s) (>=1)
	x: double array of dimension *n,
		return(s)
	y: double array of dimension *m,
		grid (increasing order)

OUTPUTS
	z: double array of dimension *m,
		Gaussian kernel estimation
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREanalysisGaussianKernel(
	CONST int * n, CONST int * m, CONST double * x, 
	CONST double * y,  double * z,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREensimagTest
							
	Tests the call to the library
	Tests the call to the library and provides some products informations.

***********************************************************************************

INPUTS

OUTPUTS
	ProductId: integer array of dimension 1,
		Product Identifier
	MajorVersion: integer array of dimension 1,
		Major version
	MinorVersion: integer array of dimension 1,
		Minor version
	build_number: integer array of dimension 1,
		version number (3rd)
	magicnumber1: integer array of dimension 1,
		The magic number (staff only)
	magicnumber2: integer array of dimension 1,
		An other magic number (staff only)
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREensimagTest(
	 int * ProductId,  int * MajorVersion,  int * MinorVersion, 
	 int * build_number,  int * magicnumber1,  int * magicnumber2);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREmodelingCorr
							
	Correlation Matrix
	

***********************************************************************************

INPUTS
	nbValues: integer,
		number of points (>1)
	nbAssets: integer,
		number of variables (>=1)
	assetsReturns: double array of dimension *nbValues by *nbAssets,
		returns matrix

OUTPUTS
	corr: double array of dimension *nbAssets by *nbAssets,
		correlation matrix
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREmodelingCorr(
	CONST int * nbValues, CONST int * nbAssets, CONST double * assetsReturns, 
	 double * corr,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREmodelingCov
							
	Covariance Matrix
	

***********************************************************************************

INPUTS
	nbValues: integer,
		number of points (>1)
	nbAssets: integer,
		number of variables (>=1)
	assetsReturns: double array of dimension *nbValues by *nbAssets,
		returns matrix

OUTPUTS
	cov: double array of dimension *nbAssets by *nbAssets,
		covariance matrix
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREmodelingCov(
	CONST int * nbValues, CONST int * nbAssets, CONST double * assetsReturns, 
	 double * cov,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREmodelingLogReturns
							
	Logarithmic return(s)
	

***********************************************************************************

INPUTS
	nbValues: integer,
		number of historical points (>=2)
	nbAssets: integer,
		number of variables (>=1)
	assetsValues: double array of dimension *nbValues by *nbAssets,
		matrix of values (>0)
	horizon: integer,
		investment horizon (0 > horizon < nbValues)

OUTPUTS
	assetsReturns: double array of dimension *nbValues-*horizon by *nbAssets,
		matrix of returns
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREmodelingLogReturns(
	CONST int * nbValues, CONST int * nbAssets, CONST double * assetsValues, 
	CONST int * horizon,  double * assetsReturns,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREmodelingReturns
							
	Simple Net return(s) (Arithmetic)
	

***********************************************************************************

INPUTS
	nbValues: integer,
		number of historical points (>=2)
	nbAssets: integer,
		number of variables (>=1)
	assetsValues: double array of dimension *nbValues by *nbAssets,
		matrix of values (>0)
	horizon: integer,
		investment horizon (0 > horizon < nbValues)

OUTPUTS
	assetsReturns: double array of dimension *nbValues-*horizon by *nbAssets,
		matrix of returns
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREmodelingReturns(
	CONST int * nbValues, CONST int * nbAssets, CONST double * assetsValues, 
	CONST int * horizon,  double * assetsReturns,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREmodelingSDLS
							
	Semi-Definite Least Square (SDLS) optimization
	

***********************************************************************************

INPUTS
	p: integer,
		size of the matrix (>=1)
	Q: double array of dimension *p by *p,
		input matrix
	nbEqConst: integer,
		number of equality constraints (>=0)
	Ceq: double array of dimension *p by *nbEqConst*(*p),
		symmetric matrices of equality constraints
	bEq: double array of dimension *nbEqConst,
		vector of equality constraints
	nbIneqConst: integer,
		number of inequality constraints (>=0)
	Cineq: double array of dimension *p by *nbIneqConst*(*p),
		symmetric matrices of inequality constraints
	bLowerIneq: double array of dimension *nbIneqConst,
		vector of lower constraints
	bUpperIneq: double array of dimension *nbIneqConst,
		vector of upper constraints
	constPrecision: double,
		constraints precision (>0)
	minEigenValue: double,
		minimum level of eigenvalue desired in the output matrix (>=0)

OUTPUTS
	X: double array of dimension *p by *p,
		corrected matrix
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREmodelingSDLS(
	CONST int * p, CONST double * Q, CONST int * nbEqConst, 
	CONST double * Ceq, CONST double * bEq, CONST int * nbIneqConst, 
	CONST double * Cineq, CONST double * bLowerIneq, CONST double * bUpperIneq, 
	CONST double * constPrecision, CONST double * minEigenValue,  double * X, 
	 int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREmodelingSDLScorr
							
	Semi-Definite Least Square (SDLS) optimization for a correlation matrix
	

***********************************************************************************

INPUTS
	p: integer,
		size of the matrix (>=1)
	Q: double array of dimension *p by *p,
		input matrix
	constPrecision: double,
		constraints precision (>0)
	minEigenValue: double,
		minimum level of eigenvalue desired in the output matrix (>=0 and <1)

OUTPUTS
	X: double array of dimension *p by *p,
		corrected correlation matrix
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREmodelingSDLScorr(
	CONST int * p, CONST double * Q, CONST double * constPrecision, 
	CONST double * minEigenValue,  double * X,  int * info);
#ifdef __cplusplus
}
#endif 

/**********************************************************************************
								WREsimulGeometricBrownianX
							
	Simulation of a Multidimentional Geometric Brownian Motion
	

***********************************************************************************

INPUTS
	p: integer,
		number of assets (>1)
	T: integer,
		time (days, months, years) (>0)
	N: integer,
		number of sub-division(s) (>1)
	S: double array of dimension *p,
		initial values
	mu: double array of dimension *p,
		process drifts
	cov: double array of dimension *p by *p,
		covariance matrix

OUTPUTS
	y: double array of dimension *N by *p,
		generated process
	info: integer array of dimension 1,
		diagnostic argument
	
RETURN PARAMETER
	integer, error code
	= 0, successful exit,
	!= 0, see error codes list
*/
#ifdef __cplusplus
extern "C" {
#endif 
int WREsimulGeometricBrownianX(
	CONST int * p, CONST int * T, CONST int * N, 
	CONST double * S, CONST double * mu, CONST double * cov, 
	 double * y,  int * info);
#ifdef __cplusplus
}
#endif 

