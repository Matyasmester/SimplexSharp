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

        private int index;

        private List<NonBaseVariable> variables;
        public string Name
        {
            get => name;
            private set => name = value;
        }

        public int Index
        {
            get => index;
            private set => index = value;
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


        public BaseVariable(string name, int index)
        {
            this.name = name;
            this.index = index;
            this.variables = new List<NonBaseVariable>();
        }

        public void AddVariable(NonBaseVariable variable)
        {
            variables.Add(variable);
        }

        public override string ToString()
        {
            string retval = Simplex.SubscriptNumbers(name) + " = " + Math.Round(constant, 2).ToString();

            foreach (NonBaseVariable variable in variables)
            { 
                retval += variable.ToString();
            }

            return retval;
        }
    }
}
