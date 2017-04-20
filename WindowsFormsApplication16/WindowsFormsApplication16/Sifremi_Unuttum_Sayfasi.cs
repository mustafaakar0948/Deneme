using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace WindowsFormsApplication16
{
    public partial class Sifremi_Unuttum_Sayfasi : Form
    {
        public Sifremi_Unuttum_Sayfasi()
        {
            InitializeComponent();
        }

        private void Sifremi_Unuttum_Sayfasi_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "Seçiminiz";
            comboBox1.Items.Add("İlk Okul Ögretmeninizin Adı");
            comboBox1.Items.Add("Annenizin Kızlık Soyadı");
            comboBox1.Items.Add("En Sevdiğiniz Renk");
            comboBox1.Items.Add("İlk Hayvanınızın Adı");
            comboBox1.Items.Add("Tutugunuz Takım");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciadi = textBox2.Text;
            string cevap = textBox1.Text;
            string soru = comboBox1.Text;
            string email = textBox3.Text;

            if (textBox1.Text!=""&&textBox2.Text!=""&&textBox3.Text!=""&&comboBox1.Text!= "Seçiminiz")
            {
                MySqlConnection baglanti = new MySqlConnection("Server=localhost; Database=kullanici_bilgileri; Uid=root;");
                baglanti.Open();

                string sorgu = "select * from kullanici_bilgileri.kullanicilar where kullanici_adi='"+kullaniciadi+"' and email='"+email+"' and soru='"+soru+"' and cevap='"+cevap+"'";

                MySqlCommand command = new MySqlCommand(sorgu,baglanti);
                MySqlDataReader yazdir = command.ExecuteReader();
                int var = 0;
                while (yazdir.Read())
                {

                    var = 1;
                    
                }

                if (var == 0) { MessageBox.Show("Böyle Bir Kullanıcı Yok..."); }
                else
                {
                    MessageBox.Show(""+yazdir["sifre"]);
                    baglanti.Close();
                }
                    
            }
            else
            {
                MessageBox.Show("Lütfen Alanları Eksiksiz Doldurunuz");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}