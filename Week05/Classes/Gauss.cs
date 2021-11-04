using System;
using System.Linq;
using System.Collections.Generic;

namespace Week05
{
    class GaussInterpolation : CentralInterpolation
    {
        public List<double> Gauss { get; private set; } = new List<double>();

        public GaussInterpolation(string filePath) : base(filePath) {}

        public override List<double> Perform(int idx)
        {
            List<double> res = new List<double>();
            int[,] t_table;
            int k = 0;

            /* Ban đầu */
            above.Add(Y[idx]);
            below.Add(Y[idx]);
            Gauss.Add(Y[idx]);

            /* Thêm lần lượt các mốc nội suy PT */
            for (k = 1; k <= Math.Min(idx, X.Count-idx-1); ++k)
            {
                below = Add_Below(idx+k);
                above = Add_Above(idx-k);
                Gauss.AddRange(below.TakeLast(2));
            }

            for (int i = 0; i < Gauss.Count; ++i)
            {
                Gauss[i] = Gauss[i]/Enumerable.Range(1, i).Aggregate(1, (p, item) => p*item);
            }

            /* Xây dựng ma trận hệ số t */
            t_table = Construct_table(2*k-1);

            res = Program.matMul(Gauss, t_table);

            return res;
        }
        public override int[,] Construct_table(int n)
        {
            int[,] matrix = new int[n, n];
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