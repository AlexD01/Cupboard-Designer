using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Collections;
using System.Globalization;
using System.Windows.Media.Animation;
using Npgsql;
using System.Windows.Threading;
namespace WpfApp13
{
    public partial class SaveCupWIN : Window
    {
        static public void createRequestNonReader(string connectionString, string request)
        {
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(request, npgSqlConnection);
            npgSqlCommand.ExecuteNonQuery();
            npgSqlConnection.Close();
        }
        string connectionString;
        MainWindow win;
        string[] elements;
        public SaveCupWIN(MainWindow win,string[] elements,string cs)
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
            this.win = win;
            this.elements = elements;
            connectionString = cs;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            win.IsEnabled = true;
        }
        string filename="";
        private void SelImgCup_Click(object sender, RoutedEventArgs e)
        {
            Button tmpbt = sender as Button;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Images";
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPG|*.jpg";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                filename = dlg.FileName;
                if (filename.IndexOf(AppDomain.CurrentDomain.BaseDirectory) != -1)
                {
                    filename = filename.Remove(0, filename.IndexOf("Images"));
                    sourseCup.Text = filename;
                }
                else
                {
                    sourseCup.Text = filename;
                }
            }
        }
        private void addCup_Click(object sender, RoutedEventArgs e)
        {
            string name = "";
            string group = "";
            string ingroup = "";
            string sourse = "";          
            try
            {
                name = nameCup.Text;
                group = groupCup.Text;
                ingroup = ingroupCup.Text;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (filename != "") 
            if (filename.IndexOf(AppDomain.CurrentDomain.BaseDirectory) != -1)
            {
                filename = filename.Remove(0, filename.IndexOf("Images"));
                sourse = filename;
            }
            else sourse = filename;
            string tmp = "";
            char ch = '\'';
            for(int i=0;i< elements.Length; i++)
            {
                if(i< elements.Length-1)tmp += ch + elements[i] +ch+ ",";
                else tmp += ch + elements[i] + ch;   
            }
            string request="";
            request = "INSERT INTO public.cupboards (group_, ingroup_, sourseimg,baseelements, name) VALUES ( '" + group + "' , '" + ingroup + "', '" + sourse + "',ARRAY[" + tmp + "],'" + name + "');";        
            createRequestNonReader(connectionString, request);
            this.Close();
        }
    }
}
