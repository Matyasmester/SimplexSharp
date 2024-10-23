using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    public static class Simplex
    {
        // :D
        public static List<KeyValuePair<string, List<KeyValuePair<string, double>>>> dict = new List<KeyValuePair<string, List<KeyValuePair<string, double>>>>();

        public static List<KeyValuePair<string, double>> function = new List<KeyValuePair<string, double>>();

        private static string eventLog = string.Empty;

        private static void IterateWith(PivotRule rule)
        {
            // Choose entering variable from positive function coefficients
            var positiveCoeffs = function.Where(x => x.Value > 0);

            KeyValuePair<string, double> enteringVariable;

            if(rule == PivotRule.Classic)
            {
                // Select lowest index of biggest coefficient
                enteringVariable = positiveCoeffs.First(x => x.Value == positiveCoeffs.Max(y => y.Value));
            }
            else
            {
                // Select lowest index 
                enteringVariable = positiveCoeffs.First(x => !x.Key.Equals("c"));
            }

            eventLog += "Valasztott belepo valtozo: " + enteringVariable.Key + "\n";

            // Find negative coeffs in every base variable line, then get the minimum 
            // of ratio (coeff / constant)
            double minimumRatio = double.PositiveInfinity;

            KeyValuePair<string, List<KeyValuePair<string, double>>> leavingVariableRow = default;
            double leavingConstant = 0;

            foreach (var row in dict)
            {
                double constantValue = row.Value.First(x => x.Key.Equals("c")).Value;

                foreach(var x in row.Value)
                {
                    if(!x.Key.Equals(enteringVariable.Key)) continue;

                    double currentValue = x.Value;

                    if(currentValue < 0)
                    {
                        double ratio = constantValue / currentValue;

                        if(ratio < minimumRatio)
                        {
                            minimumRatio = ratio;
                            leavingVariableRow = row;
                            leavingConstant = constantValue;
                        }
                    }
                }
            }

            if(leavingVariableRow.Equals(default))
            {
                eventLog += " --- Nem tudtunk kilepo valtozot valasztani, tehat a feladat nem korlatos. --- ";
                return;
            }

            // We have a degenerate base, we make note of this
            if (leavingConstant == 0) eventLog += "[!] Degeneralt szotarunk van. [!]\n";

            eventLog += "Valasztott kilepo valtozo: " + leavingVariableRow.Key + "\n";

            // Now we express the entering from the leaving variable's row
            int leavingVariableIndex = dict.IndexOf(leavingVariableRow);

            var pivotRow = new KeyValuePair<string, List<KeyValuePair<string, double>>>(enteringVariable.Key, leavingVariableRow.Value);

            pivotRow.Value.Add(new KeyValuePair<string, double>(leavingVariableRow.Key, -1));

            for(int i = 0; i < pivotRow.Value.Count; i++)
            {
                var current = pivotRow.Value[i];

                pivotRow.Value[i] = new KeyValuePair<string, double>(current.Key, current.Value / enteringVariable.Value);
            }

            dict[leavingVariableIndex] = pivotRow;
        }

        public static void OptimizeCurrent(PivotRule rule)
        {
            IterateWith(rule);

            // Check if the current dictionary is optimal
            // Meaning the function has a non-negative constant value,
            // and the variables all have negative coefficients
        }
    }
}
