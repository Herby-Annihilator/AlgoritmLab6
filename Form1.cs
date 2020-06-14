using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryForAlgLab;
using TerritoryAndBases;
using System.IO;

namespace MilitaryBases
{
    public partial class MainWindow : Form
    {
        public Graph Graph { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Graph = new Graph();
        }
        /// <summary>
        /// Задать матрицу смежности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            CreateAdjacencyMatrix createAdjacencyMatrix = new CreateAdjacencyMatrix();
            createAdjacencyMatrix.ShowDialog();

            int[][] adjacencyMatrix = GetAdjacencyMatrixFromFile("input.dat");
            if (adjacencyMatrix == null)
            {
                MessageBox.Show("Матрицы нет в файле!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
                {
                    Graph.Vertices.Add(new Vertex(i, 0, 0));
                }
                for (int i = 0; i < Graph.Vertices.Count; i++)
                {
                    for (int j = i; j < Graph.Vertices.Count; j++)
                    {
                        if (adjacencyMatrix[i][j] > 0)
                        {
                            if (!Graph.ConnectVerties(i, j))
                            {
                                MessageBox.Show("Не удалось соединить вершины" + i + " " + j, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Graph.Vertices.Clear();
                                Graph.Edges.Clear();
                                return;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Восстановить матрицу смежности из файла
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private int[][] GetAdjacencyMatrixFromFile(string fileName)
        {
            int[][] toReturn = null;
            if (!File.Exists(fileName))
            {
                MessageBox.Show("Файл " + fileName + " не найден или его не существует", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                StreamReader reader = new StreamReader(fileName);
                string[] strs = reader.ReadToEnd().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                toReturn = new int[strs.Length][];
                for (int i = 0; i < strs.Length; i++)
                {
                    toReturn[i] = new int[strs.Length];
                }
                for (int i = 0; i < strs.Length; i++)
                {
                    string[] numbers = strs[i].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < numbers.Length; j++)
                    {
                        try
                        {
                            toReturn[i][j] = Convert.ToInt32(numbers[j]);
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("Неверный формат матрицы смежности", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            reader.Close();
                            return null;
                        }
                    }
                }
                reader.Close();
            }
            return toReturn;
        }
        /// <summary>
        /// Задать характеристики районам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            SetCharacteristics setCharacteristics = new SetCharacteristics(this);
            setCharacteristics.ShowDialog();
        }
        /// <summary>
        /// Восстановить из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists("Graph.dat"))
            {
                Graph = (Graph)SaverLoader.LoadFromFile("Graph.dat");
            }
            else
            {
                MessageBox.Show("Граф не был восстановлен", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// Узнать расположение баз
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (Graph != null)
            {
                List<int> list = Graph.GetRegionsToBuiltBases();
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        textBox1.Text += list[i] + " ";
                    }
                }
                else
                {
                    MessageBox.Show("Списка нет!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Хера с два!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
