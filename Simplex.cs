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

        private static bool isNotLimited = false;

        private static void IterateCurrent(PivotRule rule)
        {
            // Choose entering variable from positive function coefficients
            var positiveCoeffs = function.FindAll(x => x.Value > 0 && !x.Key.Equals("c"));

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
                isNotLimited = true;

                return;
            }

            // We have a degenerate base, we make note of this
            if (leavingConstant == 0) eventLog += "[!] Degeneralt szotarunk van. [!]\n";

            eventLog += "Legkisebb talalt hanyados: " + minimumRatio + "\n";
            eventLog += "Valasztott kilepo valtozo: " + leavingVariableRow.Key + "\n\n";

            // Now we express the entering from the leaving variable's row
            int leavingVariableIndex = dict.IndexOf(leavingVariableRow);

            double enteringValue = leavingVariableRow.Value.First(x => x.Key.Equals(enteringVariable.Key)).Value;

            leavingVariableRow.Value.RemoveAll(x => x.Key.Equals(enteringVariable.Key));

            var pivotRow = new KeyValuePair<string, List<KeyValuePair<string, double>>>(enteringVariable.Key, leavingVariableRow.Value);

            pivotRow.Value.Add(new KeyValuePair<string, double>(leavingVariableRow.Key, -1));

            //double pivotEnteringValue = pivotRow.Value.First(x => x.Key.Equals(enteringVariable.Key)).Value;

            for(int i = 0; i < pivotRow.Value.Count; i++)
            {
                var current = pivotRow.Value[i];

                pivotRow.Value[i] = new KeyValuePair<string, double>(current.Key, -1 * current.Value / enteringValue);
            }

            dict[leavingVariableIndex] = pivotRow;

            // Lastly, substitute this variable in every row
            for (int i = 0; i < dict.Count; i++)
            {
                string varName = dict[i].Key;

                if(varName.Equals(enteringVariable.Key)) continue;

                var row = dict[i].Value;

                var newRow = new KeyValuePair<string, List<KeyValuePair<string, double>>>(varName, new List<KeyValuePair<string, double>>());

                var currentEntering = row.Any(x => x.Key.Equals(enteringVariable.Key)) ?

                                    row.First(x => x.Key.Equals(enteringVariable.Key)) :

                                    new KeyValuePair<string, double>(enteringVariable.Key, 0);

                double coefficient = currentEntering.Value;

                // Pivotrowban menjünk inkább
                for(int k = 0; k < pivotRow.Value.Count; k++)
                {
                    var pivot = pivotRow.Value[k];

                    if (pivot.Key.Equals(enteringVariable.Key)) continue;

                    var current = row.Any(x => x.Key.Equals(pivot.Key)) ? 

                                row.First(x => x.Key.Equals(pivot.Key)) : 

                                new KeyValuePair<string, double>(pivot.Key, 0);

                    double newValue = current.Value + (pivot.Value * coefficient);

                    var substituted = new KeyValuePair<string, double>(current.Key, newValue);

                    newRow.Value.Add(substituted);
                }

                dict[i] = newRow;
            }

            var funcEntering = function.Any(x => x.Key.Equals(enteringVariable.Key)) ?

                             function.First(x => x.Key.Equals(enteringVariable.Key)) : 

                             new KeyValuePair<string, double>(enteringVariable.Key, 0);

            double funcCoefficient = funcEntering.Value;

            var newFunc = new List<KeyValuePair<string, double>>();

            // In the function as well
            for (int i = 0; i < pivotRow.Value.Count; i++)
            {
                var pivot = pivotRow.Value[i];

                var current = function.Any(x => x.Key.Equals(pivot.Key)) ?

                                function.First(x => x.Key.Equals(pivot.Key)) :

                                new KeyValuePair<string, double>(pivot.Key, 0);

                double newValue = current.Value + (pivot.Value * funcCoefficient);

                var substituted = new KeyValuePair<string, double>(current.Key, newValue);

                newFunc.Add(substituted);
            }

            function = newFunc;
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

                    retval += " + " + currentVar.Value + " * " + currentVar.Key;
                }

                retval += "\n";
            }

            retval += "-----------------------------\n";

            retval += "z = " + function[0].Value;

            for(int i = 1; i < function.Count; i++)
            {
                var current = function[i];

                retval += " + " + current.Value + " * " + current.Key;
            }

            return retval;
        }

        public static void Optimize(PivotRule rule)
        {
            eventLog = string.Empty;
            int nIterations = 0;

            // Iterate until optimal
            while(!IsCurrentOptimal() && !isNotLimited)
            {
                IterateCurrent(rule);
                nIterations++;

                eventLog += MakeTextDump();

                eventLog += "\n\n--- " + nIterations + ". iteracio vege. ---\n";
            }

            if(!isNotLimited) eventLog += "--- Optimalis szotarra jutottunk. ---";
            else eventLog += "Nem tudtunk kilepo valtozot valasztani, tehat a feladat nem korlatos.";
        }

        public static string GetEventLog()
        {
            return eventLog;
        }
    }
}
