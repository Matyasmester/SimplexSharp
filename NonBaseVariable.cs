using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    internal class NonBaseVariable
    {
        private string name;

        private double coefficient;

        private int index;

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

        public double Coefficient
        {
            get => coefficient;
            private set => this.coefficient = value;
        }

        public NonBaseVariable(string name, double coefficient, int index)
        {
            this.name = name;
            this.coefficient = coefficient;
            this.index = index;
        }

        public override string ToString()
        {
            return  coefficient >= 0 ? " + " + Math.Round(coefficient, 2) + Simplex.SubscriptNumbers(name) :
                    " - " + Math.Round(-1 * coefficient, 2) + Simplex.SubscriptNumbers(name);
        }
    }
}
