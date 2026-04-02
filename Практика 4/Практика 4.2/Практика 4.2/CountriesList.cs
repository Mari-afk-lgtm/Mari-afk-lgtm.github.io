using System.Collections.Generic;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace Практика_4._2
{
    public class CountriesList
    {
        private List<Country> countries;

        public CountriesList()
        {countries = new List<Country>();}

        public int Count => countries.Count;

        public void Add(Country c)
        {countries.Add(c);}

        public Country this[int index]
        {
            get
            {
                if (index >= 0 && index < countries.Count)
                    return countries[index];
                return null;
            }
        }
        public void SaveToFile(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (var c in countries)
                {sw.WriteLine($"{c.Name};{c.Gold};{c.Silver};{c.Bronze}");}
            }
        }
        public void LoadFromFile(string filename)
        {
            countries.Clear();
            using (StreamReader sr = new StreamReader(filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 4)
                    {
                        string name = parts[0];
                        int gold = int.Parse(parts[1]);
                        int silver = int.Parse(parts[2]);
                        int bronze = int.Parse(parts[3]);
                        countries.Add(new Country(name, gold, silver, bronze));
                    }
                }
            }
        }
        public void DrawPieChart(Chart chart)
        {
            chart.Series.Clear();
            chart.BackColor = Color.Gray;
            chart.BackSecondaryColor = Color.WhiteSmoke;
            chart.BackGradientStyle = GradientStyle.DiagonalRight;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderlineColor = Color.Gray;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;

            chart.ChartAreas[0].BackColor = Color.White;
            chart.ChartAreas[0].Area3DStyle.Enable3D = true;

            chart.Titles.Clear();
            chart.Titles.Add("Олимпийские игры: баллы стран");
            chart.Titles[0].Font = new Font("Utopia", 16);

            Series series = new Series("PointsSeries")
            {
                ChartType = SeriesChartType.Pie
            };
            chart.Series.Add(series);

            foreach (var c in countries)
            {series.Points.AddXY(c.Name, c.TotalPoints);}

            series.IsValueShownAsLabel = true;
            series.Label = "#VALX";
        }
    }
}
