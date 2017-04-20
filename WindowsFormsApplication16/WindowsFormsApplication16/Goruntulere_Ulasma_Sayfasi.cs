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
using System.IO;
namespace WindowsFormsApplication16
{
    public partial class Goruntulere_Ulasma_Sayfasi : Form
    {
        int i = 0;
        MySqlConnection baglanti = new MySqlConnection("Server=localhost; Database=kullanici_bilgileri; Uid=root;");
        public Goruntulere_Ulasma_Sayfasi()
        {
          
            InitializeComponent();
        }

        ListBox bilgisayaradi = new ListBox();
        ListBox saat = new ListBox();
        ListBox tarihi = new ListBox();
        ListBox ip_adresi = new ListBox();
        ListBox mac_adresi = new ListBox();
        ListBox resim = new ListBox();
        ListBox sehir = new ListBox();
        ListBox baglantisahibi = new ListBox();

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Text = "İlk Kaydı Göster->";
            button2.Enabled = true;
            //listBox1.Items.Clear();
            bilgisayaradi.Items.Clear();
            saat.Items.Clear();
            tarihi.Items.Clear();
            ip_adresi.Items.Clear();
            mac_adresi.Items.Clear();
            resim.Items.Clear();
            sehir.Items.Clear();
            baglantisahibi.Items.Clear();

            string tarih = dateTimePicker1.Text;
            baglanti.Open();
            i = 0;
            string sorgu="select * from kullanici_bilgileri.resimler where tarih='"+tarih+"'";
            MySqlCommand emir = new MySqlCommand(sorgu,baglanti);
            MySqlDataReader yazdir = emir.ExecuteReader();
            
            while (yazdir.Read())
            {
                //listBox1.Items.Add(yazdir["tarih"]+"\t"+yazdir["saat"]);
                resim.Items.Add(yazdir["resim"]);
                tarihi.Items.Add(yazdir["tarih"]);
                bilgisayaradi.Items.Add(yazdir["bilgisayar_adi"]);
                saat.Items.Add(yazdir["saat"]);
                ip_adresi.Items.Add(yazdir["ip_adresi"]);
                mac_adresi.Items.Add(yazdir["mac_adresi"]);
                sehir.Items.Add(yazdir["sehir"]);
                baglantisahibi.Items.Add(yazdir["interneti_sahibi"]);
                
               
            }
            
            baglanti.Close();
            label9.Text = "Bugüne ait " + resim.Items.Count + " adet kayıt bulundu";
            if (resim.Items.Count == 0) { button2.Enabled = false; MessageBox.Show("Bugune Ait Hiç Kayıt Bulunamadı");  }
        }

        void button2_Click(object sender, EventArgs e)
        {
            button2.Text = "Sonraki Kayıt->";
            if (i == resim.Items.Count) { button2.Enabled = false; MessageBox.Show("Bugune ait kayıtlar sona erdi."); }
            
            else
            {

            label2.Text = tarihi.Items[i].ToString();
            label3.Text = saat.Items[i].ToString();
            label4.Text = bilgisayaradi.Items[i].ToString();
            label5.Text = ip_adresi.Items[i].ToString();
            label6.Text = mac_adresi.Items[i].ToString();
            label7.Text = sehir.Items[i].ToString();
            label8.Text = baglantisahibi.Items[i].ToString();

            byte[] img1 = (byte[])(resim.Items[i]);
            MemoryStream ms1 = new MemoryStream(img1);
            pictureBox1.Image = Image.FromStream(ms1);
            i = i + 1;

            }
           
            
        }

        private void Goruntulere_Ulasma_Sayfasi_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }
    }
}
