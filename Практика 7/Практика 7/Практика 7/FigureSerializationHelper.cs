using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace VectorEditor
{
    public static class FigureSerializationHelper
    {
        public static void SaveToStream(Stream stream, List<Figure> listToSave)
        {
            try
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, listToSave);
                stream.Position = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static List<Figure> LoadFromStream(Stream stream)
        {
            try
            {
                var formatter = new BinaryFormatter();
                stream.Position = 0;
                return (List<Figure>)formatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Figure>();
            }
        }
        public static void SaveToFile(string filePath, List<Figure> figures)
        {
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                SaveToStream(fs, figures);
            }
        }
        public static List<Figure> LoadFromFile(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                return LoadFromStream(fs);
            }
        }
    }
}