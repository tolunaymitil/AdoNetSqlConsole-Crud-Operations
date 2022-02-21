using System;
using System.Data.SqlClient;
using System.Data;
namespace ConsoleSql
{
    class Program
    {
        const string CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Etrade2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static void Main(string[] args)
        {
            Console.WriteLine("1.Ekle");
            Console.WriteLine("2.Listele");
            Console.WriteLine("3.Sil");
            Console.WriteLine("4.Güncelle");
            Console.WriteLine("5.Birtane");
            Console.WriteLine("Seçiminiz:");
            string sec = Console.ReadLine();
            if (sec == "1")
            {

                Insert();
                List();

            }
            else if (sec == "2")
            {
                List();
            }
            else if (sec == "3")
            {
                List();
                Delete();
            }
            else if (sec == "4")
            {
                List();
                Console.WriteLine("Müşteri numarası giriniz");

                string mno = Console.ReadLine();
                PersonelBilgileri currentPersonal = GetById(mno);

                Console.WriteLine("Güncellemek istemediğiniz alanı boş bırakın.");
                Console.WriteLine("Yeni Adsoyad:");
                string newName = Console.ReadLine();
                Console.WriteLine("Telefon:");
                string newPhone = Console.ReadLine();
                Console.WriteLine("Adres:");
                string newAdres = Console.ReadLine();
                Console.WriteLine("Email:");
                string newEmail = Console.ReadLine();

                PersonelBilgileri newPersonal = new PersonelBilgileri
                {
                    adsoyad = newName,
                    telefon = newPhone,
                    adres = newAdres,
                    email = newEmail

                };






                PersonelBilgileri matchedData = MatchData(currentPersonal, newPersonal);

                bool resultUpdate = Update(matchedData);

                if (resultUpdate == true)
                {
                    Console.WriteLine("Başarılı güncelleme");

                }
                else
                {
                    Console.WriteLine("Başarısız Güncelleme");
                }
            }
            else if (sec == "5")
            {
                List();
                Console.WriteLine("Müşteri numarası giriniz");
                string mno = Console.ReadLine();
                GetById(mno);
            }
            Console.ReadLine();
        }

        static void Insert()
        {
            SqlConnection baglanti = new SqlConnection(CONNECTION_STRING);

            Console.WriteLine("Müşteri No:");
            int m_no = int.Parse(Console.ReadLine());
            Console.WriteLine("Adı Soyadı");
            string _adsoyad = Console.ReadLine();
            Console.WriteLine("Telefon");
            string _telefon = Console.ReadLine();
            Console.WriteLine("Adres");
            string _adres = Console.ReadLine();
            Console.WriteLine("E-mail");
            string _email = Console.ReadLine();
            baglanti.Open();

            SqlCommand komut = new SqlCommand("insert into personelbilgileri(mno,adsoyad,telefon,adres,email)" +
                "VALUES('" + m_no + "'  , '" + _adsoyad + "' , '" + _telefon + "', '" + _adres + "' ,'" + _email + "')", baglanti);

            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            Console.WriteLine(sonuc + "Kayıt Eklendi");
            Console.ReadKey();


        }

        static bool Update(PersonelBilgileri personel)
        {

            int sonuc = 0;

            using (SqlConnection baglanti = new SqlConnection(CONNECTION_STRING))
            {
                baglanti.Open();
                string komutMetni = $"update personelbilgileri set adsoyad='{personel.adsoyad}', telefon='{personel.telefon}', adres='{personel.adres}', email='{personel.email}'  where mno={personel.mno}";

                SqlCommand komut = new SqlCommand(komutMetni, baglanti);
                sonuc = komut.ExecuteNonQuery();
                baglanti.Close();
            }

            if (sonuc == 1)
            {
                return true;

            }
            else
            {
                return false;
            }




        }

        static PersonelBilgileri MatchData(PersonelBilgileri current, PersonelBilgileri newPersonal)
        {
            PersonelBilgileri matchedData = current;
            if (string.IsNullOrEmpty(newPersonal.adsoyad) == false)
            {
                current.adsoyad = newPersonal.adsoyad;
            }
            if (string.IsNullOrEmpty(newPersonal.telefon) == false)
            {
                current.telefon = newPersonal.telefon;
            }
            if (string.IsNullOrEmpty(newPersonal.adres) == false)
            {
                current.adres = newPersonal.adres;
            }
            if (string.IsNullOrEmpty(newPersonal.email) == false)
            {
                current.email = newPersonal.email;
            }
            return matchedData;
        }

        static void List()
        {
            SqlConnection baglanti = new SqlConnection(CONNECTION_STRING);
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from personelbilgileri", baglanti);
            SqlDataReader oku = komut.ExecuteReader();
            int kayitsayisi = 0;
            while (oku.Read())
            {
                kayitsayisi++;
                Console.WriteLine(oku[0] + "\t" + oku[1] + "\t" + oku[2] + "\t" + oku[3] + "\t" + oku[4]);
            }
            baglanti.Close();
            Console.WriteLine();
            Console.WriteLine(kayitsayisi + "kayit listelendi");
        }
        static PersonelBilgileri GetById(string mno)
        {
            SqlConnection baglanti = new SqlConnection(CONNECTION_STRING);
            baglanti.Open();
            SqlCommand komut = new SqlCommand($"select top 1 *  from personelbilgileri where mno={mno}", baglanti);
            SqlDataReader oku = komut.ExecuteReader();
            PersonelBilgileri personelBilgileri = new PersonelBilgileri();
            while (oku.Read())
            {


                personelBilgileri.mno = Int32.Parse(oku[0].ToString());
                personelBilgileri.adsoyad = oku[1].ToString();
                personelBilgileri.telefon = oku[2].ToString();
                personelBilgileri.adres = oku[3].ToString();
                personelBilgileri.email = oku[4].ToString();

                Console.WriteLine("------Personel Bilgileri----------");
                Console.WriteLine($"mno:{personelBilgileri.mno} ");
                Console.WriteLine($"adsoyad:{personelBilgileri.adsoyad} ");
                Console.WriteLine($"telefon:{personelBilgileri.telefon} ");
                Console.WriteLine($"adres:{personelBilgileri.adres} ");
                Console.WriteLine($"email:{personelBilgileri.email}");
                Console.WriteLine("------Personel Bilgileri----------");
            }
            baglanti.Close();
            return personelBilgileri;
        }
        static void Delete()
        {
            SqlConnection baglanti = new SqlConnection(CONNECTION_STRING);
            Console.WriteLine("Silinecek No:");
            int m_no = int.Parse(Console.ReadLine());
            int sonuc = 0;
            baglanti.Open();

            SqlCommand komut = new SqlCommand("delete from personelbilgileri where mno='" + m_no + "'", baglanti);
            sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            Console.WriteLine(sonuc + "kayit silindi");

        }
    }
    public class PersonelBilgileri
    {
        public int mno { get; set; }
        public string adsoyad { get; set; }
        public string telefon { get; set; }
        public string adres { get; set; }
        public string email { get; set; }
    }
}

