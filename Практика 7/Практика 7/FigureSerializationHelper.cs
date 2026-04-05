using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

public static class FigureSerializationHelper
{
    public static void SaveToStream(Stream stream, List<Figure> figures)
    {
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, figures);
        stream.Position = 0;
    }

    public static List<Figure> LoadFromStream(Stream stream)
    {
        try
        {
            var formatter = new BinaryFormatter();
            stream.Position = 0;
            return (List<Figure>)formatter.Deserialize(stream);
        }
        catch
        {
            MessageBox.Show("Ошибка загрузки файла");
            return new List<Figure>();
        }
    }
}