
using System.Windows.Forms;

namespace ShootingGame
{
    public partial class LoginForm : Form
    {
        public string Login { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.Text = "Авторизация";
            this.Size = new System.Drawing.Size(400, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label label = new Label()
            {
                Text = "Введите ваш логин:",
                Location = new System.Drawing.Point(50, 30),
                Size = new System.Drawing.Size(300, 25),
                Font = new System.Drawing.Font("Arial", 12)
            };
            TextBox textBox = new TextBox()
            {
                Location = new System.Drawing.Point(50, 60),
                Size = new System.Drawing.Size(280, 25),
                Font = new System.Drawing.Font("Arial", 12)
            };
            Button okButton = new Button()
            {
                Text = "OK",
                Location = new System.Drawing.Point(100, 110),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.OK
            };
            Button cancelButton = new Button()
            {
                Text = "Отмена",
                Location = new System.Drawing.Point(200, 110),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.Cancel
            };
            okButton.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {Login = textBox.Text;}
                else
                {MessageBox.Show("Введите логин!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;}
            };
            this.Controls.Add(label);
            this.Controls.Add(textBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }
    }
}