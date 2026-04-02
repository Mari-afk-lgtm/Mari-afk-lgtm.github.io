using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Практика_4._2
{
    public class Country
    {
        private string name;
        private int gold;
        private int silver;
        private int bronze;

        public Country(string name, int gold, int silver, int bronze)
        {
            this.name = name;
            this.gold = gold;
            this.silver = silver;
            this.bronze = bronze;
        }
        public string Name => name;
        public int Gold { get => gold; set => gold = value; }
        public int Silver { get => silver; set => silver = value; }
        public int Bronze { get => bronze; set => bronze = value; }
        public int TotalPoints => gold * 9 + silver * 7 + bronze * 5;
    }
}
