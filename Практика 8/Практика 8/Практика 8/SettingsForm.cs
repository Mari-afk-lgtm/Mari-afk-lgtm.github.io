
using System.Drawing;
using System.Windows.Forms;

namespace ShootingGame
{
    public partial class SettingsForm : Form
    {
        public string Difficulty { get; private set; }
        public Color CrosshairColor { get; private set; }

        private ComboBox difficultyCombo;
        private Button colorButton;

        public SettingsForm(string currentDifficulty, Color currentColor)
        {
            Difficulty = currentDifficulty;
            CrosshairColor = currentColor;
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.Text = "Настройки игры";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label diffLabel = new Label()
            {
                Text = "Уровень сложности:",
                Location = new System.Drawing.Point(50, 30),
                Size = new System.Drawing.Size(150, 25),
                Font = new System.Drawing.Font("Arial", 11)
            };
            difficultyCombo = new ComboBox()
            {
                Location = new System.Drawing.Point(200, 30),
                Size = new System.Drawing.Size(120, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            difficultyCombo.Items.AddRange(new[] { "Легкий", "Средний", "Сложный" });
            difficultyCombo.SelectedItem = Difficulty;

            Label colorLabel = new Label()
            {
                Text = "Цвет прицела:",
                Location = new System.Drawing.Point(50, 80),
                Size = new System.Drawing.Size(150, 25),
                Font = new System.Drawing.Font("Arial", 11)
            };
            colorButton = new Button()
            {
                Text = "Выбрать цвет",
                Location = new System.Drawing.Point(200, 80),
                Size = new System.Drawing.Size(120, 30),
                BackColor = CrosshairColor
            };
            colorButton.Click += (s, e) =>
            {
                using (ColorDialog cd = new ColorDialog())
                {
                    cd.Color = CrosshairColor;
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        CrosshairColor = cd.Color;
                        colorButton.BackColor = CrosshairColor;
                    }
                }
            };
            Button okButton = new Button()
            {
                Text = "Сохранить",
                Location = new System.Drawing.Point(80, 200),
                Size = new System.Drawing.Size(100, 35),
                DialogResult = DialogResult.OK
            };
            okButton.Click += (s, e) =>
            {Difficulty = difficultyCombo.SelectedItem.ToString();};

            Button cancelButton = new Button()
            {
                Text = "Отмена",
                Location = new System.Drawing.Point(200, 200),
                Size = new System.Drawing.Size(100, 35),
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(diffLabel);
            this.Controls.Add(difficultyCombo);
            this.Controls.Add(colorLabel);
            this.Controls.Add(colorButton);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }
    }
}