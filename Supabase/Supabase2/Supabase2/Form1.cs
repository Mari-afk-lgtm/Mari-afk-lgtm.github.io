using System.ComponentModel.DataAnnotations.Schema;
using Supabase;
using Supabase.Postgrest.Models;
namespace Supabase2
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
            public string login { get; set; }
            [Column("password")]
            public string password { get; set; }

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
                if (model.login == login && model.password == password)
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

        private async void button2_Click(object sender, EventArgs e)
        {
            string login = textBox3.Text;
            string password = textBox4.Text;
            Users newUser = new Users()
            {
                login = login,
                password = password
            };
            await client.From<Users>().Insert(newUser);
            
           
            
             MessageBox.Show("Вы зарегистрированы", "Успех");
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new Supabase.Client(url, key);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
