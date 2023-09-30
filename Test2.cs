using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BenchM1
{
    internal class Test2
    {
        public long Start()
        {
            try
            {
                string str = ""; // Инициализируем пустую строку
                Stopwatch stopWatch = new Stopwatch();
                StreamReader f = new StreamReader("Test2.txt"); //Открываем и читаем текстовый файл
                stopWatch.Start(); // Запускаем таймер
                while (!f.EndOfStream)
                {
                    string s = f.ReadLine();
                    str = string.Concat(str, s); //Построчно записываем текстовые значения из файла
                }
                f.Close();
                stopWatch.Stop();// останавливаем таймер
                return stopWatch.ElapsedMilliseconds; // Возвращаем значение затраченных секунд на выполнение теста 2
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 0;
            }
        }
    }
}
