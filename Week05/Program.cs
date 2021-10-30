using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Week05
{
    class CentralInterpolation
    {
        private List<double> X;
        private List<double> Y;

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

        public void Display()
        {
            this.X.ForEach(x => Console.Write(x + " "));
            Console.Write("\n");
            this.Y.ForEach(y => Console.Write(y + " "));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = "input.txt";
            CentralInterpolation p = new CentralInterpolation(path);
            p.Display();
        }
    }
}
