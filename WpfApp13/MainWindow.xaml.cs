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
using HelixToolkit;
using HelixToolkit.Wpf;
using System.IO;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Npgsql;

namespace WpfApp13
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            rectangleobject.Add(new Point3D(0, 0, 0));
            rectangleobject.Add(new Point3D(1, 0, 0));
            rectangleobject.Add(new Point3D(0, 1, 0));
            rectangleobject.Add(new Point3D(1, 1, 0));
            rectangleobject.Add(new Point3D(0, 0, 0));
            rectangleobject.Add(new Point3D(0, 0, 1));
            rectangleobject.Add(new Point3D(0, 1, 0));
            rectangleobject.Add(new Point3D(0, 1, 1));
            rectangleobject.Add(new Point3D(0, 0, 0));
            rectangleobject.Add(new Point3D(1, 0, 0));
            rectangleobject.Add(new Point3D(0, 0, 1));
            rectangleobject.Add(new Point3D(1, 0, 1));
            rectangleobject.Add(new Point3D(1, 0, 0));
            rectangleobject.Add(new Point3D(1, 1, 1));
            rectangleobject.Add(new Point3D(1, 0, 1));
            rectangleobject.Add(new Point3D(1, 1, 0));
            rectangleobject.Add(new Point3D(0, 0, 1));
            rectangleobject.Add(new Point3D(1, 0, 1));
            rectangleobject.Add(new Point3D(0, 1, 1));
            rectangleobject.Add(new Point3D(1, 1, 1));
            rectangleobject.Add(new Point3D(0, 1, 0));
            rectangleobject.Add(new Point3D(0, 1, 1));
            rectangleobject.Add(new Point3D(1, 1, 0));
            rectangleobject.Add(new Point3D(1, 1, 1));
            foreach (int index in trianglerectangle)
            {
                trianglerectangleobject.Add(index);
            }
            textures = new List<Point>();
            textures.Add(new Point(0, 0));
            textures.Add(new Point(1, 0));
            textures.Add(new Point(0, 1));
            textures.Add(new Point(1, 1));
            textures.Add(new Point(0, 1));
            textures.Add(new Point(0, 0));
            textures.Add(new Point(1, 1));
            textures.Add(new Point(1, 0));
            textures.Add(new Point(1, 1));
            textures.Add(new Point(0, 1));
            textures.Add(new Point(1, 0));
            textures.Add(new Point(0, 0));
            textures.Add(new Point(1, 1));
            textures.Add(new Point(0, 0));
            textures.Add(new Point(1, 0));
            textures.Add(new Point(0, 1));
            textures.Add(new Point(0, 0));
            textures.Add(new Point(1, 0));
            textures.Add(new Point(0, 1));
            textures.Add(new Point(1, 1));
            textures.Add(new Point(1, 1));
            textures.Add(new Point(1, 0));
            textures.Add(new Point(0, 1));
            textures.Add(new Point(0, 0));
            connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=12345;Database=cupboardBase;";  
            
            ModelVisual3D dd=vv as ModelVisual3D;
            dd.Transform = new TranslateTransform3D(-0.5, -0.5, 0);
            
            Camera.Camera.Position = new Point3D(1, 1, 1);
            Camera.Camera.Reset();
        }

        public void checkConfig()
        {

            if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Config.txt") == true)
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Config.txt");
                string s = sr.ReadToEnd();
                string[] ss = s.Split('~');
                if (ss.Length >= 6)
                {
                    connectionString = "Server=" + ss[2].Split('%')[1] + ";Port=" + ss[3].Split('%')[1] + ";User Id=" + ss[0].Split('%')[1] + ";Password=" + ss[1].Split('%')[1] + ";Database=" + ss[4].Split('%')[1] + ";";
                }

                if(ss.Length >= 9)
                {
                    soureseDefault = ss[5].Split('%')[1];
                    iddefault= int.Parse(ss[5].Split('%')[2]);
                    soureseDefaultB = ss[6].Split('%')[1];
                    iddefaultB = int.Parse(ss[6].Split('%')[2]);
                    defcolor = new SolidColorBrush(Color.FromRgb(byte.Parse(ss[7].Split('%')[1]), byte.Parse(ss[7].Split('%')[2]), byte.Parse(ss[7].Split('%')[3])));
                }

                try
                {
                    
                    NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
                    npgSqlConnection.Open();
                    npgSqlConnection.Close();
                }
                catch (Exception ee)
                {
                    OptionWIN win = new OptionWIN(this, connectionString);
                    win.Grids.IsEnabled = false;
                    this.IsEnabled = false;
                    win.Show();
                }

            }
            else
            {
                OptionWIN win = new OptionWIN(this,connectionString);
                win.Grids.IsEnabled = false;
                this.IsEnabled = false;
                win.Show();
            }
        }
        public string connectionString;
        public List<Point> textures;
        public Point3DCollection rectangleobject = new Point3DCollection();
        public Int32Collection trianglerectangleobject = new Int32Collection();
        public int[] trianglerectangle = {0,2,1, 1,2,3,
                                   4,5,6 ,6,5,7,
                                   8,9,10 ,9,11,10,
                                   12,13,14 ,12,15,13,
                                   16,17,18, 19,18,17,
                                   20,21,22, 22,21,23};
        public ModelUIElement3D selectedobject = null;
        public ModelUIElement3D twoselectedobject = null;
        public ModelUIElement3D threeselectedobject = null;
        public ModelUIElement3D fourselectedobject = null;     
        public  Hashtable CreatedElements = new Hashtable();
        public bool edited = true;
        public double xclickonView;
        public double yclickonView;
        public double zclickonView;
        public ArrayList arrTextScale = new ArrayList();
        public SolidColorBrush defcolor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        public string soureseDefault = @"Images//Венге.jpg";
        public string soureseDefaultB = @"Images//Задняя стена белый.jpg";
        public int iddefault =-1;
        public int iddefaultB = -1;
        public void ClickOnModel(object sender, MouseButtonEventArgs e)
        {
            if (timer.IsEnabled) timer.Stop();
            ElementCabinet tempmodel;
            ModelUIElement3D tt = (ModelUIElement3D)sender;
            tempmodel = CreatedElements[tt.GetHashCode()] as ElementCabinet;
            if (selectedobject == null)
            {
                selectedobject = (ModelUIElement3D)sender;
                edited = false;
                tempmodel = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
            }
            else
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    if (fourselectedobject != null && threeselectedobject != null && twoselectedobject != null)
                    {
                        tempmodel = CreatedElements[fourselectedobject.GetHashCode()] as ElementCabinet;
                    }
                    if (fourselectedobject == null && threeselectedobject != null && twoselectedobject != null)
                    {
                        fourselectedobject = (ModelUIElement3D)sender;
                        edited = false;
                        tempmodel = CreatedElements[fourselectedobject.GetHashCode()] as ElementCabinet;
                        _3DObject tempmodel1 = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
                        _3DObject tempmodel2 = CreatedElements[twoselectedobject.GetHashCode()] as ElementCabinet;
                        _3DObject tempmodel3 = CreatedElements[threeselectedobject.GetHashCode()] as ElementCabinet;
                        if (tempmodel.groupel != tempmodel1.groupel) { fourselectedobject = null; ; }
                        if (tempmodel == tempmodel1) { fourselectedobject = null; }
                        if (tempmodel == tempmodel2) { fourselectedobject = null; }
                        if (tempmodel == tempmodel3) { fourselectedobject = null; }
                    }
                    if (threeselectedobject == null && twoselectedobject != null)
                    {
                        threeselectedobject = (ModelUIElement3D)sender;
                        edited = false;
                        tempmodel = CreatedElements[threeselectedobject.GetHashCode()] as ElementCabinet;
                        _3DObject tempmodel1 = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
                        _3DObject tempmodel2 = CreatedElements[twoselectedobject.GetHashCode()] as _3DObject;
                        if (tempmodel.groupel != tempmodel1.groupel) { threeselectedobject = null; }
                        if (tempmodel == tempmodel1) { threeselectedobject = null; }
                        if (tempmodel == tempmodel2) { threeselectedobject = null; }
                    }
                    if (twoselectedobject == null)
                    {
                        twoselectedobject = (ModelUIElement3D)sender;
                        edited = false;
                        tempmodel = CreatedElements[twoselectedobject.GetHashCode()] as ElementCabinet;
                        _3DObject tempmodel1 = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
                        if (tempmodel.groupel != tempmodel1.groupel) { twoselectedobject = null; }
                        if (tempmodel == tempmodel1) { twoselectedobject = null; }
                    }
                }
                else return;
            }
            if (tempmodel is null) tempmodel = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
            CostOne.IsEnabled = true;
            saveModelButt.IsEnabled = true;
            DeleteElement.IsEnabled = true;           
            DiffuseMaterial texture = tempmodel.thismodel.Material as DiffuseMaterial;
           if(tt==selectedobject|| tt == twoselectedobject || tt == threeselectedobject || tt == fourselectedobject ) if (texture != null) { 
            ImageBrush texture1 = texture.Brush as ImageBrush;
                if (texture1 != null && tempmodel.basevisstatus == false) {
                    texture1.Opacity = 0.5;
                    tempmodel.thismodel.Material = new DiffuseMaterial(texture1);
                }
                else
                {
                    SolidColorBrush texture2 = texture.Brush as SolidColorBrush;
                    if (texture2 != null && tempmodel.basevisstatus == false)
                    {
                        texture2.Opacity = 0.5;
                    }
                }
            }
            else
            {
                tempmodel.thismodel.Material = new DiffuseMaterial(new SolidColorBrush(Colors.White));
            }
            edited = true;
            ElementCabinet tmp11 = tempmodel as ElementCabinet;
            tmp11.outControlElements(ControlElemetsCvs, selectedobject, twoselectedobject, threeselectedobject, fourselectedobject, this);
            foreach (var item in ControlElemetsCvs.Children)
            {
                if (item is TextBox)
                {
                    TextBox tmp = item as TextBox;
                    if (tmp.Tag == null) continue;
                    if (tmp.Tag.ToString() == "axisX") AxisX = tmp;
                    if (tmp.Tag.ToString() == "axisY") AxisY = tmp;
                    if (tmp.Tag.ToString() == "axisZ") AxisZ = tmp;
                    if (tmp.Tag.ToString() == "lenX") LenX = tmp;
                    if (tmp.Tag.ToString() == "lenY") LenY = tmp;
                    if (tmp.Tag.ToString() == "lenZ") LenZ = tmp;
                }
            }
            Transform3DGroup group3d = tmp11.thismodel.Transform as Transform3DGroup;
            TranslateTransform3D t1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            ScaleTransform3D t2 = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            if (tt == selectedobject || tt == twoselectedobject || tt == threeselectedobject || tt == fourselectedobject) if (tmp11.objgr != null)
                if (tmp11.objgr == "W" || tmp11.objgr == "S" || tmp11.objgr == "WBF")
                {
                    TextVisual3D tv1 = new TextVisual3D();
                    tv1.Text = "←" + t2.ScaleZ * 100 + "→";
                    if (tmp11.objgr == "W") tv1.Position = new Point3D(t1.OffsetX + t2.ScaleX / 2 + 0.10, t1.OffsetY + t2.ScaleY / 2, t1.OffsetZ + t2.ScaleZ / 2);
                    if (tmp11.objgr == "S") tv1.Position = new Point3D(t1.OffsetX + t2.ScaleX / 2, t1.OffsetY + t2.ScaleY / 2, t1.OffsetZ + t2.ScaleZ / 2 + 0.20);
                    if (tmp11.objgr == "WBF") tv1.Position = new Point3D(t1.OffsetX + t2.ScaleX / 2, t1.OffsetY + t2.ScaleY / 2 + 0.10, t1.OffsetZ + t2.ScaleZ / 2);
                    tv1.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0.001, 0), 90), new Point3D(tv1.Position.X, tv1.Position.Y, tv1.Position.Z));
                    tv1.Height = 0.1;
                    tv1.FontWeight = System.Windows.FontWeights.Bold;
                    container.Children.Add(tv1);
                    arrTextScale.Add(tv1);
                    TextVisual3D tv2 = new TextVisual3D();
                    tv2.Text = "←" + t2.ScaleX * 100 + "→";
                    if (tmp11.objgr == "W") tv2.Position = new Point3D(t1.OffsetX + t2.ScaleX / 2 - 0.20, t1.OffsetY + t2.ScaleY / 2, t1.OffsetZ + t2.ScaleZ / 2);
                    if (tmp11.objgr == "S") tv2.Position = new Point3D(t1.OffsetX + t2.ScaleX / 2, t1.OffsetY + t2.ScaleY / 2, t1.OffsetZ + t2.ScaleZ / 2 - 0.10);
                    if (tmp11.objgr == "WBF") tv2.Position = new Point3D(t1.OffsetX + t2.ScaleX / 2, t1.OffsetY + t2.ScaleY / 2 + 0.10, t1.OffsetZ + t2.ScaleZ / 2 - 0.20);
                    tv2.Height = 0.1;
                    tv2.FontWeight = System.Windows.FontWeights.Bold;
                    container.Children.Add(tv2);
                    arrTextScale.Add(tv2);
                    TextVisual3D tv3 = new TextVisual3D();
                    tv3.Text = "←" + t2.ScaleY * 100 + "→";
                    if (tmp11.objgr == "W") tv3.Position = new Point3D(t1.OffsetX + t2.ScaleX / 2 - 0.20, t1.OffsetY + t2.ScaleY / 2, t1.OffsetZ + t2.ScaleZ / 2 - 0.1);
                    if (tmp11.objgr == "S") tv3.Position = new Point3D(t1.OffsetX + t2.ScaleX / 2, t1.OffsetY + t2.ScaleY / 2, t1.OffsetZ + t2.ScaleZ / 2 - 0.20);
                    if (tmp11.objgr == "WBF") tv3.Position = new Point3D(t1.OffsetX + t2.ScaleX / 2, t1.OffsetY + t2.ScaleY / 2 + 0.20, t1.OffsetZ + t2.ScaleZ / 2 - 0.40);
                    tv3.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 0.001), 90), new Point3D(tv3.Position.X, tv3.Position.Y, tv3.Position.Z));
                    tv3.Height = 0.1;
                    tv3.FontWeight = System.Windows.FontWeights.Bold;
                    container.Children.Add(tv3);
                    arrTextScale.Add(tv3);
                    tv1.Foreground = Brushes.Red;
                    tv2.Foreground = Brushes.Red;
                    tv3.Foreground = Brushes.Red;
                }
        }
        public void ClickOnCamera(object sender, MouseButtonEventArgs e)
        {
            if (timer.IsEnabled) timer.Stop();
            xclickonView = Camera.CurrentPosition.X;
            yclickonView = Camera.CurrentPosition.Y;
            zclickonView = Camera.CurrentPosition.Z;
            ControlElemetsCvs.Children.Clear();
            saveModelButt.IsEnabled = false;
            DeleteElement.IsEnabled = false;
            CostOne.IsEnabled = false;          
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            { if (fourselectedobject == null) return; }
            ElementCabinet tempmodel = null;
            if (selectedobject != null) tempmodel = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
            selectedobject = null;
            if (tempmodel != null)
            {
                DiffuseMaterial texture = tempmodel.thismodel.Material as DiffuseMaterial;
                if (texture != null)
                {
                    ImageBrush texture1 = texture.Brush as ImageBrush;
                    if (texture1 != null && tempmodel.basevisstatus == false)
                    {
                        texture1.Opacity = 1;
                        tempmodel.thismodel.Material = new DiffuseMaterial(texture1);
                    }
                    else
                    {
                        SolidColorBrush texture2 = texture.Brush as SolidColorBrush;
                        if (texture2 != null && tempmodel.basevisstatus == false)
                        {
                            texture2.Opacity = 1;
                            tempmodel.thismodel.Material = new DiffuseMaterial(texture2);
                        }
                    }
                }
            }
            if (twoselectedobject != null)
            {
                tempmodel = CreatedElements[twoselectedobject.GetHashCode()] as ElementCabinet;
                if (tempmodel != null)
                {
                    twoselectedobject = null;
                    DiffuseMaterial texture = tempmodel.thismodel.Material as DiffuseMaterial;
                    if (texture != null)
                    {
                        ImageBrush texture1 = texture.Brush as ImageBrush;
                        if (texture1 != null && tempmodel.basevisstatus == false)
                        {
                            texture1.Opacity = 1;
                            tempmodel.thismodel.Material = new DiffuseMaterial(texture1);
                        }
                        else
                        {
                            SolidColorBrush texture2 = texture.Brush as SolidColorBrush;
                            if (texture2 != null && tempmodel.basevisstatus == false)
                            {
                                texture2.Opacity = 1;
                                tempmodel.thismodel.Material = new DiffuseMaterial(texture2);
                            }
                        }
                    }
                }
                twoselectedobject = null;
            }
            if (threeselectedobject != null)
            {
                tempmodel = CreatedElements[threeselectedobject.GetHashCode()] as ElementCabinet;
                if (tempmodel != null)
                {
                    threeselectedobject = null;
                    DiffuseMaterial texture = tempmodel.thismodel.Material as DiffuseMaterial;
                    if (texture != null)
                    {
                        ImageBrush texture1 = texture.Brush as ImageBrush;
                        if (texture1 != null && tempmodel.basevisstatus == false)
                        {
                            texture1.Opacity = 1;
                            tempmodel.thismodel.Material = new DiffuseMaterial(texture1);
                        }
                        else
                        {
                            SolidColorBrush texture2 = texture.Brush as SolidColorBrush;
                            if (texture2 != null && tempmodel.basevisstatus == false)
                            {
                                texture2.Opacity = 1;
                                tempmodel.thismodel.Material = new DiffuseMaterial(texture2);
                            }
                        }
                    }
                }
                threeselectedobject = null;
            }
            if (fourselectedobject != null)
            {
                tempmodel = CreatedElements[fourselectedobject.GetHashCode()] as ElementCabinet;
                if (tempmodel != null)
                {
                    fourselectedobject = null;
                    DiffuseMaterial texture = tempmodel.thismodel.Material as DiffuseMaterial;
                    if (texture != null)
                    {
                        ImageBrush texture1 = texture.Brush as ImageBrush;
                        if (texture1 != null && tempmodel.basevisstatus == false)
                        {
                            texture1.Opacity = 1;
                            tempmodel.thismodel.Material = new DiffuseMaterial(texture1);
                        }
                        else
                        {
                            SolidColorBrush texture2 = texture.Brush as SolidColorBrush;
                            if (texture2 != null && tempmodel.basevisstatus == false)
                            {
                                texture2.Opacity = 1;
                                tempmodel.thismodel.Material = new DiffuseMaterial(texture2);
                            }
                        }
                    }
                }
                fourselectedobject = null;
            }
            foreach (object tmp in arrTextScale)
            {
                container.Children.Remove((Visual3D)tmp);
            }
        }
        public TextBox LenX;
        public TextBox LenY;
        public TextBox LenZ;
        public TextBox AxisX;
        public TextBox AxisY;
        public TextBox AxisZ;
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
        public void _TextChangedTH(object sender, TextChangedEventArgs e)
        {
            if (!edited) return;
            if (selectedobject == null) return;
            TextBox tbne = sender as TextBox;
            if (tbne.Text == "") return;
            if (tbne.Text[tbne.Text.Length - 1] == ',') return;
            if (tbne.Text[tbne.Text.Length - 1] == '-') return;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            ElementCabinet tmpel = null;
            if (tbne.Tag.ToString().IndexOf("1") != -1)
            {
                 tmpel = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
            }
            if (tbne.Tag.ToString().IndexOf("2") != -1)
            {
                tmpel = CreatedElements[twoselectedobject.GetHashCode()] as ElementCabinet;
            }
            if (tbne.Tag.ToString().IndexOf("3") != -1)
            {
                tmpel = CreatedElements[threeselectedobject.GetHashCode()] as ElementCabinet;
            }
            if (tbne.Tag.ToString().IndexOf("4") != -1)
            {
                tmpel = CreatedElements[fourselectedobject.GetHashCode()] as ElementCabinet;
            }
            if (tbne.Tag.ToString().IndexOf("Th") != -1) tmpel.thickness = Double.Parse(tbne.Text) / 100;
            if (tbne.Tag.ToString().IndexOf("Base") != -1) tmpel.descreaselen = Double.Parse(tbne.Text) / 100;
            if (tbne.Tag.ToString().IndexOf("Box") != -1)
                foreach (var item in CreatedElements)
                {
                    DictionaryEntry tmp1 = (DictionaryEntry)item;
                    if (tmp1.Value is ElementCabinet)
                    {
                        ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                        if (tmpel.ingroupel == tmp2.ingroupel)
                        {
                            if (tbne.Tag.ToString().IndexOf("lenBox") != -1) tmp2.lenbox = Double.Parse(tbne.Text) / 100;
                            if (tbne.Tag.ToString().IndexOf("otXBox") != -1) tmp2.otsx = Double.Parse(tbne.Text) / 100;
                            if (tbne.Tag.ToString().IndexOf("otYBox") != -1) tmp2.otsy = Double.Parse(tbne.Text) / 100;
                            if (tbne.Tag.ToString().IndexOf("otZBox") != -1) if (Double.Parse(tbne.Text) / 100 < tmp2.lenbox / 2) tmp2.otsz = Double.Parse(tbne.Text) / 100;
                        }
                    }
                }
            for (int ii = 0; ii < 2; ii++)
                if (AxisX.Text != "" && AxisY.Text != "" && AxisZ.Text != "" && LenX.Text != "" && LenY.Text != "" && LenZ.Text != "")
                {
                    if (tempmodel.groupel is null)
                    {
                        tempmodel.changeObjectLen(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100);
                        tempmodel.changeObjectPos(Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                    }
                    else
                    {
                        string[] tmp = tempmodel.groupel.Split('-');
                        for (int i = 0; i < 2; i++)
                            foreach (var item in CreatedElements)
                            {
                                DictionaryEntry tmp1 = (DictionaryEntry)item;
                                if (tmp1.Value is ElementCabinet)
                                {
                                    ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                                    if (tempmodel.groupel == tmp2.groupel)
                                    {
                                        tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                                        tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);

                                    }
                                }
                            }

                    }
                }
            foreach (object tmp in arrTextScale)
            {
                container.Children.Remove((Visual3D)tmp);
            }
        }
        public void _TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!edited) return;
            if (selectedobject == null) return;
            TextBox tbne = sender as TextBox;
            if (tbne.Text == "") return;
            if (tbne.Text[tbne.Text.Length - 1] == ',') return;
            if (tbne.Text[tbne.Text.Length - 1] == '-') return;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            for (int ii = 0; ii < 3; ii++)
                if (AxisX.Text != "" && AxisY.Text != "" && AxisZ.Text != "" && LenX.Text != "" && LenY.Text != "" && LenZ.Text != "")
                {
                    if (tempmodel.groupel is null)
                    {
                        tempmodel.changeObjectLen(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100);
                        tempmodel.changeObjectPos(Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                    }
                    else
                    {
                        string[] tmp = tempmodel.groupel.Split('-');
                        for (int i = 0; i < 2; i++)
                            foreach (var item in CreatedElements)
                            {
                                DictionaryEntry tmp1 = (DictionaryEntry)item;
                                if (tmp1.Value is ElementCabinet)
                                {
                                    ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                                    if (tempmodel.groupel == tmp2.groupel)
                                    {
                                        tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                                        tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                                    }
                                }
                            }
                    }
                }
            foreach (object tmp in arrTextScale)
            {
                container.Children.Remove((Visual3D)tmp);
            }
        }
        public void CreateCab_Click(object sender, RoutedEventArgs e)
        {
            CreateElements ce = new CreateElements(this);
            MenuItem mi = sender as MenuItem;
            if (mi.Tag.ToString() == "MainCabinet-")
                ce.CreateFoundation("MainCabinet-");
            if (mi.Tag.ToString() == "NCabinet-")
                ce.CreateFoundationN("NCabinet-");
        }
        public void CreateShelfG_Click(object sender, RoutedEventArgs e)
        {
            if (selectedobject == null) return;
            Button bt = sender as Button;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            CreateElements ce = new CreateElements(this);
            ce.CreateShaft(tempmodel.groupel, bt.Tag as string, e as MouseButtonEventArgs, zclickonView);
        }
        public void CreateRack_Click(object sender, RoutedEventArgs e)
        {
            if (selectedobject == null) return;
            Button bt = sender as Button;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            CreateElements ce = new CreateElements(this);
            ce.CreateRack(tempmodel.groupel, bt.Tag as string, e as MouseButtonEventArgs, zclickonView, yclickonView);
        }
        public void CreateBox_Click(object sender, RoutedEventArgs e)
        {
            if (selectedobject == null) return;
            Button bt = sender as Button;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            CreateElements ce = new CreateElements(this);
            ce.CreateBox(tempmodel.groupel, bt.Tag as string, e as MouseButtonEventArgs, 0.3, zclickonView);
        }
        public void CreateShelfV_Click(object sender, RoutedEventArgs e)
        {
            if (selectedobject == null) return;
            Button bt = sender as Button;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            CreateElements ce = new CreateElements(this);
            ce.CreateWallC(tempmodel.groupel, bt.Tag as string, e as MouseButtonEventArgs, xclickonView);
        }
        public void SlideRast_ChangeZpos(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!edited) return;
            Slider tmpslider = sender as Slider;
            ElementCabinet tmp11 = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
            if (tmpslider.Tag.ToString().IndexOf("1")!=-1) tmp11 = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
            if (tmpslider.Tag.ToString().IndexOf("2") != -1) tmp11 = CreatedElements[twoselectedobject.GetHashCode()] as ElementCabinet;
            if (tmpslider.Tag.ToString().IndexOf("3") != -1) tmp11 = CreatedElements[threeselectedobject.GetHashCode()] as ElementCabinet;
            if (tmpslider.Tag.ToString().IndexOf("4") != -1) tmp11 = CreatedElements[fourselectedobject.GetHashCode()] as ElementCabinet;
            if (tmpslider.Tag.ToString().IndexOf("W") != -1 || tmpslider.Tag.ToString().IndexOf("Box") != -1 || tmpslider.Tag.ToString().IndexOf("RZ") != -1)
            {
                if (tmp11.ingroupel == "WallC") tmp11.center = tmpslider.Value / 100 ;
                if (tmp11.ingroupel == "ShelfC")
                    tmp11.center = tmpslider.Value / 100;
                string[] tmpstr = tmp11.ingroupel.Split('-');
                if (tmpstr[0] == "Box")
                {
                    foreach (var item in CreatedElements)
                    {
                        DictionaryEntry tmp1 = (DictionaryEntry)item;
                        if (tmp1.Value is ElementCabinet)
                        {
                            ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                            if (tmp11.ingroupel == tmp2.ingroupel)
                            {
                                tmp2.center = tmpslider.Value / 100 ;
                            }
                        }
                    }
                }
            }
            if (tmpslider.Tag.ToString().IndexOf("RY") != -1)
            {
                tmp11.centery = tmpslider.Value / 100 ;
            }
            if (tmpslider.Tag.ToString().IndexOf("RZ") != -1)
            {
                tmp11.center = tmpslider.Value / 100;
            }
            if (tmpslider.Tag.ToString().IndexOf("UL") != -1)
            {
                tmp11.otUL = tmpslider.Value / 100 ;
            }
            if (tmpslider.Tag.ToString().IndexOf("DR") != -1)
            {
                tmp11.otDR = tmpslider.Value / 100;
            }
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            for (int ii = 0; ii < 2; ii++)
                if (AxisX.Text != "" && AxisY.Text != "" && AxisZ.Text != "" && LenX.Text != "" && LenY.Text != "" && LenZ.Text != "")
                {
                    if (tempmodel.groupel is null)
                    {
                        tempmodel.changeObjectLen(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100);
                        tempmodel.changeObjectPos(Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                    }
                    else
                    {
                        string[] tmp = tempmodel.groupel.Split('-');
                        for (int i = 0; i < 2; i++)
                            foreach (var item in CreatedElements)
                            {
                                DictionaryEntry tmp1 = (DictionaryEntry)item;
                                if (tmp1.Value is ElementCabinet)
                                {

                                    ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                                    if (tempmodel.groupel == tmp2.groupel)
                                    {
                                        tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                                        tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                                    }
                                }
                            }
                    }
                }
            foreach (object tmp in arrTextScale)
            {
                container.Children.Remove((Visual3D)tmp);
            }
        }
        public void SlideElement_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!edited) return;
            Slider tmpslider = sender as Slider;
            ElementCabinet tmp11 = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
            if (tmpslider.Tag.ToString().IndexOf("1") != -1) tmp11 = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
            if (tmpslider.Tag.ToString().IndexOf("2") != -1) tmp11 = CreatedElements[twoselectedobject.GetHashCode()] as ElementCabinet;
            if (tmpslider.Tag.ToString().IndexOf("3") != -1) tmp11 = CreatedElements[threeselectedobject.GetHashCode()] as ElementCabinet;
            if (tmpslider.Tag.ToString().IndexOf("4") != -1) tmp11 = CreatedElements[fourselectedobject.GetHashCode()] as ElementCabinet;
            foreach (var item in CreatedElements)
            {
                DictionaryEntry tmp1 = (DictionaryEntry)item;
                if (tmp1.Value is ElementCabinet)
                {
                    ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                    if (tmp11.ingroupel == tmp2.ingroupel)
                    {
                        if (tmpslider.Tag.ToString().IndexOf("M")!=-1){
                            tmp2.changeObjectElln(tmpslider.Value / 100, tmpslider.Value / 100, tmpslider.Value / 100);
                        }
                        if (tmpslider.Tag.ToString().IndexOf("P") != -1)
                        {
                            int anX = tmp2.getAngleX();
                            int anY = tmp2.getAngleY();
                            int anZ = tmp2.getAngleZ();
                            tmp2.ChangeAngleX(0);
                            tmp2.ChangeAngleY(0);
                            tmp2.ChangeAngleZ(0);
                            if (tmpslider.Tag.ToString().IndexOf("PX") != -1) { tmp2.changeObjectElaxX(tmpslider.Value / 100 + tmp11.select.getPosX()); tmp2.otsx = tmpslider.Value / 100; }
                            if (tmpslider.Tag.ToString().IndexOf("PY") != -1) { tmp2.changeObjectElaxY(tmpslider.Value / 100 + tmp11.select.getPosY()); tmp2.otsy = tmpslider.Value / 100; }
                            if (tmpslider.Tag.ToString().IndexOf("PZ") != -1) { tmp2.changeObjectElaxZ(tmpslider.Value / 100 + tmp11.select.getPosZ()); tmp2.otsz = tmpslider.Value / 100; }
                            tmp2.ChangeAngleX(anX);
                            tmp2.ChangeAngleY(anY);
                            tmp2.ChangeAngleZ(anZ);
                        }                      
                        if (tmpslider.Tag.ToString().IndexOf("AX") != -1)
                        {
                            int anX = tmp2.getAngleX();
                            int anY = tmp2.getAngleY();
                            int anZ = tmp2.getAngleZ();
                            tmp2.ChangeAngleX(0);
                            tmp2.ChangeAngleY(0);
                            tmp2.ChangeAngleZ(0);
                            tmp2.ChangeAngleX(Convert.ToInt32(tmpslider.Value));
                            tmp2.ChangeAngleY(anY);
                            tmp2.ChangeAngleZ(anZ);
                        }
                        if (tmpslider.Tag.ToString().IndexOf("AY") != -1)
                        {
                            int anX = tmp2.getAngleX();
                            int anY = tmp2.getAngleY();
                            int anZ = tmp2.getAngleZ();
                            tmp2.ChangeAngleX(0);
                            tmp2.ChangeAngleY(0);
                            tmp2.ChangeAngleZ(0);
                            tmp2.ChangeAngleY(Convert.ToInt32(tmpslider.Value));
                            tmp2.ChangeAngleX(anX);
                            tmp2.ChangeAngleZ(anZ);
                        }
                        if (tmpslider.Tag.ToString().IndexOf("AZ") != -1)
                        {
                            int anX = tmp2.getAngleX();
                            int anY = tmp2.getAngleY();
                            int anZ = tmp2.getAngleZ();
                            tmp2.ChangeAngleX(0);
                            tmp2.ChangeAngleY(0);
                            tmp2.ChangeAngleZ(0);
                            tmp2.ChangeAngleZ(Convert.ToInt32(tmpslider.Value));
                            tmp2.ChangeAngleX(anX);
                            tmp2.ChangeAngleY(anY);
                        }
                    }
                }
            }
        }
        public void DeleteElement_Click(object sender, RoutedEventArgs e)
        {
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            string[] tmpstr = tempmodel.ingroupel.Split('-');
            if (tempmodel.ingroupel == "ShelfC" || tempmodel.ingroupel == "WallC" || tempmodel.ingroupel == "Door" || tempmodel.ingroupel == "Rack")
            {
                ElementCabinet tmp1 = tempmodel as ElementCabinet;
                CreatedElements.Remove(selectedobject.GetHashCode());
                container.Children.Remove(tmp1.thisobject);
                ElementCabinet deleteel = null;
                bool b = true;
                while (b) {
                    foreach (var item in CreatedElements)
                    {
                        DictionaryEntry tmp2 = (DictionaryEntry)item;
                        if (tmp2.Value is ElementCabinet)
                        {
                            ElementCabinet tmp3 = tmp2.Value as ElementCabinet;

                            if (tmp3.ingroupel.IndexOf("Element") != -1)
                            { 
                                if (tmp3.select != null)
                                    if (tmp3.select.Equals(tmp1))
                                    {
                                        deleteel = tmp3;
                                        break;
                                        
                                    }
                            }
                        }
                        b = false;
                    }
                    if (deleteel != null)
                    {
                        CreatedElements.Remove(deleteel.thisobject.GetHashCode());
                        container.Children.Remove(deleteel.thisobject);
                        deleteel = null;
                    }
                }

                foreach (var item in CreatedElements)
                {
                    DictionaryEntry tmp2 = (DictionaryEntry)item;
                    if (tmp2.Value is ElementCabinet)
                    {
                        ElementCabinet tmp3 = tmp2.Value as ElementCabinet;
                        if (tmp3.ingroupel == "ShelfC" || tmp3.ingroupel == "WallC" || tmpstr[0] == "Box" || tmpstr[0] == "Door")
                        {
                            if (tmp3.select != null)
                                if (tmp3.select.Equals(tmp1)) tmp3.select = null;
                            if (tmp3.twoselect != null)
                                if (tmp3.twoselect.Equals(tmp1)) tmp3.twoselect = null;
                            if (tmp3.threeselect != null)
                                if (tmp3.threeselect.Equals(tmp1)) tmp3.threeselect = null;
                            if (tmp3.fourselect != null)
                                if (tmp3.fourselect.Equals(tmp1)) tmp3.fourselect = null;
                        }
                    }

                }

                for (int ii = 0; ii < 2; ii++)
                    if (AxisX.Text != "" && AxisY.Text != "" && AxisZ.Text != "" && LenX.Text != "" && LenY.Text != "" && LenZ.Text != "")
                    {
                        if (tempmodel.groupel is null)
                        {
                            tempmodel.changeObjectLen(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100);
                            tempmodel.changeObjectPos(Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                        }
                        else
                        {
                            string[] tmp = tempmodel.groupel.Split('-');
                            for (int i = 0; i < 2; i++)
                                foreach (var item in CreatedElements)
                                {
                                    DictionaryEntry tmp11 = (DictionaryEntry)item;
                                    if (tmp11.Value is ElementCabinet)
                                    {

                                        ElementCabinet tmp2 = tmp11.Value as ElementCabinet;
                                        if (tempmodel.groupel == tmp2.groupel)
                                        {
                                            tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                                            tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                                        }
                                    }
                                }
                        }
                    }
                selectedobject = null;
                ClickOnCamera(sender, e as MouseButtonEventArgs);
            }
            else
            if (tmpstr[0] == "Box"|| tmpstr[0] == "Element"|| tmpstr[0] == "Door")
            {
                int kolv = 0;
                int kolvel = 0;


                foreach (var item in CreatedElements)
                {
                    DictionaryEntry tmp2 = (DictionaryEntry)item;
                    ElementCabinet tmp3 = tmp2.Value as ElementCabinet;
                    if (tempmodel.ingroupel == tmp3.ingroupel) {
                        kolv++;
                        foreach (var item1 in CreatedElements)
                        {
                            DictionaryEntry tmp21 = (DictionaryEntry)item1;
                            ElementCabinet tmp31 = tmp21.Value as ElementCabinet;
                            if (tmp31.ingroupel.IndexOf("Element") != -1) if (tmp31.select.thisobject == tmp3.thisobject) kolvel++;
                        }
                        }
                    if(tmp3.ingroupel.IndexOf("Element")!=-1)if(tmp3.select.thisobject == tempmodel.thisobject) kolvel++; 
                }

                while (kolvel != 0){
                    foreach (var item in CreatedElements)
                    {
                        DictionaryEntry tmp2 = (DictionaryEntry)item;
                        ElementCabinet tmp3 = tmp2.Value as ElementCabinet;
                        if (tmp3.ingroupel.IndexOf("Element") != -1)
                            if (tmp3.select.thisobject == tempmodel.thisobject)
                            {
                                CreatedElements.Remove(tmp3.thisobject.GetHashCode());
                                container.Children.Remove(tmp3.thisobject);
                                break;
                            }
                        if (tempmodel.ingroupel == tmp3.ingroupel)
                        {
                            bool b = false;
                            foreach (var item1 in CreatedElements)
                            {
                                DictionaryEntry tmp21 = (DictionaryEntry)item1;
                                ElementCabinet tmp31 = tmp21.Value as ElementCabinet;
                                if (tmp31.ingroupel.IndexOf("Element") != -1) if (tmp31.select.thisobject == tmp3.thisobject)
                                    {
                                        CreatedElements.Remove(tmp31.thisobject.GetHashCode());
                                        container.Children.Remove(tmp31.thisobject);
                                        b = true;
                                        break;
                                    }
                            }
                            if (b) break;
                        }
                    }
                    kolvel = 0;
                    foreach (var item in CreatedElements)
                    {
                        DictionaryEntry tmp2 = (DictionaryEntry)item;
                        ElementCabinet tmp3 = tmp2.Value as ElementCabinet;
                        if (tempmodel.ingroupel == tmp3.ingroupel)
                        {
                            foreach (var item1 in CreatedElements)
                            {
                                DictionaryEntry tmp21 = (DictionaryEntry)item1;
                                ElementCabinet tmp31 = tmp21.Value as ElementCabinet;
                                if (tmp31.ingroupel.IndexOf("Element") != -1) if (tmp31.select.thisobject == tmp3.thisobject) kolvel++;
                            }
                        }
                        if (tmp3.ingroupel.IndexOf("Element") != -1) if (tmp3.select.thisobject == tempmodel.thisobject) kolvel++;
                    }
                }


                while (kolv != 0)
                {
                    foreach (var item in CreatedElements)
                    {
                        DictionaryEntry tmp2 = (DictionaryEntry)item;
                        ElementCabinet tmp3 = tmp2.Value as ElementCabinet;
                        if (tempmodel.ingroupel == tmp3.ingroupel )
                            {
                            CreatedElements.Remove(tmp3.thisobject.GetHashCode());
                            container.Children.Remove(tmp3.thisobject);
                            break;
                        }
                    }
                    kolv = 0;
                    foreach (var item in CreatedElements)
                    {
                        DictionaryEntry tmp2 = (DictionaryEntry)item;
                        ElementCabinet tmp3 = tmp2.Value as ElementCabinet;
                        if (tempmodel.ingroupel == tmp3.ingroupel)
                        {
                            kolv++;
                        }
                    }

                }
                ClickOnCamera(sender, e as MouseButtonEventArgs);
            }
            else
            {
                int kolv = 0;
                foreach (var item in CreatedElements)
                {
                    DictionaryEntry tmp2 = (DictionaryEntry)item;
                    _3DObject tmp3 = tmp2.Value as _3DObject;
                    if (tempmodel.groupel == tmp3.groupel) kolv++;
                }
                while (kolv != 0)
                {
                    foreach (var item in CreatedElements)
                    {
                        DictionaryEntry tmp2 = (DictionaryEntry)item;
                        _3DObject tmp3 = tmp2.Value as _3DObject;
                        if (tempmodel.groupel == tmp3.groupel)
                        {
                            CreatedElements.Remove(tmp3.thisobject.GetHashCode());
                            container.Children.Remove(tmp3.thisobject);
                            break;
                        }
                    }
                    kolv = 0;
                    foreach (var item in CreatedElements)
                    {
                        DictionaryEntry tmp2 = (DictionaryEntry)item;
                        _3DObject tmp3 = tmp2.Value as _3DObject;
                        if (tempmodel.groupel == tmp3.groupel) kolv++;
                    }
                }
                ClickOnCamera(sender, e as MouseButtonEventArgs);
            }
        }
        public void ChangeTexture_Click(object sender, RoutedEventArgs e)
        {
            Button tmpbt = sender as Button;
            MaterialsWIN win = new MaterialsWIN(this, tmpbt,connectionString);
            win.Show();
            this.IsEnabled = false;
        }
        public void ChangeTexture_(Button sender, String filename, int idtexture)
        {
            Button tmpbt = sender as Button;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            if (tmpbt.Tag == "All" || tmpbt.Tag == "All1") tempmodel.changeTexture(filename, idtexture);
            if (twoselectedobject != null && (tmpbt.Tag == "All" || tmpbt.Tag == "All2"))
            {
                tempmodel = CreatedElements[twoselectedobject.GetHashCode()] as _3DObject;
                tempmodel.changeTexture(filename, idtexture);
            }
            if (threeselectedobject != null && (tmpbt.Tag == "All" || tmpbt.Tag == "All3"))
            {
                tempmodel = CreatedElements[threeselectedobject.GetHashCode()] as _3DObject;
                tempmodel.changeTexture(filename, idtexture);
            }
            if (fourselectedobject != null && (tmpbt.Tag == "All" || tmpbt.Tag == "All4"))
            {
                tempmodel = CreatedElements[fourselectedobject.GetHashCode()] as _3DObject;
                tempmodel.changeTexture(filename, idtexture);
            }
        }
        public void CreateShelfVG_Click(object sender, RoutedEventArgs e)
        {
            if (selectedobject == null || twoselectedobject == null) return;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel1 = CreatedElements[twoselectedobject.GetHashCode()] as _3DObject;
            CreateElements ce = new CreateElements(this);
            if (tempmodel.ingroupel == "ShelfC" || tempmodel.ingroupel == "Floor" || tempmodel.ingroupel == "Ceiling" || tempmodel.ingroupel == "FloorFoot")
                if (tempmodel1.ingroupel == "ShelfC" || tempmodel1.ingroupel == "Floor" || tempmodel1.ingroupel == "Ceiling" || tempmodel1.ingroupel == "FloorFoot")
                {
                    ce.CreateWallC(tempmodel.groupel, tempmodel.takePositionV(tempmodel1), e as MouseButtonEventArgs, xclickonView);
                }

            if (tempmodel.ingroupel == "WallC" || tempmodel.ingroupel == "WallR" || tempmodel.ingroupel == "WallL")
                if (tempmodel1.ingroupel == "WallC" || tempmodel1.ingroupel == "WallR" || tempmodel1.ingroupel == "WallL")
                {
                    ce.CreateShaft(tempmodel.groupel, tempmodel.takePositionG(tempmodel1), e as MouseButtonEventArgs, zclickonView);
                }
        }
        public void CreateBOX_Click(object sender, RoutedEventArgs e)
        {
            if (selectedobject == null || twoselectedobject == null) return;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel1 = CreatedElements[twoselectedobject.GetHashCode()] as _3DObject;
            CreateElements ce = new CreateElements(this);
            if (tempmodel.ingroupel == "WallC" || tempmodel.ingroupel == "WallR" || tempmodel.ingroupel == "WallL")
                if (tempmodel1.ingroupel == "WallC" || tempmodel1.ingroupel == "WallR" || tempmodel1.ingroupel == "WallL")
                {
                    ce.CreateBox(tempmodel.groupel, tempmodel.takePositionG(tempmodel1), e as MouseButtonEventArgs, 0.3, zclickonView);
                }
        }
        public void CreateRackTWOSel_Click(object sender, RoutedEventArgs e)
        {
            if (selectedobject == null || twoselectedobject == null) return;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel1 = CreatedElements[twoselectedobject.GetHashCode()] as _3DObject;
            CreateElements ce = new CreateElements(this);
            if (tempmodel.ingroupel == "WallC" || tempmodel.ingroupel == "WallR" || tempmodel.ingroupel == "WallL")
                if (tempmodel1.ingroupel == "WallC" || tempmodel1.ingroupel == "WallR" || tempmodel1.ingroupel == "WallL")
                {
                    ce.CreateRack(tempmodel.groupel, tempmodel.takePositionG(tempmodel1), e as MouseButtonEventArgs, zclickonView, yclickonView);
                }
        }
        public void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox tbne = sender as CheckBox;
            if (tbne.Tag.ToString().IndexOf("1") !=-1)
            {
                ElementCabinet tmp11 = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
                if (tbne.Tag.ToString().IndexOf("B") != -1) tmp11.basewallstatus = false;
                if (tbne.Tag.ToString().IndexOf("V") != -1) {
                    DiffuseMaterial texture11 = tmp11.thismodel.Material as DiffuseMaterial;
                    ImageBrush texture22 = texture11.Brush as ImageBrush;
                    texture22.Opacity = 0.5;
                    tmp11.basevisstatus = false;
                }
            }
            if (tbne.Tag.ToString().IndexOf("2") != -1)
            {
                ElementCabinet tmp11 = CreatedElements[twoselectedobject.GetHashCode()] as ElementCabinet;
                if (tbne.Tag.ToString().IndexOf("B") != -1) tmp11.basewallstatus = false;
                if (tbne.Tag.ToString().IndexOf("V") != -1)
                {
                    DiffuseMaterial texture11 = tmp11.thismodel.Material as DiffuseMaterial;
                    ImageBrush texture22 = texture11.Brush as ImageBrush;
                    texture22.Opacity = 0.5;
                    tmp11.basevisstatus = false;
                }
            }
            if (tbne.Tag.ToString().IndexOf("3") != -1)
            {
                ElementCabinet tmp11 = CreatedElements[threeselectedobject.GetHashCode()] as ElementCabinet;
                if (tbne.Tag.ToString().IndexOf("B") != -1) tmp11.basewallstatus = false;
                if (tbne.Tag.ToString().IndexOf("V") != -1)
                {
                    DiffuseMaterial texture11 = tmp11.thismodel.Material as DiffuseMaterial;
                    ImageBrush texture22 = texture11.Brush as ImageBrush;
                    texture22.Opacity = 0.5;
                    tmp11.basevisstatus = false;
                }
            }
            if (tbne.Tag.ToString().IndexOf("4") != -1)
            {
                ElementCabinet tmp11 = CreatedElements[fourselectedobject.GetHashCode()] as ElementCabinet;
                if (tbne.Tag.ToString().IndexOf("B") != -1) tmp11.basewallstatus = false;
                if (tbne.Tag.ToString().IndexOf("V") != -1)
                {
                    DiffuseMaterial texture11 = tmp11.thismodel.Material as DiffuseMaterial;
                    ImageBrush texture22 = texture11.Brush as ImageBrush;
                    texture22.Opacity = 0.5;
                    tmp11.basevisstatus = false;
                }
            }
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            for (int ii = 0; ii < 2; ii++)
                if (AxisX.Text != "" && AxisY.Text != "" && AxisZ.Text != "" && LenX.Text != "" && LenY.Text != "" && LenZ.Text != "")
                {
                    if (tempmodel.groupel is null)
                    {
                        tempmodel.changeObjectLen(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100);
                        tempmodel.changeObjectPos(Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                    }
                    else
                    {
                        string[] tmp = tempmodel.groupel.Split('-');
                        for (int i = 0; i < 2; i++)
                            foreach (var item in CreatedElements)
                            {
                                DictionaryEntry tmp1 = (DictionaryEntry)item;
                                if (tmp1.Value is ElementCabinet)
                                {

                                    ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                                    if (tempmodel.groupel == tmp2.groupel)
                                    {
                                        tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                                        tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);

                                    }
                                }

                            }
                    }
                }
        }
        public void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox tbne = sender as CheckBox;
            if (tbne.Tag.ToString().IndexOf("1") != -1)
            {
                ElementCabinet tmp11 = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
                if (tbne.Tag.ToString().IndexOf("B") != -1) tmp11.basewallstatus = true;
                if (tbne.Tag.ToString().IndexOf("V") != -1)
                {
                    DiffuseMaterial texture11 = tmp11.thismodel.Material as DiffuseMaterial;
                    ImageBrush texture22 = texture11.Brush as ImageBrush;
                    texture22.Opacity = 0.1;
                    tmp11.basevisstatus = true;
                }
            }
            if (tbne.Tag.ToString().IndexOf("2") != -1)
            {
                ElementCabinet tmp11 = CreatedElements[twoselectedobject.GetHashCode()] as ElementCabinet;
                if (tbne.Tag.ToString().IndexOf("B") != -1) tmp11.basewallstatus = true;
                {
                    DiffuseMaterial texture11 = tmp11.thismodel.Material as DiffuseMaterial;
                    ImageBrush texture22 = texture11.Brush as ImageBrush;
                    texture22.Opacity = 0.1;
                    tmp11.basevisstatus = true;
                }
            }
            if (tbne.Tag.ToString().IndexOf("3") != -1)
            {
                ElementCabinet tmp11 = CreatedElements[threeselectedobject.GetHashCode()] as ElementCabinet;
                if (tbne.Tag.ToString().IndexOf("B") != -1) tmp11.basewallstatus = true;
                {
                    DiffuseMaterial texture11 = tmp11.thismodel.Material as DiffuseMaterial;
                    ImageBrush texture22 = texture11.Brush as ImageBrush;
                    texture22.Opacity = 0.1;
                    tmp11.basevisstatus = true;
                }
            }
            if (tbne.Tag.ToString().IndexOf("4") != -1)
            {
                ElementCabinet tmp11 = CreatedElements[fourselectedobject.GetHashCode()] as ElementCabinet;
                if (tbne.Tag.ToString().IndexOf("B") != -1) tmp11.basewallstatus = true;
                {
                    DiffuseMaterial texture11 = tmp11.thismodel.Material as DiffuseMaterial;
                    ImageBrush texture22 = texture11.Brush as ImageBrush;
                    texture22.Opacity = 0.1;
                    tmp11.basevisstatus = true;
                }
            }
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            for (int ii = 0; ii < 2; ii++)
                if (AxisX.Text != "" && AxisY.Text != "" && AxisZ.Text != "" && LenX.Text != "" && LenY.Text != "" && LenZ.Text != "")
                {
                    if (tempmodel.groupel is null)
                    {
                        tempmodel.changeObjectLen(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100);
                        tempmodel.changeObjectPos(Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                    }
                    else
                    {
                        string[] tmp = tempmodel.groupel.Split('-');
                        for (int i = 0; i < 2; i++)
                            foreach (var item in CreatedElements)
                            {
                                DictionaryEntry tmp1 = (DictionaryEntry)item;
                                if (tmp1.Value is ElementCabinet)
                                {

                                    ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                                    if (tempmodel.groupel == tmp2.groupel)
                                    {
                                        tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);
                                        tmp2.changeObject(Convert.ToDouble(LenX.Text) / 100, Convert.ToDouble(LenY.Text) / 100, Convert.ToDouble(LenZ.Text) / 100, Convert.ToDouble(AxisX.Text) / 100, Convert.ToDouble(AxisY.Text) / 100, Convert.ToDouble(AxisZ.Text) / 100);

                                    }
                                }

                            }

                    }
                }
        }
        public TextBox CountB;
        public void CreateDOOR_Click(object sender, RoutedEventArgs e)
        {
            if (selectedobject == null || twoselectedobject == null || threeselectedobject == null || fourselectedobject == null) return;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel1 = CreatedElements[twoselectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel2 = CreatedElements[threeselectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel3 = CreatedElements[fourselectedobject.GetHashCode()] as _3DObject;
            Button tmpb = sender as Button;
            CreateElements ce = new CreateElements(this);
            switch (tempmodel.objgr)
            {
                case "W":
                    switch (tempmodel1.objgr)
                    {
                        case "W":
                            switch (tempmodel2.objgr)
                            {     
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "S": {
                                                if(tmpb.Tag=="1")ce.CreateDoor(tempmodel.groupel, tempmodel2.takePositionV(tempmodel3), tempmodel.takePositionG(tempmodel1), e as MouseButtonEventArgs, "2301"); if (tmpb.Tag == "2") ce.CreateDoorTwo(tempmodel.groupel, tempmodel2.takePositionV(tempmodel3), tempmodel.takePositionG(tempmodel1), e as MouseButtonEventArgs, "2301"); break;

                                            }
                                    }
                                    break;
                            }
                            break;
                        case "S":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "S": { if (tmpb.Tag == "1")ce.CreateDoor(tempmodel.groupel, tempmodel1.takePositionV(tempmodel3), tempmodel.takePositionG(tempmodel2), e as MouseButtonEventArgs, "1302"); if (tmpb.Tag == "2") ce.CreateDoorTwo(tempmodel.groupel, tempmodel1.takePositionV(tempmodel3), tempmodel.takePositionG(tempmodel2), e as MouseButtonEventArgs, "1302"); break; }
                                    }
                                    break;
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { if (tmpb.Tag == "1") ce.CreateDoor(tempmodel.groupel, tempmodel1.takePositionV(tempmodel2), tempmodel.takePositionG(tempmodel3), e as MouseButtonEventArgs, "1203"); if (tmpb.Tag == "2") ce.CreateDoorTwo(tempmodel.groupel, tempmodel1.takePositionV(tempmodel2), tempmodel.takePositionG(tempmodel3), e as MouseButtonEventArgs, "1203"); break; }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case "S":
                    switch (tempmodel1.objgr)
                    {
                        case "W":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "S": { if (tmpb.Tag == "1") ce.CreateDoor(tempmodel.groupel, tempmodel.takePositionV(tempmodel3), tempmodel2.takePositionG(tempmodel1), e as MouseButtonEventArgs, "0321"); if (tmpb.Tag == "2") ce.CreateDoorTwo(tempmodel.groupel, tempmodel.takePositionV(tempmodel3), tempmodel2.takePositionG(tempmodel1), e as MouseButtonEventArgs, "0321"); break; }
                                    }
                                    break;
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { if (tmpb.Tag == "1") ce.CreateDoor(tempmodel.groupel, tempmodel2.takePositionV(tempmodel), tempmodel3.takePositionG(tempmodel1), e as MouseButtonEventArgs, "2031"); if (tmpb.Tag == "2") ce.CreateDoorTwo(tempmodel.groupel, tempmodel2.takePositionV(tempmodel), tempmodel3.takePositionG(tempmodel1), e as MouseButtonEventArgs, "2031"); break; }
                                    }
                                    break;
                            }
                            break;
                        case "S":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { if (tmpb.Tag == "1") ce.CreateDoor(tempmodel.groupel, tempmodel.takePositionV(tempmodel1), tempmodel2.takePositionG(tempmodel3), e as MouseButtonEventArgs, "0123"); if (tmpb.Tag == "2") ce.CreateDoorTwo(tempmodel.groupel, tempmodel.takePositionV(tempmodel1), tempmodel2.takePositionG(tempmodel3), e as MouseButtonEventArgs, "0123"); break; }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
        public void CreateNW_Click(object sender, RoutedEventArgs e)
        {
            if (selectedobject == null || twoselectedobject == null || threeselectedobject == null || fourselectedobject == null) return;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel1 = CreatedElements[twoselectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel2 = CreatedElements[threeselectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel3 = CreatedElements[fourselectedobject.GetHashCode()] as _3DObject;
            CreateElements ce = new CreateElements(this);
            switch (tempmodel.objgr)
            {
                case "W":
                    switch (tempmodel1.objgr)
                    {
                        case "W":
                            switch (tempmodel2.objgr)
                            {
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "S": { ce.CreateCountW(tempmodel.groupel, tempmodel2.takePositionV(tempmodel3), tempmodel.takePositionG(tempmodel1), e as MouseButtonEventArgs, "2301"); break; }
                                    }
                                    break;
                            }
                            break;
                        case "S":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "S": { ce.CreateCountW(tempmodel.groupel, tempmodel1.takePositionV(tempmodel3), tempmodel.takePositionG(tempmodel2), e as MouseButtonEventArgs, "1302"); break; }
                                    }
                                    break;
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { ce.CreateCountW(tempmodel.groupel, tempmodel1.takePositionV(tempmodel2), tempmodel.takePositionG(tempmodel3), e as MouseButtonEventArgs, "1203"); break; }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case "S":
                    switch (tempmodel1.objgr)
                    {
                        case "W":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "S": { ce.CreateCountW(tempmodel.groupel, tempmodel.takePositionV(tempmodel3), tempmodel2.takePositionG(tempmodel1), e as MouseButtonEventArgs, "0321"); break; }
                                    }
                                    break;
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { ce.CreateCountW(tempmodel.groupel, tempmodel2.takePositionV(tempmodel), tempmodel3.takePositionG(tempmodel1), e as MouseButtonEventArgs, "2031"); break; }
                                    }
                                    break;
                            }
                            break;
                        case "S":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { ce.CreateCountW(tempmodel.groupel, tempmodel.takePositionV(tempmodel1), tempmodel2.takePositionG(tempmodel3), e as MouseButtonEventArgs, "0123"); break; }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
        public void CreateNS_Click(object sender, RoutedEventArgs e)
        {
            if (selectedobject == null || twoselectedobject == null || threeselectedobject == null || fourselectedobject == null) return;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel1 = CreatedElements[twoselectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel2 = CreatedElements[threeselectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel3 = CreatedElements[fourselectedobject.GetHashCode()] as _3DObject;
            CreateElements ce = new CreateElements(this);
            switch (tempmodel.objgr)
            {
                case "W":
                    switch (tempmodel1.objgr)
                    {
                        case "W":
                            switch (tempmodel2.objgr)
                            {
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": break;
                                        case "S": { ce.CreateCountS(tempmodel.groupel, tempmodel2.takePositionV(tempmodel3), tempmodel.takePositionG(tempmodel1), e as MouseButtonEventArgs, "2301"); break; }
                                    }
                                    break;
                            }
                            break;
                        case "S":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "S": { ce.CreateCountS(tempmodel.groupel, tempmodel1.takePositionV(tempmodel3), tempmodel.takePositionG(tempmodel2), e as MouseButtonEventArgs, "1302"); break; }
                                    }
                                    break;
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { ce.CreateCountS(tempmodel.groupel, tempmodel1.takePositionV(tempmodel2), tempmodel.takePositionG(tempmodel3), e as MouseButtonEventArgs, "1203"); break; }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case "S":
                    switch (tempmodel1.objgr)
                    {
                        case "W":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "S": { ce.CreateCountS(tempmodel.groupel, tempmodel.takePositionV(tempmodel3), tempmodel2.takePositionG(tempmodel1), e as MouseButtonEventArgs, "0321"); break; }
                                    }
                                    break;
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { ce.CreateCountS(tempmodel.groupel, tempmodel2.takePositionV(tempmodel), tempmodel3.takePositionG(tempmodel1), e as MouseButtonEventArgs, "2031"); break; }
                                    }
                                    break;
                            }
                            break;
                        case "S":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { ce.CreateCountS(tempmodel.groupel, tempmodel.takePositionV(tempmodel1), tempmodel2.takePositionG(tempmodel3), e as MouseButtonEventArgs, "0123"); break; }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
        public void CreateNB_Click(object sender, RoutedEventArgs e)
        {
            if (selectedobject == null || twoselectedobject == null || threeselectedobject == null || fourselectedobject == null) return;
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel1 = CreatedElements[twoselectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel2 = CreatedElements[threeselectedobject.GetHashCode()] as _3DObject;
            _3DObject tempmodel3 = CreatedElements[fourselectedobject.GetHashCode()] as _3DObject;
            CreateElements ce = new CreateElements(this);
            switch (tempmodel.objgr)
            {
                case "W":
                    switch (tempmodel1.objgr)
                    {
                        case "W":
                            switch (tempmodel2.objgr)
                            {
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "S": { ce.CreateCountB(tempmodel.groupel, tempmodel2.takePositionV(tempmodel3), tempmodel.takePositionG(tempmodel1), e as MouseButtonEventArgs, "2301"); break; }
                                    }
                                    break;
                            }
                            break;
                        case "S":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "S": { ce.CreateCountB(tempmodel.groupel, tempmodel1.takePositionV(tempmodel3), tempmodel.takePositionG(tempmodel2), e as MouseButtonEventArgs, "1302"); break; }
                                    }
                                    break;
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { ce.CreateCountS(tempmodel.groupel, tempmodel1.takePositionV(tempmodel2), tempmodel.takePositionG(tempmodel3), e as MouseButtonEventArgs, "1203"); break; }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case "S":
                    switch (tempmodel1.objgr)
                    {
                        case "W":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "S": { ce.CreateCountB(tempmodel.groupel, tempmodel.takePositionV(tempmodel3), tempmodel2.takePositionG(tempmodel1), e as MouseButtonEventArgs, "0321"); break; }
                                    }
                                    break;
                                case "S":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { ce.CreateCountB(tempmodel.groupel, tempmodel2.takePositionV(tempmodel), tempmodel3.takePositionG(tempmodel1), e as MouseButtonEventArgs, "2031"); break; }
                                    }
                                    break;
                            }
                            break;
                        case "S":
                            switch (tempmodel2.objgr)
                            {
                                case "W":
                                    switch (tempmodel3.objgr)
                                    {
                                        case "W": { ce.CreateCountB(tempmodel.groupel, tempmodel.takePositionV(tempmodel1), tempmodel2.takePositionG(tempmodel3), e as MouseButtonEventArgs, "0123"); break; }
                                        case "S": break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
        public void SaveCab_Click(object sender, RoutedEventArgs e)
        {
            _3DObject tempmodel = CreatedElements[selectedobject.GetHashCode()] as _3DObject;
            ElementCabinet tmp1 = tempmodel as ElementCabinet;
            ArrayList tempcont = new ArrayList();
            ArrayList tempcont1 = new ArrayList();
            foreach (var item in container.Children)
            {
                if (item is ModelUIElement3D tmp2)
                {
                    ElementCabinet tmp3 = CreatedElements[tmp2.GetHashCode()] as ElementCabinet;
                    if (tmp3.groupel == tmp1.groupel)
                    {
                        if (tmp3.ingroupel.IndexOf("Element") == -1)
                            tempcont.Add(tmp2);
                        tempcont1.Add(tmp2);
                    }
                }
            }
            string[] str = new string[tempcont1.Count];
            int ind = 0;
            foreach (var item in tempcont1)
            {
                if (item is ModelUIElement3D tmp2)
                {
                    ElementCabinet tmp3 = CreatedElements[tmp2.GetHashCode()] as ElementCabinet;
                    if (tmp3.groupel == tmp1.groupel)
                    {
                        if (tmp3.ingroupel == "WallL")
                        {
                            str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                        }
                        if (tmp3.ingroupel == "WallR")
                        {
                            str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                        }
                        if (tmp3.ingroupel == "WallB")
                        {
                            str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                        }
                        if (tmp3.ingroupel == "WallF")
                        {
                            str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                        }
                        if (tmp3.ingroupel == "Ceiling")
                        {
                            str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                        }
                        if (tmp3.ingroupel == "Floor")
                        {
                            str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                        }
                        if (tmp3.ingroupel == "FloorFoot")
                        {
                            str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                        }
                        if (tmp3.ingroupel == "ShelfC")
                        {
                            str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont.IndexOf(tmp3.select.thisobject) + "|" + tempcont.IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.basewallstatus + "|" + tmp3.descreaselen + "|" + tmp3.otDR + "|" + tmp3.otUL);
                        }
                        if (tmp3.ingroupel == "WallC")
                        {
                            str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont.IndexOf(tmp3.select.thisobject) + "|" + tempcont.IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.basewallstatus + "|" + tmp3.descreaselen + "|" + tmp3.otDR + "|" + tmp3.otUL);
                        }
                        if (tmp3.ingroupel == "Rack")
                        {
                            str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont.IndexOf(tmp3.select.thisobject) + "|" + tempcont.IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.centery);
                        }
                        string[] tmpstr = tmp3.ingroupel.Split('-');
                        if (tmpstr[0] == "Box")
                        {
                            if (tmp3.iningroupel == "WR")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont.IndexOf(tmp3.select.thisobject) + "|" + tempcont.IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|"   + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                            }
                            if (tmp3.iningroupel == "WL")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont.IndexOf(tmp3.select.thisobject) + "|" + tempcont.IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|"  + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                            }
                            if (tmp3.iningroupel == "WF")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont.IndexOf(tmp3.select.thisobject) + "|" + tempcont.IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|"   + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                            }
                            if (tmp3.iningroupel == "WB")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont.IndexOf(tmp3.select.thisobject) + "|" + tempcont.IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|"  + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                            }
                            if (tmp3.iningroupel == "WFF")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont.IndexOf(tmp3.select.thisobject) + "|" + tempcont.IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|"  + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                            }
                            if (tmp3.iningroupel == "WD")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont.IndexOf(tmp3.select.thisobject) + "|" + tempcont.IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                            }
                        }
                        if (tmpstr[0] == "Element") {
                            if (tmp3.iningroupel == "El")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.getPosX() + "|" + tmp3.getPosY() + "|" + tmp3.getPosZ() + "|" + tmp3.getLenX() + "|" + tmp3.getLenY() + "|" + tmp3.getLenZ() + "|" + tmp3.getAngleX() + "|" + tmp3.getAngleY() + "|" + tmp3.getAngleZ() + "|" + tmp3.idelement + "|" + tempcont.IndexOf(tmp3.select.thisobject) + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                            }
                        }
                        if (tmp3.ingroupel.IndexOf("Door")!=-1)
                        {
                            str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont.IndexOf(tmp3.select.thisobject) + "|" + tempcont.IndexOf(tmp3.twoselect.thisobject) + "|" + tempcont.IndexOf(tmp3.threeselect.thisobject) + "|" + tempcont.IndexOf(tmp3.fourselect.thisobject) + "|" + tmp3.napr +  "|" + tmp3.naprv );
                        }
                    }
                }
            }

            SaveCupWIN win = new SaveCupWIN(this, str, connectionString );
            win.Show();
            this.IsEnabled = false;
        }
        public Random rand = new Random();
        public void openCub(string[] str, TextBlock wall, TextBlock wallb)
        {
            ArrayList conttemp = new ArrayList();
            string[] temp1 = str[0].Split('|');
            string newgroup = temp1[0] + rand.Next(1, 100000);
            for (int i = 0; i < str.Length; i++)
            {
                string line = str[i];
                if (line == "NEW" && i < str.Length-1) { string line1 = str[i+1]; string[] tempp = line1.Split('|'); newgroup = tempp[0] + rand.Next(1, 100000); conttemp.Clear(); continue;  }
                string[] temp = line.Split('|');
                temp[0] = newgroup;
                LoadElements le = new LoadElements(this);
                if (temp[1] == "WallL" || temp[1] == "WallR" || temp[1] == "WallB" || temp[1] == "Floor" || temp[1] == "FloorFoot" || temp[1] == "WallF" || temp[1] == "Ceiling")
                {
                    if (temp[1] == "WallB")
                    {
                        if (wallb.Text != "") le.CreateFoundationEl(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp, wallb.Text, int.Parse(wallb.Tag.ToString()));
                        else le.CreateFoundationEl(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp);
                    }
                    else
                    {
                        if (wall.Text != "") le.CreateFoundationEl(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp, wall.Text, int.Parse(wall.Tag.ToString()));
                        else le.CreateFoundationEl(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp);
                    }
                }
                if (temp[1] == "WallC" || temp[1] == "ShelfC")
                {
                    if (wall.Text != "") le.CreateShelfWall(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp[int.Parse(temp[10])] as ModelUIElement3D, conttemp[int.Parse(temp[11])] as ModelUIElement3D, int.Parse(temp[12]), double.Parse(temp[13]), bool.Parse(temp[14]), double.Parse(temp[15]), conttemp, double.Parse(temp[16]), double.Parse(temp[17]), wall.Text, int.Parse(wall.Tag.ToString()));
                    else le.CreateShelfWall(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp[int.Parse(temp[10])] as ModelUIElement3D, conttemp[int.Parse(temp[11])] as ModelUIElement3D, int.Parse(temp[12]), double.Parse(temp[13]), bool.Parse(temp[14]), double.Parse(temp[15]), conttemp, double.Parse(temp[16]), double.Parse(temp[17]));
                }
                if (temp[1] == "Rack")
                {
                    if (wallb.Text != "") le.CreateRackEl(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp[int.Parse(temp[10])] as ModelUIElement3D, conttemp[int.Parse(temp[11])] as ModelUIElement3D, int.Parse(temp[12]), double.Parse(temp[13]), double.Parse(temp[14]), conttemp, wallb.Text, int.Parse(wallb.Tag.ToString()));
                    else le.CreateRackEl(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp[int.Parse(temp[10])] as ModelUIElement3D, conttemp[int.Parse(temp[11])] as ModelUIElement3D, int.Parse(temp[12]), double.Parse(temp[13]), double.Parse(temp[14]), conttemp);
                }
                string[] tmp = temp[1].Split('-');
                if (tmp[0] == "Box")
                {
                    if (temp[2] == "WD")
                    {
                        if (wallb.Text != "") le.CreateBoxEl(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp[int.Parse(temp[10])] as ModelUIElement3D, conttemp[int.Parse(temp[11])] as ModelUIElement3D, int.Parse(temp[12]), double.Parse(temp[13]),  double.Parse(temp[14]), double.Parse(temp[15]), double.Parse(temp[16]), double.Parse(temp[17]), conttemp, wallb.Text, int.Parse(wallb.Tag.ToString()));
                        else le.CreateBoxEl(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp[int.Parse(temp[10])] as ModelUIElement3D, conttemp[int.Parse(temp[11])] as ModelUIElement3D, int.Parse(temp[12]), double.Parse(temp[13]), double.Parse(temp[14]), double.Parse(temp[15]), double.Parse(temp[16]), double.Parse(temp[17]), conttemp);
                    }
                    else
                    {
                        if (wall.Text != "") le.CreateBoxEl(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp[int.Parse(temp[10])] as ModelUIElement3D, conttemp[int.Parse(temp[11])] as ModelUIElement3D, int.Parse(temp[12]), double.Parse(temp[13]), double.Parse(temp[14]), double.Parse(temp[15]), double.Parse(temp[16]), double.Parse(temp[17]), conttemp, wall.Text, int.Parse(wall.Tag.ToString()));
                        else le.CreateBoxEl(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), conttemp[int.Parse(temp[10])] as ModelUIElement3D, conttemp[int.Parse(temp[11])] as ModelUIElement3D, int.Parse(temp[12]), double.Parse(temp[13]),  double.Parse(temp[14]), double.Parse(temp[15]), double.Parse(temp[16]), double.Parse(temp[17]), conttemp);
                    }
                }
                if (tmp[0].IndexOf("Element") != -1)
                {
                    if (wallb.Text != "") le.CreateElement(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), int.Parse(temp[9]), int.Parse(temp[10]), int.Parse(temp[11]), int.Parse(temp[12]), conttemp[int.Parse(temp[13])] as ModelUIElement3D, double.Parse(temp[14]), double.Parse(temp[15]), double.Parse(temp[16]), wallb.Text, int.Parse(wallb.Tag.ToString()));
                    else le.CreateElement(temp[0], temp[1], temp[2], double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), int.Parse(temp[9]),int.Parse(temp[10]), int.Parse(temp[11]), int.Parse(temp[12]), conttemp[int.Parse(temp[13])] as ModelUIElement3D, double.Parse(temp[14]), double.Parse(temp[15]), double.Parse(temp[16]));
                }
                if (tmp[0].IndexOf("Door") != -1)
                {
                    
                    if (wall.Text != "") le.CreateDoorEl(temp[0], temp[1], temp[2], temp[3], double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), double.Parse(temp[10]), conttemp[int.Parse(temp[11])] as ModelUIElement3D, conttemp[int.Parse(temp[12])] as ModelUIElement3D, conttemp[int.Parse(temp[13])] as ModelUIElement3D, conttemp[int.Parse(temp[14])] as ModelUIElement3D, int.Parse(temp[15]), int.Parse(temp[16]), conttemp, wall.Text, int.Parse(wall.Tag.ToString()));
                    else le.CreateDoorEl(temp[0], temp[1], temp[2], temp[3], double.Parse(temp[4]), double.Parse(temp[5]), double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]), double.Parse(temp[9]), double.Parse(temp[10]), conttemp[int.Parse(temp[11])] as ModelUIElement3D, conttemp[int.Parse(temp[12])] as ModelUIElement3D, conttemp[int.Parse(temp[13])] as ModelUIElement3D, conttemp[int.Parse(temp[14])] as ModelUIElement3D, int.Parse(temp[15]), int.Parse(temp[16]),  conttemp);
                }
            }
        }
        public DispatcherTimer timer = new DispatcherTimer();
        public  bool naprtimer = true;
        public string tag;
        public void OnTimerTick(object sender, object e)
        {
            ElementCabinet tempmodel = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet; ;
            if (tag.IndexOf('1') > 0) tempmodel = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
            if (twoselectedobject != null) if (tag.IndexOf('2') > 0) tempmodel = CreatedElements[twoselectedobject.GetHashCode()] as ElementCabinet;
            if (threeselectedobject != null) if (tag.IndexOf('3') > 0) tempmodel = CreatedElements[threeselectedobject.GetHashCode()] as ElementCabinet;
            if (fourselectedobject != null) if (tag.IndexOf('4') > 0) tempmodel = CreatedElements[fourselectedobject.GetHashCode()] as ElementCabinet;
            DiffuseMaterial texture = tempmodel.thismodel.Material as DiffuseMaterial;
            if (texture != null)
            {
                ImageBrush texture1 = texture.Brush as ImageBrush;
                if (texture1 != null && tempmodel.basevisstatus == false)
                {
                    double op = texture1.Opacity;
                    if (naprtimer) op = 1.0;
                    else op = 0.2;
                    texture1.Opacity = op;
                    if (op == 1.0) naprtimer = false;
                    if (op == 0.2) naprtimer = true;
                    tempmodel.thismodel.Material = new DiffuseMaterial(texture1);
                }
                else
                {
                    if (texture.Brush != null && tempmodel.basevisstatus == false)
                    {
                        SolidColorBrush texture2 = texture.Brush as SolidColorBrush;
                        double op = texture2.Opacity;
                        if (naprtimer) op = 1.0;
                        else op = 0.2;
                        texture2.Opacity = op;
                        if (op == 1.0) naprtimer = false;
                        if (op == 0.2) naprtimer = true;
                        tempmodel.thismodel.Material = new DiffuseMaterial(texture2);
                    }
                }
            }
            

        }
        public void OBJ_MouseMove(object sender, MouseEventArgs e)
        {
            Control tmp1 = sender as Control;
            tag = tmp1.Tag + "";
            if (!timer.IsEnabled)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(0.5);
                timer.Tick += OnTimerTick;
                timer.Start();
            }

        }
        public void OBJ_MouseMoveTB(object sender, MouseEventArgs e)
        {
            TextBlock tmp1 = sender as TextBlock;
            tag = tmp1.Tag + "";
            if (!timer.IsEnabled)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(0.5);
                timer.Tick += OnTimerTick;
                timer.Start();
            }
        }
        public void OBJ_MouseLeave(object sender, MouseEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                ElementCabinet tempmodel = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
                if (tag.IndexOf('1') > 0) tempmodel = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
                if (twoselectedobject != null) if (tag.IndexOf('2') > 0) tempmodel = CreatedElements[twoselectedobject.GetHashCode()] as ElementCabinet;
                if (threeselectedobject != null) if (tag.IndexOf('3') > 0) tempmodel = CreatedElements[threeselectedobject.GetHashCode()] as ElementCabinet;
                if (fourselectedobject != null) if (tag.IndexOf('4') > 0) tempmodel = CreatedElements[fourselectedobject.GetHashCode()] as ElementCabinet;
                DiffuseMaterial texture = tempmodel.thismodel.Material as DiffuseMaterial;
                if (texture != null && tempmodel.basevisstatus == false)
                {
                    ImageBrush texture1 = texture.Brush as ImageBrush;
                    if (texture1 != null)
                    {
                        texture1.Opacity = 0.5;
                        tempmodel.thismodel.Material = new DiffuseMaterial(texture1);
                    }
                    else
                    {
                        SolidColorBrush texture2 = texture.Brush as SolidColorBrush;
                        if (texture2 != null)
                        {
                            texture2.Opacity = 0.5;
                            tempmodel.thismodel.Material = new DiffuseMaterial(texture2);
                        }
                    }
                }
            }
        }
        public void MaterialSel_Click(object sender, RoutedEventArgs e)
        {
            MaterialsWIN win = new MaterialsWIN(this,connectionString);
            win.Show();
            this.IsEnabled = false;
        }
        public void CupboardSel_Click(object sender, RoutedEventArgs e)
        {
            if (iddefault == -1 || iddefaultB == -1) { MessageBox.Show("Выберите в настройках материалы по умолчанию"); return; }
            CupboardsWIN win = new CupboardsWIN(this, connectionString);
            win.Show();
            this.IsEnabled = false;
        }
        public void ElementSel_Click(object sender, RoutedEventArgs e)
        {
            ElementWIN win = new ElementWIN(this,selectedobject, connectionString);
            win.Show();
            this.IsEnabled = false;
        }            
        public void CostOne_Click(object sender, RoutedEventArgs e)
        {
            ArrayList tempcont = new ArrayList();
            foreach (var item in container.Children)
            {
                if (item is ModelUIElement3D tmp2)
                {
                    ElementCabinet tmp3 = CreatedElements[tmp2.GetHashCode()] as ElementCabinet;
                    tempcont.Add(tmp2);
                    if (tmp3.otDR != 0 || tmp3.otUL != 0) {
                        bool b = false;
                        foreach (var item1 in container.Children)
                        {
                            if (item1 is ModelUIElement3D tmp21)
                            {
                                ElementCabinet tmp31 = CreatedElements[tmp21.GetHashCode()] as ElementCabinet;
                                if (tmp31.select == tmp3 || tmp31.twoselect == tmp3) b = true;
                            }
                        }
                        if (b == false) { MessageBox.Show("Обнаруженные висячие стены или полочки", "",MessageBoxButton.OK,MessageBoxImage.Information);}
                    }
                }
            }
            ElementCabinet tempmodel = CreatedElements[selectedobject.GetHashCode()] as ElementCabinet;
            Hashtable hash = new Hashtable();
            foreach (var item in tempcont)
            {
                ElementCabinet tmp2 = CreatedElements[item.GetHashCode()] as ElementCabinet;
                if (tempmodel.groupel == tmp2.groupel)
                {
                    if (hash[tmp2.groupel] == null) { ArrayList ar = new ArrayList(); ar.Add(tmp2); hash.Add(tmp2.groupel, ar); }
                    else { ArrayList ar = hash[tmp2.groupel] as ArrayList; ar.Add(tmp2); hash[tmp2.groupel] = ar; }
                }        
            }
            CostWIN win = new CostWIN(this, hash, connectionString);
            win.Show();
            this.IsEnabled = false;
        }
        public void CostAll_Click(object sender, RoutedEventArgs e)
        {
            ArrayList tempcont = new ArrayList();
            foreach (var item in container.Children)
            {
                if (item is ModelUIElement3D tmp2)
                {
                    ElementCabinet tmp3 = CreatedElements[tmp2.GetHashCode()] as ElementCabinet;
                    tempcont.Add(tmp2);
                    if (tmp3.otDR != 0 || tmp3.otUL != 0)
                    {
                        bool b = false;
                        foreach (var item1 in container.Children)
                        {
                            if (item1 is ModelUIElement3D tmp21)
                            {
                                ElementCabinet tmp31 = CreatedElements[tmp21.GetHashCode()] as ElementCabinet;
                                if (tmp31.select == tmp3 || tmp31.twoselect == tmp3) b = true;
                            }
                        }
                        if (b == false) { MessageBox.Show("Обнаруженные висячие стены или полочки", "", MessageBoxButton.OK, MessageBoxImage.Information); }
                    }
                }
            }
            Hashtable hash = new Hashtable();
            foreach (var item in tempcont)
            {
                    ElementCabinet tmp2 = CreatedElements[item.GetHashCode()] as ElementCabinet;
                    if (hash[tmp2.groupel] == null) { ArrayList ar = new ArrayList(); ar.Add(tmp2); hash.Add(tmp2.groupel, ar); }
                    else { ArrayList ar = hash[tmp2.groupel] as ArrayList; ar.Add(tmp2); hash[tmp2.groupel] = ar; }
            }
            CostWIN win = new CostWIN(this, hash, connectionString);
            win.Show();
            this.IsEnabled = false;
        }
        public void Export_Click(object sender, RoutedEventArgs e)
        {
            ClickOnCamera(new object(), e as MouseButtonEventArgs);
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.DefaultExt = "stl";
            saveFileDialog.Filter = "STL|*.stl";
            if (saveFileDialog.ShowDialog() == true) {
                Camera.Children.Remove(vv);
                Camera.Children.Remove(vv1);
                Camera.Children.Remove(vv2);
                Camera.Children.Remove(vv3);
                Camera.Export(saveFileDialog.FileName);
                Camera.Children.Add(vv);
                Camera.Children.Add(vv1);
                Camera.Children.Add(vv2);
                Camera.Children.Add(vv3);
            }
        }
        public void Option_Click(object sender, RoutedEventArgs e)
        {
            OptionWIN win = new OptionWIN(this, connectionString);
            win.Grids.IsEnabled = true;
            this.IsEnabled = false;
            win.Show();
        }
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            checkConfig();
        }
        public void Window_Closed(object sender, EventArgs e)
        {
            foreach (Window w in App.Current.Windows)
                w.Close();
        }

        private void saveModelButt1_Click(object sender, RoutedEventArgs e)
        {
           

            ArrayList groups = new ArrayList();
            foreach (var item in container.Children)
            {
                if (item is ModelUIElement3D tmp2)
                {
                    ElementCabinet tmp3 = CreatedElements[tmp2.GetHashCode()] as ElementCabinet;
                    if(groups.IndexOf(tmp3.groupel)==-1)groups.Add(tmp3.groupel);
                }
            }

            ArrayList []tempcont = new ArrayList[groups.Count];
            ArrayList []tempcont1 = new ArrayList[groups.Count];
            for(int ii = 0; ii < groups.Count; ii++)
            {
                tempcont[ii] = new ArrayList();
                tempcont1[ii] = new ArrayList();
                foreach (var item in container.Children)
                {
                    if (item is ModelUIElement3D tmp2)
                    {
                        ElementCabinet tmp3 = CreatedElements[tmp2.GetHashCode()] as ElementCabinet;
                        if (tmp3.groupel == groups[ii].ToString())
                        {
                            if (tmp3.ingroupel.IndexOf("Element") == -1)
                                tempcont[ii].Add(tmp2);
                            tempcont1[ii].Add(tmp2);
                        }
                    }
                }
            }
            int counts = 0;
            for (int ii = 0; ii < groups.Count; ii++)
            {
                counts += tempcont1[ii].Count;
            }
            counts += groups.Count-1;
            if (counts == -1) return;
            string[] str = new string[counts];
            int ind = 0;
            for (int ii = 0; ii < groups.Count; ii++)
            {                
                foreach (var item in tempcont1[ii])
                {
                    if (item is ModelUIElement3D tmp2)
                    {
                        ElementCabinet tmp3 = CreatedElements[tmp2.GetHashCode()] as ElementCabinet;
                        if (tmp3.groupel == groups[ii].ToString())
                        {
                            if (tmp3.ingroupel == "WallL")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                            }
                            if (tmp3.ingroupel == "WallR")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                            }
                            if (tmp3.ingroupel == "WallB")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                            }
                            if (tmp3.ingroupel == "WallF")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                            }
                            if (tmp3.ingroupel == "Ceiling")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                            }
                            if (tmp3.ingroupel == "Floor")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                            }
                            if (tmp3.ingroupel == "FloorFoot")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness);
                            }
                            if (tmp3.ingroupel == "ShelfC")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont[ii].IndexOf(tmp3.select.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.basewallstatus + "|" + tmp3.descreaselen + "|" + tmp3.otDR + "|" + tmp3.otUL);
                            }
                            if (tmp3.ingroupel == "WallC")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont[ii].IndexOf(tmp3.select.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.basewallstatus + "|" + tmp3.descreaselen + "|" + tmp3.otDR + "|" + tmp3.otUL);
                            }
                            if (tmp3.ingroupel == "Rack")
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont[ii].IndexOf(tmp3.select.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.centery);
                            }
                            string[] tmpstr = tmp3.ingroupel.Split('-');
                            if (tmpstr[0] == "Box")
                            {
                                if (tmp3.iningroupel == "WR")
                                {
                                    str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont[ii].IndexOf(tmp3.select.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                                }
                                if (tmp3.iningroupel == "WL")
                                {
                                    str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont[ii].IndexOf(tmp3.select.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                                }
                                if (tmp3.iningroupel == "WF")
                                {
                                    str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont[ii].IndexOf(tmp3.select.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                                }
                                if (tmp3.iningroupel == "WB")
                                {
                                    str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont[ii].IndexOf(tmp3.select.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                                }
                                if (tmp3.iningroupel == "WFF")
                                {
                                    str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont[ii].IndexOf(tmp3.select.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                                }
                                if (tmp3.iningroupel == "WD")
                                {
                                    str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont[ii].IndexOf(tmp3.select.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.twoselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.center + "|" + tmp3.lenbox + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                                }
                            }
                            if (tmpstr[0] == "Element")
                            {
                                if (tmp3.iningroupel == "El")
                                {
                                    str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.getPosX() + "|" + tmp3.getPosY() + "|" + tmp3.getPosZ() + "|" + tmp3.getLenX() + "|" + tmp3.getLenY() + "|" + tmp3.getLenZ() + "|" + tmp3.getAngleX() + "|" + tmp3.getAngleY() + "|" + tmp3.getAngleZ() + "|" + tmp3.idelement + "|" + tempcont[ii].IndexOf(tmp3.select.thisobject) + "|" + tmp3.otsx + "|" + tmp3.otsy + "|" + tmp3.otsz);
                                }
                            }
                            if (tmp3.ingroupel.IndexOf("Door") != -1)
                            {
                                str[ind++] = (tmp3.groupel + "|" + tmp3.ingroupel + "|" + tmp3.iningroupel + "|" + tmp3.objgr + "|" + tmp3.AxisX + "|" + tmp3.AxisY + "|" + tmp3.AxisZ + "|" + tmp3.LenX + "|" + tmp3.LenY + "|" + tmp3.LenZ + "|" + tmp3.thickness + "|" + tempcont[ii].IndexOf(tmp3.select.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.twoselect.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.threeselect.thisobject) + "|" + tempcont[ii].IndexOf(tmp3.fourselect.thisobject) + "|" + tmp3.napr + "|" + tmp3.naprv);
                            }
                        }
                    }
                }
                if(ii < groups.Count-1)str[ind++] = "NEW";
            }
            SaveCupWIN win = new SaveCupWIN(this, str, connectionString);
            win.Show();
            this.IsEnabled = false;
        }
    }
}
