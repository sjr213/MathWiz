using System;

namespace MathWiz
{
    public struct  ProblemArguments
    {
        public int left;
        public int right;
    }

    public enum enumSign { addition, subtraction, multiplication, division };

    public enum clownType { none, happy, sad };

    public class MathWizModel
    {
        Random _random = new Random();

        public ProblemArguments GetProblemArguments(int minValue, int maxValue, enumSign sign)
        {
            if (minValue > maxValue)
            {
                int temp = minValue;
                minValue = maxValue;
                maxValue = temp;
            }

            if (maxValue <= 2 * minValue)
                maxValue = 2 * minValue;

            ProblemArguments args = new ProblemArguments();

            // The arguments are limited by the min/max
            if (sign == enumSign.subtraction)
            {
                args.left = _random.Next(maxValue - minValue + 1) + minValue;

                args.right = _random.Next(args.left - minValue + 1) + minValue;
            }
            else if(sign == enumSign.addition)
            {
                args.left = _random.Next(maxValue - minValue + 1) + minValue;

                args.right = _random.Next(maxValue - minValue + 1) + minValue;
            }
            else if(sign == enumSign.multiplication)
            {
                args.left = _random.Next(maxValue - minValue + 1) + minValue;

                args.right = _random.Next(args.left - minValue + 1) + minValue;
            }
            else if(sign == enumSign.division)
            {
                args.right = _random.Next(maxValue - minValue + 1) + minValue;
                if (args.right < 1)
                    args.right = 1;

                var mult = _random.Next(maxValue - minValue + 1) + minValue;
                args.left = args.right * mult;
            }

            return args;
        }

        public int GetProblemResult(int leftValue, int rightValue, enumSign sign)
        {
            if (sign == enumSign.subtraction)
                return leftValue - rightValue;
            else if (sign == enumSign.addition)
                return leftValue + rightValue;
            else if (sign == enumSign.multiplication)
                return leftValue * rightValue;
            else if (sign == enumSign.division)
                return leftValue / rightValue;

                return 0;
        }
    }
}
