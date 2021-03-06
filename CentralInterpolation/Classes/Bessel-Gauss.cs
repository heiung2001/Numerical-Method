using System;
using System.Linq;
using System.Collections.Generic;

namespace Week05
{
    class BesselGaussInterpolation : CentralInterpolation
    {
        public List<double> Bessel { get; private set; } = new List<double>();
        public GaussInterpolation Gauss_I;
        public GaussInterpolation Gauss_II;

        public BesselGaussInterpolation(string filePath) : base(filePath)
        {
            Gauss_I = new GaussInterpolation(filePath);
            Gauss_II = new GaussInterpolation(filePath);
        }

        public List<double> Find_Even(double[,] table)
        {
            List<double> temp = new List<double>();

            for (int i = 0; i < Bessel.Count; i += 2)
            {
                temp.Add(Bessel[i]);
            }
            temp = Program.matMul(temp, table);

            return temp;
        }
        public List<double> Find_Odd(double[,] table)
        {
            List<double> temp = new List<double>();

            for (int i = 1; i < Bessel.Count; i += 2)
            {
                temp.Add(Bessel[i]);
            }

            return Program.matMul(temp, table);
        }
        public List<double> Merge(double[,] table)
        {
            var res = new List<double>();
            var even = new Queue<double>(Find_Even(table));
            var odd = new Queue<double>(Find_Odd(table));
            var n = Math.Min(even.Count, odd.Count);

            for (int i = 0; i < n; i++)
            {
                res.Add(even.Dequeue());
                res.Add(odd.Dequeue());
            }

            return res;
        }

        public override List<double> Perform(int pivot)
        {
            int l = pivot;
            int r = pivot+1;
            var res = new List<double>();

            Gauss_I.Perform_I(l);
            Gauss_II.Perform_II(r);
            var pol1 = Gauss_I.Gauss;
            var pol2 = Gauss_II.Gauss;

            try
            {
                pol1.AddRange(Gauss_I.Add_Below(pol1.Count).TakeLast(1));
            }
            catch (IndexOutOfRangeException)
            {
                pol1.RemoveAt(0);
            }

            try
            {
                pol2.AddRange(Gauss_II.Add_Above(0).TakeLast(1));
            }
            catch (IndexOutOfRangeException)
            {
                pol2.RemoveAt(pol2.Count-1);
            }


            for (int i = 0; i < pol1.Count; i++)
            {
                if (i % 2 == 0) { Bessel.Add((pol1[i]+pol2[i])/2); }
                if (i % 2 != 0) { Bessel.Add(pol1[i]); }
                Bessel[i] = (Bessel[i]/Enumerable.Range(1, i).Aggregate(1, (p, item) => p*item));
            }

            // var table = Construct_table((Bessel.Count)/2);
            // res = Merge(table);

            return res;
        }
    }
}