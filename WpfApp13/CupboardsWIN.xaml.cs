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
    public partial class CupboardsWIN : Window
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
        ArrayList items = new ArrayList();
        ArrayList filters = new ArrayList();
        public CupboardsWIN()
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
        }
        public CupboardsWIN(MainWindow win, string sc)
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
            this.win = win;
            connectionString = sc;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string request = "Select * from cupboards";
            UpdateWin(request);
            FileInfo[] sourses = new FileInfo[ItemCanvas.Children.Count];
            for (int i = 0; i < ItemCanvas.Children.Count; i++)
            {
                Canvas cvs = ItemCanvas.Children[i] as Canvas;
                Border brd = cvs.Children[0] as Border;
                Image pict = brd.Child as Image;
                if (pict.Source != null) { 
                BitmapImage bmi = pict.Source as BitmapImage;
                Uri ur = bmi.UriSource;
                sourses[i] = new FileInfo(ur.LocalPath);}
            }
            DirectoryInfo dr = new DirectoryInfo("Images//CupBoardImg");
            if (!Directory.Exists("Images//CupBoardImg")) dr.Create();
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
                        string group = reader.GetString(3);
                        string ingroup = reader.GetString(4);
                        string sourse = reader.GetString(5);
                        string[] element = reader[2] as string[];
                        string name = reader.GetString(1);
                        string elements = "";
                        for(int ii = 0; ii < element.Length; ii++)
                        {
                            if(ii< element.Length-1)elements += element[ii] + "^";
                            else elements += element[ii] ;
                        }
                        items.Add(id + "%" + group + "%" + ingroup + "%" + sourse + "%" + elements + "%" + name );
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

                        TextBlock namecup = new TextBlock
                        {
                            Text = name,
                            FontSize = 14,
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };

                        namecup.Height = 50;
                        namecup.Width = imgMat.Width;
                        double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                        if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                        Canvas.SetTop(namecup, nach1 + 60);
                        Canvas.SetLeft(namecup, thisItem.Width / 2 - namecup.Width / 2);
                        thisItem.Children.Add(namecup);
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
                    string group = reader.GetString(3);
                    string ingroup = reader.GetString(4);
                    string sourse = reader.GetString(5);
                    string[] element = reader[2] as string[];
                    string name = reader.GetString(1);
                    string elements = "";
                    for (int ii = 0; ii < element.Length; ii++)
                    {
                        if (ii < element.Length - 1) elements += element[ii] + "^";
                        else elements += element[ii];
                    }
                    items.Add(id + "%" + group + "%" + ingroup + "%" + sourse + "%" + elements + "%" + name);
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
                    TextBlock namecup = new TextBlock
                    {
                        Text = name,
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    namecup.Height = 50;
                    namecup.Width = imgMat.Width;
                    double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                    if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                    Canvas.SetTop(namecup, nach1 + 60);
                    Canvas.SetLeft(namecup, thisItem.Width / 2 - namecup.Width / 2);
                    thisItem.Children.Add(namecup);
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
        public void LoadWin(string req)
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
                        string group = reader.GetString(3);
                        string ingroup = reader.GetString(4);
                        string sourse = reader.GetString(5);
                        string[] element = reader[2] as string[];
                        string name = reader.GetString(1);
                        string elements = "";
                        for (int ii = 0; ii < element.Length; ii++)
                        {
                            if (ii < element.Length - 1) elements += element[ii] + "^";
                            else elements += element[ii];
                        }
                        items.Add(id + "%" + group + "%" + ingroup + "%" + sourse + "%" + elements + "%" + name);
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
                        TextBlock namecup = new TextBlock
                        {
                            Text = name,
                            FontSize = 14,
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };
                        namecup.Height = 50;
                        namecup.Width = imgMat.Width;
                        double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                        if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                        Canvas.SetTop(namecup, nach1 + 60);
                        Canvas.SetLeft(namecup, thisItem.Width / 2 - namecup.Width / 2);
                        thisItem.Children.Add(namecup);
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
                    string group = reader.GetString(3);
                    string ingroup = reader.GetString(4);
                    string sourse = reader.GetString(5);
                    string[] element = reader[2] as string[];
                    string name = reader.GetString(1);
                    string elements = "";
                    for (int ii = 0; ii < element.Length; ii++)
                    {
                        if (ii < element.Length - 1) elements += element[ii] + "^";
                        else elements += element[ii];
                    }
                    items.Add(id + "%" + group + "%" + ingroup + "%" + sourse + "%" + elements + "%" + name);
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
                    TextBlock namecup = new TextBlock
                    {
                        Text = name,
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    namecup.Height = 50;
                    namecup.Width = imgMat.Width;
                    double nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 1]);
                    if (double.IsNaN(nach1)) { nach1 = Canvas.GetTop(thisItem.Children[thisItem.Children.Count - 2]); }
                    Canvas.SetTop(namecup, nach1 + 60);
                    Canvas.SetLeft(namecup, thisItem.Width / 2 - namecup.Width / 2);
                    thisItem.Children.Add(namecup);
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
        private void UseFilter_Click(object sender, RoutedEventArgs e)
        {
            string request = "Select * from cupboards";
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
                request = "Select * from cupboards where ";
                for (int i = 0; i < lists.Count; i++)
                {
                    string str = lists[i] as string;
                    string[] str1 = str.Split('|');
                    if (str1.Length == 1) request += "( group_='" + str + "' )";
                    if (str1.Length == 2) request += "( group_='" + str1[0] + "' and ingroup_='" + str1[1] + "' )";
                    if (i < lists.Count - 1) request += "or";
                }
            }
            LoadWin(request);
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
        private void ItemCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas.SetLeft(ShowWin, -210);
            Editcup.IsEnabled = false;
            Delcup.IsEnabled = false;
            if (selectCVS == null) return;
            selectCVS.Background = Brushes.Bisque;
            selectCVS = null;
        }
        Canvas selectCVS;
        int selectid;
        String sourseselect;
        String[] elementsselect;
        public void elementCanvas_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Editcup.IsEnabled = true;
            Delcup.IsEnabled = true;
            selectCVS = sender as Canvas;
            selectCVS.Background = Brushes.Beige;
            Canvas.SetLeft(ShowWin, 0);
            selectid = int.Parse(selectCVS.Tag.ToString());
            for (int i = 0; i < items.Count; i++)
            {
                string str = items[i].ToString();
                string[] str1 = str.Split('%');
                if (int.Parse(str1[0]) == selectid)
                {
                    groupCupS.Text = str1[1]; ingroupCupS.Text = str1[2]; sourseCupS.Text = str1[3]; nameCupS.Text = str1[5];
                    groupCup.Text = str1[1]; ingroupCup.Text = str1[2]; sourseCup.Text = str1[3]; nameCup.Text = str1[5];
                    sourseselect = str1[3];
                    elementsselect = str1[4].Split('^');
                }
            }
        }
        DispatcherTimer timer1 = new DispatcherTimer();
        bool naprtimer1 = true;
        private void OnTimerTick1(object sender, object e)
        {
            double i = Canvas.GetLeft(EditWin);
            if (naprtimer1) i += 2;
            if (!naprtimer1) i -= 2;
            if (i >= -2 && naprtimer1) { timer1.Stop(); naprtimer1 = false; }
            if (i <= -210 && !naprtimer1) { timer1.Stop(); naprtimer1 = true; }
            Canvas.SetLeft(EditWin, i);
        }
        private void Editcup_Click(object sender, RoutedEventArgs e)
        {
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
        private void Delcup_Click(object sender, RoutedEventArgs e)
        {
            string request = "DELETE FROM public.cupboards WHERE id=" + selectid;
            createRequestNonReader(connectionString, request);
            request = "Select * from cupboards";
            UpdateWin(request);
            ItemCanvas_MouseDown(sender, e as MouseButtonEventArgs);
            naprtimer1 = false;
            Editcup_Click(sender, e);
        }
        private void SelectMat_Click(object sender, RoutedEventArgs e)
        {
            Button tmp = sender as Button;
            if (tmp.Tag.ToString() == "wall")
            {
                MaterialsWIN win = new MaterialsWIN(this, matwall, connectionString);
                win.Show();
            }
            if (tmp.Tag.ToString() == "bwall")
            {
                MaterialsWIN win = new MaterialsWIN(this, matbwall, connectionString);
                win.Show();
            }
            this.IsEnabled = false;
        }
        private void LoadCup_Click(object sender, RoutedEventArgs e)
        {        
            win.openCub(elementsselect, matwall, matbwall);
        }
        private void editCup_Click_1(object sender, RoutedEventArgs e)
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
            if (filename != "") {
                FileInfo fn = new FileInfo(filename);
                int ind = 0;
                string nameE = fn.Name;
                string nameEE = ind + nameE;
                FileInfo fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Images//CupBoardImg//" + ind + nameE);
                while (fntest.Exists)
                {
                    nameEE = ind + nameE;
                    fntest = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Images//CupBoardImg//" + nameEE);
                    ind++;
                }
                fn.CopyTo(AppDomain.CurrentDomain.BaseDirectory + "Images//CupBoardImg//" + nameEE, true);
                sourse = "Images//MaterialImg//" + nameEE;
            }
            string request="";
            request = "UPDATE public.cupboards SET group_ = '" + group + "', ingroup_ = '" + ingroup + "', sourseimg = '" + sourse + "', name ='" + name + "' where id = " + selectid;
            createRequestNonReader(connectionString, request);
            request = "Select * from cupboards";
            UpdateWin(request);
            Canvas.SetLeft(ShowWin, -210);
            ItemCanvas_MouseDown(sender, e as MouseButtonEventArgs);
            naprtimer1 = false;
            Editcup_Click(sender, e);
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
            }
            sourseCup.Text = filename;
            sourseCupS.Text = filename;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (win != null)
                win.IsEnabled = true;
        }
    }
}
