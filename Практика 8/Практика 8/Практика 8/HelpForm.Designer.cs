using System.Drawing;
using System.Windows.Forms;

namespace ShootingGame
{
    partial class HelpForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Text = "Справка";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Location = new System.Drawing.Point(20, 20);
            richTextBox.Size = new System.Drawing.Size(440, 280);
            richTextBox.ReadOnly = true;
            richTextBox.BackColor = Color.White;
            richTextBox.Text =
                "=== ИГРА «СТРЕЛЬБА ПО ДВИЖУЩИМСЯ МИШЕНЯМ» ===\n\n" +
                "Управление:\n" +
                "• Стрелки ← → ↑ ↓ - движение прицела\n" +
                "• Пробел или ЛКМ - выстрел\n\n" +
                "Цель игры:\n" +
                "• Попасть в движущиеся мишени до истечения времени\n" +
                "• Количество необходимых попаданий зависит от сложности\n\n" +
                "Сложность:\n" +
                "• Лёгкая - 30 сек, 10 попаданий, 3 мишени\n" +
                "• Средняя - 20 сек, 15 попаданий, 4 мишени\n" +
                "• Сложная - 15 сек, 20 попаданий, 5 мишеней\n\n" +
                "Особенности:\n" +
                "• Мишени отражаются от стен\n" +
                "• Результаты сохраняются в двоичный файл\n" +
                "• Можно изменить цвет прицела в настройках";

            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new System.Drawing.Point(200, 320);
            okButton.Size = new System.Drawing.Size(80, 35);
            okButton.DialogResult = DialogResult.OK;

            this.Controls.Add(richTextBox);
            this.Controls.Add(okButton);
        }
    }
}