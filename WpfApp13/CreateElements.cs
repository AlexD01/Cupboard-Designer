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
    class CreateElements
    {
        MainWindow win;
        public CreateElements(MainWindow win)
        {
            this.win = win;
        }
        public void CreateFoundation(string type)
        {
            if (win.iddefault == -1 || win.iddefaultB == -1) { MessageBox.Show("Выберите в настройках материалы по умолчанию"); return; }
            int idd = win.rand.Next(0, 100000);
            for (int i = 0; i < 6; i++)
            {
                MeshGeometry3D meshgeometry = new MeshGeometry3D();
                meshgeometry.TriangleIndices = win.trianglerectangleobject;
                meshgeometry.Positions = win.rectangleobject;
                GeometryModel3D modelgeometry = new GeometryModel3D();
                modelgeometry.Geometry = meshgeometry;
                meshgeometry.TextureCoordinates = new PointCollection(win.textures);
                ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(@"Images//Венге.jpg", UriKind.Relative)));
                modelgeometry.Material = new DiffuseMaterial(texture);
                ModelUIElement3D modelUI = new ModelUIElement3D();
                modelUI.MouseUp += win.ClickOnModel;
                modelUI.Model = modelgeometry;
                ElementCabinet tempmodel = new ElementCabinet(modelUI, 1, 1, 2, 0, 0, 0);
                tempmodel.groupel = type + idd;
                if (i == 0)
                {
                    tempmodel.ingroupel = "WallR";
                    tempmodel.objgr = "W";
                    tempmodel.thickness = 0.015;
                }
                if (i == 1)
                {
                    tempmodel.ingroupel = "WallL";
                    tempmodel.objgr = "W";
                    tempmodel.thickness = 0.015;
                }
                if (i == 2)
                {
                    tempmodel.ingroupel = "Ceiling";
                    tempmodel.objgr = "S";
                    tempmodel.thickness = 0.015;
                }
                if (i == 3)
                {
                    tempmodel.ingroupel = "WallB";
                    tempmodel.thickness = 0.003;
                    tempmodel.objgr = "WBF";
                }
                if (i == 4)
                {
                    tempmodel.ingroupel = "Floor";
                    tempmodel.objgr = "S";
                    tempmodel.thickness = 0.015;
                }
                if (i == 5)
                {
                    tempmodel.ingroupel = "WallF";
                    tempmodel.thickness = 0.015;
                    tempmodel.objgr = "WBF";
                }
                tempmodel.hashtable = win.CreatedElements;
                tempmodel.changeObject(1, 1, 2, 0, 0, 0);
                win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
                win.container.Children.Add(modelUI);
                if (i == 3) tempmodel.changeTexture(win.soureseDefaultB, win.iddefaultB);
                else tempmodel.changeTexture(win.soureseDefault, win.iddefault);
            }
        }
        public void CreateFoundationN(string type)
        {
            if (win.iddefault == -1 || win.iddefaultB == -1) { MessageBox.Show("Выберите в настройках материалы по умолчанию"); return; }
            int idd = win.rand.Next(0, 100000);
            for (int i = 0; i < 5; i++)
            {
                MeshGeometry3D meshgeometry = new MeshGeometry3D();
                meshgeometry.TriangleIndices = win.trianglerectangleobject;
                meshgeometry.Positions = win.rectangleobject;
                GeometryModel3D modelgeometry = new GeometryModel3D();
                modelgeometry.Geometry = meshgeometry;
                meshgeometry.TextureCoordinates = new PointCollection(win.textures);
                ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(@"Images//Венге.jpg", UriKind.Relative)));
                modelgeometry.Material = new DiffuseMaterial(texture);
                ModelUIElement3D modelUI = new ModelUIElement3D();
                modelUI.MouseUp += win.ClickOnModel;
                modelUI.Model = modelgeometry;
                ElementCabinet tempmodel = new ElementCabinet(modelUI, 1, 1, 2, 0, 0, 0);
                tempmodel.groupel = type + idd;
                if (i == 0)
                {
                    tempmodel.ingroupel = "WallR";
                    tempmodel.objgr = "W";
                    tempmodel.thickness = 0.015;
                }
                if (i == 1)
                {
                    tempmodel.ingroupel = "WallL";
                    tempmodel.objgr = "W";
                    tempmodel.thickness = 0.015;
                }
                if (i == 2)
                {
                    tempmodel.ingroupel = "Ceiling";
                    tempmodel.objgr = "S";
                    tempmodel.thickness = 0.015;
                }
                if (i == 3)
                {
                    tempmodel.ingroupel = "WallB";
                    tempmodel.thickness = 0.003;
                    tempmodel.objgr = "WBF";
                }
                if (i == 4)
                {
                    tempmodel.ingroupel = "FloorFoot";
                    tempmodel.objgr = "S";
                    tempmodel.thickness = 0.015;
                }
                tempmodel.hashtable = win.CreatedElements;
                tempmodel.changeObject(1, 1, 2, 0, 0, 0);
                win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
                win.container.Children.Add(modelUI);
                if (i == 3) tempmodel.changeTexture(win.soureseDefaultB, win.iddefaultB);
                else tempmodel.changeTexture(win.soureseDefault, win.iddefault);
            }
        }
        public void CreateShaft(string gr, string type, MouseButtonEventArgs e, double centerz)
        {
            MeshGeometry3D meshgeometry = new MeshGeometry3D();
            meshgeometry.TriangleIndices = win.trianglerectangleobject;
            meshgeometry.Positions = win.rectangleobject;
            GeometryModel3D modelgeometry = new GeometryModel3D();
            modelgeometry.Geometry = meshgeometry;
            modelgeometry.Material = new DiffuseMaterial(
            new SolidColorBrush(Colors.Green));
            meshgeometry.TextureCoordinates = new PointCollection(win.textures);
            ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(@"Images//Венге.jpg", UriKind.Relative)));
            modelgeometry.Material = new DiffuseMaterial(texture);
            ModelUIElement3D modelUI = new ModelUIElement3D();
            modelUI.MouseUp += win.ClickOnModel;
            modelUI.Model = modelgeometry;
            ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
            tempmodel.groupel = gr;
            tempmodel.ingroupel = "ShelfC";
            tempmodel.objgr = "S";
            tempmodel.thickness = 0.015;
            tempmodel.select = win.CreatedElements[win.selectedobject.GetHashCode()] as ElementCabinet;
            if (win.twoselectedobject != null) tempmodel.twoselect = win.CreatedElements[win.twoselectedobject.GetHashCode()] as ElementCabinet;
            if (type == "L") tempmodel.napr = 1;
            if (type == "R") tempmodel.napr = -1;
            tempmodel.hashtable = win.CreatedElements;
            tempmodel.center = centerz - tempmodel.select.AxisZ;
            tempmodel.changeTexture(win.soureseDefault, win.iddefault);
            _3DObject tempmodel1 = win.CreatedElements[win.selectedobject.GetHashCode()] as _3DObject;
            Transform3DGroup group3d = tempmodel1.thismodel.Transform as Transform3DGroup;
            ScaleTransform3D tln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            tempmodel.changeObject(0, tln.ScaleY, 0, 0, 0, 0);
            win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
            win.container.Children.Add(modelUI);
            win.ClickOnCamera(modelUI, e as MouseButtonEventArgs);
            win.ClickOnModel(modelUI, e);
        }
        public void CreateRack(string gr, string type, MouseButtonEventArgs e, double centerz, double centery)
        {
            PipeVisual3D pipe = new PipeVisual3D();
            pipe.Diameter = 1;
            pipe.Point1 = new Point3D(0, 0, 0);
            pipe.Point2 = new Point3D(1, 0, 0);
            MeshElement3D tpipe = pipe as MeshElement3D;
            tpipe.Model.Material = Materials.White;
            ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(@"Images//Задняя стена белый.jpg", UriKind.Relative)));
            tpipe.Model.Material = new DiffuseMaterial(texture);
            ModelUIElement3D modelUI = new ModelUIElement3D();
            modelUI.MouseUp += win.ClickOnModel;
            modelUI.Model = tpipe.Model;
            ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
            tempmodel.groupel = gr;
            tempmodel.ingroupel = "Rack";
            tempmodel.objgr = "S";
            tempmodel.thickness = 0.015;
            tempmodel.select = win.CreatedElements[win.selectedobject.GetHashCode()] as ElementCabinet;
            if (win.twoselectedobject != null) tempmodel.twoselect = win.CreatedElements[win.twoselectedobject.GetHashCode()] as ElementCabinet;
            if (type == "L") tempmodel.napr = 1;
            if (type == "R") tempmodel.napr = -1;
            tempmodel.hashtable = win.CreatedElements;
            tempmodel.center = centerz - tempmodel.select.AxisZ;
            tempmodel.centery = centery - tempmodel.select.AxisY;
            tempmodel.changeTexture(win.soureseDefault, win.iddefault);
            _3DObject tempmodel1 = win.CreatedElements[win.selectedobject.GetHashCode()] as _3DObject;
            Transform3DGroup group3d = tempmodel1.thismodel.Transform as Transform3DGroup;
            ScaleTransform3D tln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            tempmodel.changeObject(0, tln.ScaleY, 0, 0, 0, 0);
            win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
            win.container.Children.Add(modelUI);
            win.ClickOnCamera(modelUI, e as MouseButtonEventArgs);
            win.ClickOnModel(modelUI, e);
        }
        public void CreateBox(string gr, string type, MouseButtonEventArgs e, double len, double centerz)
        {
            int idd = win.rand.Next(0, 100000);
            for (int i = 0; i < 6; i++)
            {
                MeshGeometry3D meshgeometry = new MeshGeometry3D();
                meshgeometry.TriangleIndices = win.trianglerectangleobject;
                meshgeometry.Positions = win.rectangleobject;
                GeometryModel3D modelgeometry = new GeometryModel3D();
                modelgeometry.Geometry = meshgeometry;
                modelgeometry.Material = new DiffuseMaterial(
                new SolidColorBrush(Colors.Green));

                meshgeometry.TextureCoordinates = new PointCollection(win.textures);
                ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(@"Images//Венге.jpg", UriKind.Relative)));
                modelgeometry.Material = new DiffuseMaterial(texture);

                ModelUIElement3D modelUI = new ModelUIElement3D();
                modelUI.MouseUp += win.ClickOnModel;
                modelUI.Model = modelgeometry;
                ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
                tempmodel.groupel = gr;
                tempmodel.ingroupel = "Box-" + idd;
                if (i == 0) tempmodel.iningroupel = "WF";
                if (i == 1) tempmodel.iningroupel = "WB";
                if (i == 2) tempmodel.iningroupel = "WR";
                if (i == 3) tempmodel.iningroupel = "WL";
                if (i == 4) tempmodel.iningroupel = "WFF";
                if (i == 5) tempmodel.iningroupel = "WD";
                if (i == 5) tempmodel.thickness = 0.003;
                else tempmodel.thickness = 0.015;
                tempmodel.otsx = 0.02;
                tempmodel.otsy = 0.02;
                tempmodel.otsz = 0.02;
                tempmodel.lenbox = len;
                tempmodel.select = win.CreatedElements[win.selectedobject.GetHashCode()] as ElementCabinet;
                if (win.twoselectedobject != null) tempmodel.twoselect = win.CreatedElements[win.twoselectedobject.GetHashCode()] as ElementCabinet;
                if (type == "L") tempmodel.napr = 1;
                if (type == "R") tempmodel.napr = -1;
                tempmodel.hashtable = win.CreatedElements;
                tempmodel.center = centerz - tempmodel.select.AxisZ;
                _3DObject tempmodel1 = win.CreatedElements[win.selectedobject.GetHashCode()] as _3DObject;
                Transform3DGroup group3d = tempmodel1.thismodel.Transform as Transform3DGroup;
                ScaleTransform3D tln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                tempmodel.changeObject(0, tln.ScaleY, 0, 0, 0, 0);
                win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
                win.container.Children.Add(modelUI);
                if (i == 5) tempmodel.changeTexture(win.soureseDefaultB, win.iddefaultB);
                else tempmodel.changeTexture(win.soureseDefault, win.iddefault);
            }
        }
        public void CreateWallC(string gr, string type, MouseButtonEventArgs e, double centerx)
        {
            MeshGeometry3D meshgeometry = new MeshGeometry3D();
            meshgeometry.TriangleIndices = win.trianglerectangleobject;
            meshgeometry.Positions = win.rectangleobject;
            GeometryModel3D modelgeometry = new GeometryModel3D();
            modelgeometry.Geometry = meshgeometry;
            meshgeometry.TextureCoordinates = new PointCollection(win.textures);
            ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(@"Images//Венге.jpg", UriKind.Relative)));
            modelgeometry.Material = new DiffuseMaterial(texture);
            ModelUIElement3D modelUI = new ModelUIElement3D();
            modelUI.MouseUp += win.ClickOnModel;
            modelUI.Model = modelgeometry;
            ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
            tempmodel.groupel = gr;
            tempmodel.ingroupel = "WallC";
            tempmodel.objgr = "W";
            tempmodel.thickness = 0.015;
            tempmodel.hashtable = win.CreatedElements;
            tempmodel.select = win.CreatedElements[win.selectedobject.GetHashCode()] as ElementCabinet;
            if (win.twoselectedobject != null) { tempmodel.twoselect = win.CreatedElements[win.twoselectedobject.GetHashCode()] as ElementCabinet; }
            if (type == "U") tempmodel.napr = 1;
            if (type == "D") tempmodel.napr = -1;
            tempmodel.center = centerx - tempmodel.select.AxisX;
            _3DObject tempmodel1 = win.CreatedElements[win.selectedobject.GetHashCode()] as _3DObject;
            Transform3DGroup group3d = tempmodel1.thismodel.Transform as Transform3DGroup;
            ScaleTransform3D tln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            tempmodel.changeObject(0, tln.ScaleY, 0, 0, 0, 0);
            win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
            win.container.Children.Add(modelUI);
            win.ClickOnCamera(modelUI, e as MouseButtonEventArgs);
            win.ClickOnModel(modelUI, e);
            tempmodel.changeTexture(win.soureseDefault, win.iddefault);
        }
        public void CreateDoor(string gr, string type, string type2, MouseButtonEventArgs e, string port)
        {
            MeshGeometry3D meshgeometry = new MeshGeometry3D();
            meshgeometry.TriangleIndices = win.trianglerectangleobject;
            meshgeometry.Positions = win.rectangleobject;
            GeometryModel3D modelgeometry = new GeometryModel3D();
            modelgeometry.Geometry = meshgeometry;
            meshgeometry.TextureCoordinates = new PointCollection(win.textures);
            ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(@"Images//Венге.jpg", UriKind.Relative)));
            modelgeometry.Material = new DiffuseMaterial(texture);
            ModelUIElement3D modelUI = new ModelUIElement3D();
            modelUI.MouseUp += win.ClickOnModel;
            modelUI.Model = modelgeometry;
            ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
            tempmodel.groupel = gr;
            tempmodel.ingroupel = "Door";
            tempmodel.thickness = 0.015;
            tempmodel.hashtable = win.CreatedElements;
            tempmodel.objgr = "WBF";
            string[] tmp1 = port.Split('|');
            ElementCabinet[] tempel = new ElementCabinet[4];
            tempel[port.IndexOf('0')] = win.CreatedElements[win.selectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('1')] = win.CreatedElements[win.twoselectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('2')] = win.CreatedElements[win.threeselectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('3')] = win.CreatedElements[win.fourselectedobject.GetHashCode()] as ElementCabinet;
            tempmodel.select = tempel[2];
            tempmodel.twoselect = tempel[3];
            tempmodel.threeselect = tempel[0];
            tempmodel.fourselect = tempel[1];
            if (type == "D") tempmodel.naprv = 1;//23
            if (type == "U") tempmodel.naprv = -1;
            if (type2 == "L") tempmodel.napr = 1;//01
            if (type2 == "R") tempmodel.napr = -1;
            tempmodel.center = win.xclickonView;
            _3DObject tempmodel1 = win.CreatedElements[win.selectedobject.GetHashCode()] as _3DObject;
            Transform3DGroup group3d = tempmodel1.thismodel.Transform as Transform3DGroup;
            ScaleTransform3D tln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            tempmodel.changeObject(0, tln.ScaleY, 0, 0, 0, 0);
            tempmodel.changeObject(0, tln.ScaleY, 0, 0, 0, 0);
            win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
            win.container.Children.Add(modelUI);
            win.ClickOnCamera(modelUI, e as MouseButtonEventArgs);
            win.ClickOnModel(modelUI, e);
            tempmodel.changeTexture(win.soureseDefault, win.iddefault);
        }
        public void CreateDoorTwo(string gr, string type, string type2, MouseButtonEventArgs e, string port)
        {
            int idd = win.rand.Next(0, 100000);
            ModelUIElement3D tmpmod = null;
            for (int i = 0; i < 2; i++)
            {
                MeshGeometry3D meshgeometry = new MeshGeometry3D();
                meshgeometry.TriangleIndices = win.trianglerectangleobject;
                meshgeometry.Positions = win.rectangleobject;
                GeometryModel3D modelgeometry = new GeometryModel3D();
                modelgeometry.Geometry = meshgeometry;
                meshgeometry.TextureCoordinates = new PointCollection(win.textures);
                ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(@"Images//Венге.jpg", UriKind.Relative)));
                modelgeometry.Material = new DiffuseMaterial(texture);
                ModelUIElement3D modelUI = new ModelUIElement3D();
                modelUI.MouseUp += win.ClickOnModel;
                modelUI.Model = modelgeometry;
                ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
                tempmodel.groupel = gr;
                tempmodel.ingroupel = "Door-" + idd;
                if (i == 0) tempmodel.iningroupel = "DoorL";
                if (i == 1) tempmodel.iningroupel = "DoorR";
                tempmodel.thickness = 0.015;
                tempmodel.hashtable = win.CreatedElements;
                tempmodel.objgr = "WBF";
                string[] tmp1 = port.Split('|');
                ElementCabinet[] tempel = new ElementCabinet[4];
                tempel[port.IndexOf('0')] = win.CreatedElements[win.selectedobject.GetHashCode()] as ElementCabinet;
                tempel[port.IndexOf('1')] = win.CreatedElements[win.twoselectedobject.GetHashCode()] as ElementCabinet;
                tempel[port.IndexOf('2')] = win.CreatedElements[win.threeselectedobject.GetHashCode()] as ElementCabinet;
                tempel[port.IndexOf('3')] = win.CreatedElements[win.fourselectedobject.GetHashCode()] as ElementCabinet;
                tempmodel.select = tempel[2];
                tempmodel.twoselect = tempel[3];
                tempmodel.threeselect = tempel[0];
                tempmodel.fourselect = tempel[1];
                if (type == "D") tempmodel.naprv = 1;
                if (type == "U") tempmodel.naprv = -1;
                if (type2 == "L") tempmodel.napr = 1;
                if (type2 == "R") tempmodel.napr = -1;
                tempmodel.center = win.xclickonView;
                _3DObject tempmodel1 = win.CreatedElements[win.selectedobject.GetHashCode()] as _3DObject;
                Transform3DGroup group3d = tempmodel1.thismodel.Transform as Transform3DGroup;
                ScaleTransform3D tln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                tempmodel.changeObject(0, tln.ScaleY, 0, 0, 0, 0);
                tempmodel.changeObject(0, tln.ScaleY, 0, 0, 0, 0);
                win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
                win.container.Children.Add(modelUI);
                tempmodel.changeTexture(win.soureseDefault, win.iddefault);
                tmpmod = modelUI;
            }
            win.ClickOnCamera(tmpmod, e as MouseButtonEventArgs);
            win.ClickOnModel(tmpmod, e);
        }
        public void CreateCountW(string gr, string type, string type2, MouseButtonEventArgs e, string port)
        {
            ElementCabinet[] tempel = new ElementCabinet[4];
            tempel[port.IndexOf('0')] = win.CreatedElements[win.selectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('1')] = win.CreatedElements[win.twoselectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('2')] = win.CreatedElements[win.threeselectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('3')] = win.CreatedElements[win.fourselectedobject.GetHashCode()] as ElementCabinet;
            int countW = Convert.ToInt32(win.CountB.Text);
            ElementCabinet tm1 = tempel[2];
            Transform3DGroup group3d1 = tm1.thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tln1 = group3d1.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            ElementCabinet tm2 = tempel[3];
            Transform3DGroup group3d2 = tm2.thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tln2 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            double otstup = 0;
            double nach = 0;
            if (tln1.OffsetX - tln2.OffsetX >= 0)
            {
                otstup = tln1.OffsetX - tln2.OffsetX;
                otstup -= tm2.thickness;
                otstup /= countW + 1;
                nach = tln2.OffsetX;
            }
            if (tln2.OffsetX - tln1.OffsetX >= 0)
            {
                otstup = tln2.OffsetX - tln1.OffsetX;
                otstup -= tm1.thickness;
                otstup /= countW + 1;
                nach = tln1.OffsetX;
            }
            nach += otstup;
            for (int i = 0; i < countW; i++)
            {
                MeshGeometry3D meshgeometry = new MeshGeometry3D();
                meshgeometry.TriangleIndices = win.trianglerectangleobject;
                meshgeometry.Positions = win.rectangleobject;
                GeometryModel3D modelgeometry = new GeometryModel3D();
                modelgeometry.Geometry = meshgeometry;
                meshgeometry.TextureCoordinates = new PointCollection(win.textures);
                ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(@"Images//Венге.jpg", UriKind.Relative)));
                modelgeometry.Material = new DiffuseMaterial(texture);
                ModelUIElement3D modelUI = new ModelUIElement3D();
                modelUI.MouseUp += win.ClickOnModel;
                modelUI.Model = modelgeometry;
                ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
                tempmodel.groupel = gr;
                tempmodel.ingroupel = "WallC";
                tempmodel.objgr = "W";
                tempmodel.thickness = 0.015;
                tempmodel.hashtable = win.CreatedElements;
                tempmodel.select = tempel[0];
                tempmodel.twoselect = tempel[1];
                if (type == "U") tempmodel.napr = 1;
                if (type == "D") tempmodel.napr = -1;
                tempmodel.center = nach;
                _3DObject tempmodel1 = win.CreatedElements[win.selectedobject.GetHashCode()] as _3DObject;
                Transform3DGroup group3d = tempmodel1.thismodel.Transform as Transform3DGroup;
                ScaleTransform3D tln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                tempmodel.changeObject(0, tln.ScaleY, 0, 0, 0, 0);
                win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
                win.container.Children.Add(modelUI);
                tempmodel.changeTexture(win.soureseDefault, win.iddefault);
                nach += otstup;
            }
        }
        public void CreateCountS(string gr, string type, string type2, MouseButtonEventArgs e, string port)
        {
            ElementCabinet[] tempel = new ElementCabinet[4];
            tempel[port.IndexOf('0')] = win.CreatedElements[win.selectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('1')] = win.CreatedElements[win.twoselectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('2')] = win.CreatedElements[win.threeselectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('3')] = win.CreatedElements[win.fourselectedobject.GetHashCode()] as ElementCabinet;
            int countW = Convert.ToInt32(win.CountB.Text);
            ElementCabinet tm1 = tempel[0];
            Transform3DGroup group3d1 = tm1.thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tln1 = group3d1.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            ElementCabinet tm2 = tempel[1];
            Transform3DGroup group3d2 = tm2.thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tln2 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            double otstup = 0;
            double nach = 0;
            if (tln1.OffsetZ - tln2.OffsetZ >= 0)
            {
                otstup = tln1.OffsetZ - tln2.OffsetZ;
                otstup -= tm2.thickness;
                otstup /= countW + 1;
                nach = tln2.OffsetZ;
            }
            if (tln2.OffsetZ - tln1.OffsetZ >= 0)
            {
                otstup = tln2.OffsetZ - tln1.OffsetZ;
                otstup -= tm1.thickness;
                otstup /= countW + 1;
                nach = tln1.OffsetZ;
            }
            nach += otstup;
            for (int i = 0; i < countW; i++)
            {

                MeshGeometry3D meshgeometry = new MeshGeometry3D();
                meshgeometry.TriangleIndices = win.trianglerectangleobject;
                meshgeometry.Positions = win.rectangleobject;
                GeometryModel3D modelgeometry = new GeometryModel3D();
                modelgeometry.Geometry = meshgeometry;
                meshgeometry.TextureCoordinates = new PointCollection(win.textures);
                ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(@"Images//Венге.jpg", UriKind.Relative)));
                modelgeometry.Material = new DiffuseMaterial(texture);
                ModelUIElement3D modelUI = new ModelUIElement3D();
                modelUI.MouseUp += win.ClickOnModel;
                modelUI.Model = modelgeometry;
                ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
                tempmodel.groupel = gr;
                tempmodel.ingroupel = "ShelfC";
                tempmodel.objgr = "S";
                tempmodel.thickness = 0.015;
                tempmodel.hashtable = win.CreatedElements;
                tempmodel.select = tempel[2];
                tempmodel.twoselect = tempel[3];
                if (type2 == "L") tempmodel.napr = 1;
                if (type2 == "R") tempmodel.napr = -1;
                tempmodel.center = nach;
                _3DObject tempmodel1 = win.CreatedElements[win.selectedobject.GetHashCode()] as _3DObject;
                Transform3DGroup group3d = tempmodel1.thismodel.Transform as Transform3DGroup;
                ScaleTransform3D tln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                tempmodel.changeObject(0, tln.ScaleY, 0, 0, 0, 0);
                win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
                win.container.Children.Add(modelUI);
                tempmodel.changeTexture(win.soureseDefault, win.iddefault);
                nach += otstup;
            }
        }
        public void CreateCountB(string gr, string type, string type2, MouseButtonEventArgs e, string port)
        {
            ElementCabinet[] tempel = new ElementCabinet[4];
            tempel[port.IndexOf('0')] = win.CreatedElements[win.selectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('1')] = win.CreatedElements[win.twoselectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('2')] = win.CreatedElements[win.threeselectedobject.GetHashCode()] as ElementCabinet;
            tempel[port.IndexOf('3')] = win.CreatedElements[win.fourselectedobject.GetHashCode()] as ElementCabinet;
            int countW = Convert.ToInt32(win.CountB.Text);
            ElementCabinet tm1 = tempel[0];
            Transform3DGroup group3d1 = tm1.thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tln1 = group3d1.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            ElementCabinet tm2 = tempel[1];
            Transform3DGroup group3d2 = tm2.thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tln2 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            double otstup = 0;
            double nach = 0;
            if (tln1.OffsetZ - tln2.OffsetZ >= 0)
            {
                otstup = tln1.OffsetZ - tln2.OffsetZ;
                otstup /= countW;
                otstup -= 0.005;
                nach = tln2.OffsetZ;
            }
            if (tln2.OffsetZ - tln1.OffsetZ >= 0)
            {
                otstup = tln2.OffsetZ - tln1.OffsetZ;
                otstup /= countW;
                otstup -= 0.005;
                nach = tln1.OffsetZ;
            }
            for (int i = 0; i < countW; i++)
            {
                int idd = win.rand.Next(0, 100000);
                for (int h = 0; h < 6; h++)
                {
                    MeshGeometry3D meshgeometry = new MeshGeometry3D();
                    meshgeometry.TriangleIndices = win.trianglerectangleobject;
                    meshgeometry.Positions = win.rectangleobject;
                    GeometryModel3D modelgeometry = new GeometryModel3D();
                    modelgeometry.Geometry = meshgeometry;
                    modelgeometry.Material = new DiffuseMaterial(
                    new SolidColorBrush(Colors.Green));
                    meshgeometry.TextureCoordinates = new PointCollection(win.textures);
                    ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(@"Images//Венге.jpg", UriKind.Relative)));
                    modelgeometry.Material = new DiffuseMaterial(texture);
                    ModelUIElement3D modelUI = new ModelUIElement3D();
                    modelUI.MouseUp += win.ClickOnModel;
                    modelUI.Model = modelgeometry;
                    ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
                    tempmodel.groupel = gr;
                    tempmodel.ingroupel = "Box-" + idd;
                    if (h == 0) tempmodel.iningroupel = "WF";
                    if (h == 1) tempmodel.iningroupel = "WB";
                    if (h == 2) tempmodel.iningroupel = "WR";
                    if (h == 3) tempmodel.iningroupel = "WL";
                    if (h == 4) tempmodel.iningroupel = "WFF";
                    if (h == 5) tempmodel.iningroupel = "WD";
                    if (h == 5) tempmodel.thickness = 0.003;
                    else tempmodel.thickness = 0.015;
                    tempmodel.otsx = 0.02;
                    tempmodel.otsy = 0.02;
                    tempmodel.otsz = 0.02;
                    tempmodel.lenbox = otstup;
                    tempmodel.select = tempel[3];
                    tempmodel.twoselect = tempel[2];
                    if (type2 == "L") tempmodel.napr = 1;
                    if (type2 == "R") tempmodel.napr = -1;
                    tempmodel.hashtable = win.CreatedElements;
                    tempmodel.center = nach;
                    tempmodel.changeObject(0, 0, 0, 0, 0, 0);
                    win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
                    win.container.Children.Add(modelUI);
                    if (h == 5) tempmodel.changeTexture(win.soureseDefaultB, win.iddefaultB);
                    else tempmodel.changeTexture(win.soureseDefault, win.iddefault);
                }
                nach += otstup + 0.005;
            }
        }
        public void AddElement(string smodel, string simg, int simgid, int id, ModelUIElement3D mod)
        {
            if (simg == "") simg = win.soureseDefaultB;
            if (id == 0) id = win.iddefaultB;
            if (smodel == "") return;
            ModelImporter importer = new ModelImporter();
            Material material1 = new DiffuseMaterial(win.defcolor);
            importer.DefaultMaterial = material1;
            double maxx = 0;
            ModelUIElement3D[] masel = new ModelUIElement3D[4];
            masel[0] = win.selectedobject;
            masel[1] = win.twoselectedobject;
            masel[2] = win.threeselectedobject;
            masel[3] = win.fourselectedobject;
            int countmasel = 0;
            for (int i = 0; i < 4; i++) if (masel[i] != null) countmasel++;
            for (int j = 0; j < countmasel; j++)
            {
                Model3DGroup model = importer.Load(smodel);
                int idd = win.rand.Next(0, 100000);
                for (int i = 0; i < model.Children.Count; i++)
                {
                    GeometryModel3D gmodel = model.Children[i] as GeometryModel3D;
                    MeshGeometry3D mmodel = (MeshGeometry3D)gmodel.Geometry;
                    Point3DCollection p3dc = mmodel.Positions;
                }
                double xx = 9999;
                double yy = 9999;
                double zz = 9999;
                for (int i = 0; i < model.Children.Count; i++)
                {
                    GeometryModel3D gmodel = model.Children[i] as GeometryModel3D;
                    MeshGeometry3D mmodel = (MeshGeometry3D)gmodel.Geometry;
                    Point3DCollection p3dc = mmodel.Positions;
                    if (gmodel.Bounds.SizeX > maxx) maxx = gmodel.Bounds.SizeX;
                    if (gmodel.Bounds.SizeY > maxx) maxx = gmodel.Bounds.SizeY;
                    if (gmodel.Bounds.SizeZ > maxx) maxx = gmodel.Bounds.SizeZ;
                    if (gmodel.Bounds.X < xx) xx = gmodel.Bounds.X;
                    if (gmodel.Bounds.Y < yy) yy = gmodel.Bounds.Y;
                    if (gmodel.Bounds.Z < zz) zz = gmodel.Bounds.Z;
                }
                for (int i = 0; i < model.Children.Count; i++)
                {
                    GeometryModel3D gmodel = new GeometryModel3D();
                    gmodel = model.Children[i] as GeometryModel3D;
                    MeshGeometry3D mmodel = (MeshGeometry3D)gmodel.Geometry;
                    gmodel.Material = new DiffuseMaterial(win.defcolor);
                    Point3DCollection p3dc = mmodel.Positions;
                    for (int ii = 0; ii < p3dc.Count; ii++)
                    {
                        Point3D tp = p3dc[ii];
                        tp.X = tp.X / maxx;
                        tp.Y = tp.Y / maxx;
                        tp.Z = tp.Z / maxx;
                        p3dc[ii] = tp;
                    }
                    for (int ii = 0; ii < p3dc.Count; ii++)
                    {
                        Point3D tp = p3dc[ii];
                        tp.X = tp.X - xx / maxx;
                        tp.Y = tp.Y - yy / maxx;
                        tp.Z = tp.Z - zz / maxx;
                        p3dc[ii] = tp;
                    }
                    ModelUIElement3D modelUI = new ModelUIElement3D();
                    modelUI.Model = gmodel;
                    _3DObject tempmodel1 = win.CreatedElements[mod.GetHashCode()] as _3DObject;
                    ElementCabinet tempmodel = new ElementCabinet(modelUI, 1, 1, 1, 0, 0, 0);
                    tempmodel.idtexture = simgid;
                    tempmodel.groupel = tempmodel1.groupel;
                    tempmodel.ingroupel = "Element-" + idd;
                    tempmodel.iningroupel = "El";
                    tempmodel.hashtable = win.CreatedElements;
                    win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
                    win.container.Children.Add(modelUI);
                    modelUI.MouseUp += win.ClickOnModel;
                    tempmodel.LenZ = 2;
                    tempmodel.idelement = id;
                    ElementCabinet tmp = win.CreatedElements[masel[j].GetHashCode()] as ElementCabinet;
                    tempmodel.select = tmp;
                    tempmodel.changeObjectLen(0.1, 0.1, 0.1);
                    tempmodel.changeObjectPos(tmp.getPosX() + tmp.getLenX() / 2, tmp.getPosY() + tmp.getLenY() / 2, tmp.getPosZ() + tmp.getLenZ() / 2);
                    tempmodel.otsx = tmp.getLenX() / 2;
                    tempmodel.otsy = tmp.getLenY() / 2;
                    tempmodel.otsz = tmp.getLenZ() / 2;
                    tempmodel.thismodel.Material = new DiffuseMaterial(new SolidColorBrush(win.defcolor.Color));
                }
            }
        }
    }
}
