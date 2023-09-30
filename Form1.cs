using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BenchM1
{
    public partial class Form1 : Form
    {
        //ПЕРЕМЕННЫЕ
        bool coeffForm = true; // Статус режима (обычный или коэффициентный)
        long test1Time, test2Time, test3Time; // Время выполнения тестов
        string testName = ""; //Имя теста
        double maxValueTest = 0; // максимальное значение теста для поиска самого медленного теста
        int indexMinValue = 0; // Индекс строки самого долгого и медленного теста

        public Form1()
        {
            InitializeComponent();
            panelTest1.Visible = false; // Скрыввем панели 
            panelTest2.Visible = false;
            panelTest3.Visible = false;

            // Устрановка визуальной настойки отображаения данных DataGrid
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(161, 153, 139);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridView1.ReadOnly = true;
            LoadDataGrid();// Вызов метода заполнения таблицы данными
        }

        //Собитие нажатия на кнопку НАЧАТЬ
        private async void StartButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Введите имя теста", "ПУСТАЯ СТРОКА", buttons: MessageBoxButtons.OK);
                return;
            }

            testName = textBox1.Text;

            if (coeffForm == false)
            {
                dataGridView1.Rows.Clear(); // Очищаем и сбрасываем таблицу
                dataGridView1.Refresh();
                LoadDataGrid();
                coeffForm = true;
            }
            StartButton.Visible = false;
            panel2.Visible = false;

            // Скрываем и записываем в лэйблы значения по умолчанию (при повторном запуске теста)
            panelTest3.Refresh();
            labelTestTime1.Text = "Время выполнения: \r\n\r\n";
            labelTestTime2.Text = "Время выполнения: \r\n\r\n";
            labelTestTime3.Text = "Время выполнения: \r\n\r\n";
            panelTest2.Visible = false;
            panelTest3.Visible = false;
            labelDone1.Visible = false;
            labelDone2.Visible = false;
            labelDone3.Visible = false;
            label8.Visible = false;
            label8.Visible = false;

            panelTest1.Visible = true;
            await Task.Run(() => // Асинхронный запуск 1 теста (асинхронность нужна для того, чтобы окно не зависло)
            {
                Test1 test1 = new Test1();
                test1Time = test1.Start();
                labelTestTime1.Text += "             " + Convert.ToString(test1Time) + "МС";
            });
            label7.Visible = true;
            labelDone1.Visible = true;

            panelTest2.Visible = true;
            await Task.Run(() => // Асинхронный запуск 2 теста (асинхронность нужна для того, чтобы окно не зависло)
            {
                Test2 test2 = new Test2();
                test2Time = test2.Start();
                labelTestTime2.Text += "             " + Convert.ToString(test2Time) + "МС";
            });
            label8.Visible = true;
            labelDone2.Visible = true;

            panelTest3.Visible = true;
            await Task.Run(() => // Асинхронный запуск 3 теста (асинхронность нужна для того, чтобы окно не зависло)
            {
                Test3 test3 = new Test3();
                test3Time = test3.Start(panelTest3);
                labelTestTime3.Text += "             " + Convert.ToString(test3Time) + "МС";
            });
            labelDone3.Visible = true;

            dataGridView1.Rows.Add(testName, test1Time, test2Time, test3Time, Math.Pow(test1Time * test2Time * test3Time, 1.0 / 3));

            WriteResults();
            StartButton.Visible = true;
            panel2.Visible = true;
        }
        
        //Записываем полученные данные из таблицы в файл, чтбы открыть его в следующий раз
        private void WriteResults() 
        {
            // Это структура, которая позволяет обработать исключения (ошибки),
            // которые могут возникнуть внутри блока try. Если происходит ошибка,
            // выполнение кода переходит в блок catch, где можно обработать исключение
            // и выполнить соответствующие действия.
            try
            {
                StreamWriter file = new StreamWriter("Res.txt");
                string sLine = "";

                // Этот цикл for повторяется через каждую строку в таблице
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    //Это для цикла, проходящего через каждый столбец, и номер строки
                    //передается из цикла for выше.
                    for (int j = 0; j <= dataGridView1.Columns.Count - 1; j++)
                    {
                        sLine = sLine + dataGridView1.Rows[i].Cells[j].Value;
                        if (j != dataGridView1.Columns.Count - 1)
                        {
                            //Символ "=" добавляется в качестве разделителя значений в таблице
                            sLine = sLine + "=";
                        }
                    }
                    //Текст записывается в текстовый файл построчно
                    file.WriteLine(sLine);
                    sLine = "";
                }

                file.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        // Мeтод заплнения таблицы
        private void LoadDataGrid() 
        {
            try
            {


                string[] lines = File.ReadAllLines("Res.txt");
                string[] values;


                // повторяется каждую строку в таблице
                for (int i = 0; i < lines.Length; i++)
                {
                    values = lines[i].ToString().Split('='); // Разделяем записи через поиск символа "=" и присваиваем значение в i элемент массива
                    string[] row = new string[values.Length];

                    for (int j = 0; j < values.Length; j++)
                    {
                        row[j] = values[j].Trim();
                    }
                    dataGridView1.Rows.Add(row);
                }
            }
            catch
            {
                MessageBox.Show("Кажется, программа была запущена впервые. Два момента: \n" +
                    "1.В папке создан файл \"Res.txt\" - в нем будут хранится результаты тестов \n" +
                    "2.В папке создан файл \"Test2.txt\" - он содержит огромный(1652КБ) текст-рыбу, который используется для чтения во втором тесте\n\n\n" +
                    "Успешных тестов!", "ФАЙЛЫ СОЗДАНЫ", buttons: MessageBoxButtons.OK);
               
                //Создание файла
                StreamWriter file = new StreamWriter("Res.txt"); 
                file.Close();
                
                //Создание файла и запись в него текста 
                StreamWriter fileTest2 = new StreamWriter("Test2.txt");
                fileTest2.WriteAsync(Properties.Resources.Test2);
            }
        }

        // Событие нажатия на крзину - удаление данных из файла и очищение таблицы
        private void panel1_Click(object sender, EventArgs e)
        {
            indexMinValue = 0;
            File.WriteAllText("Res.txt", string.Empty); // Перезаписываем файл (в итоге имеем пустой файл txt)
            dataGridView1.Rows.Clear(); // Очищаем и сбрасываем таблицу
            dataGridView1.Refresh();
        }

        // Событие нажатия на кнопку "показать коэффициенты"
        private void panel2_Click(object sender, EventArgs e)
        {

            if (coeffForm == true)
            {
                double[] buffer = new double[4];

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++) // Поиск самого медленного
                {
                    if (maxValueTest < Convert.ToDouble(dataGridView1[4, i].Value))
                    {
                        maxValueTest = Convert.ToDouble(dataGridView1[4, i].Value);
                        indexMinValue = i;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    buffer[i] = Convert.ToDouble(dataGridView1.Rows[indexMinValue].Cells[i + 1].Value); // запись худших результатов по всем тестам во временный массив 
                }

                for (int i = 0; i < dataGridView1.RowCount - 1; i++) // Запись во вторую таблицу осуществляется через расчеты. Т.к нужно всего лишь посчитать коэффициенты 
                {
                    for (int j = 1; j < 5; j++)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value)/ buffer[j - 1]) , 5));
                    }
                }
                coeffForm = false;
            }
            else
            {
                dataGridView1.Rows.Clear(); // Очищаем и сбрасываем таблицу
                dataGridView1.Refresh();
                LoadDataGrid();
                coeffForm = true;
            }
        }

        // Изменение фона(цвета) кнопки начать при наведении мышки в зону кнопки
        private void StartButton_MouseMove(object sender, MouseEventArgs e)
        {
            StartButton.Image = Properties.Resources.startGreen;
        }

        // Изменение фона(цвета) кнопки начать при выходе мышки из зоны кнопки
        private void StartButton_MouseLeave(object sender, EventArgs e)
        {
            StartButton.Image = Properties.Resources.startStatic;
        }
    }
}
