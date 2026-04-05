using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using VectorEditor;

namespace VectorEditor
{
    public partial class Form1 : Form
    {
        // Основные данные
        private List<Figure> figures = new List<Figure>();
        private List<Figure> clipboard = new List<Figure>();
        private StackMemory undoStack;
        private StackMemory redoStack;
        private Figure selectedFigure = null;

        // Компоненты формы
        private MenuStrip mainMenu;
        private ToolStrip toolStrip;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private Panel drawingPanel;

        // Диалоги
        private ColorDialog colorDialog;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        public Form1()
        {
            InitializeComponent();
            InitializeEditor();
        }
        private void InitializeComponent()
        {
            this.Text = "Векторный редактор - Вариант 12";
            this.Size = new Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;

            // Создание компонентов
            mainMenu = new MenuStrip();
            toolStrip = new ToolStrip();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("Готов к работе");
            drawingPanel = new Panel();
            colorDialog = new ColorDialog();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();

            // Настройка drawingPanel
            drawingPanel.Dock = DockStyle.Fill;
            drawingPanel.BackColor = Color.White;
            drawingPanel.Paint += DrawingPanel_Paint;
            drawingPanel.MouseClick += DrawingPanel_MouseClick;

            // Настройка statusStrip
            statusStrip.Items.Add(statusLabel);

            // Добавление элементов на форму
            this.Controls.Add(drawingPanel);
            this.Controls.Add(statusStrip);
            this.Controls.Add(mainMenu);
            this.Controls.Add(toolStrip);

            // Создание меню
            CreateMenu();

            // Создание панели инструментов
            CreateToolbar();

            // Настройка диалогов
            openFileDialog.Filter = "Vector files (*.vec)|*.vec|All files (*.*)|*.*";
            saveFileDialog.Filter = "Vector files (*.vec)|*.vec|All files (*.*)|*.*";
        }
        private void InitializeEditor()
        {
            undoStack = new StackMemory(20);
            redoStack = new StackMemory(20);
            SaveState();
        }
        private void CreateMenu()
        {
            // Файл
            var fileMenu = new ToolStripMenuItem("Файл");
            fileMenu.DropDownItems.Add("Новый", null, NewFile_Click);
            fileMenu.DropDownItems.Add("Открыть...", null, OpenFile_Click);
            fileMenu.DropDownItems.Add("Сохранить", null, SaveFile_Click);
            fileMenu.DropDownItems.Add("Сохранить как...", null, SaveAsFile_Click);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add("Выход", null, (s, e) => Application.Exit());

            // Правка
            var editMenu = new ToolStripMenuItem("Правка");
            editMenu.DropDownItems.Add("Отменить", null, Undo_Click);
            editMenu.DropDownItems.Add("Повторить", null, Redo_Click);
            editMenu.DropDownItems.Add(new ToolStripSeparator());
            editMenu.DropDownItems.Add("Вырезать", null, Cut_Click);
            editMenu.DropDownItems.Add("Копировать", null, Copy_Click);
            editMenu.DropDownItems.Add("Вставить", null, Paste_Click);
            editMenu.DropDownItems.Add(new ToolStripSeparator());
            editMenu.DropDownItems.Add("Удалить", null, Delete_Click);

            // Фигуры
            var figuresMenu = new ToolStripMenuItem("Фигуры");
            figuresMenu.DropDownItems.Add("Прямая линия", null, (s, e) => AddLine());
            figuresMenu.DropDownItems.Add("Дуга", null, (s, e) => AddArc());
            figuresMenu.DropDownItems.Add("Круг", null, (s, e) => AddCircle());
            figuresMenu.DropDownItems.Add("Эллипс", null, (s, e) => AddEllipse());

            // Контур
            var strokeMenu = new ToolStripMenuItem("Контур");

            var colorItem = new ToolStripMenuItem("Цвет контура");
            colorItem.Click += (s, e) => ChangeStrokeColor();
            strokeMenu.DropDownItems.Add(colorItem);

            var widthMenu = new ToolStripMenuItem("Толщина контура");
            widthMenu.DropDownItems.Add("1 px", null, (s, e) => ChangeStrokeWidth(1));
            widthMenu.DropDownItems.Add("2 px", null, (s, e) => ChangeStrokeWidth(2));
            widthMenu.DropDownItems.Add("3 px", null, (s, e) => ChangeStrokeWidth(3));
            widthMenu.DropDownItems.Add("5 px", null, (s, e) => ChangeStrokeWidth(5));
            strokeMenu.DropDownItems.Add(widthMenu);

            var styleMenu = new ToolStripMenuItem("Стиль линии");
            styleMenu.DropDownItems.Add("Сплошная", null, (s, e) => ChangeLineStyle(DashStyle.Solid));
            styleMenu.DropDownItems.Add("Пунктирная", null, (s, e) => ChangeLineStyle(DashStyle.Dash));
            styleMenu.DropDownItems.Add("Точечная", null, (s, e) => ChangeLineStyle(DashStyle.Dot));
            strokeMenu.DropDownItems.Add(styleMenu);

            mainMenu.Items.Add(fileMenu);
            mainMenu.Items.Add(editMenu);
            mainMenu.Items.Add(figuresMenu);
            mainMenu.Items.Add(strokeMenu);
        }
        private void CreateToolbar()
        {
            AddToolButton("Линия", "Line", (s, e) => AddLine());
            AddToolButton("Дуга", "Arc", (s, e) => AddArc());
            AddToolButton("Круг", "Circle", (s, e) => AddCircle());
            AddToolButton("Эллипс", "Ellipse", (s, e) => AddEllipse());

            toolStrip.Items.Add(new ToolStripSeparator());

            AddToolButton("Отменить", "Undo", Undo_Click);
            AddToolButton("Повторить", "Redo", Redo_Click);

            toolStrip.Items.Add(new ToolStripSeparator());

            AddToolButton("Вырезать", "Cut", Cut_Click);
            AddToolButton("Копировать", "Copy", Copy_Click);
            AddToolButton("Вставить", "Paste", Paste_Click);
            AddToolButton("Удалить", "Delete", Delete_Click);

            toolStrip.Items.Add(new ToolStripSeparator());

            AddToolButton("Сплошная", "Solid", (s, e) => ChangeLineStyle(DashStyle.Solid));
            AddToolButton("Пунктирная", "Dash", (s, e) => ChangeLineStyle(DashStyle.Dash));
            AddToolButton("Точечная", "Dot", (s, e) => ChangeLineStyle(DashStyle.Dot));
        }

