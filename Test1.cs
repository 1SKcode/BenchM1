using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchM1
{
    internal class Test1
    {
        public long Start()
        {
            int[] array = new int[20000];
            Random random = new Random();

            for (int i = 0; i < array.Length; i++)  // Инициализация массива рандомными значениями 
            {
                array[i] = random.Next(-10000, 10000); // Присваиваем i элементу рандомное значение от -10000 до 10000
            }

            Stopwatch stopWatch = new Stopwatch(); // Объект позволяет хасекать время выполнения кода

            stopWatch.Start(); // Начинаем отсчет затраченного времени
            for (int i = 0; i < array.Length; i++) // Начало выполнения сортировки пузырьком
            {
                for (int j = 0; j < array.Length - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            stopWatch.Stop(); // Останавливаем отсчет

            return stopWatch.ElapsedMilliseconds; // Возвращаем значение затраченных секунд на выполнение теста 1 
        }
    }
}
