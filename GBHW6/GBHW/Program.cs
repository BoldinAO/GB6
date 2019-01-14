//Болдин
/*
2. Модифицировать программу нахождения минимума функции так, чтобы можно было передавать функцию в виде делегата.
а) Сделайте меню с различными функциями и предоставьте пользователю выбор, для какой функции и на каком отрезке находить минимум.
б) Используйте массив (или список) делегатов, в котором хранятся различные функции.
в) *Переделайте функцию Load, чтобы она возвращала массив считанных значений. Пусть она
возвращает минимум через параметр.
*/

using System;
using System.IO;

namespace DoubleBinary
{
    class Program
    {
        readonly public static string path = "data.bin";

        static bool retMin;

        public delegate double FU(double x);

        public static double F(double x)
        {
            return x * x - 50 * x + 10;
        }

        public static double Z(double x)
        {
            return x * x - 50 * x + 20;
        }

        static void SaveFunc(string fileName, double a, double b, double h, FU fU)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            double x = a;
            while (x <= b)
            {
                bw.Write(fU(x));
                x += h;
            }
            bw.Close();
            fs.Close();
            Load(path, retMin);
        }

        static void Load(string fileName, bool returnMin)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader bw = new BinaryReader(fs);
            double[] min = new double[1];
            min[0] = double.MaxValue;
            double[] arrVal = new double[fs.Length-1];
            double d;
            for (int i = 0; i < fs.Length / sizeof(double); i++)
            {
                // Считываем значение и переходим к следующему
                d = bw.ReadDouble();
                arrVal[i] = d;
                if (d < min[0]) min[0] = d;
            }
            bw.Close();
            fs.Close();
            if(returnMin)
                foreach(double s in min)
                    Console.WriteLine(s);
            else
                foreach (double s in arrVal)
                    Console.WriteLine(s);
        }

        public static void Menu()
        {
            
            FU[] fun = new FU[2];
            fun[0] = new FU(F);
            fun[1] = new FU(Z);
            int[] cut = new int[2];
            double step;
            int chouseFunc;

            do
            {
                Console.WriteLine("Меню\nВыберите функцию {0}, {1}", "0", "1");
                int.TryParse(Console.ReadLine(), out chouseFunc);
                Console.WriteLine("Введите отрезок для нахождения минимума:");
                int.TryParse(Console.ReadLine(), out cut[0]);
                int.TryParse(Console.ReadLine(), out cut[1]);
                double.TryParse(Console.ReadLine(), out step);
            } while (cut[0] >= cut[1] || step <= 0 || (chouseFunc < 0 && chouseFunc > 1));

            Console.WriteLine("Вернуть минимум? (+ / -)");
            retMin = Console.ReadLine() == "+" ? true : false;

            SaveFunc(path, cut[0], cut[1], step, fun[chouseFunc]);
        }

        static void Main(string[] args)
        {
            Menu();
            Console.ReadKey();
        }
    }
}