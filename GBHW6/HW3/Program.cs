//Болдин
/*
Переделать программу «Пример использования коллекций» для решения следующих задач:
а) Подсчитать количество студентов учащихся на 5 и 6 курсах;
б) подсчитать сколько студентов в возрасте от 18 до 20 лет на каком курсе учатся (частотный массив);
в) отсортировать список по возрасту студента;
г) *отсортировать список по курсу и возрасту студента;
д) разработать единый метод подсчета количества студентов по различным параметрам
выбора с помощью делегата и методов предикатов. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace StudentInfo
{
    struct Student
    {
        public string FirstName { get; }

        public string LastName { get; }

        public string Univercity { get; }

        public string F_and_dep { get; }

        public int Age { get; }

        public int Course { get; }

        public int Group { get; }

        public string City { get; }

        //Создаем конструктор
        public Student(string firstName, string lastName, string univercity, string f_and_dep, int age, int course, int group, string city)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Univercity = univercity;
            this.F_and_dep = f_and_dep;
            this.Age = age;
            this.Course = course;
            this.Group = group;
            this.City = city;
        }
    }

    class Program
    {
        static void ReadInfo(string filePath, ref List<string> info)
        {
            StreamReader sr = new StreamReader(filePath);
            try
            {
                while (!sr.EndOfStream)
                {
                    info.Add(sr.ReadLine());
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
            catch (ArgumentException exc)
            {
                Console.WriteLine(exc.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }


        static void WriteToFile(string path, ref List<Student> students)
        {
            StreamWriter writer = new StreamWriter(path);
            for (int i = 0; i < students.Count; i++)
            {
                writer.WriteLine(students[i].FirstName + ";" +
                    students[i].LastName + ";" +
                    students[i].Univercity + ";" +
                    students[i].F_and_dep + ";" +
                    students[i].Age + ";" +
                    students[i].Course + ";" +
                    students[i].Group + ";" +
                    students[i].City + ";");
            }
            writer.Close();
        }

        delegate bool Predicate(Student student);

        static bool CountStudentsOnCourse1(Student student)
        {
            if (student.Course == 5 || student.Course == 6) return true; return false;
        }

        static bool CountStudentsOnAllCourses(Student students)
        {
            if (students.Age >= 18 && students.Age <= 20) return true; return false;
        }


        static int Comparer(Student o1, Student o2)
        {
            if (o1.Course > o2.Course) return 1;
            if (o1.Course < o2.Course) return -1;
            if (o1.Age > o2.Age) return 1;
            if (o1.Age < o2.Age) return -1;
            return 0;
        }

        

        static int CountStudentBy(List<Student> students, Predicate predicate)
        {
            int count = 0;
            foreach(Student student in students)
            {
                count = predicate(student) ? ++count : count;
            }
            return count;
        }

        static void Main(string[] args)
        {
            Regex regexPath = new Regex(@"[A-Z]{1}[:]{1}[\\]{1}([A-Za-z0-9_ ]*[\\]*)*[^/?""<>|:*]");
            string filePath;
            do
            {
                Console.WriteLine("Введите путь, куда сохранить файл: ");
                filePath = Console.ReadLine();
            } while (!regexPath.IsMatch(filePath));

            string s;
            do
            {
                Console.WriteLine("Введите необходимое количество студентов: ");
                s = Console.ReadLine();
            } while (!int.TryParse(s, out int result));
            int N = int.Parse(s);

            //считали данные для студентов
            List<string> female_names = new List<string>();
            ReadInfo("female_names.txt", ref female_names);
            List<string> male_names = new List<string>();
            ReadInfo("male_names.txt", ref male_names);
            List<string> surnames = new List<string>();
            ReadInfo("surnames.txt", ref surnames);
            List<string> universities = new List<string>();
            ReadInfo("universities.txt", ref universities);
            List<string> f_and_dep = new List<string>();
            ReadInfo("faculties_and_departments.csv", ref f_and_dep);
            List<string> cities = new List<string>();
            ReadInfo("cities.txt", ref cities);

            //заполнили коллекцию объектами студентов
            List<Student> students = new List<Student>();
            Random rand = new Random();
            int num0, num1, num2, num3, num4, num5, num6, num7;
            for (int i = 0; i < N; i++)
            {
                num1 = rand.Next(0, surnames.Count);
                num2 = rand.Next(0, universities.Count);
                num3 = rand.Next(0, f_and_dep.Count);
                num4 = rand.Next(18, 30);
                num5 = rand.Next(1, 7);
                num6 = rand.Next(1, 5);
                num7 = rand.Next(0, cities.Count);
                int gender = rand.Next(0, 2);
                if (gender == 0)
                {
                    num0 = rand.Next(0, female_names.Count);
                    Student t = new Student(female_names[num0], surnames[num1], universities[num2], f_and_dep[num3], num4, num5, num6, cities[num7]);
                    students.Add(t);
                }
                else
                {
                    num0 = rand.Next(0, male_names.Count);
                    Student t = new Student(male_names[num0], surnames[num1], universities[num2], f_and_dep[num3], num4, num5, num6, cities[num7]);
                    students.Add(t);
                }
            }

            //создали файл
            string fileName = "StudentsInfo.csv";
            string pathString = Path.Combine(filePath, fileName);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (!File.Exists(pathString))
            {
                using (FileStream fs = File.Create(pathString)) { };
            }

            //записали в файл студентов
            WriteToFile(pathString, ref students);
            Console.WriteLine("Файл создан.");

            FileInfo file = new FileInfo(pathString);
            double fileSize = (double)file.Length / 1024;
            Console.WriteLine($"Размер файла:\n{file.Length} B\n{fileSize:f} KB");
            
            Console.WriteLine(CountStudentBy(students, new Predicate(CountStudentsOnAllCourses)));
            
            students.Sort(Comparer);
            Console.ReadKey();
        }
    }
}