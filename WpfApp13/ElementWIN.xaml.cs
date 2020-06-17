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
    public partial class ElementWIN : Window
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
        public ElementWIN()
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
        }
        ArrayList items = new ArrayList();
        ArrayList filters = new ArrayList();
        Canvas selectCVS;
        int selectid;
        String sourseselect;
       
        MainWindow win;
        Button tmpbt;
        ModelUIElement3D model;
        public ElementWIN(MainWindow win,ModelUIElement3D tmpmod,string sc)
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
            this.win = win;
            model = tmpmod;
            if (model == null) SelEl.IsEnabled = false;
            connectionString = sc;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string request = "Select * from elements";
            UpdateWin(request);
            FileInfo[] sourses = new FileInfo[ItemCanvas.Children.Count];
            for (int i = 0; i < ItemCanvas.Children.Count; i++)
            {
                Canvas cvs = ItemCanvas.Children[i] as Canvas;
                Border brd = cvs.Children[0] as Border;
                if (brd.Child != null) { 
                Image pict = brd.Child as Image;
                if (pict.Source != null) { 
                BitmapImage bmi = pict.Source as BitmapImage;
                Uri ur = bmi.UriSource;
                sourses[i] = new FileInfo(ur.LocalPath);}
            }}
            DirectoryInfo dr = new DirectoryInfo("Images//ElementImg");
            if (!Directory.Exists("Images//ElementImg")) dr.Create();
            FileInfo[] fn = dr.GetFiles();
            try
            {
                for (int j = 0; j < fn.Length; j++)
                {

                    bool b = false;
                    int ind = 0;
                    for (int i = 0; i < sourses.Length; i++)
                    {
                        if (sourses[i] == null) continue;
                        if (sourses[i].FullName != fn[j].FullName) { b = true; ind = j; }
                        else { b = false; break; }
                    }
                    if (b == true) fn[ind].Delete();
                }
            }
            catch (Exception ee)
            {
            }
            FileInfo[] sourses1 = new FileInfo[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                string str = items[i].ToString();
                string[] str1 = str.Split('|');
                if(str1[3]!=null)
                sourses1[i] = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + str1[3]);
            }
            DirectoryInfo dr1 = new DirectoryInfo("objects");
            if (!Directory.Exists("objects")) dr1.Create();
            FileInfo[] fn1 = dr1.GetFiles();
            try
            {
                for (int j = 0; j < fn1.Length; j++)
                {

                    bool b = false;
                    int ind = 0;
                    for (int i = 0; i < sourses1.Length; i++)
                    {
                        if (sourses1[i] == null) continue;
                        if (sourses1[i].FullName != fn1[j].FullName) { b = true; ind = j; }
                        else { b = false; break; }
                    }
                    if (b == true) fn1[ind].Delete();
                }
            }
            catch (Exception ee)
            {
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (win != null)
                win.IsEnabled = true;
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
                        string sourseimg = reader.GetString(4);
                        double price = reader.GetDouble(5);
                        string name = reader.GetString(6);
                        items.Add(id + "|" + group + "|" + ingroup + "|" + sourse + "|" + sourseimg + "|" + price + "|" + name + "|");
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
                        Border imgEl = new Border
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
                        if (sourseimg != "")
                        {
                            var uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + sourseimg);
                            var bitmap = new BitmapImage(uri);
                            img.Source = bitmap;
                        }
                        imgEl.Child = img;
                        Canvas.SetTop(imgEl, 0);
                        Canvas.SetLeft(imgEl, 10);
                        thisItem.Children.Add(imgEl);
                        TextBlock nameEl = new TextBlock
                        {
                            Text = name,
                            FontSize = 14,
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };
                        nameEl.Height = 50;
                        nameEl.Width = imgEl.Width;
                        double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                        if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                        Canvas.SetTop(nameEl, nach1 + 60);
                        Canvas.SetLeft(nameEl, thisItem.Width / 2 - nameEl.Width / 2);
                        thisItem.Children.Add(nameEl);
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
                    string sourseimg = reader.GetString(4);
                    double price = reader.GetDouble(5);
                    string name = reader.GetString(6);
                    items.Add(id + "|" + group + "|" + ingroup + "|" + sourse + "|" + sourseimg + "|" + price + "|" + name + "|");
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
                    Border imgEl = new Border
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
                    if (sourseimg != "")
                    {
                        var uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + sourseimg);
                        var bitmap = new BitmapImage(uri);
                        img.Source = bitmap;
                        imgEl.Child = img;
                    }
                    Canvas.SetTop(imgEl, 0);
                    Canvas.SetLeft(imgEl, 10);
                    thisItem.Children.Add(imgEl);
                    TextBlock nameEl = new TextBlock
                    {
                        Text = name,
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    nameEl.Height = 50; ;
                    nameEl.Width = imgEl.Width;
                    double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                    if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                    Canvas.SetTop(nameEl, nach1 + 60);
                    Canvas.SetLeft(nameEl, thisItem.Width / 2 - nameEl.Width / 2);
                    thisItem.Children.Add(nameEl);
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
            if (nach3 > ItemCanvas.Height) ItemCanvas.Height = nach3;
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
        public void LoadElements(string req) {
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
                        string sourseimg = reader.GetString(4);
                        double price = reader.GetDouble(5);
                        string name = reader.GetString(6);
                        items.Add(id + "|" + group + "|" + ingroup + "|" + sourse + "|" + sourseimg + "|" + price + "|" + name + "|");
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
                        Border imgEl = new Border
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
                        if (sourseimg != "")
                        {
                            var uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + sourseimg);
                            var bitmap = new BitmapImage(uri);
                            img.Source = bitmap;
                        }
                        imgEl.Child = img;
                        Canvas.SetTop(imgEl, 0);
                        Canvas.SetLeft(imgEl, 10);
                        thisItem.Children.Add(imgEl);

                        TextBlock nameEl = new TextBlock
                        {
                            Text = name,
                            FontSize = 14,
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };
                        nameEl.Height = 50;
                        nameEl.Width = imgEl.Width;
                        double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                        if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                        Canvas.SetTop(nameEl, nach1 + 60);
                        Canvas.SetLeft(nameEl, thisItem.Width / 2 - nameEl.Width / 2);
                        thisItem.Children.Add(nameEl);
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
                    string sourseimg = reader.GetString(4);
                    double price = reader.GetDouble(5);
                    string name = reader.GetString(6);
                    items.Add(id + "|" + group + "|" + ingroup + "|" + sourse + "|" + sourseimg + "|" + price + "|" + name + "|");
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
                    Border imgEl = new Border
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
                    if (sourseimg != "")
                    {
                        var uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + sourseimg);
                        var bitmap = new BitmapImage(uri);
                        img.Source = bitmap;
                        imgEl.Child = img;
                    }
                    Canvas.SetTop(imgEl, 0);
                    Canvas.SetLeft(imgEl, 10);
                    thisItem.Children.Add(imgEl);
                    TextBlock nameEl = new TextBlock
                    {
                        Text = name,
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    nameEl.Height = 50; ;
                    nameEl.Width = imgEl.Width;
                    double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                    if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                    Canvas.SetTop(nameEl, nach1 + 60);
                    Canvas.SetLeft(nameEl, thisItem.Width / 2 - nameEl.Width / 2);
                    thisItem.Children.Add(nameEl);
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
            if (nach3 > ItemCanvas.Height) ItemCanvas.Height = nach3;
            if (ItemCanvas.Height <= 380) ItemCanvas.Height = 380;
        }
        public void elementCanvas_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            EditEl.IsEnabled = true;
            DelEl.IsEnabled = true;
            selectCVS = sender as Canvas;
            selectCVS.Background = Brushes.Beige;
            Canvas.SetLeft(ShowWin, 0);
            selectid = int.Parse(selectCVS.Tag.ToString());
            for (int i = 0; i < items.Count; i++)
            {
                string str = items[i].ToString();
                string[] str1 = str.Split('|');
                if (int.Parse(str1[0]) == selectid)
                {
                    groupElS.Text = str1[1]; ingroupElS.Text = str1[2]; sourseElS.Text = str1[3]; sourseimgElS.Text = str1[4]; priceElS.Text = str1[5]; nameElS.Text = str1[6];
                    groupElE.Text = str1[1]; ingroupElE.Text = str1[2]; sourseElEE.Text = str1[3]; sourseimgElEE.Text = str1[4]; priceElE.Text = str1[5]; nameElE.Text = str1[6];
                    sourseselect = str1[3];
                   // throw new Exception();
                }
            }
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
            if (tmpcb.IsChecked == true)
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
        private void ItemCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas.SetLeft(ShowWin, -210);
            EditEl.IsEnabled = false;
            DelEl.IsEnabled = false;        
            if (selectCVS == null) return;
            selectCVS.Background = Brushes.Bisque;
            selectCVS = null;
        }
        private void UseFilter_Click(object sender, RoutedEventArgs e)
        {
            string request = "Select * from elements";
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
                    if (nal == false) lists.Add(tmpcb1.Tag);
                }
            }
            if (lists.Count != 0)
            {
                request = "Select * from elements where ";
                for (int i = 0; i < lists.Count; i++)
                {
                    string str = lists[i] as string;
                    string[] str1 = str.Split('|');
                    if (str1.Length == 1) request += "( group_='" + str + "' )";
                    if (str1.Length == 2) request += "( group_='" + str1[0] + "' and ingroup_='" + str1[1] + "' )";
                    if (i < lists.Count - 1) request += "or";
                }

            }
            LoadElements(request);
        }
        private void AddEl_Click(object sender, RoutedEventArgs e)
        {
            filenames = "";
            filenameimgs = "";
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
        private void EditEl_Click(object sender, RoutedEventArgs e)
        {
            filenames = "";
            filenameimgs = "";
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
        private void DelEl_Click(object sender, RoutedEventArgs e)
        {
            string request = "DELETE FROM public.elements WHERE id=" + selectid;
            createRequestNonReader(connectionString, request);
            request = "Select * from elements";
            UpdateWin(request);
            ItemCanvas_MouseDown(sender, e as MouseButtonEventArgs);
            naprtimer1 = false;
            EditEl_Click(sender, e);
        }
        string filenameimgs = "";
        string filenames = "";
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
        private void sourseimgEl_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Images";
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPG|*.jpg";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                filenameimgs = dlg.FileName;
            }
                sourseimgElEE.Text = filenameimgs;
                sourseimgEll.Text = filenameimgs;
        }
        private void sourseEl_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Images";
            dlg.DefaultExt = ".obj";
            dlg.Filter = "OBJ or STL|*.obj;*.stl";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                filenames = dlg.FileName;
            }
            sourseElEE.Text = filenames;
            sourseEll.Text = filenames;
        }
        private void addElement_Click(object sender, RoutedEventArgs e)
        {
            string name = "";
            string group = "";
            string ingroup = "";
            string sourse = "";
            string sourseimg = "";
            double price = 0;
            try
            {
                name = nameEl.Text;
                group = groupEl.Text;
                ingroup = ingroupEl.Text;         
                price = double.Parse(priceEl.Text);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (filenames != "")
            {
                FileInfo fn = new FileInfo(filenames);
                int ind = 0;
                string nameE = fn.Name;
                string nameEE = ind + nameE;
                FileInfo fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "objects//" + ind + nameE);
                while (fntest.Exists)
                {
                    nameEE = ind + nameE;
                    fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "objects//" + nameEE);
                    ind++;
                }
                fn.CopyTo(AppDomain.CurrentDomain.BaseDirectory + "objects//" + nameEE, true);
                sourse = "objects//" + nameEE;
            }
            if (filenameimgs != "")
            {
                FileInfo fn = new FileInfo(filenameimgs);
                int ind = 0;
                string nameE = fn.Name;
                string nameEE = ind + nameE;
                FileInfo fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Images//ElementImg//" + ind + nameE);
                while (fntest.Exists)
                {
                    nameEE = ind + nameE;
                    fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Images//ElementImg//" + nameEE);
                    ind++;
                }
                fn.CopyTo(AppDomain.CurrentDomain.BaseDirectory + "Images//ElementImg//" + nameEE, true);
                sourseimg = "Images//ElementImg//" + nameEE;
            }
            string request = "INSERT INTO public.elements (group_, ingroup_, sourse, sourseimg, price,name) VALUES ( '" + group + "' , '" + ingroup + "', '" + sourse + "', '" + sourseimg + "', " + price + ",'" + name + "');";
            createRequestNonReader(connectionString, request);
            request = "Select * from elements";
            UpdateWin(request);
        }
        private void editElement_Click(object sender, RoutedEventArgs e)
        {
            string name = "";
            string group = "";
            string ingroup = "";
            string sourse = "";
            string sourseimg = "";
            double price = 0;
            try
            {
                name = nameElE.Text;
                group = groupElE.Text;
                ingroup = ingroupElE.Text;
                price = double.Parse(priceElE.Text);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            filenames = sourseElEE.Text;
            filenameimgs = sourseimgElEE.Text;
            if (filenames != "") {
                FileInfo fn = new FileInfo(filenames);
                int ind = 0;
                string nameE = fn.Name;
                string nameEE = ind + nameE;
                FileInfo fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "objects//" + ind + nameE);
                while (fntest.Exists)
                {
                    nameEE = ind + nameE;
                    fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "objects//" + nameEE);
                    ind++;
                }
                fn.CopyTo(AppDomain.CurrentDomain.BaseDirectory + "objects//" + nameEE, true);
                sourse = "objects//" + nameEE;
            }
            if (filenameimgs != "") {

                FileInfo fn = new FileInfo(filenameimgs);
                int ind = 0;
                string nameE = fn.Name;
                string nameEE = ind + nameE;
                FileInfo fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Images//ElementImg//" + ind + nameE);
                while (fntest.Exists)
                {
                    nameEE = ind + nameE;
                    fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Images//ElementImg//" + nameEE);
                    ind++;
                }
                fn.CopyTo(AppDomain.CurrentDomain.BaseDirectory + "Images//ElementImg//" + nameEE, true);
                sourseimg = "Images//ElementImg//" + nameEE;
            }

            string request = "UPDATE public.elements SET group_ = '" + group + "', ingroup_ = '" + ingroup + "', sourse = '" + sourse + "', sourseimg = '" + sourseimg + "', price = '" + price + "', name ='" + name + "' where id = " + selectid;
            createRequestNonReader(connectionString, request);
            request = "Select * from elements";
            UpdateWin(request);
            Canvas.SetLeft(ShowWin, -210);
            ItemCanvas_MouseDown(sender, e as MouseButtonEventArgs);
            naprtimer1 = false;
            EditEl_Click(sender, e);
        }
        private void SelEl_Click(object sender, RoutedEventArgs e)
        {
            win.IsEnabled = true;
            CreateElements ce = new CreateElements(win);
            ce.AddElement(sourseselect, matEll.Text, Convert.ToInt32(matEll.Tag), selectid,model);
            this.Close();
        }
    }
}
