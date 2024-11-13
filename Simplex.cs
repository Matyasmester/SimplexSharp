using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    internal static class Simplex
    {
        // :D
        public static List<BaseVariable> dict = new List<BaseVariable>();

        public static SimplexFunction function = new SimplexFunction();

        private static string eventLog = string.Empty;

        private static bool isNotLimited = false;

        private const int subscriptUnicodeOffset = 0x2080;

        private static void IterateCurrent(PivotRule rule)
        {
            // Choose entering variable from positive function coefficients
            var positiveCoeffs = function.Variables.FindAll(x => x.Coefficient > 0 && !x.Name.Equals("c"));

            NonBaseVariable enteringVariable;

            positiveCoeffs = positiveCoeffs.OrderBy(x => x.Index).ToList();

            if(rule == PivotRule.Classic)
            {
                // Select lowest index of biggest coefficient
                enteringVariable = positiveCoeffs.First(x => x.Coefficient == positiveCoeffs.Max(y => y.Coefficient));
            }
            else
            {
                // Select lowest index 
                enteringVariable = positiveCoeffs.First();
            }

            string enteringName = enteringVariable.Name;

            eventLog += "Valasztott belepo valtozo: " + SubscriptNumbers(enteringName) + "\n";

            // Find negative coeffs in every base variable line, then get the minimum 
            // of ratio (coeff / constant)
            double minimumRatio = double.MaxValue;

            BaseVariable leavingVariable = default;
            double leavingConstant = 0;

            foreach (BaseVariable baseVar in dict)
            {
                double constantValue = baseVar.Constant;

                foreach(NonBaseVariable nonBaseVar in baseVar.Variables)
                {
                    if(!nonBaseVar.Name.Equals(enteringVariable.Name)) continue;

                    double currentValue = nonBaseVar.Coefficient;

                    if(currentValue < 0)
                    {
                        double ratio = constantValue / (-1*currentValue);

                        if(ratio < minimumRatio)
                        {
                            minimumRatio = ratio;
                            leavingVariable = baseVar;
                            leavingConstant = constantValue;
                        }
                    }
                }
            }

            if(leavingVariable == default)
            {
                isNotLimited = true;

                return;
            }

            string leavingName = leavingVariable.Name;

            // We have a degenerate base, we make note of this
            if (leavingConstant == 0) eventLog += "[!] Degeneralt szotarunk van. [!]\n";

            eventLog += "Legkisebb talalt hanyados: " + Math.Round(minimumRatio, 3) + "\n";
            eventLog += "Valasztott kilepo valtozo: " + SubscriptNumbers(leavingName) + "\n\n";

            // Now we express the entering from the leaving variable's row
            int leavingVariableIndex = dict.IndexOf(leavingVariable);

            double enteringValue = leavingVariable.Variables.First(x => x.Name.Equals(enteringName)).Coefficient;

            leavingVariable.Variables.RemoveAll(x => x.Name.Equals(enteringName));

            BaseVariable pivotVar = new BaseVariable(enteringName, enteringVariable.Index);
            pivotVar.Constant = -1 * leavingVariable.Constant / enteringValue;

            foreach(NonBaseVariable var in leavingVariable.Variables)
            {
                pivotVar.AddVariable(var);
            }

            pivotVar.AddVariable(new NonBaseVariable(leavingName, -1, leavingVariable.Index));

            for(int i = 0; i < pivotVar.Variables.Count; i++)
            {
                var current = pivotVar.Variables[i];

                pivotVar.Variables[i] = new NonBaseVariable(current.Name, -1 * current.Coefficient / enteringValue, current.Index);
            }

            dict[leavingVariableIndex] = pivotVar;

            // Lastly, substitute this variable in every row
            for (int i = 0; i < dict.Count; i++)
            {
                string varName = dict[i].Name;

                if(varName.Equals(enteringName)) continue;

                var row = dict[i].Variables;

                BaseVariable newRow = new BaseVariable(varName, dict[i].Index);

                NonBaseVariable currentEntering = FindByName(row, enteringName, enteringVariable.Index);

                double coefficient = currentEntering.Coefficient;

                newRow.Constant = dict[i].Constant + (coefficient * pivotVar.Constant);

                for(int k = 0; k < pivotVar.Variables.Count; k++)
                {
                    NonBaseVariable pivot = pivotVar.Variables[k];

                    string pivotName = pivot.Name;

                    if (pivotName.Equals(enteringName)) continue;

                    NonBaseVariable current = FindByName(row, pivotName, pivot.Index);

                    double newValue = current.Coefficient + (pivot.Coefficient * coefficient);

                    NonBaseVariable substituted = new NonBaseVariable(pivotName, newValue, pivot.Index);

                    newRow.AddVariable(substituted);
                }

                dict[i] = newRow;
            }

            NonBaseVariable funcEntering = FindByName(function.Variables, enteringName, enteringVariable.Index);

            double funcCoefficient = funcEntering.Coefficient;

            SimplexFunction newFunc = new SimplexFunction();

            newFunc.Constant = function.Constant + (pivotVar.Constant * funcCoefficient);

            // In the function as well
            for (int i = 0; i < pivotVar.Variables.Count; i++)
            {
                NonBaseVariable pivot = pivotVar.Variables[i];

                string pivotName = pivot.Name;

                if (pivot.Name.Equals(enteringVariable.Name)) continue;

                NonBaseVariable current = FindByName(function.Variables, pivotName, pivot.Index);

                double newValue = current.Coefficient + (pivot.Coefficient * funcCoefficient);

                NonBaseVariable substituted = new NonBaseVariable(pivotName, newValue, pivot.Index);

                newFunc.AddVariable(substituted);
            }

            function = newFunc;

            dict.OrderBy(x => x.Index);
            function.Variables.OrderBy(x => x.Index);
        }

        private static NonBaseVariable FindByName(List<NonBaseVariable> list, string name, int index)
        {
            return list.Any(x => x.Name.Equals(name)) ? list.First(x => x.Name.Equals(name)) : new NonBaseVariable(name, 0, index);
        }

        private static bool IsCurrentOptimal()
        {
            // Check if the current dictionary is optimal
            // Meaning the function has a non-negative constant value,
            // and the variables all have negative coefficients

            if (function.Constant < 0) return false;

            for(int i = 0; i < function.Variables.Count; i++)
            {
                double coefficient = function.Variables[i].Coefficient;

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
                retval += dict[i].ToString();

                retval += "\n";
            }

            retval += "----------------------------------\n";

            retval += function.ToString() + "\n";

            return retval;
        }

        public static void Optimize(PivotRule rule)
        {
            eventLog = string.Empty;
            int nIterations = 0;

            // Iterate until optimal
            while(!IsCurrentOptimal() && !isNotLimited)
            {
                nIterations++;

                eventLog += "\n--- " + nIterations + ". iteracio kezdete. ---\n\n";

                IterateCurrent(rule);

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

        public static void Reset()
        {
            dict.Clear();
            function = new SimplexFunction();
            isNotLimited = false;
        }

        public static bool SaveLog(string path)
        {
            try
            {
                File.WriteAllText(path, eventLog);
                return true;
            }
            catch (Exception) { return false; }
        }

        public static string SubscriptNumbers(string s)
        {
            string retval = "";

            foreach(char c in s)
            {
                if(c < '0' || c > '9') retval += c;
                else retval += (char)(c - '0' + subscriptUnicodeOffset);
            }

            return retval;
        }
    }
}
