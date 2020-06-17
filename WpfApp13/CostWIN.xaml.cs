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
using TemplateEngine.Docx;

namespace WpfApp13
{
    public partial class CostWIN : Window
    {
        string connectionString;
        MainWindow win;
        Hashtable hash;
        struct Row
        {
            public string name;
            public string material;
            public string size;
            public double price;
        };
        public CostWIN()
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
        }
        public CostWIN(MainWindow win, Hashtable hash,string sc)
        {
            InitializeComponent();
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";
            this.win = win;
            this.hash = hash;
            connectionString= sc+ "Pooling=false";        
        }
        ArrayList[] CostArr;
        ArrayList[] materialArr;
        ArrayList[] sizeArr;
        ArrayList[] nameArr;
        ArrayList[] all;
        ArrayList[] counts;
        public void DefineCost()
        {
            CostArr = new ArrayList[hash.Count];
            materialArr = new ArrayList[hash.Count];
            sizeArr = new ArrayList[hash.Count];
            nameArr = new ArrayList[hash.Count];
            for (int i = 0; i < CostArr.Length; i++)
            {
                CostArr[i] = new ArrayList();
                materialArr[i] = new ArrayList();
                sizeArr[i] = new ArrayList();
                nameArr[i] = new ArrayList();
            }
            int ind = 0;
            foreach (var item in hash)
            {
                DictionaryEntry tmp1 = (DictionaryEntry)item;
                ArrayList tmp2 = tmp1.Value as ArrayList;
                for (int i = 0; i < tmp2.Count; i++)
                {
                    ElementCabinet elcab = tmp2[i] as ElementCabinet;
                    string[] tmpstr = elcab.ingroupel.Split('-');
                    if (tmpstr[0] != "Element")
                    {
                        string request = "select sizes,price,name from materials where id=" + elcab.idtexture;
                        NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
                        npgSqlConnection.Open();
                        NpgsqlCommand npgSqlCommand = new NpgsqlCommand(request, npgSqlConnection);
                        NpgsqlDataReader reader = npgSqlCommand.ExecuteReader();
                        int[] sizes = null;
                        double price = 0;
                        while (reader.Read())
                        {
                            sizes = reader[0] as int[];
                            price = reader.GetDouble(1);
                            materialArr[ind].Add(reader.GetString(2));
                        }
                        long valueEl = sizes[0] * sizes[1] * sizes[2];
                        double price1Cube = price / valueEl;

                        if (elcab.ingroupel != "Rack") CostArr[ind].Add(Math.Abs(Math.Round( elcab.getLenX() * 1000 * elcab.getLenY() * 1000 * elcab.getLenZ() * 1000 * price1Cube,2)));
                        else CostArr[ind].Add(Math.Abs(Math.Round(elcab.getLenX() * 1000 * Math.PI * ((elcab.getLenZ() * 1000 / 2) * (elcab.getLenZ() * 1000 / 2)) * price1Cube, 2)));
                        sizeArr[ind].Add(Math.Abs(Math.Round(elcab.getLenX(),3)) * 1000 +" x "+ Math.Abs(Math.Round(elcab.getLenY(), 3)) * 1000 + " x " + Math.Abs(Math.Round(elcab.getLenZ(), 3)) * 1000);
                    }
                    else
                    {
                        string request = "select price,name from elements where id=" + elcab.idelement;
                        NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
                        npgSqlConnection.Open();
                        NpgsqlCommand npgSqlCommand = new NpgsqlCommand(request, npgSqlConnection);
                        NpgsqlDataReader reader = npgSqlCommand.ExecuteReader();
                        double price = 0;
                        while (reader.Read())
                        {
                            price = reader.GetDouble(0);
                            materialArr[ind].Add(reader.GetString(1));
                        }
                        CostArr[ind].Add(Math.Round(price, 2));
                        sizeArr[ind].Add("-");
                    }
                    ElementCabinet element = tmp2[i] as ElementCabinet;
                    string name = "";
                    if (element.ingroupel == "WallL") name = "Стена корпуса левая.";
                    if (element.ingroupel == "WallR") name = "Стена корпуса правая.";
                    if (element.ingroupel == "WallB") name = "Стена корпуса задняя.";
                    if (element.ingroupel == "WallF") name = "Стена корпуса - цоколь.";
                    if (element.ingroupel == "Ceiling") name = "Потолок корпуса.";
                    if (element.ingroupel == "Floor" || element.ingroupel == "FloorFoot") name = "Пол корпуса.";
                    if (element.ingroupel == "Rack") name = "Вешалка.";
                    if (element.ingroupel == "ShelfC") name = "Полочка.";
                    if (element.ingroupel == "WallC") name = "Стена.";
                    if (element.ingroupel == "Door") name = "Двери.";
                    if (element.iningroupel == "WR") name = "Стена ящика правая.";
                    if (element.iningroupel == "WL") name = "Стена ящика левая.";
                    if (element.iningroupel == "WF") name = "Стена ящика передняя.";
                    if (element.iningroupel == "WB") name = "Стена ящика задняя.";
                    if (element.iningroupel == "WFF") name = "Стена ящика лицевая.";
                    if (element.iningroupel == "WD") name = "Пол ящика.";
                    if (element.iningroupel == "El") name = "Елемент мебели.";
                    if (element.iningroupel == "DoorL") name = "Левая дверь.";
                    if (element.iningroupel == "DoorR") name = " Правая дверь.";
                    nameArr[ind].Add(name);                 
                }
                ind++;
            }
            all = new ArrayList[hash.Count];
            counts = new ArrayList[hash.Count];
            for (int i = 0; i < all.Length; i++)
            {
                all[i] = new ArrayList();
                counts[i] = new ArrayList();
            }
            for(int i = 0; i < all.Length; i++)
            {
                ArrayList tmp = nameArr[i] as ArrayList;
                for (int j = 0; j < tmp.Count; j++)
                {
                    Row tmprow= new Row{ name= nameArr[i][j].ToString(),material=materialArr[i][j].ToString(), size = sizeArr[i][j].ToString(),price=Convert.ToDouble(CostArr[i][j]) };
                    if (all[i].IndexOf(tmprow) == -1) { all[i].Add(tmprow); counts[i].Add(1); }
                    else
                    {
                        int id = all[i].IndexOf(tmprow);
                        int tmpcount = (int)counts[i][id];
                        tmpcount++;
                        counts[i][id] = tmpcount;
                    }
                }
            }
            ArrayList []alldp = new ArrayList[all.Length];
            ArrayList[] countsdp = new ArrayList[all.Length];
            for (int i = 0; i < alldp.Length; i++)
            {
                alldp[i] = new ArrayList();
                countsdp[i] = new ArrayList();
            }
            for (int i = 0; i < alldp.Length; i++)
            {
                ArrayList tmp1 = all[i] as ArrayList;
                ArrayList tmp2 = counts[i] as ArrayList;
                for (int j = 0; j < tmp1.Count; j++)
                {
                    
                    Row tmprow = (Row)tmp1[j];
                    int tmpcount = (int)tmp2[j];
                    Row tmprow1 = (Row)tmp1[j];
                    int tmpcount1 = (int)tmp2[j];
                    if (tmprow.name.IndexOf("корпуса") != -1) {
                        if (tmprow.name == "Стена корпуса задняя.") { string[] ss = tmprow.size.Split('x'); double dd = (2*int.Parse(ss[0]) + 2 * int.Parse(ss[2])) / 100; tmpcount = (int)dd; tmprow.name = "Гвоздь обивочный"; tmprow.material = "Сталь"; tmprow.size = "1x0,1"; tmprow.price = 0.25; }
                        else { tmprow.name = "Конфирмат"; tmprow.material = "Сталь"; tmprow.size = "5x0,5"; tmprow.price = 0.5; tmpcount = 2; }
                        alldp[i].Add(tmprow); countsdp[i].Add(tmpcount);
                        string[] ss1 = tmprow1.size.Split('x');
                        int[] arr = new int[] { int.Parse(ss1[0]) , int.Parse(ss1[1]) , int.Parse(ss1[2]) };
                        Array.Sort(arr);                       
                        double dd1 = (2 * arr[1] + 2 * arr[2])/1000.0; tmpcount = (int)Math.Ceiling(dd1); tmprow.name = "Раскрой"; tmprow.material = "-"; tmprow.size = "-"; tmprow.price = 7;                       
                        alldp[i].Add(tmprow); countsdp[i].Add(tmpcount);
                    }
                    
                    if (tmprow.name.IndexOf("ящика") != -1)
                    {
                        if (tmprow.name == "Пол ящика.") { string[] ss = tmprow.size.Split('x'); double dd = (2 * int.Parse(ss[0]) + 2 * int.Parse(ss[1])) / 100; tmpcount = (int)dd; tmprow.name = "Гвоздь обивочный"; tmprow.material = "Сталь"; tmprow.size = "1x0,1"; tmprow.price = 0.25; }
                        else { tmprow.name = "Конфирмат"; tmprow.material = "Сталь"; tmprow.size = "5x0,5"; tmprow.price = 0.5; tmpcount = 2; }
                        alldp[i].Add(tmprow); countsdp[i].Add(tmpcount);
                        string[] ss1 = tmprow1.size.Split('x');
                        int[] arr = new int[] { int.Parse(ss1[0]), int.Parse(ss1[1]), int.Parse(ss1[2]) };
                        Array.Sort(arr);
                        double dd1 = (2 * arr[1] + 2 * arr[2]) / 1000.0; tmpcount = (int)Math.Ceiling(dd1); tmprow.name = "Раскрой"; tmprow.material = "-"; tmprow.size = "-"; tmprow.price = 7;
                        alldp[i].Add(tmprow); countsdp[i].Add(tmpcount);
                    }
                    if (tmprow.name.IndexOf("вер") != -1)
                    {
                        tmprow.name = "Петля накладная"; tmprow.material = "Сталь"; tmprow.size = "-"; tmprow.price = 6.5; tmpcount = 2;
                        alldp[i].Add(tmprow); countsdp[i].Add(tmpcount);
                        string[] ss1 = tmprow1.size.Split('x');
                        int[] arr = new int[] { int.Parse(ss1[0]), int.Parse(ss1[1]), int.Parse(ss1[2]) };
                        Array.Sort(arr);
                        double dd1 = (2 * arr[1] + 2 * arr[2]) / 1000.0; tmpcount = (int)Math.Ceiling(dd1); tmprow.name = "Раскрой"; tmprow.material = "-"; tmprow.size = "-"; tmprow.price = 7;
                        alldp[i].Add(tmprow); countsdp[i].Add(tmpcount);
                    }
                    if (tmprow.name == "Полочка."|| tmprow.name == "Стена.")
                    {
                        tmprow.name = "Конфирмат"; tmprow.material = "Сталь"; tmprow.size = "5x0,5"; tmprow.price = 0.5; tmpcount = 4;
                        alldp[i].Add(tmprow); countsdp[i].Add(tmpcount);
                        string[] ss1 = tmprow1.size.Split('x');
                        int[] arr = new int[] { int.Parse(ss1[0]), int.Parse(ss1[1]), int.Parse(ss1[2]) };
                        Array.Sort(arr);
                        double dd1 = (2 * arr[1] + 2 * arr[2]) / 1000.0; tmpcount = (int)Math.Ceiling(dd1); tmprow.name = "Раскрой"; tmprow.material = "-"; tmprow.size = "-"; tmprow.price = 7;
                        alldp[i].Add(tmprow); countsdp[i].Add(tmpcount);
                    }
                    
                }
            }

            for (int i = 0; i < alldp.Length; i++)
            {
                ArrayList tmp1 = alldp[i] as ArrayList;
                ArrayList tmp2 = countsdp[i] as ArrayList;
                for (int j = 0; j < tmp1.Count; j++)
                {
                    Row tmprow = (Row)tmp1[j];
                    int tmpcount = (int)tmp2[j];
                    if (all[i].IndexOf(tmprow) == -1) { all[i].Add(tmprow); counts[i].Add(tmpcount); }
                    else
                    {
                        int id = all[i].IndexOf(tmprow);
                        int tmpcount1 = (int)counts[i][id];
                        tmpcount1+= tmpcount;
                        counts[i][id] = tmpcount1;
                    }
                }
            }


        } 
        public void LoadCost()
        {
            int i = 0;           
            foreach (var item in hash)
            {
                DictionaryEntry tmp1 = (DictionaryEntry)item;
                ArrayList tmp2 = tmp1.Value as ArrayList;
                double nach = 10;
                if (ItemCanvas.Children.Count == 0) nach = 10;
                else
                {
                    nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 70;
                    if (double.IsNaN(nach)) { nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 70; }
                }
                TextBlock nameCupb = new TextBlock
                {
                    Text = "Изделие - Шкаф "+ (i+1),
                    FontSize = 24,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center
                };
                nameCupb.Height = 50;
                nameCupb.Width = ItemCanvas.Width;
                Canvas.SetTop(nameCupb, nach + 10);
                Canvas.SetLeft(nameCupb, ItemCanvas.Width / 2 - nameCupb.Width / 2);
                ItemCanvas.Children.Add(nameCupb);
                TextBlock Header = new TextBlock
                {
                    Text = "Деталь                                Материал                     Размеры              Кол-во       Цена     Общая цена",
                                                                                                                        
                    FontSize = 18,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center
                };
                Header.Height = 50;
                Header.Width = ItemCanvas.Width;
                Canvas.SetTop(Header, nach + 40);
                Canvas.SetLeft(Header, ItemCanvas.Width / 2 - Header.Width / 2);
                ItemCanvas1.Children.Add(Header);
                double allprice = 0;
                for (int j = 0; j < all[i].Count; j++)
                {
                    if(j==0) if (ItemCanvas.Children.Count == 0) nach = 10;
                        else
                        {
                            nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 60;
                            if (double.IsNaN(nach)) { nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 60; }
                        }
                    if (j>0)
                    if (ItemCanvas.Children.Count == 0) nach = 10;
                    else
                    {
                        nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 90;
                        if (double.IsNaN(nach)) { nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 90; }
                    }
                    Row tmprow = (Row)all[i][j];
                    int count = (int)counts[i][j];
                    Canvas thisItem = new Canvas
                    {
                        Width = ItemCanvas.Width - 10 * 4,
                        Height = 80,
                        Background = Brushes.Bisque,
                    };
                    ItemCanvas.Children.Add(thisItem);
                    Canvas.SetTop(thisItem, nach);
                    Canvas.SetLeft(thisItem, ItemCanvas.Width / 2 - thisItem.Width / 2);                    
                    TextBlock nameEl = new TextBlock
                    {
                        Text = tmprow.name,
                        FontSize = 20,
                        TextWrapping = TextWrapping.Wrap,
                        VerticalAlignment = VerticalAlignment.Center,
                        Background = Brushes.Beige
                    };
                    nameEl.Height = 80;
                    nameEl.Width = 220;
                    Canvas.SetTop(nameEl, 0);
                    Canvas.SetLeft(nameEl,0);
                    thisItem.Children.Add(nameEl);
                    TextBlock matEl = new TextBlock
                    {
                        Text = tmprow.material,
                        FontSize = 20,
                        TextWrapping = TextWrapping.Wrap,
                        VerticalAlignment = VerticalAlignment.Center,
                        
                    };
                    matEl.Height = 80;
                    matEl.Width = 160;
                    Canvas.SetTop(matEl, 0);
                    Canvas.SetLeft(matEl, 220);
                    thisItem.Children.Add(matEl);
                    TextBlock sizeEl = new TextBlock
                    {
                        Text = tmprow.size,
                        FontSize = 20,
                        TextWrapping = TextWrapping.Wrap,
                        VerticalAlignment = VerticalAlignment.Center,
                        Background = Brushes.Beige
                    };
                    sizeEl.Height = 80;
                    sizeEl.Width = 160;
                    Canvas.SetTop(sizeEl, 0);
                    Canvas.SetLeft(sizeEl, 400);
                    thisItem.Children.Add(sizeEl);
                    TextBlock countEl = new TextBlock
                    {
                        Text = counts[i][j]+"",
                        FontSize = 20,
                        TextWrapping = TextWrapping.Wrap,
                        VerticalAlignment = VerticalAlignment.Center,
                        
                    };
                    countEl.Height = 80;
                    countEl.Width = 60;
                    Canvas.SetTop(countEl, 0);
                    Canvas.SetLeft(countEl, 560);
                    thisItem.Children.Add(countEl);
                    TextBlock priceEl = new TextBlock
                    {
                        Text = tmprow.price + "",
                        FontSize = 20,
                        TextWrapping = TextWrapping.Wrap,
                        VerticalAlignment = VerticalAlignment.Center,
                        Background = Brushes.Beige
                    };
                    priceEl.Height = 80;
                    priceEl.Width = 80;
                    Canvas.SetTop(priceEl, 0);
                    Canvas.SetLeft(priceEl, 640);
                    thisItem.Children.Add(priceEl);
                    TextBlock price = new TextBlock
                    {
                        Text = tmprow.price * (int)counts[i][j] + "",
                        FontSize = 20,
                        TextWrapping = TextWrapping.Wrap,
                        VerticalAlignment = VerticalAlignment.Center,
                       
                    };
                    price.Height = 80;
                    price.Width = 80;
                    Canvas.SetTop(price, 0);
                    Canvas.SetLeft(price, 720);
                    thisItem.Children.Add(price);
                    allprice += tmprow.price * (int)counts[i][j];
                }
                TextBlock price1 = new TextBlock
                {
                    Text = "" + Math.Round(allprice, 2)+ "(грн)",
                    FontSize = 20,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Center,
                    Background = Brushes.Bisque
                };
                if (ItemCanvas.Children.Count == 0) nach = 10;
                else
                {
                    nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 80;
                    if (double.IsNaN(nach)) { nach = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 80; }               
                }
                price1.Height = 50;
                price1.Width = 110;
                Canvas.SetTop(price1, nach);
                Canvas.SetLeft(price1, 740);
                ItemCanvas.Children.Add(price1);
                i++;
            }
            double nach3 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 80;
            if (double.IsNaN(nach3)) { nach3 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 80; }
            Button create = new Button
            {     
                Content = "Создать накладную",
                Width = 200,
                Height = 50,
                FontSize = 20,
            };
            ItemCanvas.Children.Add(create);
            Canvas.SetTop(create, nach3);
            Canvas.SetLeft(create, ItemCanvas.ActualWidth / 2 - create.Width / 2 + 25);
            create.Click += Create_Doc;
            nach3 = 10;
            if (ItemCanvas.Children.Count == 0) nach3 = 10;
            if (ItemCanvas.Height <= 380) ItemCanvas.Height = 420;
            else
            {
                nach3 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 1]) + 110;
                if (double.IsNaN(nach3)) { nach3 = Canvas.GetTop(ItemCanvas.Children[ItemCanvas.Children.Count - 2]) + 110; }
            }
            if (nach3 > ItemCanvas.Height) ItemCanvas.Height = nach3;
        }
        private void Create_Doc(object sender, RoutedEventArgs e)
        {
            string file = AppDomain.CurrentDomain.BaseDirectory+"/tempplates/nak.docx";
            Content values = new Content();
            values.Fields.Add(new FieldContent("data", DateTime.Now.ToShortDateString().ToString()));
            values.Lists.Add(new ListContent("List"));
            ListContent ll = values.Lists.First();
            for (int i = 0; i < all.Length; i++)
            {
                TableContent tt = new TableContent("table");
                ArrayList tmp = all[i] as ArrayList;
                double allprice = 0;
                for (int j = 0; j < tmp.Count; j++)
                {
                    Row tmprow = (Row)all[i][j];
                    int count = (int)counts[i][j];
                    tt.AddRow(new FieldContent("nameD", tmprow.name), new FieldContent("Mater", tmprow.material), new FieldContent("Size", tmprow.size), new FieldContent("Count", count + ""), new FieldContent("Price", tmprow.price + ""), new FieldContent("allPrice", tmprow.price * count + ""));
                    allprice += tmprow.price * count;
                }
                ll.AddItem(new FieldContent("N", (i + 1) + ""),tt, new FieldContent("all", allprice + ""));               
            }
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.DefaultExt = "docx";
            saveFileDialog.Filter = "Docx|*.docx";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.Copy(file, saveFileDialog.FileName);
                using (var outputDocument = new TemplateProcessor(saveFileDialog.FileName)
                .SetRemoveContentControls(true))
                {
                    outputDocument.FillContent(values);
                    outputDocument.SaveChanges();
                }
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (win != null)
                win.IsEnabled = true;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (hash.Count != 0)
            {
                DefineCost();
                LoadCost();
            }

        }
    }
}
