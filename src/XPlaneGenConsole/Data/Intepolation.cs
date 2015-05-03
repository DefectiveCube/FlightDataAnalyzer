using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace XPlaneGenConsole
{		/*

[3]

0: x-1 * x-2
1: x-0 * x-2
2: x-0 * x-1


(x-1) * (x-3) * (x-4) * (x-6)
		(x-1) * (x-3) = x^2 -4x 	4
		(x-4) * (x-6) = x^2 -10x 	25

x^2 -4x + 5
x^2 -10x + 25

x^4 -10x^3 25x^2 -4x^3  5x^2
		*/
	
	public class LagrangePolynomial
	{
		public List<Tuple<double,double>> Points{ get; set; }


		public void GetExpression(){
			double total = 0;


			for(int i = 0; i < Points.Count; i++){


				double product = 1;

				for (int j = 0; j < Points.Count; j++) {
					if (j != i) {


					}
				}

			}
		}
	}

	public static class Regression
	{

	}

	public static class Intepolation
	{
		public static double Linear(double y1, double y2, double mu){
			return y1 * (1 - mu) + y2 * mu;
		}

		public static double Cosine(double y1, double y2)
		{
			return 0;
		}
	}
}