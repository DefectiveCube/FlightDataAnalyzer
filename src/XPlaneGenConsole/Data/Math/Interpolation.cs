using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expr = MathNet.Symbolics.Expression;

using MathNet;
using MathNet.Numerics;
using MathNet.Symbolics;

namespace XPlaneGenConsole.Data.Math
{
    public class Interpolation
    {
        static void LagrangePolynomial()
        {
            var x = Expr.Symbol("x");
            var sb = new StringBuilder();
            sb.Append("2 * (x - 1) * (x - 3) * (x - 4) * (x - 6) / [(0 - 1) * (0 - 3) * (0 - 4) * (0 - 6)]");
            sb.Append("+ 1 * (x - 0) * (x - 3) * (x - 4) * (x - 6) / [(1 - 0) * (1 - 3) * (1 - 4) * (1 - 6)]");
            sb.Append("+ 3 * (x - 0) * (x - 1) * (x - 4) * (x - 6) / [(3 - 0) * (3 - 1) * (3 - 4) * (3 - 6)]");
            sb.Append("+ 0 * (x - 0) * (x - 1) * (x - 3) * (x - 6) / [(4 - 0) * (4 - 1) * (4 - 3) * (4 - 6)]");
            sb.Append("+ 5 * (x - 0) * (x - 1) * (x - 3) * (x - 4) / [(6 - 0) * (6 - 1) * (6 - 3) * (6 - 4)]");

            var result = Infix.Parse(sb.ToString());

            
        }
    }
}
