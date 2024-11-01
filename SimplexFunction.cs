using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    internal class SimplexFunction
    {
        private List<NonBaseVariable> variables;

        private double constant;

        public List<NonBaseVariable> Variables
        {
            get => variables;
            private set => variables = value;
        }

        public double Constant
        {
            get => constant;
            set => constant = value;
        }

        public SimplexFunction()
        {
            variables = new List<NonBaseVariable>();
        }

        public void AddVariable(NonBaseVariable variable)
        {
            variables.Add(variable);
        }

        public override string ToString()
        {
            string retval = "z = " + Math.Round(constant, 2).ToString();

            foreach(NonBaseVariable variable in variables)
            {
                retval += " + " + variable.ToString();
            }

            return retval;
        }
    }
}
