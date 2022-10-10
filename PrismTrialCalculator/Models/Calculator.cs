using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismTrialCalculator.Models
{
    public class Calculator
    {
        public int Calculate(EOperator ope, int operand1, int operand2)
        {
            return ope switch
            {
                EOperator.Add => operand1 + operand2,
                EOperator.Subtract => operand1 - operand2,
                EOperator.Multiply => operand1 * operand2,
                EOperator.Divide => operand1 / operand2,
                _ => throw new ArgumentException("対応していない演算子です．", nameof(ope))
            };
        }
    }

    public enum EOperator
    {
        Add,
        Subtract,
        Multiply,
        Divide,
    }
}
