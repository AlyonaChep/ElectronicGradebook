/*
Файл: Project_TIK.cs
Теорiя iнформацiї та кодування
Проєкт
Автори: Золотих-Ванiна В.О., Ганзера М.О., Чепелевич А.І
Електронна залiковка
Дата: 27.12.22
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace FinaleProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int choice, semester;
            double[] marks = new double[10];

            Console.WriteLine("\t\tЕлектронна залiкова книжка\n");
            for (; ; )
            {
                do
                {
                    Console.Write("\nОберiть семестр: ");
                    semester = Convert.ToInt32(Console.ReadLine());
                } while (semester < 1 && semester > 8);
                do
                {
                    Console.Write("Яким чином ввести данi? (0 - Самостiйно, 1 - Автоматично, 2 - Iмпортувати з файлу)\nВибiр: ");
                    choice = Convert.ToInt32(Console.ReadLine());
                } while (choice < 0 && choice > 2);

                switch (choice)
                {
                    case 0: Functions.ManualInput(ref marks); break;
                    case 1: Functions.AutoInput(ref semester, ref marks); break;
                    case 2: Functions.ImportInput(ref marks); break;
                    default: Console.WriteLine("Помилка."); break;
                }

                int totalLength = Arr.mainSubjects[semester - 1].Length + Arr.additionalSubjects[semester - 1].Length;

                string[] subjects = new string[totalLength];
                double[] credits = new double[totalLength];

                switch (choice)
                {
                    case 0:
                        {
                            for (int i = 0; i < Arr.manualSubjects.Length; i++)
                                subjects[i] = Arr.manualSubjects[i];
                            for (int i = 0; i < Arr.manualCredits.Length; i++)
                                credits[i] = Arr.manualCredits[i];
                        }
                        break;
                    case 1:
                        {
                            int k = 0;
                            for (int i = 0; i < Arr.mainSubjects[semester - 1].Length; i++)
                                subjects[k++] = Arr.mainSubjects[semester - 1][i];
                            for (int i = 0; i < Arr.additionalSubjects[semester - 1].Length; i++)
                                subjects[k++] = Arr.additionalSubjects[semester - 1][i];

                            int l = 0;
                            for (int i = 0; i < Arr.mainCredits[semester - 1].Length; i++)
                                credits[l++] = Arr.mainCredits[semester - 1][i];
                            for (int i = 0; i < Arr.additionalCredits[semester - 1].Length; i++)
                                credits[l++] = Arr.additionalCredits[semester - 1][i];
                        }
                        break;
                    case 2:
                        {
                            for (int i = 0; i < Arr.manualSubjects.Length; i++)
                                subjects[i] = Arr.manualSubjects[i];
                            for (int i = 0; i < Arr.manualCredits.Length; i++)
                                credits[i] = Arr.manualCredits[i];

                        }
                        break;
                    default: break;
                }

                Console.WriteLine("\n\t\tВаша залiкова книжка:\n");
                Console.WriteLine($" {Arr.years[semester - 1]} КУРС - {semester} СЕМЕСТР");
                Console.WriteLine("+-----------------------------------+---------+---------+");
                Console.WriteLine("|          Назва дисциплiни         | Кредити |  Оцiнка |");
                Console.WriteLine("+-----------------------------------+---------+---------+");

                for (int i = 0; i < subjects.Length; i++)
                {
                    if (subjects[i] == null) break;
                    Console.WriteLine("| {0,-33} |   {1,-5:F} |  {2,-6:F2} |", subjects[i], credits[i], marks[i]);
                    Console.WriteLine("+-----------------------------------+---------+---------+");
                }

                double rate = 0;
                Console.WriteLine("\nРозрахунок Вашого первинного рейтингового балу... ");
                for (int i = 0; i < totalLength; i++)
                    rate += (credits[i] / credits.Sum()) * marks[i];
                    
                if (rate >=90 && rate <= 100) Console.ForegroundColor = ConsoleColor.Green;
                else if (rate >= 75 && rate <= 89) Console.ForegroundColor = ConsoleColor.Yellow;
                else if (rate >= 60 && rate <= 74) Console.ForegroundColor = ConsoleColor.DarkYellow;
                else Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine("\tВаш поточний рейтинговий бал: {0:F2} ", rate);
                Console.ResetColor();

                int changes = 1;
                while (changes == 1)
                {
                    Console.Write("\nОберiть дiю: (0 - Редагувати, 1 - Видалити, 2 - Експортувати данi)\nВибiр: ");
                    int pick = Convert.ToInt32(Console.ReadLine());
                    switch (pick)
                    {
                        case 0: Functions.Edit(ref subjects, ref credits, ref marks); break;
                        case 1: Functions.Delete(ref subjects, ref credits, ref marks); break;
                        case 2: Functions.Export(ref subjects, ref credits, ref marks); break;
                        default: Console.WriteLine("Помилка."); break;
                    }
                    Console.WriteLine("\n\t\tОновлена залiкова книжка:\n");
                    Console.WriteLine($" {Arr.years[semester - 1]} КУРС - {semester} СЕМЕСТР");
                    Console.WriteLine("+-----------------------------------+---------+---------+");
                    Console.WriteLine("|          Назва дисциплiни         | Кредити |  Оцiнка |");
                    Console.WriteLine("+-----------------------------------+---------+---------+");

                    for (int i = 0; i < subjects.Length; i++)
                    {
                        if (subjects[i] == null) break;
                        Console.WriteLine("| {0,-33} |   {1,-5:F} |  {2,-6:F2} |", subjects[i], credits[i], marks[i]);
                        Console.WriteLine("+-----------------------------------+---------+---------+");
                    }
                    Console.WriteLine("\nРозрахунок Вашого оновленного рейтингового балу... ");
                    rate = 0;
                    for (int i = 0; i < totalLength; i++)
                        rate += (credits[i] / credits.Sum()) * marks[i];
                    if (rate >= 90 && rate <= 100) Console.ForegroundColor = ConsoleColor.Green;
                    else if (rate >= 75 && rate <= 89) Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (rate >= 60 && rate <= 74) Console.ForegroundColor = ConsoleColor.DarkYellow;
                    else Console.ForegroundColor = ConsoleColor.DarkRed;

                    Console.WriteLine("\tВаш поточний рейтинговий бал: {0:F2} ", rate);
                    Console.ResetColor();

                    Console.Write("\nЩе щось зробити? (1 - Так, 0 - Нi)\nВибiр: ");
                    changes = Convert.ToInt32(Console.ReadLine());
                } 
            } 
        }
    }
    public class Functions
    {
        public static void ManualInput(ref double[] marks)
        {
            int more = 1;

            while (more != 0)
            {
                int i = 0;
                Console.Write("Введiть назву компоненту: ");
                Arr.manualSubjects[i] = Console.ReadLine();

                do
                {
                    Console.Write("Введiть кiлькiсть кредитiв: ");
                    Arr.manualCredits[i] = Convert.ToDouble(Console.ReadLine());
                } while (Arr.manualCredits[i] <= 0 && Arr.manualCredits[i] > 10);

                do
                {
                    Console.Write("Введiть кiлькiсть балiв: ");
                    marks[i] = Convert.ToDouble(Console.ReadLine());
                } while (marks[i] < 0 && marks[i] > 100);

                do
                {
                    Console.Write("Ввести ще один компонент? (1 - Так, 0 - Нi)\nВибiр: ");
                    more = Convert.ToInt32(Console.ReadLine());
                } while (more != 0 && more != 1);

                if (more == 0) break;
                else continue;
            }
        }
        public static void AutoInput(ref int semester, ref double[] marks)
        {
            for (int i = 0; i < Arr.mainSubjects[semester - 1].Length; i++)
            {
                do
                {
                    Console.Write("Введiть кiлькiсть балiв з обов'язкового компоненту \"{0}\": ", Arr.mainSubjects[semester - 1][i]);
                    marks[i] = Convert.ToDouble(Console.ReadLine());
                } while (marks[i] < 0 && marks[i] > 100);
            }
            for (int i = 0; i < Arr.additionalSubjects[semester - 1].Length; i++)
            {
                do
                {
                    Console.Write("Введiть кiлькiсть балiв з додаткового компоненту \"{0}\": ", Arr.additionalSubjects[semester - 1][i]);
                    marks[Arr.mainSubjects[semester - 1].Length + i] = Convert.ToDouble(Console.ReadLine());
                } while (marks[i] < 0 && marks[i] > 100);
            }
        }
        public static void Edit(ref string[] subjects, ref double[] credits, ref double[] marks)
        {
            Console.Write("\nЯкий предмет треба змiнити?\nВведiть порядковий номер: ");
            int index = Convert.ToInt32(Console.ReadLine()) - 1;

            Console.Write($"\nВи обрали \"{subjects[index]}\". Що саме замiнити? (0 - Назву, 1 - Кредити, 2 - Оцiнку)\nВибiр: ");
            int pick =  Convert.ToInt32(Console.ReadLine());

            switch (pick)
            {
                case 0: 
                    {
                        Console.Write($"\nВведiть нову назву для предмета \"{subjects[index]}\": ");
                        subjects[index] = Convert.ToString(Console.ReadLine());
                    } break;
                case 1: 
                    {
                        Console.Write($"\nВведiть новi кредити для предмета \"{subjects[index]}\": ");
                        credits[index] = Convert.ToDouble(Console.ReadLine());
                    } break;
                case 2:
                    {
                        Console.Write($"\nВведiть нову оцiнку для предмета \"{subjects[index]}\": ");
                        marks[index] = Convert.ToDouble(Console.ReadLine());
                    } break;
                default:
                    break;
            }
            Console.WriteLine("\n\t----------------");
              Console.WriteLine("\tУспiшно змiнено.");
              Console.WriteLine("\t----------------");
        }
        public static void Delete(ref string[] subjects, ref double[] credits, ref double[] marks)
        {
            Console.WriteLine("\n\t--------------------------------");
              Console.WriteLine("\t! ! ! Незабаром реалiзацiя ! ! !");
              Console.WriteLine("\t--------------------------------");
        }
        public static void Export(ref string[] subjects, ref double[] credits, ref double[] marks)
        {
            TextWriter tw = new StreamWriter("save.txt");

            // write lines of text to the file
            for (int i = 0; i < subjects.Length; i++)
                tw.WriteLine(subjects[i] + "|" + credits[i] + "|" + marks[i] + "|");
            // close the stream     
            tw.Close();
            Console.WriteLine("\n\t----------------------");
              Console.WriteLine("\tФайл успiшно створено.");
              Console.WriteLine("\t----------------------");
        }
        
       
        public static void ImportInput(ref double[] marks)
        {
            string[] readText = File.ReadAllText("save.txt").Split("|");
            int i = 0;
            for (int j = 0; j < 7; j++)
            {
                if (readText[j] == "\r\n") break;
                Arr.manualSubjects[j] = Convert.ToString(readText[i].Replace("\n", "").Replace("\r", "")); i++;
                Arr.manualCredits[j] = Convert.ToDouble(readText[i]); i++;
                marks[j] = Convert.ToDouble(readText[i]); i++;

            }
            

            // string[] temp = new string[] { };
            // 
            // string[] lines = File.ReadAllLines("C:\\Users\\Alona\\Desktop\\save.txt");
            // //foreach (var line in lines)
            // //{
            //     temp = lines;
            // //}
            // string[] readText = File.ReadAllText("input.txt").Split(",");
            // 
            // for (int i = 0; i < readText.Length; i++)
            // {
            //     subject[i] = readText[0];
            // }
            // 
            // credits[] = Convert.ToDouble(readText[1]);
            // int c = Convert.ToDouble(readText[2]);
            // 
            // string[] readText = Split(temp, ";");

        }
        /*public static void SaveFile()
        {
            try
            {
                //Open the File
                StreamWriter sw = new StreamWriter("C:\\Test1.txt", true, Encoding.ASCII);
                //Writeout the numbers 1 to 10 on the same line.
                for (int x = 0; x < 10; x++)
                {
                    sw.Write(x);
                }
                //close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
            // Create sample file; replace if exists.
            string str = "Some text";
            File.WriteAllText("test.txt", str);

            string txt = File.ReadAllText("test.txt");
            Console.WriteLine(txt);
        }
        */
    }


    class Arr
    {
        public static string[][] mainSubjects = new string[][]
        {                                                             
            new string[] { "Вища математика", "Дискретна математика", "Основи функцiонування комп'ютерiв", "Технологiї програмування" },
            new string[] { "Вища математика", "Фiзика", "Дискретна математика", "Комп'ютерна електронiка", "Технологiї програмування", "Навчальна практика" },
            new string[] { "Вища математика", "Архiтектура комп'ютера", "Комп'ютерна схемотехнiка", "Моделi та структури даних", "Теорiя iнформацiї та кодування", "Технологiї програмування" }
        };

        public static string[][] additionalSubjects = new string[][]
        {
            new string[] { "Гуманiтарна дисциплiна", "Українськi студiї", "Мовнi компетентностi" },
            new string[] { "Право", "Мовнi компетентностi" },
            new string[] { "Фiлософiя" }
        };

        public static double[][] mainCredits = new double[][]
        {
            new double[] { 5, 4.5, 5, 5 },
            new double[] { 5, 5, 4, 4, 4.5, 3 },
            new double[] { 5, 4, 4, 4.5, 4, 4 },
        };

        public static double[][] additionalCredits = new double[][]
        {
            new double[] { 3, 3, 3 },
            new double[] { 3, 3 },
            new double[] { 3 }
        };

        public static string[] years = new string[] { "ПЕРШИЙ", "ПЕРШИЙ", "ДРУГИЙ" };

        public static double[] manualCredits = new double[7];
        public static string[] manualSubjects = new string[7];

    }
}




