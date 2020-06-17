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
    class LoadElements
    {
        MainWindow win;
        public LoadElements(MainWindow win)
        {
            this.win = win;
        }
        public void CreateElement(string groupel, string ingroupel, string iningroupel, double getPosX, double getPosY, double getPosZ, double getLenX, double getLenY, double getLenZ, int getAngleX, int getAngleY, int getAngleZ, int idmaterial, ModelUIElement3D thisobject1, double otx, double oty, double otz, string text = "-1", int id = -1)
        {
            if (text == "-1") text = win.soureseDefault;
            if (id == -1) id = win.iddefaultB;
            ModelImporter importer = new ModelImporter();
            Material material1 = new DiffuseMaterial(win.defcolor);
            importer.DefaultMaterial = material1;
            string request = "select sourse from elements where id=" + idmaterial;
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(win.connectionString);
            npgSqlConnection.Open();
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(request, npgSqlConnection);
            NpgsqlDataReader reader = npgSqlCommand.ExecuteReader();
            string str = "";
            while (reader.Read())
            {
                str = reader.GetString(0);
            }
            Model3DGroup model = new Model3DGroup();
            model = importer.Load(str);
            double maxx = 0;
            int idd = win.rand.Next(0, 100000);
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
                GeometryModel3D gmodel = model.Children[i] as GeometryModel3D;
                MeshGeometry3D mmodel = (MeshGeometry3D)gmodel.Geometry;
                Point3DCollection p3dc = mmodel.Positions;
                gmodel.Material = new DiffuseMaterial(win.defcolor);
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
                ElementCabinet tempmodel = new ElementCabinet(modelUI, 1, 1, 1, 0, 0, 0);
                tempmodel.idtexture = id;
                tempmodel.groupel = groupel;
                tempmodel.ingroupel = ingroupel;
                tempmodel.iningroupel = iningroupel;
                tempmodel.hashtable = win.CreatedElements;
                win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
                win.container.Children.Add(modelUI);
                modelUI.MouseUp += win.ClickOnModel;
                tempmodel.LenZ = 2;
                tempmodel.changeObjectLen(getLenX, getLenY, getLenZ);
                tempmodel.changeObjectPos(getPosX, getPosY, getPosZ);
                tempmodel.ChangeAngleX(getAngleX);
                tempmodel.ChangeAngleY(getAngleY);
                tempmodel.ChangeAngleZ(getAngleZ);
                tempmodel.idelement = idmaterial;
                tempmodel.select = win.CreatedElements[thisobject1.GetHashCode()] as ElementCabinet;
                tempmodel.otsx = otx;
                tempmodel.otsy = oty;
                tempmodel.otsz = otz;
                tempmodel.thismodel.Material = new DiffuseMaterial(new SolidColorBrush(win.defcolor.Color));
            }
        }
        public void CreateDoorEl(string groupel, string ingroupel, string iningroupel, string objgr, double AxisX, double AxisY, double AxisZ, double LenX, double LenY, double LenZ, double thickness, ModelUIElement3D thisobject1, ModelUIElement3D thisobject2, ModelUIElement3D thisobject3, ModelUIElement3D thisobject4, int napr, int naprv, ArrayList conttemp, string text = "-1", int id = -1)
        {
            if (text == "-1") text = win.soureseDefault;
            if (id == -1) id = win.iddefault;
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
            tempmodel.groupel = groupel;
            tempmodel.ingroupel = ingroupel;
            tempmodel.iningroupel = iningroupel;
            tempmodel.thickness = thickness;
            tempmodel.hashtable = win.CreatedElements;
            tempmodel.objgr = objgr;
            ElementCabinet[] tempel = new ElementCabinet[4];
            tempel[2] = win.CreatedElements[thisobject1.GetHashCode()] as ElementCabinet;
            tempel[3] = win.CreatedElements[thisobject2.GetHashCode()] as ElementCabinet;
            tempel[0] = win.CreatedElements[thisobject3.GetHashCode()] as ElementCabinet;
            tempel[1] = win.CreatedElements[thisobject4.GetHashCode()] as ElementCabinet;

            tempmodel.select = tempel[2];
            tempmodel.twoselect = tempel[3];
            tempmodel.threeselect = tempel[0];
            tempmodel.fourselect = tempel[1];

            tempmodel.naprv = naprv;
            tempmodel.napr = napr;


            tempmodel.changeObject(LenX, LenY, LenZ, AxisX, AxisY, AxisZ);
            win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
            win.container.Children.Add(modelUI);
            conttemp.Add(modelUI);
            
            tempmodel.changeTexture(text, id);
        }
        public void CreateBoxEl(string groupel, string ingroupel, string iningroupel, double AxisX, double AxisY, double AxisZ, double LenX, double LenY, double LenZ, double thickness, ModelUIElement3D thisobject1, ModelUIElement3D thisobject2, int napr, double center, double lenbox, double otsx, double otsy, double otsz, ArrayList conttemp, string text = "-1", int id = -1)
        {
            if (text == "-1") text = win.soureseDefault;
            if (id == -1) id = win.iddefault;
            MeshGeometry3D meshgeometry = new MeshGeometry3D();
            meshgeometry.TriangleIndices = win.trianglerectangleobject;
            meshgeometry.Positions = win.rectangleobject;
            GeometryModel3D modelgeometry = new GeometryModel3D();
            modelgeometry.Geometry = meshgeometry;
            modelgeometry.Material = new DiffuseMaterial(
            new SolidColorBrush(Colors.Green));
            meshgeometry.TextureCoordinates = new PointCollection(win.textures);
            ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(text, UriKind.Relative)));
            modelgeometry.Material = new DiffuseMaterial(texture);
            ModelUIElement3D modelUI = new ModelUIElement3D();
            modelUI.MouseUp += win.ClickOnModel;
            modelUI.Model = modelgeometry;
            ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
            tempmodel.groupel = groupel;
            tempmodel.ingroupel = ingroupel;
            tempmodel.iningroupel = iningroupel;
            tempmodel.thickness = thickness;
            tempmodel.otsx = otsx;
            tempmodel.otsy = otsy;
            tempmodel.otsz = otsz;
            tempmodel.lenbox = lenbox;
            tempmodel.select = win.CreatedElements[thisobject1.GetHashCode()] as ElementCabinet;
            tempmodel.twoselect = win.CreatedElements[thisobject2.GetHashCode()] as ElementCabinet;
            tempmodel.napr = napr;
            tempmodel.hashtable = win.CreatedElements;
            tempmodel.center = center;
            tempmodel.changeObject(LenX, LenY, LenZ, AxisX, AxisY, AxisZ);
            win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
            win.container.Children.Add(modelUI);
            conttemp.Add(modelUI);
            tempmodel.changeTexture(text, id);
        }
        public void CreateRackEl(string groupel, string ingroupel, string objgr, double AxisX, double AxisY, double AxisZ, double LenX, double LenY, double LenZ, double thickness, ModelUIElement3D thisobject1, ModelUIElement3D thisobject2, int napr, double center, double centery, ArrayList conttemp, string text = "-1", int id = -1)
        {
            if (text == "-1") text = win.soureseDefault;
            if (id == -1) id = win.iddefault;
            PipeVisual3D pipe = new PipeVisual3D();
            pipe.Diameter = 1;
            pipe.Point1 = new Point3D(0, 0, 0);
            pipe.Point2 = new Point3D(1, 0, 0);
            MeshElement3D tpipe = pipe as MeshElement3D;
            tpipe.Model.Material = Materials.White;
            ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(text, UriKind.Relative)));
            tpipe.Model.Material = new DiffuseMaterial(texture);
            ModelUIElement3D modelUI = new ModelUIElement3D();
            modelUI.MouseUp += win.ClickOnModel;
            modelUI.Model = tpipe.Model;
            ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
            tempmodel.groupel = groupel;
            tempmodel.ingroupel = ingroupel;
            tempmodel.objgr = objgr;
            tempmodel.thickness = thickness;
            tempmodel.select = win.CreatedElements[thisobject1.GetHashCode()] as ElementCabinet;
            tempmodel.twoselect = win.CreatedElements[thisobject2.GetHashCode()] as ElementCabinet;
            tempmodel.napr = -1;
            tempmodel.hashtable = win.CreatedElements;
            tempmodel.center = center;
            tempmodel.centery = centery;
            tempmodel.changeObject(LenX, LenY, LenZ, AxisX, AxisY, AxisZ);
            win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
            win.container.Children.Add(modelUI);
            conttemp.Add(modelUI);
            tempmodel.changeTexture(text, id);
        }
        public void CreateShelfWall(string groupel, string ingroupel, string objgr, double AxisX, double AxisY, double AxisZ, double LenX, double LenY, double LenZ, double thickness, ModelUIElement3D thisobject1, ModelUIElement3D thisobject2, int napr, double center, bool basewallstatus, double descreaselen, ArrayList conttemp, double otDR, double otUL, string text = "-1", int id = -1)
        {
            if (text == "-1") text = win.soureseDefault;
            if (id == -1) id = win.iddefault;
            MeshGeometry3D meshgeometry = new MeshGeometry3D();
            meshgeometry.TriangleIndices = win.trianglerectangleobject;
            meshgeometry.Positions = win.rectangleobject;
            GeometryModel3D modelgeometry = new GeometryModel3D();
            modelgeometry.Geometry = meshgeometry;
            modelgeometry.Material = new DiffuseMaterial(
            new SolidColorBrush(Colors.Green));
            meshgeometry.TextureCoordinates = new PointCollection(win.textures);
            ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(text, UriKind.Relative)));
            modelgeometry.Material = new DiffuseMaterial(texture);
            ModelUIElement3D modelUI = new ModelUIElement3D();
            modelUI.MouseUp += win.ClickOnModel;
            modelUI.Model = modelgeometry;
            ElementCabinet tempmodel = new ElementCabinet(modelUI, 0, 0, 0, 0, 0, 0);
            tempmodel.groupel = groupel;
            tempmodel.ingroupel = ingroupel;
            tempmodel.objgr = objgr;
            tempmodel.thickness = thickness;
            tempmodel.select = win.CreatedElements[thisobject1.GetHashCode()] as ElementCabinet;
            tempmodel.twoselect = win.CreatedElements[thisobject2.GetHashCode()] as ElementCabinet;
            tempmodel.napr = napr;
            tempmodel.basewallstatus = basewallstatus;
            tempmodel.descreaselen = descreaselen;
            tempmodel.hashtable = win.CreatedElements;
            tempmodel.center = center;
            tempmodel.otUL = otUL;
            tempmodel.otDR = otDR;
            tempmodel.changeObject(LenX, LenY, LenZ, AxisX, AxisY, AxisZ);
            win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
            win.container.Children.Add(modelUI);
            conttemp.Add(modelUI);
            tempmodel.changeTexture(text, id);
        }
        public void CreateFoundationEl(string groupel, string ingroupel, string objgr, double AxisX, double AxisY, double AxisZ, double LenX, double LenY, double LenZ, double thickness, ArrayList cont, string text = "-1", int id = -1)
        {
            if (text == "-1") text = win.soureseDefault;
            if (id == -1) id = win.iddefault;
            MeshGeometry3D meshgeometry = new MeshGeometry3D();
            meshgeometry.TriangleIndices = win.trianglerectangleobject;
            meshgeometry.Positions = win.rectangleobject;
            GeometryModel3D modelgeometry = new GeometryModel3D();
            modelgeometry.Geometry = meshgeometry;
            meshgeometry.TextureCoordinates = new PointCollection(win.textures);
            ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(text, UriKind.Relative)));
            modelgeometry.Material = new DiffuseMaterial(texture);
            ModelUIElement3D modelUI = new ModelUIElement3D();
            modelUI.MouseUp += win.ClickOnModel;
            modelUI.Model = modelgeometry;
            ElementCabinet tempmodel = new ElementCabinet(modelUI, 1, 1, 2, 0, 0, 0);
            tempmodel.groupel = groupel;
            tempmodel.ingroupel = ingroupel;
            tempmodel.objgr = objgr;
            tempmodel.thickness = thickness;
            tempmodel.hashtable = win.CreatedElements;
            tempmodel.changeObject(LenX, LenY, LenZ, AxisX, AxisY, AxisZ);
            win.CreatedElements.Add(modelUI.GetHashCode(), tempmodel);
            win.container.Children.Add(modelUI);
            cont.Add(modelUI);
            tempmodel.changeTexture(text, id);
        }

    }
}
