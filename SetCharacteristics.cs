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
using System.IO;

namespace MilitaryBases
{
    public partial class SetCharacteristics : Form
    {
        public MainWindow MainWindow { get; set; }
        public SetCharacteristics(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            UpdateData();
        }
        /// <summary>
        /// Принять характеристики
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            bool isCorrect = true;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace((string)dataGridView1.Rows[i].Cells[1].Value) || !string.IsNullOrWhiteSpace((string)dataGridView1.Rows[i].Cells[2].Value))
                {
                    int territorySize, controlDistance;
                    if (int.TryParse((string)dataGridView1.Rows[i].Cells[1].Value, out territorySize) && int.TryParse((string)dataGridView1.Rows[i].Cells[2].Value, out controlDistance))
                    {
                        if (territorySize < 1 || controlDistance < 1)
                        {
                            isCorrect = false;
                            MessageBox.Show("Некорректное значение в поле/полях района " + MainWindow.Graph.Vertices[(int)dataGridView1.Rows[i].Cells[0].Value].Index,
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                        MainWindow.Graph.Vertices[(int)dataGridView1.Rows[i].Cells[0].Value].ControlDistance = controlDistance;
                        MainWindow.Graph.Vertices[(int)dataGridView1.Rows[i].Cells[0].Value].TerritorySize = territorySize;
                    }
                    else
                    {
                        MessageBox.Show("Некорректное значение в поле/полях района " + MainWindow.Graph.Vertices[(int)dataGridView1.Rows[i].Cells[0].Value].Index,
                            "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        isCorrect = false;
                        break;
                    }
                }
                else
                {
                    isCorrect = false;
                    MessageBox.Show("Некорректное значение в поле/полях района " + MainWindow.Graph.Vertices[(int)dataGridView1.Rows[i].Cells[0].Value].Index,
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }
            if (isCorrect)
            {
                if (File.Exists("Graph.dat"))
                {
                    File.Delete("Graph.dat");
                }
                MainWindow.Graph.UpdateEdgesData();
                SaverLoader.SaveToFile("Graph.dat", MainWindow.Graph);
                Close();
            }
        }
        /// <summary>
        /// Загружает данные графа в таблицу
        /// </summary>
        private void UpdateData()
        {
            if (MainWindow.Graph.Vertices.Count != 0)
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < MainWindow.Graph.Vertices.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = MainWindow.Graph.Vertices[i].Index;
                    dataGridView1.Rows[i].Cells[1].Value = MainWindow.Graph.Vertices[i].TerritorySize;
                    dataGridView1.Rows[i].Cells[2].Value = MainWindow.Graph.Vertices[i].ControlDistance;                    
                }
            }
        }
    }
}
