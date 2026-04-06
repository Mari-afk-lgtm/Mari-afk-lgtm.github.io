using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShootingGame
{
    public partial class GameForm : Form
    {
        private string login;
        private string difficulty;
        private Color crosshairColor;

        private Crosshair crosshair;
        private List<Target> targets;
        private Timer gameTimer;
        private Timer moveTimer;

        private int hits = 0;
        private int misses = 0;
        private int requiredHits;
        private int gameTime;
        private int timeLeft;
        private bool gameActive = true;
        private bool gameOver = false;

        private int initialTargetCount;  
        private Random random = new Random();
        private double startTime;

        public GameForm(string login, string difficulty, Color crosshairColor)
        {
            this.login = login;
            this.difficulty = difficulty;
            this.crosshairColor = crosshairColor;

            InitializeComponent();
            SetupGame();
        }
        private void InitializeComponent()
        {
            this.Text = "Стрельба по движущимся мишеням - Игра";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;

            this.KeyPreview = true;
            this.KeyDown += GameForm_KeyDown;
            this.MouseClick += GameForm_MouseClick;
            this.Paint += GameForm_Paint;
        }
        private void SetupGame()
        {
            switch (difficulty)
            {
                case "Легкий":
                    gameTime = 30;
                    requiredHits = 10;
                    initialTargetCount = 3;
                    break;
                case "Средний":
                    gameTime = 20;
                    requiredHits = 15;
                    initialTargetCount = 4;
                    break;
                case "Сложный":
                    gameTime = 15;
                    requiredHits = 20;
                    initialTargetCount = 5;
                    break;
                default:
                    gameTime = 20;
                    requiredHits = 15;
                    initialTargetCount = 4;
                    break;
            }
            timeLeft = gameTime;

            crosshair = new Crosshair(this.ClientSize.Width / 2, this.ClientSize.Height / 2);
            crosshair.Color = crosshairColor;

            CreateTargets(initialTargetCount);

            moveTimer = new Timer();
            moveTimer.Interval = 16; 
            moveTimer.Tick += MoveTimer_Tick;
            moveTimer.Start();

            gameTimer = new Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            startTime = DateTime.Now.TimeOfDay.TotalSeconds;
        }
        private void CreateTargets(int count)
        {
            if (targets == null)
                targets = new List<Target>();

            float targetSpeed;
            switch (difficulty)
            {
                case "Легкий":
                    targetSpeed = 2f;
                    break;
                case "Средний":
                    targetSpeed = 3f;
                    break;
                default:
                    targetSpeed = 4f;
                    break;
            }
            for (int i = 0; i < count; i++)
            {
                float x = random.Next(50, this.ClientSize.Width - 50);
                float y = random.Next(50, this.ClientSize.Height - 50);
                float sx = (float)(random.NextDouble() * targetSpeed * 2 - targetSpeed);
                float sy = (float)(random.NextDouble() * targetSpeed * 2 - targetSpeed);

                if (Math.Abs(sx) < 0.5f) sx = sx >= 0 ? targetSpeed / 2 : -targetSpeed / 2;
                if (Math.Abs(sy) < 0.5f) sy = sy >= 0 ? targetSpeed / 2 : -targetSpeed / 2;

                targets.Add(new Target(x, y, sx, sy));
            }
        }
        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            if (gameActive && !gameOver)
            {
                if (InputManager.IsKeyDown(Keys.Left)) crosshair.Move(-8, 0, ClientSize.Width, ClientSize.Height);
                if (InputManager.IsKeyDown(Keys.Right)) crosshair.Move(8, 0, ClientSize.Width, ClientSize.Height);
                if (InputManager.IsKeyDown(Keys.Up)) crosshair.Move(0, -8, ClientSize.Width, ClientSize.Height);
                if (InputManager.IsKeyDown(Keys.Down)) crosshair.Move(0, 8, ClientSize.Width, ClientSize.Height);

                foreach (var target in targets)
                {target.Move(ClientSize.Width, ClientSize.Height);}

                Invalidate(); 
            }
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!gameActive || gameOver) return;

            timeLeft--;
            Invalidate();

            if (timeLeft <= 0)
            {
                EndGame(false);
            }
        }
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && gameActive && !gameOver)
            {
                Shoot();
            }
        }
        private void GameForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && gameActive && !gameOver)
            {
                crosshair.X = e.X;
                crosshair.Y = e.Y;
                Shoot();
            }
        }
        private void Shoot()
        {
            bool hit = false;
            for (int i = targets.Count - 1; i >= 0; i--)
            {
                if (crosshair.GetBounds().IntersectsWith(targets[i].GetBounds()))
                {
                    targets.RemoveAt(i);
                    hits++;
                    hit = true;

                    AddNewTarget();
                    break;
                }
            }
            if (!hit)
            {misses++;}
            Invalidate();

            if (hits >= requiredHits)
            {EndGame(true);}
        }
        private void AddNewTarget()
        {
            float targetSpeed;
            switch (difficulty)
            {
                case "Легкий":
                    targetSpeed = 2f;
                    break;
                case "Средний":
                    targetSpeed = 3f;
                    break;
                default:
                    targetSpeed = 4f;
                    break;
            }
            float x = random.Next(50, this.ClientSize.Width - 50);
            float y = random.Next(50, this.ClientSize.Height - 50);
            float sx = (float)(random.NextDouble() * targetSpeed * 2 - targetSpeed);
            float sy = (float)(random.NextDouble() * targetSpeed * 2 - targetSpeed);

            if (Math.Abs(sx) < 0.5f) sx = sx >= 0 ? targetSpeed / 2 : -targetSpeed / 2;
            if (Math.Abs(sy) < 0.5f) sy = sy >= 0 ? targetSpeed / 2 : -targetSpeed / 2;

            targets.Add(new Target(x, y, sx, sy));
        }
        private void EndGame(bool win)
        {
            gameActive = false;
            gameOver = true;
            moveTimer.Stop();
            gameTimer.Stop();

            double timeSpent = DateTime.Now.TimeOfDay.TotalSeconds - startTime;

            SaveResult(win, timeSpent);

            Invalidate();

            Button exitButton = new Button()
            {
                Text = "Выйти",
                Size = new Size(150, 40),
                Location = new Point(ClientSize.Width / 2 - 75, ClientSize.Height / 2 + 100),
                BackColor = Color.DarkRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            exitButton.Click += (s, e) => this.Close();
            this.Controls.Add(exitButton);

            Button newGameButton = new Button()
            {
                Text = "Новая игра",
                Size = new Size(150, 40),
                Location = new Point(ClientSize.Width / 2 - 75, ClientSize.Height / 2 + 50),
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            newGameButton.Click += (s, e) =>
            {
                this.Controls.Clear();
                SetupGame();
                gameActive = true;
                gameOver = false;
            };
            this.Controls.Add(newGameButton);
        }
        private void SaveResult(bool win, double timeSpent)
        {
            List<GameResult> results = new List<GameResult>();
            string filePath = "results.dat";

            if (File.Exists(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    results = (List<GameResult>)formatter.Deserialize(fs);}
            }
            results.Add(new GameResult
            {
                Login = login,
                Hits = hits,
                Misses = misses,
                TimeSpent = timeSpent,
                Difficulty = difficulty,
                Win = win,
                Date = DateTime.Now
            });
            BinaryFormatter saveFormatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                saveFormatter.Serialize(fs, results);
            }
        }
        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (var target in targets)
            {target.Draw(g);}
            crosshair.Draw(g);

            using (Font font = new Font("Arial", 16, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                g.DrawString($"Попаданий: {hits} / {requiredHits}", font, brush, 20, 20);
                g.DrawString($"Промахов: {misses}", font, brush, 20, 60);
                g.DrawString($"Мишеней на поле: {targets.Count}", font, brush, 20, 100);
                g.DrawString($"Время: {timeLeft} сек", font, brush, ClientSize.Width - 150, 20);
            }
            if (gameOver)
            {
                using (Font font = new Font("Arial", 36, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(hits >= requiredHits ? Color.Green : Color.Red))
                {
                    string message = hits >= requiredHits ? "ПОБЕДА!" : "ПОРАЖЕНИЕ";
                    SizeF textSize = g.MeasureString(message, font);
                    g.DrawString(message, font, brush,
                        (ClientSize.Width - textSize.Width) / 2,
                        ClientSize.Height / 2 - 50);

                    using (Font smallFont = new Font("Arial", 16))
                    {
                        string result = $"Попаданий: {hits}, Промахов: {misses}";
                        SizeF resultSize = g.MeasureString(result, smallFont);
                        g.DrawString(result, smallFont, Brushes.White,
                            (ClientSize.Width - resultSize.Width) / 2,
                            ClientSize.Height / 2 + 20);
                    }
                }
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            moveTimer?.Stop();
            gameTimer?.Stop();
            base.OnFormClosing(e);
        }
    }
    public static class InputManager
    {
        private static Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>();

        static InputManager()
        {Application.Idle += (s, e) =>
            {foreach (var key in Enum.GetValues(typeof(Keys)))
                {keyStates[(Keys)key] = (Control.ModifierKeys & (Keys)key) != 0;}
            };
        }
        public static bool IsKeyDown(Keys key)
        {return keyStates.ContainsKey(key) && keyStates[key];}
    }
}