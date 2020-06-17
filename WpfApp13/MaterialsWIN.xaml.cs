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
using System.IO;

namespace WpfApp13
{
    public partial class MaterialsWIN : Window
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
        public MaterialsWIN()
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
        }
        MainWindow win;
        CupboardsWIN win1;
        OptionWIN win2;
        Button tmpbt;
        TextBlock tmptb;
        TextBlock tmptb1;
        public MaterialsWIN(MainWindow win, string cs)
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
            this.win = win;
            connectionString = cs;
        }
        public MaterialsWIN(MainWindow win,Button tmpbt,string cs)
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
            this.tmpbt = tmpbt;
            this.win = win;
            connectionString = cs;
        }
        public MaterialsWIN(CupboardsWIN win1, TextBlock tmptb,string cs)
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
            this.tmptb = tmptb;
            this.win1 = win1;
            connectionString = cs;
        }
        public MaterialsWIN(OptionWIN win2, TextBlock tmptb,string cs)
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
            this.tmptb1 = tmptb;
            this.win2 = win2;
            connectionString = cs;
        }
        ArrayList items = new ArrayList();
        ArrayList filters = new ArrayList();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string request = "Select * from materials";
            UpdateWin(request);
            FileInfo[] sourses=new FileInfo[ItemCanvas.Children.Count];
            for(int i = 0; i < ItemCanvas.Children.Count; i++)
            {
                Canvas cvs = ItemCanvas.Children[i] as Canvas;
                Border brd = cvs.Children[0] as Border;
                Image pict = brd.Child as Image;
                BitmapImage bmi = pict.Source as BitmapImage;
                Uri ur = bmi.UriSource;
                sourses[i] = new FileInfo(ur.LocalPath);
            }
            DirectoryInfo dr = new DirectoryInfo("Images//MaterialImg");
            if (!Directory.Exists("Images//MaterialImg")) dr.Create();
            FileInfo[] fn = dr.GetFiles();
            try {
                for (int j = 0; j < fn.Length; j++)
                {
                    bool b = false;
                    int ind = 0;
                    for (int i = 0; i < sourses.Length; i++)
                    {
                        if (sourses[i].FullName != fn[j].FullName) { b = true; ind = j; }
                        else { b = false; break; }
                    }
                    if (b == true) fn[ind].Delete();
                }
            }catch( Exception ee)
            {
            }
            DirectoryInfo dr1 = new DirectoryInfo("Images//ElementImg");
            if (!Directory.Exists("Images//ElementImg")) dr1.Create();
            FileInfo[] fn1 = dr1.GetFiles();
            try
            {
                for (int j = 0; j < fn1.Length; j++)
                {

                    bool b = false;
                    int ind = 0;
                    for (int i = 0; i < sourses.Length; i++)
                    {
                        if (sourses[i].FullName != fn1[j].FullName) { b = true; ind = j; }
                        else { b = false; break; }
                    }
                    if (b == true) fn1[ind].Delete();
                }
            }
            catch (Exception ee)
            {
            }
        }
        public void LoadMaterials(string req)
        {
            ItemCanvas.Children.Clear();
            items.Clear();
            string request = req;
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(request, npgSqlConnection);
            NpgsqlDataReader reader = npgSqlCommand.ExecuteReader();
            int countMat = 0;
            while (reader.Read())
            {
                int var = reader.GetInt32(0);
                countMat++;
            }
            int countV = countMat / 4;
            int countG = countMat - countV * 4;
            reader.Close();
            npgSqlConnection.Close();
            npgSqlConnection.Open();
            npgSqlCommand = new NpgsqlCommand(request, npgSqlConnection);
            reader = npgSqlCommand.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < countV; i++)
                {
                    double nach = 10;
                    if (ItemCanvas.Children.Count == 0) nach = 10;
                    else
                    {
                        nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 110;
                        if (double.IsNaN(nach)) { nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 110; }
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        int id = reader.GetInt32(0);
                        string group = reader.GetString(1);
                        string ingroup = reader.GetString(2);
                        string sourse = reader.GetString(3);
                        int[] sizes = reader[4] as int[];
                        double price = reader.GetDouble(5);
                        string name = reader.GetString(6);
                        items.Add(id + "|" + group + "|" + ingroup + "|" + sourse + "|" + sizes[0] + "|" + sizes[1] + "|" + sizes[2] + "|" + price + "|" + name + "|");
                        if (filters.IndexOf(group + "|" + ingroup) == -1) filters.Add(group + "|" + ingroup);
                        Canvas thisItem = new Canvas
                        {
                            Width = ItemCanvas.ActualWidth / 4 - 10 * 4,
                            Height = 100,
                            Background = Brushes.Bisque,
                            Tag = id
                        };
                        ItemCanvas.Children.Add(thisItem);
                        thisItem.MouseUp += elementCanvas_MouseUp_1;
                        Canvas.SetTop(thisItem, nach);
                        if (j == 0) Canvas.SetLeft(thisItem, (ItemCanvas.ActualWidth / 8 + ItemCanvas.ActualWidth / 4) / 2 - thisItem.Width / 2);
                        if (j == 1) Canvas.SetLeft(thisItem, (ItemCanvas.ActualWidth / 4 + ItemCanvas.ActualWidth / 2) / 2 - thisItem.Width / 2 + 15);
                        if (j == 2) Canvas.SetLeft(thisItem, ItemCanvas.ActualWidth - (ItemCanvas.ActualWidth / 4 + ItemCanvas.ActualWidth / 2) / 2 - thisItem.Width / 2 - 15);
                        if (j == 3) Canvas.SetLeft(thisItem, ItemCanvas.ActualWidth - (ItemCanvas.ActualWidth / 8 + ItemCanvas.ActualWidth / 4) / 2 - thisItem.Width / 2);
                        Border imgMat = new Border
                        {
                            Height = 60,
                            Width = thisItem.Width - 20,
                            BorderThickness = new Thickness(2),
                            BorderBrush = Brushes.Black
                        };
                        Image img = new Image
                        {
                            Stretch = Stretch.Fill
                        };
                        if (sourse != "")
                        {
                            var uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + sourse);
                            var bitmap = new BitmapImage(uri);
                            img.Source = bitmap;
                        }
                        imgMat.Child = img;
                        Canvas.SetTop(imgMat, 0);
                        Canvas.SetLeft(imgMat, 10);
                        thisItem.Children.Add(imgMat);
                        TextBlock namemat = new TextBlock
                        {
                            Text = name,
                            FontSize = 14,
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };
                        namemat.Height = 50;
                        namemat.Width = imgMat.Width;
                        double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                        if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                        Canvas.SetTop(namemat, nach1 + 60);
                        Canvas.SetLeft(namemat, thisItem.Width / 2 - namemat.Width / 2);
                        thisItem.Children.Add(namemat);
                        reader.Read();
                    }
                }
                double nach2 = 10;
                if (ItemCanvas.Children.Count == 0) nach2 = 10;
                else
                {
                    nach2 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 110;
                    if (double.IsNaN(nach2)) { nach2 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 110; }
                }
                for (int j = 0; j < countG; j++)
                {
                    int id = reader.GetInt32(0);
                    string group = reader.GetString(1);
                    string ingroup = reader.GetString(2);
                    string sourse = reader.GetString(3);
                    int[] sizes = reader[4] as int[];
                    double price = reader.GetDouble(5);
                    string name = reader.GetString(6);
                    items.Add(id + "|" + group + "|" + ingroup + "|" + sourse + "|" + sizes[0] + "|" + sizes[1] + "|" + sizes[2] + "|" + price + "|" + name + "|");
                    if (filters.IndexOf(group + "|" + ingroup) == -1) filters.Add(group + "|" + ingroup);
                    Canvas thisItem = new Canvas
                    {
                        Width = ItemCanvas.ActualWidth / 4 - 10 * 4,
                        Height = 100,
                        Background = Brushes.Bisque,
                        Tag = id
                    };
                    ItemCanvas.Children.Add(thisItem);
                    thisItem.MouseUp += elementCanvas_MouseUp_1;
                    Canvas.SetTop(thisItem, nach2);
                    if (j == 0) Canvas.SetLeft(thisItem, (ItemCanvas.ActualWidth / 8 + ItemCanvas.ActualWidth / 4) / 2 - thisItem.Width / 2);
                    if (j == 1) Canvas.SetLeft(thisItem, (ItemCanvas.ActualWidth / 4 + ItemCanvas.ActualWidth / 2) / 2 - thisItem.Width / 2 + 15);
                    if (j == 2) Canvas.SetLeft(thisItem, ItemCanvas.ActualWidth - (ItemCanvas.ActualWidth / 4 + ItemCanvas.ActualWidth / 2) / 2 - thisItem.Width / 2 - 15);
                    Border imgMat = new Border
                    {
                        Height = 60,
                        Width = thisItem.Width - 20,
                        BorderThickness = new Thickness(2),
                        BorderBrush = Brushes.Black
                    };
                    Image img = new Image
                    {
                        Stretch = Stretch.Fill
                    };
                    if (sourse != "")
                    {
                        var uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + sourse);
                        var bitmap = new BitmapImage(uri);
                        img.Source = bitmap;
                    }
                    imgMat.Child = img;
                    Canvas.SetTop(imgMat, 0);
                    Canvas.SetLeft(imgMat, 10);
                    thisItem.Children.Add(imgMat);
                    TextBlock namemat = new TextBlock
                    {
                        Text = name,
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    namemat.Height = 50; 
                    namemat.Width = imgMat.Width;
                    double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                    if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                    Canvas.SetTop(namemat, nach1 + 60);
                    Canvas.SetLeft(namemat, thisItem.Width / 2 - namemat.Width / 2);
                    thisItem.Children.Add(namemat);
                    reader.Read();
                }
            }
            double nach3 = 10;
            if (ItemCanvas.Children.Count == 0) nach3 = 10;
            if (ItemCanvas.Height <= 380) ItemCanvas.Height = 380;
            else
            {
                nach3 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 110;
                if (double.IsNaN(nach3)) { nach3 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 110; }
            }
            if (nach3 > ItemCanvas.Height) ItemCanvas.Height = nach3;
        }
        public void UpdateWin(string req)
        {
            ItemCanvas.Children.Clear();
            Filters.Items.Clear();
            items.Clear();
            filters.Clear();
            string request = req;
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(request, npgSqlConnection);
            NpgsqlDataReader reader = npgSqlCommand.ExecuteReader();
            int countMat = 0;
            while (reader.Read())
            {
                int var = reader.GetInt32(0);
                countMat++;
            }
            int countV = countMat / 4;
            int countG = countMat - countV * 4;
            reader.Close();
            npgSqlConnection.Close();
            npgSqlConnection.Open();
            npgSqlCommand = new NpgsqlCommand(request, npgSqlConnection);
            reader = npgSqlCommand.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < countV; i++)
                {
                    double nach = 10;
                    if (ItemCanvas.Children.Count == 0) nach = 10;
                    else
                    {
                        nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 110;
                        if (double.IsNaN(nach)) { nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 110; }
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        int id = reader.GetInt32(0);
                        string group = reader.GetString(1);
                        string ingroup = reader.GetString(2);
                        string sourse = reader.GetString(3);
                        int[] sizes = reader[4] as int[];
                        double price = reader.GetDouble(5);
                        string name = reader.GetString(6);
                        items.Add(id + "|" + group + "|" + ingroup + "|" + sourse + "|" + sizes[0] + "|" + sizes[1] + "|" + sizes[2] + "|" + price + "|" + name + "|");
                        if (filters.IndexOf(group + "|" + ingroup) == -1) filters.Add(group + "|" + ingroup);
                        Canvas thisItem = new Canvas
                        {
                            Width = ItemCanvas.ActualWidth / 4 - 10 * 4,
                            Height = 100,
                            Background = Brushes.Bisque,
                            Tag=id
                        };
                        ItemCanvas.Children.Add(thisItem);
                        thisItem.MouseUp += elementCanvas_MouseUp_1;
                        Canvas.SetTop(thisItem, nach);
                        if (j == 0) Canvas.SetLeft(thisItem, (ItemCanvas.ActualWidth / 8 + ItemCanvas.ActualWidth / 4) / 2 - thisItem.Width / 2);
                        if (j == 1) Canvas.SetLeft(thisItem, (ItemCanvas.ActualWidth / 4 + ItemCanvas.ActualWidth / 2) / 2 - thisItem.Width / 2 + 15);
                        if (j == 2) Canvas.SetLeft(thisItem, ItemCanvas.ActualWidth - (ItemCanvas.ActualWidth / 4 + ItemCanvas.ActualWidth / 2) / 2 - thisItem.Width / 2 - 15);
                        if (j == 3) Canvas.SetLeft(thisItem, ItemCanvas.ActualWidth - (ItemCanvas.ActualWidth / 8 + ItemCanvas.ActualWidth / 4) / 2 - thisItem.Width / 2);
                        Border imgMat = new Border
                        {
                            Height = 60,
                            Width = thisItem.Width - 20,
                            BorderThickness = new Thickness(2),
                            BorderBrush = Brushes.Black
                        };
                        Image img = new Image
                        {
                            Stretch = Stretch.Fill
                        };
                        if (sourse != "") { 
                        var uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + sourse);
                        var bitmap = new BitmapImage(uri);
                        img.Source = bitmap;}
                        imgMat.Child = img;
                        Canvas.SetTop(imgMat, 0);
                        Canvas.SetLeft(imgMat, 10);
                        thisItem.Children.Add(imgMat);
                        TextBlock namemat = new TextBlock
                        {
                            Text = name,
                            FontSize = 14,
                            TextWrapping=TextWrapping.Wrap,
                            HorizontalAlignment=HorizontalAlignment.Center
                        };                     
                        namemat.Height = 50; 
                        namemat.Width = imgMat.Width;
                        double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                        if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                        Canvas.SetTop(namemat, nach1 + 60);
                        Canvas.SetLeft(namemat, thisItem.Width / 2 - namemat.Width / 2);
                        thisItem.Children.Add(namemat);
                        reader.Read();
                    }
                }
                double nach2 = 10;
                if (ItemCanvas.Children.Count == 0) nach2 = 10;
                else
                {
                    nach2 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 110;
                    if (double.IsNaN(nach2)) { nach2 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 110; }
                }
                for (int j = 0; j < countG; j++)
                {
                    int id = reader.GetInt32(0);
                    string group = reader.GetString(1);
                    string ingroup = reader.GetString(2);
                    string sourse = reader.GetString(3);
                    int[] sizes = reader[4] as int[];
                    double price = reader.GetDouble(5);
                    string name = reader.GetString(6);
                    items.Add(id + "|" + group + "|" + ingroup + "|" + sourse + "|" + sizes[0] + "|" + sizes[1] + "|" + sizes[2] + "|" + price + "|" + name + "|");
                    if (filters.IndexOf(group + "|" + ingroup) == -1) filters.Add(group + "|" + ingroup);
                    Canvas thisItem = new Canvas
                    {
                        Width = ItemCanvas.ActualWidth / 4 - 10 * 4,
                        Height = 100,
                        Background = Brushes.Bisque,
                        Tag = id
                    };
                    ItemCanvas.Children.Add(thisItem);
                    thisItem.MouseUp += elementCanvas_MouseUp_1;
                    Canvas.SetTop(thisItem, nach2);
                    if (j == 0) Canvas.SetLeft(thisItem, (ItemCanvas.ActualWidth / 8 + ItemCanvas.ActualWidth / 4) / 2 - thisItem.Width / 2);
                    if (j == 1) Canvas.SetLeft(thisItem, (ItemCanvas.ActualWidth / 4 + ItemCanvas.ActualWidth / 2) / 2 - thisItem.Width / 2 + 15);
                    if (j == 2) Canvas.SetLeft(thisItem, ItemCanvas.ActualWidth - (ItemCanvas.ActualWidth / 4 + ItemCanvas.ActualWidth / 2) / 2 - thisItem.Width / 2 - 15);
                    Border imgMat = new Border
                    {
                        Height = 60,
                        Width = thisItem.Width - 20,
                        BorderThickness = new Thickness(2),
                        BorderBrush = Brushes.Black
                    };
                    Image img = new Image
                    {
                        Stretch = Stretch.Fill
                    };
                    if (sourse != "")
                    {
                        var uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + sourse);
                        var bitmap = new BitmapImage(uri);
                        img.Source = bitmap;
                        imgMat.Child = img;
                    }
                    Canvas.SetTop(imgMat, 0);
                    Canvas.SetLeft(imgMat, 10);
                    thisItem.Children.Add(imgMat);
                    TextBlock namemat = new TextBlock
                    {
                        Text = name,
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    namemat.Height = 50; ;
                    namemat.Width = imgMat.Width;
                    double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                    if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                    Canvas.SetTop(namemat, nach1 + 60);
                    Canvas.SetLeft(namemat, thisItem.Width / 2 - namemat.Width / 2);
                    thisItem.Children.Add(namemat);
                    reader.Read();
                }
            }
            double nach3 = 10;
            if (ItemCanvas.Children.Count == 0) nach3 = 10;
            else
            {
                nach3 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 110;
                if (double.IsNaN(nach3)) { nach3 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 110; }
            }
            if(nach3>ItemCanvas.Height) ItemCanvas.Height = nach3;
            if (ItemCanvas.Height <= 380) ItemCanvas.Height = 380;

            for (int i = 0; i < filters.Count; i++)
            {
                string str = filters[i] as string;
                string[] str1 = str.Split('|');
                bool nal = false;
                for (int j = 0; j < Filters.Items.Count; j++)
                {
                    CheckBox tmpcb = Filters.Items[j] as CheckBox;
                    if (tmpcb.Tag.ToString() == str1[0])
                    {
                        nal = true;
                        bool naling = false;
                        TreeViewItem tmptree = tmpcb.Content as TreeViewItem;
                        for (int j1 = 0; j1 < tmptree.Items.Count; j1++)
                        {
                            CheckBox tmpcb1 = tmptree.Items[j1] as CheckBox;
                            if (tmpcb.Tag.ToString() == str1[1])
                            {
                                naling = true;
                            }
                        }
                        if (naling == false)
                        {
                            CheckBox head = new CheckBox();
                            head.Tag = str;
                            head.Content = str1[1];
                            head.Checked += _ClickCB;
                            head.Unchecked += _ClickCB;
                            tmptree.Items.Add(head);
                        }
                    }
                }
                if (nal == false)
                {
                    CheckBox head = new CheckBox();
                    head.Tag = str1[0];
                    Filters.Items.Add(head);
                    head.Checked += _ClickCB;
                    head.Unchecked += _ClickCB;
                    TreeViewItem treehead = new TreeViewItem();
                    treehead.Tag = str1[0];
                    treehead.Header = str1[0];
                    head.Content = treehead;
                    CheckBox head1 = new CheckBox();
                    head1.Tag = str;
                    head1.Content = str1[1];
                    head1.Checked += _ClickCB;
                    head1.Unchecked += _ClickCB;
                    treehead.Items.Add(head1);

                }
            }
        }
        public void _PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tbne = sender as TextBox;
            if ((!Char.IsDigit(e.Text, 0)) && (e.Text != ","))
            {
                { e.Handled = true; }
            }
            else
                if (((e.Text == ",") && ((tbne.Text.IndexOf(",") != -1) || (tbne.Text == ""))))
            { e.Handled = true; }
        }
        private void _ClickCB(object sender, RoutedEventArgs e)
        {
            CheckBox tmpcb = sender as CheckBox;
            if (tmpcb.IsChecked == false)
                for (int i = 0; i < filters.Count; i++)
                {
                    string str = filters[i] as string;
                    string[] str1 = str.Split('|');
                    if (tmpcb.Tag.ToString() == str1[0])
                    {
                        TreeViewItem tmptree = tmpcb.Content as TreeViewItem;
                        for (int j = 0; j < tmptree.Items.Count; j++)
                        {
                            CheckBox tmpcb1 = tmptree.Items[j] as CheckBox;
                            if (tmpcb1.IsChecked == true) tmpcb1.IsChecked = false;
                        }
                    }
                }
            if(tmpcb.IsChecked==true)
                for (int i = 0; i < filters.Count; i++)
                {
                    string str = filters[i] as string;
                    string[] str1 = str.Split('|');
                    if (tmpcb.Tag.ToString() == str)
                    {
                        for (int j = 0; j < Filters.Items.Count; j++)
                        {
                            CheckBox tmpcb1 = Filters.Items[j] as CheckBox;
                            if (tmpcb1.Tag.ToString() == str1[0])
                            {
                                if (tmpcb1.IsChecked == false) tmpcb1.IsChecked = true;
                            }
                        }
                    }
                }
        }
        private void UseFilter_Click(object sender, RoutedEventArgs e)
        {
            string request = "Select * from materials";
            ArrayList lists = new ArrayList();
            for (int j = 0; j < Filters.Items.Count; j++)
            {
                CheckBox tmpcb1 = Filters.Items[j] as CheckBox;
                if (tmpcb1.IsChecked == true)
                {
                    bool nal = false;
                    TreeViewItem tmptree = tmpcb1.Content as TreeViewItem;
                    for (int i = 0; i < tmptree.Items.Count; i++)
                    {
                        CheckBox tmpcb2 = tmptree.Items[i] as CheckBox;
                        if (tmpcb2.IsChecked == true) { lists.Add(tmpcb2.Tag); nal = true; }
                    }
                    if(nal==false) lists.Add(tmpcb1.Tag);
                }
            }
            if (lists.Count != 0)
            {
                request = "Select * from materials where ";
                for(int i = 0; i < lists.Count; i++)
                {
                    string str = lists[i] as string;
                    string[] str1 = str.Split('|');
                    if (str1.Length == 1) request += "( group_='" + str + "' )";
                    if (str1.Length == 2) request += "( group_='" + str1[0] + "' and ingroup_='"+ str1[1]+"' )";
                    if (i< lists.Count-1) request += "or";
                }
            }
            LoadMaterials(request);
        }
        Canvas selectCVS;
        int selectid;
        String sourseselect;
        public void elementCanvas_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Editmat.IsEnabled = true;
            Delmat.IsEnabled = true;
            if(tmpbt != null)SelMat.Visibility = Visibility.Visible;
            if (tmptb != null) SelMat1.Visibility = Visibility.Visible;
            if (tmptb1 != null) SelMat1.Visibility = Visibility.Visible;
            selectCVS = sender as Canvas;
            selectCVS.Background = Brushes.Beige;
            Canvas.SetLeft(ShowWin, 0);
            selectid = int.Parse(selectCVS.Tag.ToString());
            for(int i=0;i< items.Count; i++)
            {
                string str = items[i].ToString();
                string[] str1 = str.Split('|');
                if (int.Parse(str1[0]) == selectid) {
                    groupMatS.Text = str1[1]; ingroupMatS.Text = str1[2]; sourseMatS.Text = str1[3]; lenMatS.Text = str1[4]; shirMatS.Text = str1[5]; thicMatS.Text = str1[6]; priceMatS.Text = str1[7]; nameMatS.Text = str1[8];
                    groupMatE.Text = str1[1]; ingroupMatE.Text = str1[2]; sourseMatEE.Text = str1[3]; lenMatE.Text = str1[4]; shirMatE.Text = str1[5]; thicMatE.Text = str1[6]; priceMatE.Text = str1[7]; nameMatE.Text = str1[8];
                    sourseselect = str1[3];
                }
            }
        }
        private void EditCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas.SetLeft(ShowWin, -210);
            Editmat.IsEnabled = false;
            Delmat.IsEnabled = false;
            SelMat.Visibility = Visibility.Hidden;
            if (selectCVS == null) return;
            selectCVS.Background = Brushes.Bisque;
            selectCVS = null;    
        }
        DispatcherTimer timer = new DispatcherTimer();
        bool naprtimer = true;
        DispatcherTimer timer1 = new DispatcherTimer();
        bool naprtimer1 = true;
        private void OnTimerTick(object sender, object e)
        {
            double i = Canvas.GetLeft(AddWin);
            if (naprtimer) i += 2;
            if (!naprtimer) i -= 2;
            if (i >= -2 && naprtimer) { timer.Stop(); naprtimer = false; }
            if (i <= -210 && !naprtimer) { timer.Stop(); naprtimer = true; }
            Canvas.SetLeft(AddWin, i);
        }
        private void OnTimerTick1(object sender, object e)
        {
            double i = Canvas.GetLeft(EditWin);
            if (naprtimer1) i += 2;
            if (!naprtimer1) i -= 2;
            if (i >= -2 && naprtimer1) { timer1.Stop(); naprtimer1 = false; }
            if (i <= -210 && !naprtimer1) { timer1.Stop(); naprtimer1 = true; }
            Canvas.SetLeft(EditWin, i);
        }
        private void Addmat_Click(object sender, RoutedEventArgs e)
        {
            filename = "";
            if (timer.IsEnabled)
            {
                timer.Stop();
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(1);
                timer.Tick += OnTimerTick;
                timer.Start();
                if (naprtimer) naprtimer = false;
                if (!naprtimer) naprtimer = true;
                return;
            }
            if (!timer.IsEnabled)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(1);
                timer.Tick += OnTimerTick;
                timer.Start();
                return;
            }
            

        }
        private void Editmat_Click(object sender, RoutedEventArgs e)
        {
            filename = "";
            if (timer1.IsEnabled)
            {
                timer1.Stop();
                timer1 = new DispatcherTimer();
                timer1.Interval = TimeSpan.FromMilliseconds(1);
                timer1.Tick += OnTimerTick1;
                timer1.Start();
                if (naprtimer1) naprtimer1 = false;
                if (!naprtimer1) naprtimer1 = true;
                return;
            }
            if (!timer1.IsEnabled)
            {
                timer1 = new DispatcherTimer();
                timer1.Interval = TimeSpan.FromMilliseconds(1);
                timer1.Tick += OnTimerTick1;
                timer1.Start();
                return;
            }
        }
        string filename = "";
        private void addMaterial_Click(object sender, RoutedEventArgs e)
        {
            string name="";
            string group = "";
            string ingroup = "";
            string sourse = "";
            double len=0;
            double shir=0;
            double thic = 0;
            double price = 0;
            try
            {
                name = nameMat.Text;
                group = groupMat.Text;
                ingroup = ingroupMat.Text;     
                len = double.Parse(lenMat.Text);
                shir = double.Parse(shirMat.Text);
                thic = double.Parse(thicMat.Text);
                price = double.Parse(priceMat.Text);
            }
            catch(Exception exp)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (filename == "") { MessageBox.Show("Выберите материал"); return; }
            FileInfo fn = new FileInfo(filename);          
            int ind = 0;
            string nameE = fn.Name;
            string nameEE = ind + nameE;
            FileInfo fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Images//MaterialImg//" + ind + nameE);
            while (fntest.Exists)
            {
                nameEE = ind + nameE;
                fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Images//MaterialImg//" + nameEE);
                ind++;
            }         
            fn.CopyTo(AppDomain.CurrentDomain.BaseDirectory+ "Images//MaterialImg//" + nameEE, true);           
            sourse = "Images//MaterialImg//"+ nameEE; 
            string request = "INSERT INTO public.materials (group_, ingroup_, sourse, sizes, price,name) VALUES ( '"+group+ "' , '" + ingroup + "', '" + sourse + "','{" + len+","+shir + "," + thic + "}', " + price + ",'" + name + "');";
            createRequestNonReader(connectionString, request);
            request = "Select * from materials";
            UpdateWin(request);
        }       
        private void sourseMat_Click(object sender, RoutedEventArgs e)
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
            }       
                sourseMatEE.Text = filename;
                sourseMatt.Text = filename;
        }
        private void editMaterial_Click(object sender, RoutedEventArgs e)
        {
            string name = "";
            string group = "";
            string ingroup = "";
            string sourse = "";
            double len = 0;
            double shir = 0;
            double thic = 0;
            double price = 0;
            try
            {
                name = nameMatE.Text;
                group = groupMatE.Text;
                ingroup = ingroupMatE.Text;
                len = double.Parse(lenMatE.Text);
                shir = double.Parse(shirMatE.Text);
                thic = double.Parse(thicMatE.Text);
                price = double.Parse(priceMatE.Text);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (sourseMatEE.Text != "") {
                FileInfo fn = new FileInfo(sourseMatEE.Text);
                int ind = 0;
                string nameE = fn.Name;
                string nameEE = ind + nameE;
                FileInfo fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Images//MaterialImg//" + ind + nameE);
                while (fntest.Exists)
                {
                    nameEE = ind + nameE;
                    fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Images//MaterialImg//" + nameEE);
                    ind++;
                }
                fn.CopyTo(AppDomain.CurrentDomain.BaseDirectory + "Images//MaterialImg//" + nameEE, true);
                sourse = "Images//MaterialImg//" + nameEE;
                fn = null;
            }
            string request = "UPDATE public.materials SET group_ = '" + group + "', ingroup_ = '" + ingroup + "', sourse = '" + sourse + "', sizes = '{" + len + "," + shir + "," + thic + "}', price = '" + price + "', name ='" + name + "' where id = " + selectid;
            createRequestNonReader(connectionString, request);
            request = "Select * from materials";
            UpdateWin(request);
            Canvas.SetLeft(ShowWin, -210);
            EditCanvas_MouseDown(sender, e as MouseButtonEventArgs);
            naprtimer1 = false;
            Editmat_Click(sender, e);
        }
        private void Delmat_Click(object sender, RoutedEventArgs e)
        {
            string request = "DELETE FROM public.materials WHERE id="+selectid;
            createRequestNonReader(connectionString, request);
            request = "Select * from materials";
            UpdateWin(request);
            EditCanvas_MouseDown(sender, e as MouseButtonEventArgs);
            naprtimer1 = false;
            Editmat_Click(sender, e);          
        }
        private void SelMat_Click(object sender, RoutedEventArgs e)
        {
            win.IsEnabled = true;
            win.ChangeTexture_(tmpbt, sourseselect,selectid);
            this.Close();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (win != null)
            win.IsEnabled = true;
            if (win1 != null)
                win1.IsEnabled = true;
            if (win2 != null)
                win2.IsEnabled = true;
        }
        private void SelMat1_Click(object sender, RoutedEventArgs e)
        {
            if (win1 != null) { 
                win1.IsEnabled = true;
            tmptb.Text = sourseselect;
                tmptb.Tag = selectid;
            }
            if (win2 != null)
            {
                win2.IsEnabled = true;
                tmptb1.Text = sourseselect;
                tmptb1.Tag = selectid;
            }
            this.Close();
        }
    }
}
