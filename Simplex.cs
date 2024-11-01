﻿using System;
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

        private static void IterateCurrent(PivotRule rule)
        {
            // Choose entering variable from positive function coefficients
            var positiveCoeffs = function.Variables.FindAll(x => x.Coefficient > 0 && !x.Name.Equals("c"));

            NonBaseVariable enteringVariable;

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

            eventLog += "Valasztott belepo valtozo: " + enteringVariable.Name + "\n";

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

            if(leavingVariable.Variables == default)
            {
                isNotLimited = true;

                return;
            }

            // We have a degenerate base, we make note of this
            if (leavingConstant == 0) eventLog += "[!] Degeneralt szotarunk van. [!]\n";

            eventLog += "Legkisebb talalt hanyados: " + Math.Round(minimumRatio, 3) + "\n";
            eventLog += "Valasztott kilepo valtozo: " + leavingVariable.Name + "\n\n";

            // Now we express the entering from the leaving variable's row
            int leavingVariableIndex = dict.IndexOf(leavingVariable);

            double enteringValue = leavingVariable.Variables.First(x => x.Name.Equals(enteringVariable.Name)).Coefficient;

            leavingVariable.Variables.RemoveAll(x => x.Name.Equals(enteringVariable.Name));

            BaseVariable pivotVar = new BaseVariable(enteringVariable.Name);
            pivotVar.Constant = -1 * leavingVariable.Constant / enteringValue;

            foreach(NonBaseVariable var in leavingVariable.Variables)
            {
                pivotVar.AddVariable(var);
            }

            pivotVar.AddVariable(new NonBaseVariable(leavingVariable.Name, -1));

            for(int i = 0; i < pivotVar.Variables.Count; i++)
            {
                var current = pivotVar.Variables[i];

                pivotVar.Variables[i] = new NonBaseVariable(current.Name, -1 * current.Coefficient / enteringValue);
            }

            dict[leavingVariableIndex] = pivotVar;

            // Lastly, substitute this variable in every row
            for (int i = 0; i < dict.Count; i++)
            {
                string varName = dict[i].Name;

                if(varName.Equals(enteringVariable.Name)) continue;

                var row = dict[i].Variables;

                BaseVariable newRow = new BaseVariable(varName);

                NonBaseVariable currentEntering = row.Any(x => x.Name.Equals(enteringVariable.Name)) ?

                                    row.First(x => x.Name.Equals(enteringVariable.Name)) :

                                    new NonBaseVariable(enteringVariable.Name, 0);

                double coefficient = currentEntering.Coefficient;

                newRow.Constant = dict[i].Constant + (coefficient * pivotVar.Constant);

                for(int k = 0; k < pivotVar.Variables.Count; k++)
                {
                    NonBaseVariable pivot = pivotVar.Variables[k];

                    if (pivot.Name.Equals(enteringVariable.Name)) continue;

                    NonBaseVariable current = row.Any(x => x.Name.Equals(pivot.Name)) ? 

                                row.First(x => x.Name.Equals(pivot.Name)) : 

                                new NonBaseVariable(pivot.Name, 0);

                    double newValue = current.Coefficient + (pivot.Coefficient * coefficient);

                    NonBaseVariable substituted = new NonBaseVariable(current.Name, newValue);

                    newRow.AddVariable(substituted);
                }

                dict[i] = newRow;
            }

            NonBaseVariable funcEntering = function.Variables.Any(x => x.Name.Equals(enteringVariable.Name)) ?

                             function.Variables.First(x => x.Name.Equals(enteringVariable.Name)) : 

                             new NonBaseVariable(enteringVariable.Name, 0);

            double funcCoefficient = funcEntering.Coefficient;

            SimplexFunction newFunc = new SimplexFunction();

            newFunc.Constant = function.Constant + (pivotVar.Constant * funcCoefficient);

            // In the function as well
            for (int i = 0; i < pivotVar.Variables.Count; i++)
            {
                var pivot = pivotVar.Variables[i];

                if (pivot.Name.Equals(enteringVariable.Name)) continue;

                NonBaseVariable current = function.Variables.Any(x => x.Name.Equals(pivot.Name)) ?

                                function.Variables.First(x => x.Name.Equals(pivot.Name)) :

                                new NonBaseVariable(pivot.Name, 0);

                double newValue = current.Coefficient + (pivot.Coefficient * funcCoefficient);

                NonBaseVariable substituted = new NonBaseVariable(current.Name, newValue);

                newFunc.AddVariable(substituted);
            }

            function = newFunc;
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

            retval += "-----------------------------\n";

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

        public static bool SaveLog(string path)
        {
            try
            {
                File.WriteAllText(path, eventLog);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
