
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ShootingGame
{
    public partial class ResultsForm : Form
    {
        private string login;

        public ResultsForm(string login)
        {
            this.login = login;
            InitializeComponent();
            LoadResults();
        }
        private void InitializeComponent()
        {
            this.Text = "Результаты игрока";
            this.Size = new System.Drawing.Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            ListBox listBox = new ListBox()
            {
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(540, 300),
                Font = new System.Drawing.Font("Courier New", 10)
            };
            Button closeButton = new Button()
            {
                Text = "Закрыть",
                Location = new System.Drawing.Point(240, 330),
                Size = new System.Drawing.Size(100, 35),
                DialogResult = DialogResult.OK
            };
            this.Controls.Add(listBox);
            this.Controls.Add(closeButton);

            LoadResultsToListBox(listBox);
        }
        private void LoadResultsToListBox(ListBox listBox)
        {
            string filePath = "results.dat";
            if (!File.Exists(filePath))
            {
                listBox.Items.Add("Нет сохранённых результатов");
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            List<GameResult> allResults;

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {allResults = (List<GameResult>)formatter.Deserialize(fs);}

            var playerResults = allResults.FindAll(r => r.Login == login);

            if (playerResults.Count == 0)
            {listBox.Items.Add($"Нет результатов для игрока {login}");
                return;}

            listBox.Items.Add($"{"Дата",-20} {"Попадания",-12} {"Промахи",-10} {"Время",-10} {"Сложность",-12} {"Победа",-8}");
            listBox.Items.Add(new string('-', 80));

            foreach (var result in playerResults)
            {listBox.Items.Add($"{result.Date:dd.MM.yyyy HH:mm,-20} {result.Hits,-12} {result.Misses,-10} {result.TimeSpent:F1,-10} {result.Difficulty,-12} {(result.Win ? "Да" : "Нет"),-8}");}
        }
        private void LoadResults()
        {
            // Пустой метод для совместимости
        }
    }
}