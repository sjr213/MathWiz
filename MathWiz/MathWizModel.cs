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

            // if subtraction the left argument is limited
            // if addition the result is limited by min/max
            if (sign == enumSign.subtraction)
            {
                int newValue = _random.Next(maxValue - 2 * minValue + 1) + 2 * minValue;

                args.left = newValue;

                args.right = _random.Next(args.left - minValue) + minValue;
            }
            else if(sign == enumSign.addition)
            {
                int newValue = _random.Next(maxValue - 2 * minValue + 1) + 2 * minValue;

                args.left = _random.Next(newValue - minValue + 1) + minValue;

                while (newValue - args.left < minValue)
                    --args.left;

                args.right = newValue - args.left;
            }
            else if(sign == enumSign.multiplication)
            {
                args.left = _random.Next(maxValue);
                args.right = _random.Next(maxValue);
            }
            else if(sign == enumSign.division)
            {
                args.right = _random.Next(maxValue);
                args.left = args.right * _random.Next(maxValue);
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
