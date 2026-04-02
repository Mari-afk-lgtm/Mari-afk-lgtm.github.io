using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Supabase;
using Supabase;

namespace Supabase
{

    public partial class Form1 : Form


    {
        string key = "sb_publishable_ZJXkKLGGYGy3ZpfFWNB48g_xSTF88iZ";
        string url = "https://iuclcziwfqnhmlmtowku.supabase.co";
        private Supabase.Client client;
        public Form1()
        {
            InitializeComponent();

        }
        [Table("Users")]
        public class Users : BaseModel
        {


            [Column("login")]
            public string Login { get; set; }
            [Column("password")]
            public string Password { get; set; }

        }
        private async void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            var response = await client.From<Users>().Get();
            var table = response.Models;
            bool registred = false;
            foreach (var model in table)
            {
                if (model.Login == login && model.Password == password)
                {
                    Form2 f2 = new Form2();
                    f2.Owner = this;
                    f2.Show();
                    registred = true;
                    break;
                }
            }
            if (registred == false)
            {
                MessageBox.Show("Вы не зарегистрированы", "Ошибка");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new Supabase.Client(url, key);
        }
    }
}
