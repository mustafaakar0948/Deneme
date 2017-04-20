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
    public partial class Kayıt_ol_sayfasi : Form
    {
        public Kayıt_ol_sayfasi()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                string kullaniciadi = textBox8.Text;
                string ad = textBox1.Text;
                string soyad = textBox2.Text;
                string email = textBox3.Text;
                string sifre = textBox4.Text;
                string tekrarsifre=textBox5.Text;
                string soru = comboBox1.Text;
                string cevap = textBox7.Text;
                string güvenliksorusu = textBox6.Text;


            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "" && comboBox1.Text!= "Seçiminiz" && textBox8.Text!="")
            {
                if (textBox4.Text == textBox5.Text)
                {
                    if (textBox6.Text == label1.Text)
                    {

                        MySqlConnection baglanti = new MySqlConnection("Server=localhost; Database=kullanici_bilgileri; Uid=root;");
                        baglanti.Open();

                        string sorgu = "select * from kullanici_bilgileri.kullanicilar where kullanici_adi='" + kullaniciadi + "'";
                        MySqlCommand sorgu1 = new MySqlCommand(sorgu,baglanti);
                        MySqlDataReader yazdir = sorgu1.ExecuteReader();

                        int var = 0;

                        while (yazdir.Read())
                        {
                            var = 1;
                        }
                        baglanti.Close();


                        if (var == 1)
                        {
                            MessageBox.Show("Kullanıcı Adı Daha Önce Kullanılmış");
                        }

                        else if (var == 0)
                        {

                            baglanti.Open();

                            string ekleme = "insert into kullanici_bilgileri.kullanicilar (ad,soyad,email,sifre,soru,cevap,kullanici_adi) values ('" + ad + "','" + soyad + "','" + email + "','" + sifre + "','" + soru + "','" + cevap + "','"+kullaniciadi+"')";

                            MySqlCommand command = new MySqlCommand(ekleme, baglanti);
                            command.ExecuteNonQuery();

                            MessageBox.Show("Kaydınız Başarı İle Alınmıştır");
                            baglanti.Close();
                            this.Close(); //giriş sayfasına yönlendirildi 
                        }
                        

                    }
                    else { MessageBox.Show("Güvenlik Kodunu Dogru Girdiğinizden Emin Olun"); }
                }
                else
                {
                    MessageBox.Show("Girdiğiniz Şifreler Uyuşmuyor");
                }
                
                
            }


            else
            {
                   MessageBox.Show("Alanların Hepsini Doldurduğunuzdan Emin Olun");
            
            }   
                

        }

        private void Kayıt_ol_sayfasi_Load(object sender, EventArgs e)
        {
            string[] dizi = { "a", "b", "c", "r", "t","1","2","3","4","5","6","7","8","9","0","e","y","p","v","n","m","k","d"};
            Random rastgele = new Random();
            label1.Text = "" + dizi[rastgele.Next(10)] + dizi[rastgele.Next(10)] + dizi[rastgele.Next(10)] + dizi[rastgele.Next(10)] + dizi[rastgele.Next(10)]+dizi[rastgele.Next(10)];
            comboBox1.Text = "Seçiminiz";
            comboBox1.Items.Add("İlk Okul Ögretmeninizin Adı");
            comboBox1.Items.Add("Annenizin Kızlık Soyadı");
            comboBox1.Items.Add("En Sevdiğiniz Renk");
            comboBox1.Items.Add("İlk Hayvanınızın Adı");
            comboBox1.Items.Add("Tutuğunuz Takım");
        }
    }
}
