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
namespace WpfApp13
{
    class _3DObject
    {
        public ModelUIElement3D thisobject;
        public GeometryModel3D thismodel;
        public MeshGeometry3D thismesh;
        protected double axisX;
        protected double axisY;
        protected double axisZ;
        protected double lenX;
        protected double lenY;
        protected double lenZ;
        public double angleX;
        public double angleY;
        public double angleZ;
        public string groupel;
        public string ingroupel;
        public string objgr;
        public int idtexture;
        public int idelement;
        public _3DObject(ModelUIElement3D temp)
        {
            thisobject = temp;
            thismodel = (GeometryModel3D)thisobject.Model;
            thismesh = (MeshGeometry3D)thismodel.Geometry;
            Transform3DGroup group = new Transform3DGroup();
            ScaleTransform3D tr1 = new ScaleTransform3D(1,1,1,0,0,0);
            TranslateTransform3D tr2 = new TranslateTransform3D(0,0,0);
            RotateTransform3D tr3 = new RotateTransform3D();
            RotateTransform3D tr4 = new RotateTransform3D();
            RotateTransform3D tr5 = new RotateTransform3D();
            group.Children.Add(tr1);
            group.Children.Add(tr2);
            group.Children.Add(tr3);
            group.Children.Add(tr4);
            group.Children.Add(tr5);
            thismodel.Transform = group;           
            axisX = tr2.OffsetX;
            axisY = tr2.OffsetY;
            axisZ = tr2.OffsetZ;
            lenX = tr1.ScaleX;
            lenY = tr1.ScaleY;
            lenZ = tr1.ScaleZ;
        }
        public _3DObject(ModelUIElement3D temp,double lx,double ly,double lz, double ax, double ay, double az)
        {
            thisobject = temp;
            thismodel = (GeometryModel3D)thisobject.Model;
            thismesh = (MeshGeometry3D)thismodel.Geometry;
            Transform3DGroup group = new Transform3DGroup();
            ScaleTransform3D tr1 = new ScaleTransform3D(lx,ly,lz, ax,ay,az);
            TranslateTransform3D tr2 = new TranslateTransform3D(ax,ay,az);
            RotateTransform3D tr3 = new RotateTransform3D();
            RotateTransform3D tr4 = new RotateTransform3D();
            RotateTransform3D tr5 = new RotateTransform3D();
            tr3.Rotation = new AxisAngleRotation3D();
            tr4.Rotation = new AxisAngleRotation3D();
            tr5.Rotation = new AxisAngleRotation3D();
            group.Children.Add(tr1);
            group.Children.Add(tr2);
            group.Children.Add(tr3);
            group.Children.Add(tr4);
            group.Children.Add(tr5);
            thismodel.Transform = group;
            axisX = tr2.OffsetX;
            axisY = tr2.OffsetY;
            axisZ = tr2.OffsetZ;
            lenX = tr1.ScaleX;
            lenY = tr1.ScaleY;
            lenZ = tr1.ScaleZ;
        }
        public virtual void ChangeAngleX(int ax)
        {
            thismodel = (GeometryModel3D)thisobject.Model;
            thismesh = (MeshGeometry3D)thismodel.Geometry;
            angleX = ax;
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            RotateTransform3D rr11 = group3d.Children[2] as RotateTransform3D;
            TranslateTransform3D tt11 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            ScaleTransform3D tt1 = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            rr11.CenterX = tt11.OffsetX;
            rr11.CenterY = tt11.OffsetY;
            rr11.CenterZ = tt11.OffsetZ;
            rr11.Rotation = new AxisAngleRotation3D(new Vector3D(0.001, 0, 0), angleX);
        }
        public virtual void ChangeAngleY(int ay)
        {
            thismodel = (GeometryModel3D)thisobject.Model;
            thismesh = (MeshGeometry3D)thismodel.Geometry;
            angleY = ay;
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            RotateTransform3D rr11 = group3d.Children[3] as RotateTransform3D;
            TranslateTransform3D tt11 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            ScaleTransform3D tt1 = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            rr11.CenterX = tt11.OffsetX;
            rr11.CenterY = tt11.OffsetY;
            rr11.CenterZ = tt11.OffsetZ;
            rr11.Rotation = new AxisAngleRotation3D(new Vector3D(0, 0.001, 0), angleY);
        }
        public virtual void ChangeAngleZ(int az)
        {
            thismodel = (GeometryModel3D)thisobject.Model;
            thismesh = (MeshGeometry3D)thismodel.Geometry;
            angleZ = az;
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            RotateTransform3D rr11 = group3d.Children[4] as RotateTransform3D;
            TranslateTransform3D tt11 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            ScaleTransform3D tt1 = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            rr11.CenterX = tt11.OffsetX;
            rr11.CenterY = tt11.OffsetY;
            rr11.CenterZ = tt11.OffsetZ;
            rr11.Rotation = new AxisAngleRotation3D(new Vector3D(0, 0, 0.001), angleZ);
        }
        public virtual void changeObjectLen(double lx, double ly, double lz)
        {
            thismodel = (GeometryModel3D)thisobject.Model;
            thismesh = (MeshGeometry3D)thismodel.Geometry;
            lenX = lx;
            lenY = ly;
            lenZ = lz;
            Transform3DGroup group = (Transform3DGroup)thismodel.Transform;
            ScaleTransform3D tt = group.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            tt.ScaleX = lenX;
            tt.ScaleY = lenY;
            tt.ScaleZ = lenZ;
        }
        public virtual void changeObjectPos(double ax, double ay, double az)
        {
            thismodel = (GeometryModel3D)thisobject.Model;
            thismesh = (MeshGeometry3D)thismodel.Geometry;
            axisX = ax;
            axisY = ay;
            axisZ = az;
            Transform3DGroup group = (Transform3DGroup)thismodel.Transform;
            TranslateTransform3D tt = group.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            tt.OffsetX = axisX;
            tt.OffsetY = axisY;
            tt.OffsetZ = axisZ;

        }
        public virtual void changeTexture(string pos,int id)
        {
            if (ingroupel.IndexOf("Element") == -1) { 
            ImageBrush texture = new ImageBrush(new BitmapImage(new Uri(pos, UriKind.Relative)));
            thismodel.Material = new DiffuseMaterial(texture);
            DiffuseMaterial texture1 = thismodel.Material as DiffuseMaterial;
            ImageBrush texture2 = texture1.Brush as ImageBrush;
            texture2.Opacity = 1;
            thismodel.Material = new DiffuseMaterial(texture2);
            idtexture = id;
            }
        }
        public string takePositionG(_3DObject t1)
        {
            ElementCabinet tmp1 = this as ElementCabinet;
            ElementCabinet tmp3 = t1 as ElementCabinet;
            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();

            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            double raz = tt3.OffsetX - tt1.OffsetX;
            if (raz < 0) return "R";
            else return "L";
        }
        public string takePositionV(_3DObject t1)
        {
            ElementCabinet tmp1 = this as ElementCabinet;
            ElementCabinet tmp3 = t1 as ElementCabinet;
            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();

            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            double raz = tt3.OffsetZ - tt1.OffsetZ;
            if (raz < 0) return "D";
            else return "U";
        }
        public double AxisX
        {
            get
            {
                return axisX;
            }
            set
            {
                axisX = value;
            }
        }
        public double AxisY
        {
            get
            {
                return axisY;
            }
            set
            {
                axisY = value;
            }
        }
        public double AxisZ
        {
            get
            {
                return axisZ;
            }
            set
            {
                axisZ = value;
            }
        }
        public double LenX
        {
            get
            {
                return lenX;
            }
            set
            {
                lenX = value;
            }
        }
        public double LenY
        {
            get
            {
                return lenY;
            }

            set
            {
                lenY = value;
            }
        }
        public double LenZ
        {
            get
            {
                return lenZ;
            }
            set
            {
                lenZ = value;
            }
        }
    }
}
