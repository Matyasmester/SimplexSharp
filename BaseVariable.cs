using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    internal class BaseVariable
    {
        private string name;

        private double constant;

        private List<NonBaseVariable> variables;
        public string Name
        {
            get => name;
            private set => name = value;
        }

        public double Constant
        {
            get => constant;
            set => constant = value;
        }

        public List<NonBaseVariable> Variables 
        { 
            get => variables;
            private set => variables = value;
        }


        public BaseVariable(string name)
        {
            this.name = name;
            this.variables = new List<NonBaseVariable>();
        }

        public void AddVariable(NonBaseVariable variable)
        {
            variables.Add(variable);
        }

        public override string ToString()
        {
            string retval = name + " = " + Math.Round(constant, 2).ToString();

            foreach (NonBaseVariable variable in variables)
            {
                retval += " + " + variable.ToString();
            }

            return retval;
        }
    }
}
