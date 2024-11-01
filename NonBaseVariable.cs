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

        public string Name
        { 
            get => name; 
            private set => name = value;
        }

        public double Coefficient 
        {
            get => coefficient;
            private set => this.coefficient = value;
        }

        public NonBaseVariable(string name, double coefficient)
        {
            this.name = name;
            this.coefficient = coefficient;
        }

        public override string ToString()
        {
            return name + " * " + Math.Round(coefficient, 2);
        }
    }
}
