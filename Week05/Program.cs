using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Week05
{
    class CentralInterpolation
    {
        private List<double> X = new List<double>();     // mốc nội suy
        private List<double> Y = new List<double>();     // giá trị nội suy
        private double h => Math.Abs(X[0] - X[1]);
        public List<double> above { get; private set; } = new List<double>();
        public List<double> below { get; private set; } = new List<double>();
        public List<double> Gauss { get; private set; } = new List<double>();

        public CentralInterpolation(List<double> X, List<double> Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public CentralInterpolation(string filePath)
        {
            List<string> lines = File.ReadAllLines(filePath).ToList();
            this.X = lines[0].Split(" ").Select(x => double.Parse(x)).ToList();
            this.Y = lines[1].Split(" ").Select(y => double.Parse(y)).ToList();
        }

        public List<double> Add_Above(int idx_left)     // Giả sử idx_left nằm trong [0, n-1]
        {
            var n = above.Count + 1;
            var temp = new List<double>(new double[n]);

            temp[0] = Y[idx_left];
            for (int i = 1; i < n; i++)
            {
                temp[i] = above[i-1] - temp[i-1];
            }
            below.Add(temp[n-1]);

            return temp;
        }
        public List<double> Add_Below(int idx_right)    // Giả sử idx_right nằm trong [0, n-1]
        {
            var n = below.Count + 1;
            var temp = new List<double>(new double[n]);
            
            temp[0] = Y[idx_right];
            for (int i = 1; i < n; i++)
            {
                temp[i] = temp[i-1] - below[i-1];
            }
            above.Add(temp[n-1]);

            return temp;
        }

        public List<double> Perform_1st_Gauss(int idx)     // Chỉ số của x0 (mốc nội suy trung tâm)
        {
            List<double> res = new List<double>();
            int[,] t_table;
            int k = 0;

            /* Ban đầu */
            above.Add(Y[idx]);
            below.Add(Y[idx]);
            Gauss.Add(Y[idx]);

            /* Thêm lần lượt các mốc nội suy */
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
            t_table = Program.Construct_coeff_matrix(2*k-1);

            res = Program.matMul(Gauss, t_table);
            // res.ForEach(x => Console.Write(x + " "));
            // Console.WriteLine();

            return res;
        }
        
        public void P(List<double> pol, double x, int idx)
        {
            double t = (x - X[idx])/h;
            double res = 0;

            foreach (var elm in pol)
            {
                res = res*t + elm;
            }
            Console.WriteLine(res);
        }
    }

    class Program
    {
        static public int[,] Construct_coeff_matrix(int numberOfX)
        {
            int n = numberOfX;
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
        static public List<double> matMul(List<double> lst, int[,] arr)
        {
            int n = lst.Count();
            List<double> temp = new List<double>(new double[n]);

            for (int i = 0; i < n; i++)
            {
                for (int k = 0; k < n; k++)
                {
                    temp[i] += lst[k] * arr[k, i];
                }
            }
            return temp;
        }
        
        static void Main(string[] args)
        {
            string inpPath = @".\input.txt";
            CentralInterpolation user = new CentralInterpolation(inpPath);
            var pol = user.Perform_1st_Gauss(2);

            pol.ForEach(x => Console.Write(x + " "));
            Console.WriteLine();
            user.P(pol, 2, 2);
        }
    }
}