        private void AddToolButton(string text, string name, EventHandler click)
        {
            var button = new ToolStripButton(text);
            button.Name = name;
            button.Click += click;
            button.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStrip.Items.Add(button);
        }
        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var figure in figures)
            {
                figure.Draw(e.Graphics);
            }

            if (selectedFigure != null)
            {
                selectedFigure.DrawSelectionMarkers(e.Graphics);
            }
        }
        private void DrawingPanel_MouseClick(object sender, MouseEventArgs e)
        {
            Figure clickedFigure = null;
            for (int i = figures.Count - 1; i >= 0; i--)
            {
                if (figures[i].IsHit(e.Location))
                {
                    clickedFigure = figures[i];
                    break;
                }
            }
            if (clickedFigure != null)
            {
                selectedFigure = clickedFigure;
                UpdateStatus($"Выбрана фигура: {selectedFigure.GetType().Name}");
            }
            else
            {
                selectedFigure = null;
                UpdateStatus("Ничего не выбрано");
            }

            drawingPanel.Invalidate();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (selectedFigure == null) return;

            int step = e.Shift ? 1 : 5;

            SaveState();

            switch (e.KeyCode)
            {
                case Keys.Left:
                    selectedFigure.Move(-step, 0);
                    break;
                case Keys.Right:
                    selectedFigure.Move(step, 0);
                    break;
                case Keys.Up:
                    selectedFigure.Move(0, -step);
                    break;
                case Keys.Down:
                    selectedFigure.Move(0, step);
                    break;
                case Keys.Delete:
                    DeleteSelectedFigure();
                    break;
            }

            drawingPanel.Invalidate();
            base.OnKeyDown(e);
        }

        private void AddFigure(Figure figure)
        {
            SaveState();
            figures.Add(figure);
            selectedFigure = figure;
            drawingPanel.Invalidate();
            UpdateStatus($"Добавлена фигура: {figure.GetType().Name}");
        }

        private void AddLine()
        {
            var stroke = new Stroke { Color = Color.Black, Width = 2, DashStyle = DashStyle.Solid };
            var line = new LineFigure(new Rectangle(100, 100, 150, 150), stroke);
            AddFigure(line);
        }

        private void AddArc()
        {
            var stroke = new Stroke { Color = Color.Black, Width = 2, DashStyle = DashStyle.Solid };
            var arc = new ArcFigure(new Rectangle(100, 100, 150, 100), stroke);
            AddFigure(arc);
        }

        private void AddCircle()
        {
            var stroke = new Stroke { Color = Color.Black, Width = 2, DashStyle = DashStyle.Solid };
            var circle = new CircleFigure(new Rectangle(100, 100, 100, 100), stroke);
            AddFigure(circle);
        }

        private void AddEllipse()
        {
            var stroke = new Stroke { Color = Color.Black, Width = 2, DashStyle = DashStyle.Solid };
            var ellipse = new EllipseFigure(new Rectangle(100, 100, 150, 100), stroke);
            AddFigure(ellipse);
        }

        private void ChangeStrokeColor()
        {
            if (selectedFigure == null)
            {
                MessageBox.Show("Сначала выделите фигуру", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                SaveState();
                selectedFigure.Stroke.Color = colorDialog.Color;
                drawingPanel.Invalidate();
                UpdateStatus($"Цвет контура изменён");
            }
        }

        private void ChangeStrokeWidth(float width)
        {
            if (selectedFigure == null)
            {
                MessageBox.Show("Сначала выделите фигуру", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveState();
            selectedFigure.Stroke.Width = width;
            drawingPanel.Invalidate();
            UpdateStatus($"Толщина контура: {width} px");
        }

        private void ChangeLineStyle(DashStyle style)
        {
            if (selectedFigure == null)
            {
                MessageBox.Show("Сначала выделите фигуру", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveState();
            selectedFigure.Stroke.DashStyle = style;
            drawingPanel.Invalidate();

            string styleName = style == DashStyle.Solid ? "сплошная" :
                              (style == DashStyle.Dash ? "пунктирная" : "точечная");
            UpdateStatus($"Стиль линии: {styleName}");
        }

        private void SaveState()
        {
            using (var ms = new MemoryStream())
            {
                FigureSerializationHelper.SaveToStream(ms, figures);
                undoStack.Push(ms);
            }
            redoStack.Clear();
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count == 0) return;

            using (var ms = new MemoryStream())
            {
                using (var currentMs = new MemoryStream())
                {
                    FigureSerializationHelper.SaveToStream(currentMs, figures);
                    redoStack.Push(currentMs);
                }

                undoStack.Pop(ms);
                figures = FigureSerializationHelper.LoadFromStream(ms);
                selectedFigure = null;
                drawingPanel.Invalidate();
                UpdateStatus("Отмена действия");
            }
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count == 0) return;

            using (var ms = new MemoryStream())
            {
                using (var currentMs = new MemoryStream())
                {
                    FigureSerializationHelper.SaveToStream(currentMs, figures);
                    undoStack.Push(currentMs);
                }

                redoStack.Pop(ms);
                figures = FigureSerializationHelper.LoadFromStream(ms);
                selectedFigure = null;
                drawingPanel.Invalidate();
                UpdateStatus("Повтор действия");
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (selectedFigure != null)
            {
                clipboard = new List<Figure> { DeepCloneFigure(selectedFigure) };
                UpdateStatus("Фигура скопирована");
            }
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            if (selectedFigure != null)
            {
                Copy_Click(sender, e);
                DeleteSelectedFigure();
                UpdateStatus("Фигура вырезана");
            }
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            if (clipboard != null && clipboard.Count > 0)
            {
                SaveState();
                var newFigure = DeepCloneFigure(clipboard[0]);
                newFigure.Move(20, 20);
                figures.Add(newFigure);
                selectedFigure = newFigure;
                drawingPanel.Invalidate();
                UpdateStatus("Фигура вставлена");
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            DeleteSelectedFigure();
        }

        private void DeleteSelectedFigure()
        {
            if (selectedFigure != null)
            {
                SaveState();
                figures.Remove(selectedFigure);
                selectedFigure = null;
                drawingPanel.Invalidate();
                UpdateStatus("Фигура удалена");
            }
        }

        private Figure DeepCloneFigure(Figure original)
        {
            using (var ms = new MemoryStream())
            {
                FigureSerializationHelper.SaveToStream(ms, new List<Figure> { original });
                var list = FigureSerializationHelper.LoadFromStream(ms);
                return list.Count > 0 ? list[0] : null;
            }
        }

        private void NewFile_Click(object sender, EventArgs e)
        {
            if (figures.Count > 0)
            {
                var result = MessageBox.Show("Сохранить изменения?", "Новый файл",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                    SaveFile_Click(sender, e);
                else if (result == DialogResult.Cancel)
                    return;
            }

            figures.Clear();
            selectedFigure = null;
            undoStack.Clear();
            redoStack.Clear();
            SaveState();
            drawingPanel.Invalidate();
            UpdateStatus("Создан новый документ");
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    figures = FigureSerializationHelper.LoadFromFile(openFileDialog.FileName);
                    selectedFigure = null;
                    undoStack.Clear();
                    redoStack.Clear();
                    SaveState();
                    drawingPanel.Invalidate();
                    UpdateStatus($"Загружен файл: {openFileDialog.FileName}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                SaveAsFile_Click(sender, e);
                return;
            }

            try
            {
                FigureSerializationHelper.SaveToFile(saveFileDialog.FileName, figures);
                UpdateStatus($"Сохранён файл: {saveFileDialog.FileName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveAsFile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveFile_Click(sender, e);
            }
        }
        private void UpdateStatus(string message)
        {
            statusLabel.Text = message;
        }
    }
}