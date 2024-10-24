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

        private static void IterateCurrent(PivotRule rule)
        {
            // Choose entering variable from positive function coefficients
            var positiveCoeffs = function.Where(x => x.Value > 0);

            KeyValuePair<string, double> enteringVariable;

            if(rule == PivotRule.Classic)
            {
                // Select lowest index of biggest coefficient
                enteringVariable = positiveCoeffs.First(x => x.Value == positiveCoeffs.Max(y => y.Value) && !x.Key.Equals("c"));
            }
            else
            {
                // Select lowest index 
                enteringVariable = positiveCoeffs.First(x => !x.Key.Equals("c"));
            }

            eventLog += "Valasztott belepo valtozo: " + enteringVariable.Key + "\n";

            // Find negative coeffs in every base variable line, then get the minimum 
            // of ratio (coeff / constant)
            double minimumRatio = double.MaxValue;

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
                        double ratio = constantValue / (-1*currentValue);

                        if(ratio < minimumRatio)
                        {
                            minimumRatio = ratio;
                            leavingVariableRow = row;
                            leavingConstant = constantValue;
                        }
                    }
                }
            }

            if(leavingVariableRow.Value == null)
            {
                eventLog += " --- Nem tudtunk kilepo valtozot valasztani, tehat a feladat nem korlatos. --- ";
                return;
            }

            // We have a degenerate base, we make note of this
            if (leavingConstant == 0) eventLog += "[!] Degeneralt szotarunk van. [!]\n";

            eventLog += "Legkisebb talalt hanyados: " + minimumRatio + "\n";
            eventLog += "Valasztott kilepo valtozo: " + leavingVariableRow.Key + "\n\n";

            // Now we express the entering from the leaving variable's row
            int leavingVariableIndex = dict.IndexOf(leavingVariableRow);

            //leavingVariableRow.Value.RemoveAll(x => x.Key.Equals(enteringVariable.Key));

            var pivotRow = new KeyValuePair<string, List<KeyValuePair<string, double>>>(enteringVariable.Key, leavingVariableRow.Value);

            pivotRow.Value.Add(new KeyValuePair<string, double>(leavingVariableRow.Key, -1));

            for(int i = 0; i < pivotRow.Value.Count; i++)
            {
                var current = pivotRow.Value[i];

                pivotRow.Value[i] = new KeyValuePair<string, double>(current.Key, current.Value / enteringVariable.Value);
            }

            dict[leavingVariableIndex] = pivotRow;

            // Lastly, substitute this variable in every row
            for (int i = 0; i < dict.Count; i++)
            {
                string varName = dict[i].Key;

                if(varName.Equals(enteringVariable.Key)) continue;

                var row = dict[i].Value;

                var currentEntering = row.First(x => x.Key.Equals(enteringVariable.Key));

                double coefficient = currentEntering.Value;

                int rowLength = row.Count;

                for(int k = 0; k < rowLength; k++)
                {
                    var current = row[k];

                    var pivot = pivotRow.Value.First(x => x.Key.Equals(current.Key));
                    double newValue = current.Value + (pivot.Value * coefficient);

                    var substituted = new KeyValuePair<string, double>(current.Key, newValue);

                    row[k] = substituted;
                }

                dict[i] = new KeyValuePair<string, List<KeyValuePair<string, double>>>(varName, row);
            }

            // In the function as well
            for(int i = 0; i < function.Count; i++)
            {
                var current = function[i];

                var entering = function.First(x => x.Key.Equals(enteringVariable.Key));
                var pivot = pivotRow.Value.First(x => x.Key.Equals(current.Key));

                double newValue = current.Value + (pivot.Value * entering.Value);

                var substituted = new KeyValuePair<string, double>(current.Key, newValue);

                function[i] = substituted;
            }
        }

        private static bool IsCurrentOptimal()
        {
            // Check if the current dictionary is optimal
            // Meaning the function has a non-negative constant value,
            // and the variables all have negative coefficients

            if (function.First(x => x.Key.Equals("c")).Value < 0) return false;

            for(int i = 1; i < function.Count; i++)
            {
                double coefficient = function[i].Value;

                if(coefficient > 0) return false;
            }

            return true;
        }

        private static string MakeTextDump()
        {
            string retval = string.Empty;

            retval += "Jelenlegi szotar: \n\n";

            for(int i = 0; i < dict.Count; i++)
            {
                var row = dict[i].Value;

                retval += dict[i].Key + " = " + row[0].Value;

                for(int k = 1; k < row.Count; k++)
                {
                    var currentVar = row[k];

                    retval += " + " + currentVar.Key + "*" + currentVar.Value;
                }

                retval += "\n";
            }

            retval += "-----------------------------\n";

            retval += "z = " + function[0].Value;

            for(int i = 1; i < function.Count; i++)
            {
                var current = function[i];

                retval += " + " + current.Key + "*" + current.Value;
            }

            return retval;
        }

        public static void Optimize(PivotRule rule)
        {
            eventLog = string.Empty;
            int nIterations = 0;

            // Iterate until optimal
            while(!IsCurrentOptimal())
            {
                IterateCurrent(rule);
                nIterations++;

                eventLog += MakeTextDump();

                eventLog += "\n--- " + nIterations + ". iteracio vege. ---\n";
            }

            eventLog += "--- Optimalis szotarra jutottunk. ---";
        }

        public static string GetEventLog()
        {
            return eventLog;
        }
    }
}
