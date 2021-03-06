using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Week05
{
    class CentralInterpolation
    {
        public List<double> X { get; private set; } = new List<double>();
        public List<double> Y { get; private set; } = new List<double>();
        public List<double> above { get; protected set; } = new List<double>();
        public List<double> below { get; protected set; } = new List<double>();
        protected double h => Math.Abs(X[0] - X[1]);

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

        public List<double> Add_Above(int idx_left)
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
        public List<double> Add_Below(int idx_right)
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

        public List<double> InterpolateAt(double x)
        {
            var k = Array.IndexOf(X.ToArray(), X.OrderBy(elm => Math.Abs(elm-x)).First());
            return Perform(k);
        }
        public virtual List<double> Perform(int idx) { return new List<double>(); }
        public double[,] Construct_table(double[] vec)
        {
            var n = vec.Count();
            var table = new double[n, n];

            table[0, n-1] = 1;
            for (int i = 1; i < n; i++)
            {
                table[i, n-i-1] = 1;
                for (int j = n-i; j < n; j++)
                {
                    try
                    {
                        table[i, j] = table[i-1, j+1] - vec[i]*table[i-1, j];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        table[i, j] = -vec[i]*table[i-1, j];
                    }
                }
            }

            return table;
        }

        public void P(List<double> pol, double x, int x0_idx)
        {
            double t = (x - X[x0_idx])/h;
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
        static public List<double> matMul(List<double> lst, double[,] arr)
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

        static public void PrintMtx(double[,] a)
        {
            var n = Math.Sqrt(a.Length);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(a[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        static public void PrintVec(double[] a)
        {
            var n = a.Length;
            for (int i = 0; i < n; i++)
            {
                Console.Write(a[i] + " ");
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            string inpPath = @".\input1.txt";

            /* Công thức Gauss*/
            GaussInterpolation user = new GaussInterpolation(inpPath);
            var pol = user.InterpolateAt(2.4);
            pol.ForEach(x => Console.Write(x + " "));
            Console.WriteLine();
            Console.WriteLine("----------------------->\nDecrease");
            user.P(pol, 2.6, 2);

            /* Công thức Stirling */
            // StirlingInterpolation user = new StirlingInterpolation(inpPath);
            // var pol = user.InterpolateAt(2.5);
            // pol.ForEach(x => Console.Write(x + " "));
            // Console.WriteLine();
            // Console.WriteLine("----------------------->\nDecrease");
            // user.P(pol, 2.6, 2);

            /* Công thức Bessel */
            // BesselInterpolation user = new BesselInterpolation(inpPath);
            // var pol = user.InterpolateAt(2.5);
            // pol.ForEach(x => Console.Write(x + " "));
            // Console.WriteLine();
            // Console.WriteLine("------------------------->\nDecrease");
            // user.P(pol, 2.6, 2);
        }
    }
}
