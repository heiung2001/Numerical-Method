using System;
using System.Linq;
using System.Collections.Generic;

namespace Week05
{
    class BesselInterpolation : CentralInterpolation
    {
        public List<double> Bessel { get; private set; } = new List<double>();

        public BesselInterpolation(string filePath) : base(filePath) {}

        public List<double> Find_Even(double[,] table)
        {
            var temp = new List<double>();

            for (int i = 0; i < Bessel.Count; i += 2)
            {
                temp.Add(Bessel[i]);
            }
            temp = Program.matMul(temp, table);

            return temp;
        }
        public List<double> Find_Odd(double[,] table)
        {
            var temp = new List<double>();

            for (int i = 1; i < Bessel.Count; i += 2)
            {
                temp.Add(Bessel[i]);
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
                res.Add(odd.Dequeue());
                res.Add(even.Dequeue());
            }

            return res;
        }

        public new void P(List<double> pol, double x, int x0_idx)
        {
            double t = (x - X[x0_idx])/base.h - 0.5;
            double res = 0;

            foreach (var elm in pol)
            {
                res = res*t + elm;
            }
            Console.WriteLine(res);
        }

        public override List<double> Perform(int pivot)
        {
            var u_table = new double[0, 0];
            var result = new List<double> ();
            int k = 0;

            Bessel.AddRange(new List<double> { (Y[pivot]+Y[pivot+1])/2, Y[pivot+1]-Y[pivot] });
            above.AddRange(new List<double> { Y[pivot], Y[pivot+1]-Y[pivot] });
            below.AddRange(new List<double> { Y[pivot+1], Y[pivot+1]-Y[pivot] });

            for (k = 1; k <= Math.Min(pivot, X.Count-pivot-1); k++)
            {
                below = Add_Below(pivot+k+1);
                above = Add_Above(pivot-k);
                Bessel.AddRange(new List<double> { (above[^2]+below[^2])/2, above[^1] });
            }

            for (int i = 0; i < Bessel.Count; i++)
            {
                Bessel[i] = Bessel[i]/Enumerable.Range(1, i).Aggregate(1, (p, item) => p*item);
            }

            u_table = Construct_table(k);
            result = Merge(u_table);

            return result;
        }
        public override double[,] Construct_table(int n)
        {
            double[,] table = new double[n, n];
            double[] u = new double[n];

            for (int i = 1; i < n; i++)
            {
                u[i] = Math.Pow(2*i-1, 2) / 4;
            }

            table[0, n-1] = 1;
            for (int i = 1; i < n; i++)
            {
                table[i, n-i-1] = 1;
                for (int j = n-i; j < n; j++)
                {
                    try
                    {
                        table[i, j] = table[i-1, j+1] - u[i]*table[i-1, j];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        table[i, j] = -u[i]*table[i-1, j];
                    }
                }
            }
            return table;
        }
    }
}