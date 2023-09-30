using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BenchM1
{
    internal class Test3
    {
        private Random random = new Random();
        public long Start(Panel panel)
        {
            Graphics g;
            g = panel.CreateGraphics();
            float x = 0f;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start(); // Начинаем отсчет затраченного времени на интервале

            int numCircles = 5000; // Количество кругов

            for (int i = 0; i < numCircles; i++)
            {
                stopWatch.Stop(); // время генерации рандомных чисел и выбора рандомного цыета не учитываем
                // Генерируем случайные координаты для центра круга в пределах панели
                int circleRadius = random.Next(4, 10); // Случайный радиус от 50 до 60 пикселей
                int centerX = random.Next(15, 130);
                int centerY = random.Next(105, 152);

                // Генерируем случайный цвет для каждого круга
                Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

                stopWatch.Start();
                // Создаем кисть с выбранным цветом
                using (SolidBrush brush = new SolidBrush(randomColor))
                {
                    // Рисуем круг
                    g.FillEllipse(brush, centerX - circleRadius, centerY - circleRadius, circleRadius * 2, circleRadius * 2);
                }
            }

            stopWatch.Stop(); // Останавливаем отсчет затраченного времени на интервале
            return stopWatch.ElapsedMilliseconds; // Возвращаем значение затраченных секунд на выполнение теста 3
        }
    }
}
