using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Week05
{
    class CentralInterpolation
    {
        private List<double> X;     // mốc nội suy
        private List<double> Y;     // giá trị nội suy
        private List<double> above { get; }
        private List<double> below { get; }
        private List<double> Gauss { get; }
        private double h => Math.Abs(X[0] - X[1]);

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

        public List<double> Add_Above()
        {
            return new List<double> {0};
        }
        public List<double> Add_Below()
        {
            return new List<double> {0};
        }

        public void Construct_devided_diff(int idx)     // Chỉ số của x0 (mốc nội suy trung tâm)
        {

        }
        public int[,] Construct_coeff_matrix(int numberOfX)
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
                for (int j = n-i; i < n-1; j++)
                {
                    matrix[i, j] = matrix[i-1, j+1] - x[i-1]*matrix[i-1, j];
                }
            }

            return matrix;
        }

        public void Perform_Sterling()
        {

        }
        public void Perform_Bessel()
        {

        }

        public void Display()
        {
            this.X.ForEach(x => Console.Write(x + " "));
            Console.Write("\n");
            this.Y.ForEach(y => Console.Write(y + " "));
            Console.Write("\n" + h);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = @".\input.txt";
            CentralInterpolation p = new CentralInterpolation(path);
            p.Display();
        }
    }
}
