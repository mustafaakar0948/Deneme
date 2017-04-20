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
    public partial class Giris_Sayfasi : Form
    {
        public Giris_Sayfasi()
        {
            InitializeComponent();
        }

        private void Kayıt_Ol_Click(object sender, EventArgs e)
        {
            Kayıt_ol_sayfasi  sayfa2= new Kayıt_ol_sayfasi();
            sayfa2.ShowDialog();
        }

 
        private void şifremiUnuttumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sifremi_Unuttum_Sayfasi sayfa3 = new Sifremi_Unuttum_Sayfasi();
            sayfa3.ShowDialog();
        }

        private void kayıtOlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Kayıt_ol_sayfasi sayfa2 = new Kayıt_ol_sayfasi();
            sayfa2.ShowDialog();
        }

        private void Giris_yap_Click_1(object sender, EventArgs e)
        {
            string kullaniciad = kullaniciadi.Text;
            string sifre = parola.Text;
            MySqlConnection baglanti = new MySqlConnection("Server=localhost; Database=kullanici_bilgileri; Uid=root;");
            baglanti.Open();

            string sorgu = "select * from kullanici_bilgileri.kullanicilar where kullanici_adi='" + kullaniciad + "' and sifre='" + sifre + "'";
            MySqlCommand command = new MySqlCommand(sorgu, baglanti);
            MySqlDataReader yazdir = command.ExecuteReader();
            int var = 0;
            while (yazdir.Read())
            {
                var = 1;
            }
            baglanti.Close();
            if (var == 0) { MessageBox.Show("Kullanıcı Adı veya Sifre Hatalı Girildi"); }

            else { Goruntulere_Ulasma_Sayfasi sayfayagit = new Goruntulere_Ulasma_Sayfasi(); sayfayagit.ShowDialog(); }

        }

       
    }
}
