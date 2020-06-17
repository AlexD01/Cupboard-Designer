using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;

namespace WpfApp13
{
    public partial class OptionWIN : Window
    {
        string connectionString;
        MainWindow win;
        bool checkConnect = false;
        public OptionWIN()
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
        }
        public OptionWIN(MainWindow win, string cs)
        {
            InitializeComponent();
            this.win = win;
            connectionString = cs;
        }
        static public void createRequestNonReader(string connectionString, string request)
        {
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(request, npgSqlConnection);
            npgSqlCommand.ExecuteNonQuery();
            npgSqlConnection.Close();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (win != null)
                win.IsEnabled = true;
        }
        private void SlG_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte r = Convert.ToByte(SlR.Value);
            byte g = Convert.ToByte(SlG.Value);
            byte b = Convert.ToByte(SlB.Value);
            SlR.Background= new SolidColorBrush(Color.FromRgb(r, g, b));
            SlG.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
            SlB.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(name.Text==""|| pass.Text == "" || adres.Text == "" || port.Text == "" || bd.Text == "" )
            {
                MessageBox.Show("Заполните поля");
                return;
            }  
            if(checkConnect==false)
            {
                MessageBox.Show("Проверьте подключение");
                return;
            }
            FileStream fstream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/Config.txt", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fstream);
            sw.Write("User%"+name.Text+"~");
            sw.Write("Password%" + pass.Text + "~");
            sw.Write("Server%" + adres.Text + "~");
            sw.Write("Port%" + port.Text + "~");
            sw.Write("Database%" + bd.Text + "~");
            if (Grids.IsEnabled == true)
            {
                if (MW.Text != "" && MWB.Text != "")
                {
                    sw.Write("WallMaterial%" + MW.Text + "%" + MW.Tag + "~");
                    sw.Write("WallBMaterial%" + MWB.Text + "%" + MWB.Tag + "~");
                    sw.Write("ColorElement%" + SlR.Value + "%" + SlG.Value + "%" + SlB.Value + "~");
                }
            }
            sw.Write("~");
            sw.Close();
            this.Close();
            win.checkConfig();
        }
        private void CreateConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "Server=" + adres.Text + ";Port=" + port.Text + ";User Id=" + name.Text + ";Password=" + pass.Text + ";";
                string request = "CREATE DATABASE "+bd.Text+" WITH OWNER = postgres ENCODING = 'UTF8' LC_COLLATE = 'Russian_Russia.1251' LC_CTYPE = 'Russian_Russia.1251' TABLESPACE = pg_default CONNECTION LIMIT = -1; ";
                createRequestNonReader(connectionString, request);
                connectionString = "Server=" + adres.Text + ";Port=" + port.Text + ";User Id=" + name.Text + ";Password=" + pass.Text + "; Database="+bd.Text+";";
                request = "CREATE TABLE cupboards ( id serial NOT NULL PRIMARY KEY, name character varying(255)  , baseelements character varying[] , group_ character varying(255)  , ingroup_ character varying(255) , sourseimg character varying(255) )";
                createRequestNonReader(connectionString, request);
                request = "CREATE TABLE materials ( id serial NOT NULL PRIMARY KEY  , group_ character varying(255)  , ingroup_ character varying(255), sourse character varying(255)  , sizes integer[], price double precision, name character varying(255))";
                createRequestNonReader(connectionString, request);
                request = "CREATE TABLE elements ( id serial NOT NULL PRIMARY KEY  , group_ character varying(255)  , ingroup_ character varying(255), sourse character varying(255)  , sourseimg character varying(255), price double precision, name character varying(255) )";
                createRequestNonReader(connectionString, request);
                MessageBox.Show("База данных создана.");
            }
            catch (Exception ee)
            {
                if (ee.Message + "" == "42P04: база данных \""+bd.Text+"\" уже существует") MessageBox.Show("База данных уже существует.");
                else
                if (ee.Message + "" == "Время ожидания операции истекло.") MessageBox.Show("Время ожидания операции истекло.");
                else  MessageBox.Show("Ошибка");
            }
        }
        private void CheckConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "Server=" + adres.Text + ";Port=" + port.Text + ";User Id=" + name.Text + ";Password=" + port.Text + ";Database="+bd.Text+";";
                NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
                npgSqlConnection.Open();
                npgSqlConnection.Close();
                checkConnect=true;
                MessageBox.Show("Подключение успешно.");
            }
            catch (Exception ee)
            {
                string[] temp = ee.Message.Split(' ');
                if (temp[0] == "Время") MessageBox.Show("Время ожидания операции истекло.");
                else if (temp[0] == "3D000:") MessageBox.Show("База данных не существует.");
                else MessageBox.Show("Ошибка");
                return;
            }

        }
        private void SelWM_Click(object sender, RoutedEventArgs e)
        {
            Button tmp = sender as Button;
            MaterialsWIN win = new MaterialsWIN(this, MW, connectionString);
            win.Show();
            this.IsEnabled = false;
        }
        private void SelWBM_Click(object sender, RoutedEventArgs e)
        {
            Button tmp = sender as Button;
            MaterialsWIN win = new MaterialsWIN(this, MWB, connectionString);
            win.Show();
            this.IsEnabled = false;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Config.txt") == true)
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Config.txt");
                string s = sr.ReadToEnd();
                string[] ss = s.Split('~');
                name.Text = ss[0].Split('%')[1];
                pass.Text = ss[1].Split('%')[1];
                adres.Text = ss[2].Split('%')[1];
                port.Text = ss[3].Split('%')[1];
                bd.Text = ss[4].Split('%')[1];
                if (ss.Length >= 9)
                {
                    MW.Text = ss[5].Split('%')[1];
                    MWB.Text = ss[6].Split('%')[1];
                    MW.Tag = ss[5].Split('%')[2];
                    MWB.Tag = ss[6].Split('%')[2];
                    SlR.Value=Convert.ToDouble( ss[7].Split('%')[1]);
                    SlG.Value = Convert.ToDouble(ss[7].Split('%')[2]);
                    SlB.Value = Convert.ToDouble(ss[7].Split('%')[3]);
                }

            }
        }
    }
}
