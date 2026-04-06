using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShootingGame
{
    public partial class MainForm : Form
    {
        private string currentLogin;
        private string difficulty = "Средний";
        private Color crosshairColor = Color.White;

        public MainForm()
        {
            InitializeComponent();
            LoadSettings();
        }
        private void LoadSettings()
        {
            if (Properties.Settings.Default.CrosshairColor != null)
            {crosshairColor = Properties.Settings.Default.CrosshairColor;}
            difficulty = Properties.Settings.Default.Difficulty ?? "Средний";
        }
        private void InitializeComponent()
        {
            this.Text = "Стрельба по движущимся мишеням";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            MenuStrip menuStrip = new MenuStrip();

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("Файл");
            ToolStripMenuItem exitItem = new ToolStripMenuItem("Выход");
            exitItem.Click += (s, e) => Application.Exit();
            fileMenu.DropDownItems.Add(exitItem);

            ToolStripMenuItem settingsMenu = new ToolStripMenuItem("Настройки");
            settingsMenu.Click += (s, e) => OpenSettings();

            ToolStripMenuItem resultsMenu = new ToolStripMenuItem("Результаты");
            resultsMenu.Click += (s, e) => ShowResults();

            ToolStripMenuItem helpMenu = new ToolStripMenuItem("Справка");
            helpMenu.Click += (s, e) => ShowHelp();

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(settingsMenu);
            menuStrip.Items.Add(resultsMenu);
            menuStrip.Items.Add(helpMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            Button startButton = new Button()
            {
                Text = "НАЧАТЬ ИГРУ",
                Size = new Size(200, 50),
                Location = new Point(this.ClientSize.Width / 2 - 100, this.ClientSize.Height / 2 - 100),
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                Font = new Font("Arial", 14, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            startButton.Click += StartGame;
            this.Controls.Add(startButton);

            Label infoLabel = new Label()
            {
                Text = "Управление: Стрелки - движение прицела, Пробел или ЛКМ - выстрел",
                Location = new Point(50, this.ClientSize.Height - 50),
                Size = new Size(700, 30),
                ForeColor = Color.Gray,
                BackColor = Color.Transparent
            };
            this.Controls.Add(infoLabel);
        }
        private void StartGame(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentLogin))
            {using (LoginForm loginForm = new LoginForm())
                {if (loginForm.ShowDialog() == DialogResult.OK)
                    {currentLogin = loginForm.Login;}
                    else
                    {return;}
                }
            }
            using (GameForm gameForm = new GameForm(currentLogin, difficulty, crosshairColor))
            {gameForm.ShowDialog();}
        }
        private void OpenSettings()
        {using (SettingsForm settingsForm = new SettingsForm(difficulty, crosshairColor))
            {if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    difficulty = settingsForm.Difficulty;
                    crosshairColor = settingsForm.CrosshairColor;

                    Properties.Settings.Default.Difficulty = difficulty;
                    Properties.Settings.Default.CrosshairColor = crosshairColor;
                    Properties.Settings.Default.Save();
                }
            }
        }
        private void ShowResults()
        {if (string.IsNullOrEmpty(currentLogin))
            {using (LoginForm loginForm = new LoginForm())
                {if (loginForm.ShowDialog() == DialogResult.OK)
                    {currentLogin = loginForm.Login;}
                    else
                    {return;}
                }
            }
            using (ResultsForm resultsForm = new ResultsForm(currentLogin))
            {resultsForm.ShowDialog();}
        }
        private void ShowHelp()
        {using (HelpForm helpForm = new HelpForm())
            {helpForm.ShowDialog();}
        }
    }
}