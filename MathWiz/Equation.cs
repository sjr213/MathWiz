using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathWiz
{
    // represents an equation in the Math Wiz
    class Equation
    {
        string left;
        public string Left
        {
            get { return left; }
            set { left = value; }
        }

        string right;
        public string Right
        {
            get { return right; }
            set { right = value; }
        }

        string result;
        public string Result
        {
            get { return result; }
            set { result = value; }
        }

        string sign;
        public string Sign
        {
            get { return sign; }
            set { sign = value; }
        }

        public Equation()
        {
            Left = "";
            Sign = "";
            Right = "";
            Result = "";
        }

        public Equation(string left, string sign, string right, string result)
        {
            Left = left;
            Sign = sign;
            Right = right;
            Result = result;
        }

        public static bool operator ==(Equation lhs, Equation rhs)
        {
            if (lhs.Left == rhs.Left && lhs.Sign == rhs.Sign && lhs.Right == rhs.Right && lhs.Result == rhs.Result)
                return true;
            else
                return false;
        }

        public static bool operator !=(Equation lhs, Equation rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            Equation that = (Equation)obj;
            return this == that;
        }

        // Bogus
        public override int GetHashCode()
        {
            return Left.Count() + Sign.Count() + Right.Count() + Result.Count();
        }


    }
}
