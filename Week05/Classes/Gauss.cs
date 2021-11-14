using System;
using System.Linq;
using System.Collections.Generic;

namespace Week05
{
    class GaussInterpolation : CentralInterpolation
    {
        public List<double> Gauss { get; private set; } = new List<double>();

        public GaussInterpolation(string filePath) : base(filePath) {}

        public List<double> Perform_I(int idx)
        {
            List<double> res = new List<double>();
            double[,] t_table;
            int k = 0;

            above.Add(Y[idx]);
            below.Add(Y[idx]);
            Gauss.Add(Y[idx]);

            for (k = 1; k <= Math.Min(idx, X.Count-idx-1); ++k)
            {
                below = Add_Below(idx+k);
                above = Add_Above(idx-k);
                Gauss.AddRange(below.TakeLast(2));
            }

            for (int i = 0; i < Gauss.Count; ++i)
            {
                res.Add(Gauss[i]/Enumerable.Range(1, i).Aggregate(1, (p, item) => p*item));
            }

            t_table = Construct_table(2*k-1);
            res = Program.matMul(res, t_table);

            return res;
        }
        public List<double> Perform_II(int idx)
        {
            List<double> res = new List<double>();
            double[,] t_table;
            int k = 0;

            above.Add(Y[idx]);
            below.Add(Y[idx]);
            Gauss.Add(Y[idx]);

            for (k = 1; k <= Math.Min(idx, X.Count-idx-1); ++k)
            {
                above = Add_Above(idx-k);
                below = Add_Below(idx+k);
                Gauss.AddRange(above.TakeLast(2));
            }

            for (int i = 0; i < Gauss.Count; ++i)
            {
                res.Add(Gauss[i]/Enumerable.Range(1, i).Aggregate(1, (p, item) => p*item));
            }

            t_table = Construct_table(2*k-1);
            res = Program.matMul(res, t_table);

            return res;
        }

        public override List<double> Perform(int idx)
        {
            List<double> res = new List<double>();

            Console.WriteLine("Gauss I/ Gauss II ? (1 : 2). Your choice: ");
            int choose = int.Parse(Console.ReadLine());

            if (choose == 1) { res = Perform_I(idx);  }
            if (choose == 2) { res = Perform_II(idx); }

            return res;
        }
        public override double[,] Construct_table(int n)
        {
            double[,] matrix = new double[n, n];
            int[] x = new int[n];

            matrix[0, n-1] = 1;
            for (int i = 0; i < n; i++)
            {
                if ((i+1) % 2 == 0)    { x[i] = (i+1) / 2;  }
                if ((i+1) % 2 != 0)    { x[i] = -(i+1) / 2; }
            }
            
            for (int i = 1; i < n; i++)
            {
                matrix[i, n-i-1] = 1;
                for (int j = n-i; j < n-1; j++)
                {
                    matrix[i, j] = matrix[i-1, j+1] - x[i-1]*matrix[i-1, j];
                }
            }

            return matrix;
        }
    }
}