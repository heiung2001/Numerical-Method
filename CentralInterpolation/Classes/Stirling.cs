using System;
using System.Linq;
using System.Collections.Generic;

namespace Week05
{
    class StirlingInterpolation : CentralInterpolation
    {
        public List<double> Stirling { get; private set; } = new List<double>();
        
        public StirlingInterpolation(string filePath) : base(filePath) {}
        
        public List<double> Find_Even(double[,] table)
        {
            var temp = new List<double>();

            for (int i = 2; i < Stirling.Count; i += 2)
            {
                temp.Add(Stirling[i]);
            }
            temp = Program.matMul(temp, table);
            temp.Add(Stirling[0]);

            return temp;
        }
        public List<double> Find_Odd(double[,] table)
        {
            var temp = new List<double>();

            for (int i = 1; i < Stirling.Count; i += 2)
            {
                temp.Add(Stirling[i]);
            }

            return Program.matMul(temp, table);
        }
        public List<double> Merge(double[,] table)
        {
            var even = new Queue<double>(Find_Even(table));
            var odd = new Queue<double>(Find_Odd(table));
            var res = new List<double>();
            var n = Math.Min(even.Count, odd.Count);
            
            for (int i = 0; i < n; i++)
            {
                res.Add(even.Dequeue());
                res.Add(odd.Dequeue());
            }
            res.Add(even.Dequeue());

            return res;
        }

        public override List<double> Perform(int pivot)
        {
            var result = new List<double>();
            var t_table = new double[0, 0];
            double[] t;
            int k = 0;

            above.Add(Y[pivot]);
            below.Add(Y[pivot]);
            Stirling.Add(Y[pivot]);

            for (k = 1; k <= Math.Min(pivot, X.Count-pivot-1); ++k)
            {
                above = Add_Above(pivot-k);
                below = Add_Below(pivot+k);
                Stirling.AddRange(new double[] { (above[^2]+below[^2])/2, below[^1] });   
            }

            for (int i = 0; i < Stirling.Count; i++)
            {
                Stirling[i] = Stirling[i]/Enumerable.Range(1, i).Aggregate(1, (p, item) => p*item);
            }

            t = new double[k-1];
            for (int i = 0; i < k-1; i++)
            {
                t[i] = i*i;
            }
            t_table = base.Construct_table(t);
            result = Merge(t_table);

            return result;
        }
    }
}