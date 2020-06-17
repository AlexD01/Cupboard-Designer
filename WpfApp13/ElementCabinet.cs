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

namespace WpfApp13
{
    class ElementCabinet:_3DObject
    {
        
        public double thickness;      
        public Hashtable hashtable;
        public ElementCabinet select;       
        public ElementCabinet twoselect;    
        public int napr;                   
        public double min = 0;              
        public double max = 0;               
        public double center=-999;         
        public double otUL = 0;
        public double otDR = 0;
        public double centery = -999;      
        public double miny = 0;            
        public double maxy = 0;             
        public double descreaselen=0.02;
        public string iningroupel;         
        public double otsx;               
        public double otsy;                 
        public double otsz;                
        public double lenbox;               
        public bool basewallstatus = false;
        public bool basevisstatus = false;
        public ElementCabinet threeselect;
        public ElementCabinet fourselect;
        public int naprv;       

        public ElementCabinet(ModelUIElement3D temp) : base(temp){}
        public ElementCabinet(ModelUIElement3D temp, double lx, double ly, double lz, double ax, double ay, double az):base(temp,lx,ly,lz,ax,ay,az){}
        public ElementCabinet(ModelUIElement3D temp, double lx, double ly, double lz, double ax, double ay, double az,double tol,string gr,string ingr) : base(temp, lx, ly, lz, ax, ay, az)
        {
            groupel = gr;
            ingroupel = ingr;
            thickness = tol;
            changeObject(lx, ly, lz, ax, ay, az);
        }
        public void changeObject(double lx, double ly, double lz, double ax, double ay, double az)
        {
            string[] tmp = groupel.Split('-');
                if (ingroupel == "WallR")
                {
                    double px = ax;
                    double py = ay;
                    double pz = az;
                    axisX = ax;
                    axisY = ay;
                    axisZ = az;
                    lenX = lx;
                    lenY = ly;
                    lenZ = lz;
                    double lnx = thickness;
                    double lny = ly;
                    double lnz = lz- thickness;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (ingroupel == "WallL")
                {
                    double px = lx - thickness + ax;
                    double py = ay;
                    double pz = az;
                    axisX = ax;
                    axisY = ay;
                    axisZ = az;
                    lenX = lx;
                    lenY = ly;
                    lenZ = lz;
                    double lnx = thickness;
                    double lny = ly;
                    double lnz = lz- thickness;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (ingroupel == "WallB")
                {
                    double px = ax;
                    double py = -thickness + ay;
                    double pz = az;
                    axisX = ax;
                    axisY = ay;
                    axisZ = az;
                    lenX = lx;
                    lenY = ly;
                    lenZ = lz;
                    double lnx = lx;
                    double lny = thickness;
                    double lnz = lz;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (ingroupel == "WallF")
                {
                    double px = thickness + ax;
                    double py = -2 * thickness + ay + ly;
                    double pz = az;
                    axisX = ax;
                    axisY = ay;
                    axisZ = az;
                    lenX = lx;
                    lenY = ly;
                    lenZ = lz;
                    double lnx = lx - 2 * thickness;
                    double lny = thickness;
                    double lnz = 0.1;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (ingroupel == "Ceiling")
                {
                    double px = ax;
                    double py = ay;
                    double pz = lz + az- thickness;
                    axisX = ax;
                    axisY = ay;
                    axisZ = az;
                    lenX = lx;
                    lenY = ly;
                    lenZ = lz;
                    double lnx = lx;
                    double lny = ly;
                    double lnz = thickness;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (ingroupel == "Floor")
                {
                    double px = thickness + ax;
                    double py = ay;
                    double pz = 0.1 + az; ;
                    axisX = ax;
                    axisY = ay;
                    axisZ = az;
                    lenX = lx;
                    lenY = ly;
                    lenZ = lz;
                    double lnx = lx - 2 * thickness;
                    double lny = ly;
                    double lnz = thickness;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (ingroupel == "FloorFoot")
                {
                    double px = thickness + ax;
                    double py = ay;
                    double pz = az; ;
                    axisX = ax;
                    axisY = ay;
                    axisZ = az;
                    lenX = lx;
                    lenY = ly;
                    lenZ = lz;
                    double lnx = lx - 2 * thickness;
                    double lny = ly;
                    double lnz = thickness;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
            if (ingroupel == "ShelfC")
            {
                if (select == null)
                {
                    select = twoselect;
                    napr *= -1;
                    twoselect = null;
                }
                ArrayList walls = new ArrayList();
                ArrayList floors = new ArrayList();
                foreach (var item in hashtable)
                {
                    DictionaryEntry tmp1 = (DictionaryEntry)item;
                    if (tmp1.Value is ElementCabinet)
                    {
                        ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                        if (groupel == tmp2.groupel)
                        {
                            if (tmp2.ingroupel == "WallC" || tmp2.ingroupel == "WallR" || tmp2.ingroupel == "WallL") walls.Add(tmp2);
                            if (tmp2.ingroupel == "ShelfC" || tmp2.ingroupel == "Floor" || tmp2.ingroupel == "Ceiling" || tmp2.ingroupel == "FloorFoot") floors.Add(tmp2);
                        }
                    }
                }
                axisX = select.axisX;
                axisY = select.axisY;
                axisZ = select.axisZ;
                lenX = select.lenX;
                lenY = select.lenY;
                lenZ = select.lenZ;
                double minz =0;
                double maxz = 0;
                    foreach (var item in floors)
                    {
                        ElementCabinet tmp1 = item as ElementCabinet;
                        if (tmp1.ingroupel == "Ceiling")
                        {
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            maxz = tt.OffsetZ;
                            maxz -= tmp1.thickness;
                        }
                        if (tmp1.ingroupel == "Floor" || tmp1.ingroupel == "FloorFoor")
                        {
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            minz = tt.OffsetZ;
                            minz += tmp1.thickness;
                        }
                    }
                min = minz-axisZ;
                max = maxz - axisZ;
                double thL = 0;
                double thR = 0;
                double axR = 0;
                double axL = 0;
                double minyln = 0;
                if (walls.Count == 2)
                {                   
                    ElementCabinet tmp1 = walls[0] as ElementCabinet;
                    ElementCabinet tmp2 = walls[1] as ElementCabinet;
                    Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d2 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt2 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt2ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (tt1.OffsetX>tt2.OffsetX) 
                    {
                        thL = tmp1.thickness;
                        thR = tmp2.thickness;
                        axL = tt1.OffsetX;
                        axR = tt2.OffsetX;
                        if (tt1ln.ScaleY > tt2ln.ScaleY) minyln = tt2ln.ScaleY;
                        else minyln = tt1ln.ScaleY;
                        twoselect = tmp1;
                        select = tmp2;
                    }
                    else
                    {
                        thR = tmp1.thickness;
                        thL = tmp2.thickness;
                        axR = tt1.OffsetX;
                        axL = tt2.OffsetX;
                        if (tt1ln.ScaleY > tt2ln.ScaleY) minyln = tt2ln.ScaleY;
                        else minyln = tt1ln.ScaleY;
                        twoselect = tmp2;
                        select = tmp1;
                    }
                }
                if (walls.Count > 2) {
                    ElementCabinet tmp1 = select;
                    ElementCabinet tmp2 = null;
                    if (twoselect is null)
                    {
                        double minx = 999;

                        foreach (var item in walls)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1t = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            if (tt1ln.ScaleY > t3ln.ScaleY) continue;
                            if (napr == 1)
                            {
                                double raz = tt3.OffsetX - tt1.OffsetX;
                                if (raz < minx && raz > 0) minx = raz;
                            }
                            if (napr == -1)
                            {
                                double raz = tt1.OffsetX - tt3.OffsetX;
                                if (raz < minx && raz > 0) minx = raz;
                            }
                        }
                        foreach (var item in walls)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            if (tt1ln.ScaleY > t3ln.ScaleY) continue;
                            if (napr == 1)
                            {
                                double raz = tt3.OffsetX - tt1.OffsetX;
                                if (raz == minx) tmp2 = tmp3;
                            }
                            if (napr == -1)
                            {
                                double raz = tt1.OffsetX - tt3.OffsetX;
                                if (raz == minx) tmp2 = tmp3;
                            }
                        }
                        twoselect = tmp2;
                    }
                    else
                        tmp2 = twoselect;
                    Transform3DGroup group3d11 = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt11 = group3d11.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt11ln = group3d11.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d22 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt22 = group3d22.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt22ln = group3d22.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (napr == 1)
                    {
                        thR = tmp1.thickness;
                        thL = tmp2.thickness;
                        axR = tt11.OffsetX;
                        axL = tt22.OffsetX;
                        if (tt11ln.ScaleY > tt22ln.ScaleY) minyln = tt22ln.ScaleY;
                        else minyln = tt11ln.ScaleY;
                    }
                    if (napr == -1)
                    {
                        thL = tmp1.thickness;
                        thR = tmp2.thickness;
                        axL = tt11.OffsetX;
                        axR = tt22.OffsetX;
                        if (tt11ln.ScaleY > tt22ln.ScaleY) minyln = tt22ln.ScaleY;
                        else minyln = tt11ln.ScaleY;
                    }
                    if (center == -999) center = (tt11.OffsetZ + tt11ln.ScaleZ) / 2;
                }
                if (center == -999) center = (max + min) / 2;               
                double px = thR +axR+otDR;
                double py = axisY ;
                double pz = center+axisZ;             
                double lnx = axL-axR-thR-otDR-otUL;               
                double lny = minyln- descreaselen;
                if (basewallstatus) lny = minyln;
                double lnz = thickness;
                if (lnx < 0)
                {
                    ElementCabinet tel1 = hashtable[select.thisobject.GetHashCode()] as ElementCabinet;
                    ElementCabinet tel2 = hashtable[twoselect.thisobject.GetHashCode()] as ElementCabinet;
                    select = null;
                    select = tel2;
                    twoselect = null;
                    twoselect = tel1;           
                }
                changeObjectLen(lnx, lny, lnz);
                changeObjectPos(px, py, pz);
            }
            if (ingroupel == "Rack")
            {
                if (select == null)
                {
                    select = twoselect;
                    napr *= -1;
                    twoselect = null;
                }
                ArrayList walls = new ArrayList();
                ArrayList floors = new ArrayList();
                foreach (var item in hashtable)
                {
                    DictionaryEntry tmp1 = (DictionaryEntry)item;
                    if (tmp1.Value is ElementCabinet)
                    {
                        ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                        if (groupel == tmp2.groupel)
                        {
                            if (tmp2.ingroupel == "WallC" || tmp2.ingroupel == "WallR" || tmp2.ingroupel == "WallL") walls.Add(tmp2);
                            if (tmp2.ingroupel == "ShelfC" || tmp2.ingroupel == "Floor" || tmp2.ingroupel == "Ceiling" || tmp2.ingroupel == "FloorFoot") floors.Add(tmp2);
                        }
                    }
                }
                axisX = select.axisX;
                axisY = select.axisY;
                axisZ = select.axisZ;
                lenX = select.lenX;
                lenY = select.lenY;
                lenZ = select.lenZ;
                double minz = 0;
                double maxz = 0;
                foreach (var item in floors)
                {
                    ElementCabinet tmp1 = item as ElementCabinet;
                    if (tmp1.ingroupel == "Ceiling")
                    {
                        Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                        TranslateTransform3D tt = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                        maxz = tt.OffsetZ;
                        maxz -= tmp1.thickness;
                    }
                    if (tmp1.ingroupel == "Floor" || tmp1.ingroupel == "FloorFoor")
                    {
                        Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                        TranslateTransform3D tt = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                        minz = tt.OffsetZ;
                        minz += tmp1.thickness;
                    }
                }
                min = minz - axisZ;
                max = maxz - axisZ;
                double thL = 0;
                double thR = 0;
                double axR = 0;
                double axL = 0;
                double minyln = 0;
                if (walls.Count == 2)
                {
                    ElementCabinet tmp1 = walls[0] as ElementCabinet;
                    ElementCabinet tmp2 = walls[1] as ElementCabinet;
                    Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d2 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt2 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt2ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (tt1.OffsetX > tt2.OffsetX)
                    {
                        thL = tmp1.thickness;
                        thR = tmp2.thickness;
                        axL = tt1.OffsetX;
                        axR = tt2.OffsetX;
                        if (tt1ln.ScaleY >= tt2ln.ScaleY) { minyln = tt2ln.ScaleY; miny = tt2.OffsetY - axisY; maxy = tt2.OffsetY + tt2ln.ScaleY - axisY; }
                        else { minyln = tt1ln.ScaleY; miny = tt1.OffsetY - axisY; maxy = tt1.OffsetY + tt1ln.ScaleY - axisY; }
                        twoselect = tmp1;
                        select = tmp2;
                    }
                    else
                    {
                        thR = tmp1.thickness;
                        thL = tmp2.thickness;
                        axR = tt1.OffsetX;
                        axL = tt2.OffsetX;
                        if (tt1ln.ScaleY >= tt2ln.ScaleY) { minyln = tt2ln.ScaleY; miny = tt2.OffsetY - axisY; maxy = tt2.OffsetY + tt2ln.ScaleY - axisY; }
                        else { minyln = tt1ln.ScaleY; miny = tt1.OffsetY - axisY; maxy = tt1.OffsetY + tt1ln.ScaleY - axisY; }

                        twoselect = tmp2;
                        select = tmp1;
                    }
                }
                if (walls.Count > 2)
                {
                    ElementCabinet tmp1 = select;
                    ElementCabinet tmp2 = null;
                    if (twoselect is null)
                    {
                        double minx = 999;
                        foreach (var item in walls)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();

                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1t = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();


                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            if (tt1ln.ScaleY > t3ln.ScaleY) continue;
                            if (napr == 1)
                            {
                                double raz = tt3.OffsetX - tt1.OffsetX;
                                if (raz < minx && raz > 0) minx = raz;
                            }
                            if (napr == -1)
                            {
                                double raz = tt1.OffsetX - tt3.OffsetX;
                                if (raz < minx && raz > 0) minx = raz;
                            }
                        }
                        foreach (var item in walls)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            if (tt1ln.ScaleY > t3ln.ScaleY) continue;
                            if (napr == 1)
                            {
                                double raz = tt3.OffsetX - tt1.OffsetX;
                                if (raz == minx) tmp2 = tmp3;
                            }
                            if (napr == -1)
                            {
                                double raz = tt1.OffsetX - tt3.OffsetX;
                                if (raz == minx) tmp2 = tmp3;
                            }
                        }
                        twoselect = tmp2;
                    }
                    else
                        tmp2 = twoselect;
                    Transform3DGroup group3d11 = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt11 = group3d11.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt11ln = group3d11.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d22 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt22 = group3d22.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt22ln = group3d22.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (napr == 1)
                    {
                        thR = tmp1.thickness;
                        thL = tmp2.thickness;
                        axR = tt11.OffsetX;
                        axL = tt22.OffsetX;
                        if (tt11ln.ScaleY >= tt22ln.ScaleY) { minyln = tt22ln.ScaleY; miny = tt22.OffsetY; maxy = tt22.OffsetY + tt22ln.ScaleY; }
                        else { minyln = tt11ln.ScaleY; miny = tt11.OffsetY; maxy = tt11.OffsetY + tt11ln.ScaleY; }
                    }
                    if (napr == -1)
                    {
                        thL = tmp1.thickness;
                        thR = tmp2.thickness;
                        axL = tt11.OffsetX;
                        axR = tt22.OffsetX;
                        if (tt11ln.ScaleY >= tt22ln.ScaleY) { minyln = tt22ln.ScaleY; miny = tt22.OffsetY - axisY; maxy = tt22.OffsetY +tt22ln.ScaleY - axisY; }
                        else { minyln = tt11ln.ScaleY; miny = tt11.OffsetY - axisY; maxy = tt11.OffsetY + tt11ln.ScaleY - axisY; }
                    }
                    if (center == -999) center = (tt11.OffsetZ + tt11ln.ScaleZ) / 2;                   
                }
                if (center == -999) center = (max + min) / 2;
                if (centery == -999) centery = minyln / 2;
                double px = thR + axR;
                double py = centery+ axisY;
                double pz = center+ axisZ;
                double lnx = axL - axR - thR;
                double lny = 0.02;
                double lnz = 0.02;
                if (lnx < 0)
                {
                    ElementCabinet tel1 = hashtable[select.thisobject.GetHashCode()] as ElementCabinet;
                    ElementCabinet tel2 = hashtable[twoselect.thisobject.GetHashCode()] as ElementCabinet;
                    select = null;
                    select = tel2;
                    twoselect = null;
                    twoselect = tel1;
                }
                changeObjectLen(lnx, lny, lnz);
                changeObjectPos(px, py, pz);
            }
            if (ingroupel == "WallC")
            {
                if (select == null)
                {
                    select = twoselect;
                    napr *= -1;
                    twoselect = null;
                }
                ArrayList walls = new ArrayList();
                ArrayList floors = new ArrayList();
                foreach (var item in hashtable)
                {
                    DictionaryEntry tmp1 = (DictionaryEntry)item;
                    if (tmp1.Value is ElementCabinet)
                    {
                        ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                        if (groupel == tmp2.groupel)
                        {
                            if (tmp2.ingroupel == "WallR" || tmp2.ingroupel == "WallL") walls.Add(tmp2);
                            if (tmp2.ingroupel == "ShelfC" || tmp2.ingroupel == "Floor" || tmp2.ingroupel == "Ceiling" || tmp2.ingroupel == "FloorFoot") floors.Add(tmp2);
                        }
                    }
                }
                axisX = select.axisX;
                axisY = select.axisY;
                axisZ = select.axisZ;
                lenX = select.lenX;
                lenY = select.lenY;
                lenZ = select.lenZ;
                double minx = 0;
                double maxx = 0;             
                double thL = 0;
                double thR = 0;
                double azR = 0;
                double azL = 0;
                double minyln = 0;                
                    foreach (var item in walls)
                    {
                        ElementCabinet tmp1 = item as ElementCabinet;
                        if (tmp1.ingroupel == "WallL")
                        {
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            maxx = tt.OffsetX;
                            maxx -= tmp1.thickness; 
                        }
                        if (tmp1.ingroupel == "WallR")
                        {
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            minx = tt.OffsetX;
                            minx += tmp1.thickness;
                        }
                    }
                    min = minx - axisX;
                    max = maxx - axisX;
                if (floors.Count == 2)
                {
                    ElementCabinet tmp1 = floors[0] as ElementCabinet;
                    ElementCabinet tmp2 = floors[1] as ElementCabinet;
                    Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d2 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt2 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt2ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (tt1.OffsetX > tt2.OffsetX)
                    {
                        thL = tmp1.thickness;
                        thR = tmp2.thickness;
                        azL = tt1.OffsetZ;
                        azR = tt2.OffsetZ;
                        if (tt1ln.ScaleY > tt2ln.ScaleY) minyln = tt2ln.ScaleY;
                        else minyln = tt2ln.ScaleY;
                        select = tmp2;
                        twoselect = tmp1;
                    }
                    else
                    {
                        thR = tmp1.thickness;
                        thL = tmp2.thickness;
                        azR = tt1.OffsetZ;
                        azL = tt2.OffsetZ;
                        if (tt1ln.ScaleY > tt2ln.ScaleY) minyln = tt2ln.ScaleY;
                        else minyln = tt2ln.ScaleY;
                        select = tmp1;
                        twoselect = tmp2;
                    }                 
                }
                if (floors.Count > 2)
                {
                    ElementCabinet tmp1 = select;
                    ElementCabinet tmp2 = null;
                    if (twoselect is null)
                    {
                        double minz = 999;
                        foreach (var item in floors)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();                           
                            if (tt1ln.ScaleY > t3ln.ScaleY) continue;
                            if (napr == 1)
                            {
                                double raz = tt3.OffsetZ - tt1.OffsetZ;
                                if (raz < minz && raz > 0) minz = raz;
                            }
                            if (napr == -1)
                            {
                                double raz = tt1.OffsetZ - tt3.OffsetZ;
                                if (raz < minz && raz > 0) minz = raz;
                            }                           
                        }
                        foreach (var item in floors)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            if (napr == 1)
                            {
                                double raz = tt3.OffsetZ - tt1.OffsetZ;
                                if (raz == minz) tmp2 = tmp3;
                            }
                            if (napr == -1)
                            {
                                double raz = tt1.OffsetZ - tt3.OffsetZ;
                                if (raz == minz) tmp2 = tmp3;
                            }
                        }
                        if (min == 999) return;
                        twoselect = tmp2;
                    }
                    else tmp2 = twoselect;
                    Transform3DGroup group3d11 = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt11 = group3d11.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt11ln = group3d11.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d22 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt22 = group3d22.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt22ln = group3d22.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (napr == -1)
                    {
                        thR = tmp1.thickness;
                        thL = tmp2.thickness;
                        azR = tt11.OffsetZ;
                        azL = tt22.OffsetZ;
                        if (tt11ln.ScaleY > tt22ln.ScaleY) minyln = tt22ln.ScaleY;
                        else minyln = tt11ln.ScaleY;
                    }
                    if (napr == 1)
                    {
                        thL = tmp1.thickness;
                        thR = tmp2.thickness;
                        azL = tt11.OffsetZ;
                        azR = tt22.OffsetZ;
                        if (tt11ln.ScaleY > tt22ln.ScaleY) minyln = tt22ln.ScaleY;
                        else minyln = tt11ln.ScaleY;
                    }
                    if (center == -999) center = (tt11.OffsetX + tt11ln.ScaleX)/2;
                }
                if(center==-999) center = (max + min) / 2;
                
                double px = center+ axisX;
                double py = axisY;
                double pz = thL+azL+otDR;               
                double lnx = thickness ;
                double lny = minyln - descreaselen;
                if (basewallstatus) lny = minyln;
                double lnz = azR - azL - thL - otDR - otUL;
                if (lnz < 0)
                {
                    ElementCabinet tel1 = hashtable[select.thisobject.GetHashCode()] as ElementCabinet;
                    ElementCabinet tel2 = hashtable[twoselect.thisobject.GetHashCode()] as ElementCabinet;
                    select = null;
                    select = tel2;
                    twoselect = null;
                    twoselect = tel1;
                }
                changeObjectLen(lnx, lny, lnz);
                changeObjectPos(px, py, pz);
            }
            string[] tmpstr = ingroupel.Split('-');
            if (tmpstr[0] == "Box")
            {

                if (select == null)
                {
                    select = twoselect;
                    napr *= -1;
                    twoselect = null;
                }
                ArrayList walls = new ArrayList();
                ArrayList floors = new ArrayList();
                foreach (var item in hashtable)
                {
                    DictionaryEntry tmp1 = (DictionaryEntry)item;
                    if (tmp1.Value is ElementCabinet)
                    {
                        ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                        if (groupel == tmp2.groupel)
                        {
                            if (tmp2.ingroupel == "WallC" || tmp2.ingroupel == "WallR" || tmp2.ingroupel == "WallL") walls.Add(tmp2);
                            if (tmp2.ingroupel == "ShelfC" || tmp2.ingroupel == "Floor" || tmp2.ingroupel == "Ceiling" || tmp2.ingroupel == "FloorFoot") floors.Add(tmp2);
                        }
                    }
                }
                axisX = select.axisX;
                axisY = select.axisY;
                axisZ = select.axisZ;
                lenX = select.lenX;
                lenY = select.lenY;
                lenZ = select.lenZ;
                double minz = 0;
                double maxz = 0;
                foreach (var item in floors)
                {
                    ElementCabinet tmp1 = item as ElementCabinet;
                    if (tmp1.ingroupel == "Ceiling")
                    {
                        Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                        TranslateTransform3D tt = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                        maxz = tt.OffsetZ;
                        maxz -= tmp1.thickness;
                    }
                    if (tmp1.ingroupel == "Floor" || tmp1.ingroupel == "FloorFoor")
                    {
                        Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                        TranslateTransform3D tt = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                        minz = tt.OffsetZ;
                        minz += tmp1.thickness;
                    }
                }
                min = minz - axisZ;
                max = maxz - axisZ;
                double thL = 0;
                double thR = 0;
                double axR = 0;
                double axL = 0;
                double minyln = 0;
                if (walls.Count == 2)
                {
                    ElementCabinet tmp1 = walls[0] as ElementCabinet;
                    ElementCabinet tmp2 = walls[1] as ElementCabinet;
                    Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d2 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt2 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt2ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (tt1.OffsetX > tt2.OffsetX)
                    {
                        thL = tmp1.thickness;
                        thR = tmp2.thickness;
                        axL = tt1.OffsetX;
                        axR = tt2.OffsetX;
                        if (tt1ln.ScaleY > tt2ln.ScaleY) minyln = tt2ln.ScaleY;
                        else minyln = tt1ln.ScaleY;
                        twoselect = tmp1;
                        select = tmp2;
                    }
                    else
                    {
                        thR = tmp1.thickness;
                        thL = tmp2.thickness;
                        axR = tt1.OffsetX;
                        axL = tt2.OffsetX;
                        if (tt1ln.ScaleY > tt2ln.ScaleY) minyln = tt2ln.ScaleY;
                        else minyln = tt1ln.ScaleY;
                        twoselect = tmp2;
                        select = tmp1;
                    }
                }
                if (walls.Count > 2)
                {
                    ElementCabinet tmp1 = select;
                    ElementCabinet tmp2 = null;
                    if (twoselect is null)
                    {
                        double minx = 999;
                        foreach (var item in walls)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1t = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            if (tt1ln.ScaleY > t3ln.ScaleY) continue;
                            if (napr == 1)
                            {
                                double raz = tt3.OffsetX - tt1.OffsetX;
                                if (raz < minx && raz > 0) minx = raz;
                            }
                            if (napr == -1)
                            {
                                double raz = tt1.OffsetX - tt3.OffsetX;
                                if (raz < minx && raz > 0) minx = raz;
                            }
                        }
                        foreach (var item in walls)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();

                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            if (tt1ln.ScaleY > t3ln.ScaleY) continue;

                            if (napr == 1)
                            {
                                double raz = tt3.OffsetX - tt1.OffsetX;
                                if (raz == minx) tmp2 = tmp3;
                            }
                            if (napr == -1)
                            {
                                double raz = tt1.OffsetX - tt3.OffsetX;
                                if (raz == minx) tmp2 = tmp3;
                            }
                        }
                        twoselect = tmp2;
                    }
                    else
                        tmp2 = twoselect;
                    Transform3DGroup group3d11 = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt11 = group3d11.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt11ln = group3d11.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d22 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt22 = group3d22.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt22ln = group3d22.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (napr == 1)
                    {
                        thR = tmp1.thickness;
                        thL = tmp2.thickness;
                        axR = tt11.OffsetX;
                        axL = tt22.OffsetX;
                        if (tt11ln.ScaleY > tt22ln.ScaleY) minyln = tt22ln.ScaleY;
                        else minyln = tt11ln.ScaleY;

                    }
                    if (napr == -1)
                    {
                        thL = tmp1.thickness;
                        thR = tmp2.thickness;
                        axL = tt11.OffsetX;
                        axR = tt22.OffsetX;
                        if (tt11ln.ScaleY > tt22ln.ScaleY) minyln = tt22ln.ScaleY;
                        else minyln = tt11ln.ScaleY;

                    }
                    if (center == -999) center = (tt11.OffsetZ + tt11ln.ScaleZ) / 2;
                }
                if (center == -999) center = (max + min) / 2;
                if (iningroupel == "WR")
                {
                    double px = thR + axR + otsx;
                    double py = axisY + otsy;
                    double pz = center + otsz + axisZ;
                    double lnx = thickness;
                    double lny = minyln - otsy;
                    double lnz = lenbox - 2 * otsz;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }

                if (iningroupel == "WL")
                {
                    double px = axL - otsx - thickness;
                    double py = axisY + otsy;
                    double pz = center + otsz + axisZ;
                    double lnx = thickness;
                    double lny = minyln - otsy;
                    double lnz = lenbox - 2 * otsz;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (iningroupel == "WF")
                {
                    double px = axR + thR + otsx + thickness;
                    double py = axisY + minyln - thickness;
                    double pz = center + otsz + axisZ;
                    double lnx = axL - axR - thR - 2 * otsx - 2 * thickness;
                    double lny = thickness;
                    double lnz = lenbox - 2 * otsz;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (iningroupel == "WB")
                {
                    double px = axR + thR + otsx + thickness;
                    double py = axisY + otsy;
                    double pz = center + otsz + axisZ;
                    double lnx = axL - axR - thR - 2 * otsx - 2 * thickness;
                    double lny = thickness;
                    double lnz = lenbox - 2 * otsz;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (iningroupel == "WFF")
                {
                    double px = axR;
                    double py = axisY + minyln;
                    double pz = center + axisZ;
                    double lnx = axL - axR + thL;
                    double lny = thickness;
                    double lnz = lenbox;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (iningroupel == "WD")
                {
                    double px = axR + thR + otsx;
                    double py = axisY + otsy;
                    double pz = axisZ+center + otsz - thickness;
                    double lnx = axL - axR - thR - 2 * otsx;
                    double lny = minyln - otsy;
                    double lnz = thickness;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
            }
            if (tmpstr[0] == "Element")
            {
                if (iningroupel == "El")
                {
                    axisX = ax;
                    axisY = ay;
                    axisZ = az;
                    lenX = lx;
                    lenY = ly;
                    lenZ = lz;
                    int anX = getAngleX();
                    int anY = getAngleY();
                    int anZ = getAngleZ();
                    ChangeAngleX(0);
                    ChangeAngleY(0);
                    ChangeAngleZ(0);
                    changeObjectElaxX(select.getPosX()+ otsx );
                    changeObjectElaxY(select.getPosY()+ otsy );
                    changeObjectElaxZ(select.getPosZ()+ otsz );
                    ChangeAngleX(anX);
                    ChangeAngleY(anY);
                    ChangeAngleZ(anZ);
                }
            }
            if (ingroupel.IndexOf("Door") != -1)
            {
                if (select == null)
                {
                    select = twoselect;
                    napr *= -1;
                    twoselect = null;
                }
                if (threeselect == null)
                {
                    threeselect = fourselect;
                    naprv *= -1;
                    fourselect = null;
                }
                ArrayList walls = new ArrayList();
                ArrayList floors = new ArrayList();
                foreach (var item in hashtable)
                {
                    DictionaryEntry tmp1 = (DictionaryEntry)item;
                    if (tmp1.Value is ElementCabinet)
                    {
                        ElementCabinet tmp2 = tmp1.Value as ElementCabinet;
                        if (groupel == tmp2.groupel)
                        {
                            if (tmp2.ingroupel == "WallC" || tmp2.ingroupel == "WallR" || tmp2.ingroupel == "WallL") walls.Add(tmp2);
                            if (tmp2.ingroupel == "ShelfC" || tmp2.ingroupel == "Floor" || tmp2.ingroupel == "Ceiling" || tmp2.ingroupel == "FloorFoot") floors.Add(tmp2);
                        }
                    }
                }
                axisX = select.axisX;
                axisY = select.axisY;
                axisZ = select.axisZ;
                lenX = select.lenX;
                lenY = select.lenY;
                lenZ = select.lenZ;
                double thL = 0;
                double thR = 0;
                double axR = 0;
                double axL = 0;
                double minyln = 0;
                if (walls.Count == 2)
                {
                    ElementCabinet tmp1 = walls[0] as ElementCabinet;
                    ElementCabinet tmp2 = walls[1] as ElementCabinet;
                    Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d2 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt2 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt2ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (tt1.OffsetX > tt2.OffsetX)
                    {
                        thL = tmp1.thickness;
                        thR = tmp2.thickness;
                        axL = tt1.OffsetX;
                        axR = tt2.OffsetX;
                        if (tt1ln.ScaleY > tt2ln.ScaleY) minyln = tt2ln.ScaleY;
                        else minyln = tt1ln.ScaleY;
                        twoselect = tmp1;
                        select = tmp2;
                    }
                    else
                    {
                        thR = tmp1.thickness;
                        thL = tmp2.thickness;
                        axR = tt1.OffsetX;
                        axL = tt2.OffsetX;
                        if (tt1ln.ScaleY > tt2ln.ScaleY) minyln = tt2ln.ScaleY;
                        else minyln = tt1ln.ScaleY;
                        twoselect = tmp2;
                        select = tmp1;
                    }
                }
                if (walls.Count > 2)
                {
                    ElementCabinet tmp1 = select;
                    ElementCabinet tmp2 = null;
                    if (twoselect is null)
                    {
                        double minx = 999;
                        foreach (var item in walls)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1t = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            if (tt1ln.ScaleY > t3ln.ScaleY) continue;
                            if (napr == 1)
                            {
                                double raz = tt3.OffsetX - tt1.OffsetX;
                                if (raz < minx && raz > 0) minx = raz;
                            }
                            if (napr == -1)
                            {
                                double raz = tt1.OffsetX - tt3.OffsetX;
                                if (raz < minx && raz > 0) minx = raz;
                            }
                        }
                        foreach (var item in walls)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            if (tt1ln.ScaleY > t3ln.ScaleY) continue;
                            if (napr == 1)
                            {
                                double raz = tt3.OffsetX - tt1.OffsetX;
                                if (raz == minx) tmp2 = tmp3;
                            }
                            if (napr == -1)
                            {
                                double raz = tt1.OffsetX - tt3.OffsetX;
                                if (raz == minx) tmp2 = tmp3;
                            }
                        }
                        twoselect = tmp2;
                    }
                    else
                        tmp2 = twoselect;
                    Transform3DGroup group3d11 = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt11 = group3d11.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt11ln = group3d11.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d22 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt22 = group3d22.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt22ln = group3d22.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (napr == 1)
                    {
                        thR = tmp1.thickness;
                        thL = tmp2.thickness;
                        axR = tt11.OffsetX;
                        axL = tt22.OffsetX;
                        if (tt11ln.ScaleY > tt22ln.ScaleY) minyln = tt22ln.ScaleY;
                        else minyln = tt11ln.ScaleY;
                    }
                    if (napr == -1)
                    {
                        thL = tmp1.thickness;
                        thR = tmp2.thickness;
                        axL = tt11.OffsetX;
                        axR = tt22.OffsetX;
                        if (tt11ln.ScaleY > tt22ln.ScaleY) minyln = tt22ln.ScaleY;
                        else minyln = tt11ln.ScaleY;
                    }
                }
                double thU = 0;
                double thD = 0;
                double azD = 0;
                double azU = 0;
                double minylnUD = 0;
                if (floors.Count == 2)
                {
                    ElementCabinet tmp1 = floors[0] as ElementCabinet;
                    ElementCabinet tmp2 = floors[1] as ElementCabinet;
                    Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d2 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt2 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt2ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (tt1.OffsetX < tt2.OffsetX)
                    {
                        thU = tmp1.thickness;
                        thD = tmp2.thickness;
                        azU = tt1.OffsetZ;
                        azD = tt2.OffsetZ;
                        if (tt1ln.ScaleY > tt2ln.ScaleY) minylnUD = tt2ln.ScaleY;
                        else minylnUD = tt2ln.ScaleY;
                        threeselect = tmp2;
                        fourselect = tmp1;
                    }
                    else
                    {
                        thD = tmp1.thickness;
                        thU = tmp2.thickness;
                        azD = tt1.OffsetZ;
                        azU = tt2.OffsetZ;
                        if (tt1ln.ScaleY > tt2ln.ScaleY) minylnUD = tt2ln.ScaleY;
                        else minylnUD = tt2ln.ScaleY;
                        threeselect = tmp1;
                        fourselect = tmp2;
                    }
                }
                if (floors.Count > 2)
                {
                    ElementCabinet tmp1 = threeselect;
                    ElementCabinet tmp2 = null;
                    if (fourselect is null)
                    {
                        double minz = 999;
                        foreach (var item in floors)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            if (tt1ln.ScaleY > t3ln.ScaleY) continue;
                            if (naprv == 1)
                            {
                                double raz = tt3.OffsetZ - tt1.OffsetZ;
                                if (raz < minz && raz > 0) minz = raz;
                            }
                            if (naprv == -1)
                            {
                                double raz = tt1.OffsetZ - tt3.OffsetZ;
                                if (raz < minz && raz > 0) minz = raz;
                            }
                        }
                        foreach (var item in floors)
                        {
                            ElementCabinet tmp3 = item as ElementCabinet;
                            Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt1 = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3d2 = tmp3.thismodel.Transform as Transform3DGroup;
                            TranslateTransform3D tt3 = group3d2.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                            Transform3DGroup group3dd = thismodel.Transform as Transform3DGroup;
                            ScaleTransform3D tt1ln = group3dd.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t1ln = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            ScaleTransform3D t3ln = group3d2.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                            if (tt1ln.ScaleY > t3ln.ScaleY) continue;
                            if (naprv == 1)
                            {
                                double raz = tt3.OffsetZ - tt1.OffsetZ;
                                if (raz == minz) tmp2 = tmp3;
                            }
                            if (naprv == -1)
                            {
                                double raz = tt1.OffsetZ - tt3.OffsetZ;
                                if (raz == minz) tmp2 = tmp3;
                            }
                        }
                        if (min == 999) return;
                        fourselect = tmp2;
                    }
                    else tmp2 = fourselect;
                    Transform3DGroup group3d11 = tmp1.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt11 = group3d11.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt11ln = group3d11.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    Transform3DGroup group3d22 = tmp2.thismodel.Transform as Transform3DGroup;
                    TranslateTransform3D tt22 = group3d22.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    ScaleTransform3D tt22ln = group3d22.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                    if (naprv == -1)
                    {
                        thD = tmp1.thickness;
                        thU = tmp2.thickness;
                        azD = tt11.OffsetZ;
                        azU = tt22.OffsetZ;
                        if (tt11ln.ScaleY > tt22ln.ScaleY) minylnUD = tt22ln.ScaleY;
                        else minylnUD = tt11ln.ScaleY;
                    }
                    if (naprv == 1)
                    {
                        thU = tmp1.thickness;
                        thD = tmp2.thickness;
                        azU = tt11.OffsetZ;
                        azD = tt22.OffsetZ;
                        if (tt11ln.ScaleY > tt22ln.ScaleY) minylnUD = tt22ln.ScaleY;
                        else minylnUD = tt11ln.ScaleY;
                    }
                }
                double lnx = 0;
                double lnz = 0;
                if (ingroupel == "Door")
                {
                    double px = axR;
                    double py = axisY + minyln + 0.002;
                    double pz = azD;
                    lnx = axL - axR + thL;
                    double lny = thickness;
                    lnz = azU - azD + thU;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (iningroupel == "DoorL")
                {
                    double px = axR + (axL - axR + thL) / 2 + 0.002;
                    double py = axisY + minyln + 0.002;
                    double pz = azD;
                    lnx = (axL - axR + thL) / 2 - 0.002;
                    double lny = thickness;
                    lnz = azU - azD + thU;                  
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (iningroupel == "DoorR")
                {
                    double px = axR;
                    double py = axisY + minyln + 0.002;
                    double pz = azD;
                    lnx = (axL - axR + thL) / 2 - 0.002;
                    double lny = thickness;
                    lnz = azU - azD + thU;
                    changeObjectLen(lnx, lny, lnz);
                    changeObjectPos(px, py, pz);
                }
                if (lnx < 0)
                {
                    ElementCabinet tel1 = hashtable[select.thisobject.GetHashCode()] as ElementCabinet;
                    ElementCabinet tel2 = hashtable[twoselect.thisobject.GetHashCode()] as ElementCabinet;
                    select = null;
                    select = tel2;
                    twoselect = null;
                    twoselect = tel1;
                }
                if (lnz < 0)
                {
                    ElementCabinet tel1 = hashtable[threeselect.thisobject.GetHashCode()] as ElementCabinet;
                    ElementCabinet tel2 = hashtable[fourselect.thisobject.GetHashCode()] as ElementCabinet;
                    threeselect = null;
                    threeselect = tel2;
                    fourselect = null;
                    fourselect = tel1;
                }                            
            }          
        }
        public void rotateObject(int anglex,int angley,int anglez)
        {
            string[] tmpstr = ingroupel.Split('-');
            if (tmpstr[0] == "Element")
            {
                if (iningroupel == "El")
                {
                    int angx = anglex;
                    int angy = angley;
                    int angz = anglez;

                    ChangeAngleX(angx);
                    ChangeAngleY(angy);
                    ChangeAngleZ(angz);
                }
            }
        }
        public void changeObjectElln(double l,double l1,double l2)
        {
            string[] tmpstr = ingroupel.Split('-');
            if (tmpstr[0] == "Element")
            {
                if (iningroupel == "El")
                {              
                    changeObjectLen(l, l1 , l2 );
                }
            }
        }
        public void changeObjectElaxX( double ax)
        {
            string[] tmpstr = ingroupel.Split('-');
            if (tmpstr[0] == "Element")
            {
                if (iningroupel == "El")
                {
                    Transform3DGroup group = (Transform3DGroup)thismodel.Transform;
                    TranslateTransform3D tt = group.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    tt.OffsetX = ax;
                }
            }
        }
        public void changeObjectElaxY(double ay)
        {
            string[] tmpstr = ingroupel.Split('-');
            if (tmpstr[0] == "Element")
            {
                if (iningroupel == "El")
                {
                    Transform3DGroup group = (Transform3DGroup)thismodel.Transform;
                    TranslateTransform3D tt = group.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    tt.OffsetY = ay;
                }
            }
        }
        public void changeObjectElaxZ( double az)
        {
            string[] tmpstr = ingroupel.Split('-');
            if (tmpstr[0] == "Element")
            {
                if (iningroupel == "El")
                {
                    Transform3DGroup group = (Transform3DGroup)thismodel.Transform;
                    TranslateTransform3D tt = group.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                    tt.OffsetZ = az;
                }
            }
        }
        public override void changeObjectLen(double lx, double ly, double lz)
        {
            thismodel = (GeometryModel3D)thisobject.Model;
            thismesh = (MeshGeometry3D)thismodel.Geometry;
            Transform3DGroup group = (Transform3DGroup)thismodel.Transform;
            ScaleTransform3D tt = group.Children.OfType<ScaleTransform3D>().FirstOrDefault();
            tt.ScaleX =lx;
            tt.ScaleY = ly;
            tt.ScaleZ = lz;
        }
        public override void changeObjectPos(double ax, double ay, double az)
        {
            thismodel = (GeometryModel3D)thisobject.Model;
            thismesh = (MeshGeometry3D)thismodel.Geometry;

            Transform3DGroup group = (Transform3DGroup)thismodel.Transform;
            TranslateTransform3D tt = group.Children.OfType<TranslateTransform3D>().FirstOrDefault();
            tt.OffsetX = ax;
            tt.OffsetY = ay;
            tt.OffsetZ = az;
        }
       
        public int getAngleX()
        {
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            RotateTransform3D rr11 = group3d.Children[2] as RotateTransform3D;
            AxisAngleRotation3D ax = rr11.Rotation as AxisAngleRotation3D;
            return Convert.ToInt32(ax.Angle);
        }
        public int getAngleY()
        {
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            RotateTransform3D rr11 = group3d.Children[3] as RotateTransform3D;
            AxisAngleRotation3D ax = rr11.Rotation as AxisAngleRotation3D;
            return Convert.ToInt32(ax.Angle);
        }
        public int getAngleZ()
        {
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            RotateTransform3D rr11 = group3d.Children[4] as RotateTransform3D;
            AxisAngleRotation3D ax = rr11.Rotation as AxisAngleRotation3D;
            return Convert.ToInt32(ax.Angle);
        }
        public double getLenX()
        {
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            ScaleTransform3D tt1= group3d.Children[0] as ScaleTransform3D;
            return tt1.ScaleX;
        }
        public double getLenY()
        {
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            ScaleTransform3D tt1 = group3d.Children[0] as ScaleTransform3D;
            return tt1.ScaleY;
        }
        public double getLenZ()
        {
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            ScaleTransform3D tt1 = group3d.Children[0] as ScaleTransform3D;
            return tt1.ScaleZ;
        }
        public double getPosX()
        {
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tt1 = group3d.Children[1] as TranslateTransform3D;
            return tt1.OffsetX;
        }
        public double getPosY()
        {
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tt1 = group3d.Children[1] as TranslateTransform3D;
            return tt1.OffsetY;
        }
        public double getPosZ()
        {
            Transform3DGroup group3d = thismodel.Transform as Transform3DGroup;
            TranslateTransform3D tt1 = group3d.Children[1] as TranslateTransform3D;
            return tt1.OffsetZ;
        }
        public void outControlElements(Canvas elementIn, ModelUIElement3D select1, ModelUIElement3D select2, ModelUIElement3D select3, ModelUIElement3D select4, MainWindow win)
        {
            elementIn.Children.Clear();
            ModelUIElement3D[] masel = new ModelUIElement3D[4];
            masel[0] = select1;
            masel[1] = select2;
            masel[2] = select3;
            masel[3] = select4;
            double nach = -30;
            FormattedText ft;
            if (elementIn.Children.Count == 0) nach = -30;
            else
            {
                nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                if (double.IsNaN(nach)) { nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);

                }
            }
            TextBlock featurecorpus = new TextBlock
            {
                Text = "Характеристики корпуса",
                FontSize = 19
            };
            ft = new FormattedText(featurecorpus.Text, CultureInfo.CurrentUICulture, featurecorpus.FlowDirection,new Typeface(featurecorpus.FontFamily, featurecorpus.FontStyle,featurecorpus.FontWeight, featurecorpus.FontStretch),featurecorpus.FontSize, featurecorpus.Foreground);
            featurecorpus.Height = ft.Height; ;
            featurecorpus.Width = ft.Width;
            elementIn.Children.Add(featurecorpus);
            Canvas.SetTop(featurecorpus, nach + 30);
            Canvas.SetLeft(featurecorpus, elementIn.ActualWidth / 2 - featurecorpus.Width / 2);

            TextBlock featurecorpus1 = new TextBlock
            {
                Text = "Положение корпуса",
                FontSize = 17
            };
            ft = new FormattedText(featurecorpus1.Text, CultureInfo.CurrentUICulture, featurecorpus1.FlowDirection, new Typeface(featurecorpus1.FontFamily, featurecorpus1.FontStyle, featurecorpus1.FontWeight, featurecorpus1.FontStretch), featurecorpus1.FontSize, featurecorpus1.Foreground);
            featurecorpus1.Height = ft.Height; ;
            featurecorpus1.Width = ft.Width;
            elementIn.Children.Add(featurecorpus1);
            Canvas.SetTop(featurecorpus1, Canvas.GetTop(featurecorpus) + 30);
            Canvas.SetLeft(featurecorpus1, elementIn.ActualWidth / 2 - featurecorpus1.Width / 2);


            TextBox axisXc = new TextBox
            {
                Width = elementIn.ActualWidth / 2 - 30,
                Height = featurecorpus.Height + 4,
                FontSize = 17,
                Tag = "axisX",
                Text = this.axisX * 100 + ""
            };
            axisXc.PreviewTextInput += win._PreviewTextInput;
            axisXc.TextChanged += win._TextChanged;
            elementIn.Children.Add(axisXc);
            Canvas.SetTop(axisXc, Canvas.GetTop(featurecorpus1) + 30);
            Canvas.SetLeft(axisXc, 0);        
            TextBlock textaxX = new TextBlock
            {
                Text = "По оси X",
                FontSize = 17
            };
            ft = new FormattedText(textaxX.Text, CultureInfo.CurrentUICulture, textaxX.FlowDirection,new Typeface(textaxX.FontFamily, textaxX.FontStyle,textaxX.FontWeight, textaxX.FontStretch),textaxX.FontSize, textaxX.Foreground);
            textaxX.Height = ft.Height; ;
            textaxX.Width = ft.Width;
            elementIn.Children.Add(textaxX);
            Canvas.SetTop(textaxX, Canvas.GetTop(featurecorpus1) + 30);
            Canvas.SetLeft(textaxX, elementIn.ActualWidth / 2 - 30);
            TextBox axisYc = new TextBox
            {
                Width = elementIn.ActualWidth / 2 - 30,
                Height = featurecorpus.Height + 4,
                FontSize = 17,
                Tag = "axisY",
                Text = this.axisY * 100 + ""
            };
            axisYc.PreviewTextInput += win._PreviewTextInput;
            axisYc.TextChanged += win._TextChanged;
            elementIn.Children.Add(axisYc);
            Canvas.SetTop(axisYc, Canvas.GetTop(axisXc) + 40);
            Canvas.SetLeft(axisYc, 0);
            TextBlock textaxY = new TextBlock
            {
                Text = "По оси Y",
                FontSize = 17
            };
            ft = new FormattedText(textaxY.Text, CultureInfo.CurrentUICulture, textaxY.FlowDirection, new Typeface(textaxY.FontFamily, textaxY.FontStyle, textaxY.FontWeight, textaxY.FontStretch),textaxY.FontSize, textaxY.Foreground);
            textaxY.Height = ft.Height; ;
            textaxY.Width = ft.Width;
            elementIn.Children.Add(textaxY);
            Canvas.SetTop(textaxY, Canvas.GetTop(axisXc) + 40);
            Canvas.SetLeft(textaxY, elementIn.ActualWidth / 2 - 30);
            TextBox axisZc = new TextBox
            {
                Width = elementIn.ActualWidth / 2 - 30,
                Height = featurecorpus.Height + 4,
                FontSize = 17,
                Tag = "axisZ",
                Text = this.axisZ * 100 + ""
            };
            axisZc.PreviewTextInput += win._PreviewTextInput;
            axisZc.TextChanged += win._TextChanged;
            elementIn.Children.Add(axisZc);
            Canvas.SetTop(axisZc, Canvas.GetTop(axisYc) + 40);
            Canvas.SetLeft(axisZc, 0);
            TextBlock textaxZ = new TextBlock
            {
                Text = "По оси Z",
                FontSize = 17
            };
            ft = new FormattedText(textaxZ.Text, CultureInfo.CurrentUICulture, textaxZ.FlowDirection,new Typeface(textaxZ.FontFamily, textaxZ.FontStyle, textaxZ.FontWeight, textaxZ.FontStretch),textaxZ.FontSize, textaxZ.Foreground);
            textaxZ.Height = ft.Height; ;
            textaxZ.Width = ft.Width;
            elementIn.Children.Add(textaxZ);
            Canvas.SetTop(textaxZ, Canvas.GetTop(axisYc) + 40);
            Canvas.SetLeft(textaxZ, elementIn.ActualWidth / 2 - 30);

            TextBlock featurecorpus2 = new TextBlock
            {
                Text = "Размеры корпуса",
                FontSize = 17
            };
            ft = new FormattedText(featurecorpus2.Text, CultureInfo.CurrentUICulture, featurecorpus2.FlowDirection, new Typeface(featurecorpus2.FontFamily, featurecorpus2.FontStyle, featurecorpus2.FontWeight, featurecorpus2.FontStretch), featurecorpus2.FontSize, featurecorpus2.Foreground);
            featurecorpus2.Height = ft.Height; ;
            featurecorpus2.Width = ft.Width;
            elementIn.Children.Add(featurecorpus2);
            Canvas.SetTop(featurecorpus2, Canvas.GetTop(axisZc) + 40);
            Canvas.SetLeft(featurecorpus2, elementIn.ActualWidth / 2 - featurecorpus2.Width / 2);

            TextBox lenXc = new TextBox
            {
                Width = elementIn.ActualWidth / 2 - 30,
                Height = featurecorpus.Height + 4,
                FontSize = 17,
                Tag = "lenX",
                Text = this.lenX * 100 + ""
            };
            lenXc.PreviewTextInput += win._PreviewTextInput;
            lenXc.TextChanged += win._TextChanged;
            elementIn.Children.Add(lenXc);
            Canvas.SetTop(lenXc, Canvas.GetTop(featurecorpus2) + 30);
            Canvas.SetLeft(lenXc, 0);
            TextBlock textlenX = new TextBlock
            {
                Text = "Длина корпуса",
                FontSize = 17
            };
            ft = new FormattedText(textlenX.Text, CultureInfo.CurrentUICulture, textlenX.FlowDirection,  new Typeface(textlenX.FontFamily, textlenX.FontStyle,textlenX.FontWeight, textlenX.FontStretch), textlenX.FontSize, textlenX.Foreground);
            textlenX.Height = ft.Height; ;
            textlenX.Width = ft.Width;
            elementIn.Children.Add(textlenX);
            Canvas.SetTop(textlenX, Canvas.GetTop(featurecorpus2) + 30);
            Canvas.SetLeft(textlenX, elementIn.ActualWidth / 2 - 30);
            TextBox lenYc = new TextBox
            {
                Width = elementIn.ActualWidth / 2 - 30,
                Height = featurecorpus.Height + 4,
                FontSize = 17,
                Tag = "lenY",
                Text = this.lenY * 100 + ""
            };
            lenYc.PreviewTextInput += win._PreviewTextInput;
            lenYc.TextChanged += win._TextChanged;
            elementIn.Children.Add(lenYc);
            Canvas.SetTop(lenYc, Canvas.GetTop(lenXc) + 40);
            Canvas.SetLeft(lenYc, 0);
            TextBlock textlenY = new TextBlock
            {
                Text = "Глубина корпуса",
                FontSize = 17
            };
            ft = new FormattedText(textlenY.Text, CultureInfo.CurrentUICulture, textlenY.FlowDirection,new Typeface(textlenY.FontFamily, textlenY.FontStyle,textlenY.FontWeight, textlenY.FontStretch),textlenY.FontSize, textlenY.Foreground);
            textlenY.Height = ft.Height; ;
            textlenY.Width = ft.Width;
            elementIn.Children.Add(textlenY);
            Canvas.SetTop(textlenY, Canvas.GetTop(lenXc) + 40);
            Canvas.SetLeft(textlenY, elementIn.ActualWidth / 2 - 30);
            TextBox lenZc = new TextBox
            {
                Width = elementIn.ActualWidth / 2 - 30,
                Height = featurecorpus.Height + 4,
                FontSize = 17,
                Tag = "lenZ",
                Text = this.lenZ * 100 + ""
            };
            lenZc.PreviewTextInput += win._PreviewTextInput;
            lenZc.TextChanged += win._TextChanged;
            elementIn.Children.Add(lenZc);
            Canvas.SetTop(lenZc, Canvas.GetTop(lenYc) + 40);
            Canvas.SetLeft(lenZc, 0);
            TextBlock textlenZ = new TextBlock
            {
                Text = "Высота корпуса",
                FontSize = 17
            };
            ft = new FormattedText(textlenZ.Text, CultureInfo.CurrentUICulture, textlenZ.FlowDirection, new Typeface(textlenZ.FontFamily, textlenZ.FontStyle,textlenZ.FontWeight, textlenZ.FontStretch),textlenZ.FontSize, textlenZ.Foreground);
            textlenZ.Height = ft.Height; ;
            textlenZ.Width = ft.Width;
            elementIn.Children.Add(textlenZ);
            Canvas.SetTop(textlenZ, Canvas.GetTop(lenYc) + 40);
            Canvas.SetLeft(textlenZ, elementIn.ActualWidth / 2 - 30);
            Button texture = new Button
            {
                Width = elementIn.ActualWidth / 2 + elementIn.ActualWidth / 2 - 10,
                Height = featurecorpus.Height*2 + 4,
                
                VerticalAlignment = VerticalAlignment.Center,
            };
            
            texture.Click += win.ChangeTexture_Click;
            texture.Tag = "All";
            texture.Content = "Изменить материал всем \n             выбранным";
            texture.FontSize = 17;
            texture.HorizontalContentAlignment = HorizontalAlignment.Center;
            elementIn.Children.Add(texture);
            Canvas.SetTop(texture, Canvas.GetTop(lenZc) + 40);
            Canvas.SetLeft(texture, elementIn.ActualWidth / 2 - texture.Width / 2);
            Line separcrel = new Line
            {
                X1 = 0,
                X2 = elementIn.ActualWidth,
                Y1 = Canvas.GetTop(texture) + 60,
                Y2 = Canvas.GetTop(texture) + 60,
                StrokeThickness = 2,
                Stroke = Brushes.Black
            };
            elementIn.Children.Add(separcrel);
            string[,] tags = { { "Th1", "All1", "Base1","OTDR1", "OTUL1", "VID1" }, { "Th2", "All2", "Base2", "OTDR2", "OTUL2", "VID2" }, { "Th3", "All3", "Base3", "OTDR3", "OTUL3", "VID3" }, { "Th4", "All4", "Base4", "OTDR4", "OTUL4", "VID4" } };
            int countmasel = 0;
            for (int i = 0; i < 4; i++) if (masel[i] != null) countmasel++;           
            for (int i = 0; i < countmasel; i++)
            {               
                nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                if (double.IsNaN(nach))
                {
                    nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                }
                ElementCabinet tmpel1 = hashtable[masel[i].GetHashCode()] as ElementCabinet;
                if (tmpel1.ingroupel.IndexOf("Element") != -1) continue;
                TextBlock featureelement = new TextBlock
                {
                    Text = "Хар-ка " + (i + 1) + " выбранного елемента",
                    Tag = tags[i,0],
                    FontSize = 20,
                    TextWrapping=TextWrapping.Wrap,
                    TextAlignment=TextAlignment.Center
                };
                featureelement.MouseMove += win.OBJ_MouseMoveTB;
                featureelement.MouseLeave += win.OBJ_MouseLeave;
                ft = new FormattedText(featureelement.Text, CultureInfo.CurrentUICulture, featureelement.FlowDirection,new Typeface(featureelement.FontFamily, featureelement.FontStyle,featureelement.FontWeight, featureelement.FontStretch),featureelement.FontSize, featureelement.Foreground);
                featureelement.Height = ft.Height*2;
                featureelement.Width = featurecorpus.Width;
                elementIn.Children.Add(featureelement);
                Canvas.SetTop(featureelement, nach + 60);
                Canvas.SetLeft(featureelement, elementIn.ActualWidth / 2 - featureelement.Width / 2);            
                ElementCabinet tmp1 = hashtable[masel[i].GetHashCode()] as ElementCabinet;
                Transform3DGroup group3d = tmp1.thismodel.Transform as Transform3DGroup;
                ScaleTransform3D t2 = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();

                TextBlock featureelement1 = new TextBlock
                {
                    Text = "Размеры елемента",
                    FontSize = 17
                };
                ft = new FormattedText(featureelement1.Text, CultureInfo.CurrentUICulture, featureelement1.FlowDirection, new Typeface(featureelement1.FontFamily, featureelement1.FontStyle, featureelement1.FontWeight, featureelement1.FontStretch), featureelement1.FontSize, featureelement1.Foreground);
                featureelement1.Height = ft.Height; ;
                featureelement1.Width = ft.Width;
                elementIn.Children.Add(featureelement1);
                Canvas.SetTop(featureelement1, Canvas.GetTop(featureelement) + 60);
                Canvas.SetLeft(featureelement1, elementIn.ActualWidth / 2 - featureelement1.Width / 2);
                featureelement1.MouseMove += win.OBJ_MouseMoveTB;
                featureelement1.MouseLeave += win.OBJ_MouseLeave;

                TextBlock textlenXel = new TextBlock
                {
                    Text = "Длина по X " + t2.ScaleX * 100,
                    Tag = tags[i, 0],
                    FontSize = 17
                };
                ft = new FormattedText(textlenXel.Text, CultureInfo.CurrentUICulture, textlenXel.FlowDirection,new Typeface(textlenXel.FontFamily, textlenXel.FontStyle,textlenXel.FontWeight, textlenXel.FontStretch),textlenXel.FontSize, textlenXel.Foreground);
                textlenXel.Height = ft.Height; 
                textlenXel.Width = ft.Width;
                textlenXel.MouseMove += win.OBJ_MouseMoveTB;
                textlenXel.MouseLeave += win.OBJ_MouseLeave;
                elementIn.Children.Add(textlenXel);
                Canvas.SetTop(textlenXel, Canvas.GetTop(featureelement1) + 40);
                Canvas.SetLeft(textlenXel, elementIn.ActualWidth / 2 - textlenXel.Width / 2);
                TextBlock textlenYel = new TextBlock
                {
                    Text = "Длина по Y " + t2.ScaleY * 100,
                    Tag = tags[i, 0],
                    FontSize = 17
                };
                ft = new FormattedText(textlenYel.Text, CultureInfo.CurrentUICulture, textlenYel.FlowDirection, new Typeface(textlenYel.FontFamily, textlenYel.FontStyle, textlenYel.FontWeight, textlenYel.FontStretch),textlenYel.FontSize, textlenYel.Foreground);
                textlenYel.Height = ft.Height; ;
                textlenYel.Width = ft.Width;
                textlenYel.MouseMove += win.OBJ_MouseMoveTB;
                textlenYel.MouseLeave += win.OBJ_MouseLeave;
                elementIn.Children.Add(textlenYel);
                Canvas.SetTop(textlenYel, Canvas.GetTop(textlenXel) + 40);
                Canvas.SetLeft(textlenYel, elementIn.ActualWidth / 2 - textlenYel.Width / 2);
                TextBlock textlenZel = new TextBlock
                {
                    Text = "Длина по Z " + t2.ScaleZ * 100,
                    Tag = tags[i, 0],
                    FontSize = 17
                };
                ft = new FormattedText(textlenZel.Text, CultureInfo.CurrentUICulture, textlenZel.FlowDirection,new Typeface(textlenZel.FontFamily, textlenZel.FontStyle,textlenZel.FontWeight, textlenZel.FontStretch),textlenZel.FontSize, textlenZel.Foreground);
                textlenZel.Height = ft.Height; ;
                textlenZel.Width = ft.Width;
                textlenZel.MouseMove += win.OBJ_MouseMoveTB;
                textlenZel.MouseLeave += win.OBJ_MouseLeave;
                elementIn.Children.Add(textlenZel);
                Canvas.SetTop(textlenZel, Canvas.GetTop(textlenYel) + 40);
                Canvas.SetLeft(textlenZel, elementIn.ActualWidth / 2 - textlenZel.Width / 2);
                ElementCabinet tmp = hashtable[masel[i].GetHashCode()] as ElementCabinet;
                if (tmp.ingroupel != "Rack")
                {

                    TextBlock featureelement2 = new TextBlock
                    {
                        Text = "Характерные параметры",
                        FontSize = 17
                    };
                    ft = new FormattedText(featureelement2.Text, CultureInfo.CurrentUICulture, featureelement2.FlowDirection, new Typeface(featureelement2.FontFamily, featureelement2.FontStyle, featureelement2.FontWeight, featureelement2.FontStretch), featureelement2.FontSize, featureelement2.Foreground);
                    featureelement2.Height = ft.Height; ;
                    featureelement2.Width = ft.Width;
                    elementIn.Children.Add(featureelement2);
                    Canvas.SetTop(featureelement2, Canvas.GetTop(textlenZel) + 40);
                    Canvas.SetLeft(featureelement2, elementIn.ActualWidth / 2 - featureelement2.Width / 2);
                    featureelement2.MouseMove += win.OBJ_MouseMoveTB;
                    featureelement2.MouseLeave += win.OBJ_MouseLeave;

                    TextBox lenTh = new TextBox
                    {
                        Width = elementIn.ActualWidth / 2 - 40,
                        Height = featurecorpus.Height + 4,
                        FontSize = 17,
                        Tag = tags[i, 0],
                        Text = tmpel1.thickness * 100 + ""
                    };
                    lenTh.PreviewTextInput += win._PreviewTextInput;
                    lenTh.TextChanged += win._TextChangedTH;
                    elementIn.Children.Add(lenTh);
                    Canvas.SetTop(lenTh, Canvas.GetTop(featureelement2) + 40);
                    Canvas.SetLeft(lenTh, 0);
                    lenTh.MouseMove += win.OBJ_MouseMove;
                    lenTh.MouseLeave += win.OBJ_MouseLeave;
                    TextBlock textlenTh = new TextBlock
                    {
                        Text = "Толщина елемента",
                        Tag = tags[i, 0],
                        FontSize = 17
                    };
                    ft = new FormattedText(textlenTh.Text, CultureInfo.CurrentUICulture, textlenTh.FlowDirection,new Typeface(textlenTh.FontFamily, textlenTh.FontStyle, textlenTh.FontWeight, textlenTh.FontStretch),textlenTh.FontSize, textlenTh.Foreground);
                    textlenTh.Height = ft.Height; ;
                    textlenTh.Width = ft.Width;
                    elementIn.Children.Add(textlenTh);
                    Canvas.SetTop(textlenTh, Canvas.GetTop(featureelement2) + 40);
                    Canvas.SetLeft(textlenTh, elementIn.ActualWidth / 2 - 40);
                    textlenTh.MouseMove += win.OBJ_MouseMoveTB;
                    textlenTh.MouseLeave += win.OBJ_MouseLeave;
                   

                   
                   
                    if (tmp1.ingroupel == "WallC" || tmp1.ingroupel == "ShelfC")
                    {
                        nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                        if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                        TextBlock textchb = new TextBlock
                        {
                            FontSize = 17,
                            Tag = tags[i, 2]
                        };
                        if (tmp1.ingroupel == "WallC") textchb.Text = "Состояние стены";
                        if (tmp1.ingroupel == "ShelfC") textchb.Text = "Состояние полочки";
                        ft = new FormattedText(textchb.Text, CultureInfo.CurrentUICulture, textchb.FlowDirection,new Typeface(textchb.FontFamily, textchb.FontStyle,textchb.FontWeight, textchb.FontStretch), textchb.FontSize, textchb.Foreground);
                        textchb.Height = ft.Height; ;
                        textchb.Width = ft.Width;
                        elementIn.Children.Add(textchb);
                        Canvas.SetTop(textchb, nach + 40);
                        Canvas.SetLeft(textchb, elementIn.ActualWidth / 2 - textchb.Width / 2);
                        textchb.MouseMove += win.OBJ_MouseMoveTB;
                        textchb.MouseLeave += win.OBJ_MouseLeave;
                        CheckBox cb = new CheckBox
                        {
                            Tag = tags[i, 2],
                            Height = 20,
                            Width=20,
                            IsChecked=basewallstatus
                        };
                        if (basewallstatus) cb.IsChecked = false;
                        else cb.IsChecked = true;
                        cb.Checked += win.CheckBox_Checked;
                        cb.Unchecked += win.CheckBox_Unchecked;
                        elementIn.Children.Add(cb);
                        Canvas.SetTop(cb, Canvas.GetTop(textchb) + 45);
                        Canvas.SetLeft(cb, elementIn.ActualWidth / 2 - elementIn.ActualWidth / 4-30);
                        cb.MouseMove += win.OBJ_MouseMove;
                        cb.MouseLeave += win.OBJ_MouseLeave;
                        TextBox lenchb = new TextBox
                        {
                            Width = elementIn.ActualWidth / 2 -40,
                            Height = featureelement2.Height + 4,
                            FontSize = 17,
                            Tag = tags[i, 2],
                            Text = this.descreaselen * 100 + ""
                        };
                        lenchb.PreviewTextInput += win._PreviewTextInput;
                        lenchb.TextChanged += win._TextChangedTH;
                        elementIn.Children.Add(lenchb);
                        Canvas.SetTop(lenchb, Canvas.GetTop(textchb) + 40);
                        Canvas.SetLeft(lenchb, elementIn.ActualWidth / 2 - elementIn.ActualWidth / 4 + 20);
                        lenchb.MouseMove += win.OBJ_MouseMove;
                        lenchb.MouseLeave += win.OBJ_MouseLeave;
                        TextBlock textchb1 = new TextBlock
                        {
                            FontSize = 17,
                            Tag = tags[i, 2]
                        };
                        textchb1.Text = "Отступ";                       
                        ft = new FormattedText(textchb1.Text, CultureInfo.CurrentUICulture, textchb1.FlowDirection,new Typeface(textchb1.FontFamily, textchb1.FontStyle,textchb1.FontWeight, textchb1.FontStretch), textchb1.FontSize, textchb1.Foreground);
                        textchb1.Height = ft.Height; ;
                        textchb1.Width = ft.Width;
                        elementIn.Children.Add(textchb1);
                        Canvas.SetTop(textchb1, Canvas.GetTop(textchb) + 40);
                        Canvas.SetLeft(textchb1, elementIn.ActualWidth / 2 + textchb1.Width / 2+20);
                        textchb1.MouseMove += win.OBJ_MouseMoveTB;
                        textchb1.MouseLeave += win.OBJ_MouseLeave;
                        TextBlock textot1 = new TextBlock
                        {
                            FontSize = 17,
                            Tag = tags[i, 3]
                        };
                        if (tmp.ingroupel == "ShelfC") textot1.Text = "Отступ справа";
                        if (tmp.ingroupel == "WallC") textot1.Text = "Отступ снизу";
                        ft = new FormattedText(textot1.Text, CultureInfo.CurrentUICulture, textot1.FlowDirection,new Typeface(textot1.FontFamily, textot1.FontStyle,textot1.FontWeight, textot1.FontStretch),textot1.FontSize, textot1.Foreground);
                        textot1.Height = ft.Height; ;
                        textot1.Width = ft.Width;
                        elementIn.Children.Add(textot1);
                        Canvas.SetTop(textot1, Canvas.GetTop(textchb1) + 40);
                        Canvas.SetLeft(textot1, elementIn.ActualWidth / 2 - textot1.Width / 2);
                        textot1.MouseMove += win.OBJ_MouseMoveTB;
                        textot1.MouseLeave += win.OBJ_MouseLeave;
                        TextBox polw = new TextBox
                        {
                            Width = elementIn.ActualWidth / 2 - 40,
                            Height = featureelement2.Height + 4,
                            Tag = tags[i, 3],
                            FontSize = 17
                        };
                        polw.PreviewTextInput += win._PreviewTextInput;
                        elementIn.Children.Add(polw);
                        Canvas.SetTop(polw, Canvas.GetTop(textot1) + 40);
                        Canvas.SetLeft(polw, elementIn.ActualWidth / 2 + 30);
                        polw.MouseMove += win.OBJ_MouseMove;
                        polw.MouseLeave += win.OBJ_MouseLeave;
                        Slider slid = new Slider
                        {
                            Width = elementIn.ActualWidth / 2 + 30,
                            TickFrequency = 0.5,
                            IsSnapToTickEnabled = true,
                            IsDirectionReversed = true,
                            Tag = tags[i, 3]            
                        };
                        if (tmp.ingroupel == "ShelfC")
                        {
                            slid.Minimum = 0;
                            slid.Maximum = tmp.lenX * 100;
                            slid.Value = tmp.otDR * 100;
                        }
                        if (tmp.ingroupel == "WallC")
                        {
                            slid.Minimum = 0;
                            slid.Maximum = tmp.lenZ*100;
                            slid.Value = tmp.otDR * 100;
                        }
                        elementIn.Children.Add(slid);
                        Canvas.SetTop(slid, Canvas.GetTop(textot1) + 40);
                        Canvas.SetLeft(slid, 0);
                        slid.ValueChanged += win.SlideRast_ChangeZpos;
                        slid.MouseMove += win.OBJ_MouseMove;
                        slid.MouseLeave += win.OBJ_MouseLeave;
                        Binding binding = new Binding
                        {
                            Source = slid,
                            Path = new PropertyPath("Value"),
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };
                        polw.SetBinding(TextBox.TextProperty, binding);
                        TextBlock textot2 = new TextBlock
                        {
                            FontSize = 17,
                            Tag = tags[i, 4]
                        };
                        if (tmp.ingroupel == "ShelfC") textot2.Text = "Отступ слева";
                        if (tmp.ingroupel == "WallC") textot2.Text = "Отступ сверху";
                        ft = new FormattedText(textot2.Text, CultureInfo.CurrentUICulture, textot2.FlowDirection,new Typeface(textot2.FontFamily, textot2.FontStyle,textot2.FontWeight, textot2.FontStretch), textot2.FontSize, textot2.Foreground);
                        textot2.Height = ft.Height; ;
                        textot2.Width = ft.Width;
                        elementIn.Children.Add(textot2);
                        Canvas.SetTop(textot2, Canvas.GetTop(polw) + 40);
                        Canvas.SetLeft(textot2, elementIn.ActualWidth / 2 - textot2.Width / 2);
                        textot2.MouseMove += win.OBJ_MouseMoveTB;
                        textot2.MouseLeave += win.OBJ_MouseLeave;
                        TextBox polw1 = new TextBox
                        {
                            Width = elementIn.ActualWidth / 2 - 40,
                            Height = featureelement2.Height + 4,
                            Tag = tags[i, 4],
                            FontSize = 17
                        };
                        polw1.PreviewTextInput += win._PreviewTextInput;
                        elementIn.Children.Add(polw1);
                        Canvas.SetTop(polw1, Canvas.GetTop(textot2) + 40);
                        Canvas.SetLeft(polw1, elementIn.ActualWidth / 2 + 30);
                        polw1.MouseMove += win.OBJ_MouseMove;
                        polw1.MouseLeave += win.OBJ_MouseLeave;
                        Slider slid1 = new Slider
                        {
                            Width = elementIn.ActualWidth / 2 + 30,
                            TickFrequency = 0.5,
                            IsSnapToTickEnabled = true,
                            IsDirectionReversed = true,
                            Tag = tags[i, 4],

                        };
                        if (tmp.ingroupel == "ShelfC")
                        {
                            slid1.Minimum = 0;
                            slid1.Maximum = tmp.lenX * 100;
                            slid1.Value = tmp.otUL * 100;
                        }
                        if (tmp.ingroupel == "WallC")
                        {
                            slid1.Minimum = 0;
                            slid1.Maximum = tmp.lenZ * 100;
                            slid1.Value = tmp.otUL * 100;
                        }
                        elementIn.Children.Add(slid1);
                        Canvas.SetTop(slid1, Canvas.GetTop(textot2) + 40);
                        Canvas.SetLeft(slid1, 0);
                        slid1.ValueChanged += win.SlideRast_ChangeZpos;
                        slid1.MouseMove += win.OBJ_MouseMove;
                        slid1.MouseLeave += win.OBJ_MouseLeave;
                        Binding binding1 = new Binding
                        {
                            Source = slid1,
                            Path = new PropertyPath("Value"),

                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };
                        polw1.SetBinding(TextBox.TextProperty, binding1);
                    }            
                }
                nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);

                TextBlock textchb12 = new TextBlock
                {
                    FontSize = 17,
                    Tag = tags[i, 5]
                };
                textchb12.Text = "Видимость елемента";
                ft = new FormattedText(textchb12.Text, CultureInfo.CurrentUICulture, textchb12.FlowDirection, new Typeface(textchb12.FontFamily, textchb12.FontStyle, textchb12.FontWeight, textchb12.FontStretch), textchb12.FontSize, textchb12.Foreground);
                textchb12.Height = ft.Height; ;
                textchb12.Width = ft.Width;
                elementIn.Children.Add(textchb12);
                Canvas.SetTop(textchb12, nach + 30);
                Canvas.SetLeft(textchb12, elementIn.ActualWidth / 2 - textchb12.Width / 2);

                CheckBox cb1 = new CheckBox
                {
                    Tag = tags[i, 5],
                    Height = 20,
                    Width = 20,
                    IsChecked = basewallstatus
                };

                DiffuseMaterial texture11 = tmp.thismodel.Material as DiffuseMaterial;
                ImageBrush texture22 = texture11.Brush as ImageBrush;
                if (texture22.Opacity == 0.1) { cb1.IsChecked = false; basevisstatus = true; }
                else { cb1.IsChecked = true; basevisstatus = false; }

                cb1.Checked += win.CheckBox_Checked;
                cb1.Unchecked += win.CheckBox_Unchecked;
                elementIn.Children.Add(cb1);
                Canvas.SetTop(cb1, nach + 35);
                Canvas.SetLeft(cb1, elementIn.ActualWidth / 2 - elementIn.ActualWidth / 4 - 45);

                Button texture1 = new Button
                {
                    Width = elementIn.ActualWidth / 2 + elementIn.ActualWidth / 2 - 10,
                    Height = featurecorpus.Height + 4,
                    Tag = tags[i, 1],
                    Content = "Изменить материал ",
                    FontSize=17
                  
                };
                texture1.Click += win.ChangeTexture_Click;
                elementIn.Children.Add(texture1);
                Canvas.SetTop(texture1, nach + 60);
                Canvas.SetLeft(texture1, elementIn.ActualWidth / 2 - texture1.Width / 2);
                texture1.MouseMove += win.OBJ_MouseMove;
                texture1.MouseLeave += win.OBJ_MouseLeave;

                Line separelot = new Line
                    {
                        X1 = 0,
                        X2 = elementIn.ActualWidth,
                        Y1 = nach + 95,
                        Y2 = nach + 95,
                        StrokeThickness = 2,
                        Stroke = Brushes.Black
                    };
                    elementIn.Children.Add(separelot);
            }
            if ((ingroupel == "WallL" || ingroupel == "WallR" || ingroupel == "WallC") && select2 == null)
            {
                if (ingroupel == "WallR")
                {
                    nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                    if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                    Image tmpimg = new Image();
                    tmpimg.Source = new BitmapImage(new Uri("img/wallgr.bmp", UriKind.Relative));
                    Button createShelfGl = new Button
                    {
                        Tag = "L", 
                        Content = tmpimg,
                        Width = 50,
                        Height = 50
                    };          
                    elementIn.Children.Add(createShelfGl);
                    Canvas.SetTop(createShelfGl, nach + 50);
                    Canvas.SetLeft(createShelfGl, elementIn.ActualWidth / 2 - createShelfGl.Width / 2 );
                    createShelfGl.Click += win.CreateShelfG_Click;
                    Image tmpimg1 = new Image();
                    tmpimg1.Source = new BitmapImage(new Uri("img/boxgr.bmp", UriKind.Relative));
                    Button createBoxl = new Button
                    {
                        Tag = "L",
                        Content = tmpimg1,
                        Width = 50,
                        Height = 50
                    };              
                    elementIn.Children.Add(createBoxl);
                    Canvas.SetTop(createBoxl, nach + 50);
                    Canvas.SetLeft(createBoxl, elementIn.ActualWidth / 2 - createBoxl.Width / 2 + 50);
                    createBoxl.Click += win.CreateBox_Click;
                    Image tmpimg2 = new Image();
                    tmpimg2.Source = new BitmapImage(new Uri("img/rackr.bmp", UriKind.Relative));
                    Button createRackL = new Button
                    {
                        Tag = "L",
                        Content = tmpimg2,
                        Width = 50,
                        Height = 50
                    };

                    elementIn.Children.Add(createRackL);
                    Canvas.SetTop(createRackL, nach + 50);
                    Canvas.SetLeft(createRackL, elementIn.ActualWidth / 2 - createRackL.Width / 2 - 50);
                    createRackL.Click += win.CreateRack_Click;
                }
                if (ingroupel == "WallL")
                {
                    nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                    if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                    Image tmpimg = new Image();
                    tmpimg.Source = new BitmapImage(new Uri("img/wallgl.bmp", UriKind.Relative));
                    Button createShelfGR = new Button
                    {
                        Tag = "R",
                        Content = tmpimg,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createShelfGR);
                    Canvas.SetTop(createShelfGR, nach + 50);
                    Canvas.SetLeft(createShelfGR, elementIn.ActualWidth / 2 - createShelfGR.Width / 2 );
                    createShelfGR.Click += win.CreateShelfG_Click;
                    Image tmpimg1 = new Image();
                    tmpimg1.Source = new BitmapImage(new Uri("img/boxgl.bmp", UriKind.Relative));
                    Button createBoxr = new Button
                    {
                        Tag = "R",
                        Content = tmpimg1,
                        Width = 50,
                        Height = 50
                    };                                     
                    elementIn.Children.Add(createBoxr);
                    Canvas.SetTop(createBoxr, nach + 50);
                    Canvas.SetLeft(createBoxr, elementIn.ActualWidth / 2 - createBoxr.Width / 2 + 50);
                    createBoxr.Click += win.CreateBox_Click;
                    Image tmpimg2 = new Image();
                    tmpimg2.Source = new BitmapImage(new Uri("img/rackl.bmp", UriKind.Relative));
                    Button createRackR = new Button
                    {
                        Tag = "R",
                        Content = tmpimg2,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createRackR);
                    Canvas.SetTop(createRackR, nach + 50);
                    Canvas.SetLeft(createRackR, elementIn.ActualWidth / 2 - createRackR.Width / 2 - 50);
                    createRackR.Click += win.CreateRack_Click;
                }
                if (ingroupel == "WallC")
                {
                    nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                    if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                    Image tmpimg = new Image();
                    tmpimg.Source = new BitmapImage(new Uri("img/wallgr.bmp", UriKind.Relative));
                    Button createShelfGl = new Button
                    {
                        Tag = "L",
                        Content = tmpimg,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createShelfGl);
                    Canvas.SetTop(createShelfGl, nach + 50);
                    Canvas.SetLeft(createShelfGl, elementIn.ActualWidth / 2 - createShelfGl.Width / 2 - 25);                    
                    Image tmpimg1 = new Image();
                    tmpimg1.Source = new BitmapImage(new Uri("img/wallgl.bmp", UriKind.Relative));
                    Button createShelfGR = new Button
                    {
                        Tag = "R",
                        Content = tmpimg1,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createShelfGR);
                    Canvas.SetTop(createShelfGR, nach + 50);
                    Canvas.SetLeft(createShelfGR, elementIn.ActualWidth / 2 - createShelfGR.Width / 2 + 25);
                    createShelfGl.Click += win.CreateShelfG_Click;
                    createShelfGR.Click += win.CreateShelfG_Click;
                    Image tmpimg2 = new Image();
                    tmpimg2.Source = new BitmapImage(new Uri("img/boxgl.bmp", UriKind.Relative));
                    Button createBoxr = new Button
                    {
                        Tag = "R",
                        Content = tmpimg2,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createBoxr);
                    Canvas.SetTop(createBoxr, nach + 50);
                    Canvas.SetLeft(createBoxr, elementIn.ActualWidth / 2 - createBoxr.Width / 2 + 75);
                    Image tmpimg3 = new Image();
                    tmpimg3.Source = new BitmapImage(new Uri("img/boxgr.bmp", UriKind.Relative));
                    Button createBoxl = new Button
                    {
                        Tag = "L",
                        Content = tmpimg3,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createBoxl);
                    Canvas.SetTop(createBoxl, nach + 50);
                    Canvas.SetLeft(createBoxl, elementIn.ActualWidth / 2 - createBoxl.Width / 2 - 75);
                    createBoxr.Click += win.CreateBox_Click;
                    createBoxl.Click += win.CreateBox_Click;
                    Image tmpimg4 = new Image();
                    tmpimg4.Source = new BitmapImage(new Uri("img/rackl.bmp", UriKind.Relative));
                    Button createRackr = new Button
                    {
                        Tag = "R",
                        Content = tmpimg4,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createRackr);
                    Canvas.SetTop(createRackr, nach + 100);
                    Canvas.SetLeft(createRackr, elementIn.ActualWidth / 2 - createRackr.Width / 2 + 25);
                    Image tmpimg5 = new Image();
                    tmpimg5.Source = new BitmapImage(new Uri("img/rackr.bmp", UriKind.Relative));
                    Button createRackl = new Button
                    {
                        Tag = "L",
                        Content = tmpimg5,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createRackl);
                    Canvas.SetTop(createRackl, nach + 100);
                    Canvas.SetLeft(createRackl, elementIn.ActualWidth / 2 - createRackl.Width / 2 - 25);
                    createRackr.Click += win.CreateRack_Click;
                    createRackl.Click += win.CreateRack_Click;
                }

                nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                Line separelot = new Line
                {
                    X1 = 0,
                    X2 = elementIn.ActualWidth,
                    Y1 = nach + 60,
                    Y2 = nach + 60,
                    StrokeThickness = 2,
                    Stroke = Brushes.Black
                };
                elementIn.Children.Add(separelot);

            }
            if ((ingroupel == "Ceiling" || ingroupel == "Floor" || ingroupel == "FloorFoot" || ingroupel == "ShelfC") && select2 == null)
            {
                if ((ingroupel == "Floor" || ingroupel == "FloorFoot")&& select2 == null)
                {
                    nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                    if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                    Image tmpimg = new Image();
                    tmpimg.Source = new BitmapImage(new Uri("img/wallvd.bmp", UriKind.Relative));
                    Button createShelfVU = new Button
                    {
                        Tag = "U",
                        Content = tmpimg,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createShelfVU);
                    Canvas.SetTop(createShelfVU, nach + 50);
                    Canvas.SetLeft(createShelfVU, elementIn.ActualWidth / 2 - createShelfVU.Width / 2);
                    createShelfVU.Click += win.CreateShelfV_Click;
                }

                if (ingroupel == "Ceiling" && select2 == null)
                {
                    nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                    if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                    Image tmpimg = new Image();
                    tmpimg.Source = new BitmapImage(new Uri("img/wallvu.bmp", UriKind.Relative));
                    Button createShelfVD = new Button
                    {
                        Tag = "D",
                        Content = tmpimg,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createShelfVD);
                    Canvas.SetTop(createShelfVD, nach + 50);
                    Canvas.SetLeft(createShelfVD, elementIn.ActualWidth / 2 - createShelfVD.Width / 2);
                    createShelfVD.Click += win.CreateShelfV_Click;
                }
                if (ingroupel == "ShelfC")
                {
                    nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                    if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                    Image tmpimg = new Image();
                    tmpimg.Source = new BitmapImage(new Uri("img/wallvd.bmp", UriKind.Relative));
                    Button createShelfVU = new Button
                    {
                        Tag = "U",
                        Content = tmpimg,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createShelfVU);
                    Canvas.SetTop(createShelfVU, nach + 50);
                    Canvas.SetLeft(createShelfVU, elementIn.ActualWidth / 2 - createShelfVU.Width / 2 - 25);
                    Image tmpimg1 = new Image();
                    tmpimg1.Source = new BitmapImage(new Uri("img/wallvu.bmp", UriKind.Relative));
                    Button createShelfVD = new Button
                    {
                        Tag = "D",
                        Content = tmpimg1,
                        Width = 50,
                        Height = 50
                    };
                    elementIn.Children.Add(createShelfVD);
                    Canvas.SetTop(createShelfVD, nach + 50);
                    Canvas.SetLeft(createShelfVD, elementIn.ActualWidth / 2 - createShelfVD.Width / 2 + 25);
                    createShelfVU.Click += win.CreateShelfV_Click;
                    createShelfVD.Click += win.CreateShelfV_Click;
                }
                nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                Line separelot = new Line
                {
                    X1 = 0,
                    X2 = elementIn.ActualWidth,
                    Y1 = nach + 60,
                    Y2 = nach + 60,
                    StrokeThickness = 2,
                    Stroke = Brushes.Black
                };
                elementIn.Children.Add(separelot);
            }
            string[] tmpstr = ingroupel.Split('-');
            if (select1 != null && select2 != null && select3 == null && select4 == null)
            {
                ElementCabinet tmp1 = hashtable[select1.GetHashCode()] as ElementCabinet;
                ElementCabinet tmp2 = hashtable[select2.GetHashCode()] as ElementCabinet;
                if (this.objgr == tmp1.objgr && this.objgr == tmp2.objgr)
                {
                    nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                    if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                    if (this.objgr == "W")
                    {
                        Image tmpimg = new Image();
                        tmpimg.Source = new BitmapImage(new Uri("img/wallg.bmp", UriKind.Relative));
                        Button createShelfG = new Button
                        {
                            Content = tmpimg,
                            Width = 50,
                            Height = 50
                        };
                        elementIn.Children.Add(createShelfG);
                        Canvas.SetTop(createShelfG, nach + 50);
                        Canvas.SetLeft(createShelfG, elementIn.ActualWidth / 2 - createShelfG.Width / 2 );
                        createShelfG.Click += win.CreateShelfVG_Click;
                        Image tmpimg1 = new Image();
                        tmpimg1.Source = new BitmapImage(new Uri("img/boxg.bmp", UriKind.Relative));
                        Button createBoxg = new Button
                        {
                            Content = tmpimg1,
                            Width = 50,
                            Height = 50
                        };
                        elementIn.Children.Add(createBoxg);
                        Canvas.SetTop(createBoxg, nach + 50);
                        Canvas.SetLeft(createBoxg, elementIn.ActualWidth / 2 - createBoxg.Width / 2 + 50);
                        createBoxg.Click += win.CreateBOX_Click;
                        Image tmpimg2 = new Image();
                        tmpimg2.Source = new BitmapImage(new Uri("img/rack.bmp", UriKind.Relative));
                        Button createRack = new Button
                        {                         
                            Content = tmpimg2,
                            Width = 50,
                            Height = 50
                        };
                        elementIn.Children.Add(createRack);
                        Canvas.SetTop(createRack, nach + 50);
                        Canvas.SetLeft(createRack, elementIn.ActualWidth / 2 - createRack.Width / 2 - 50);
                        createRack.Click += win.CreateRackTWOSel_Click;
                        nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                        if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                        Line separelot = new Line
                        {
                            X1 = 0,
                            X2 = elementIn.ActualWidth,
                            Y1 = nach + 60,
                            Y2 = nach + 60,
                            StrokeThickness = 2,
                            Stroke = Brushes.Black
                        };
                        elementIn.Children.Add(separelot);
                    }
                    if (this.objgr == "S")
                    {
                        Image tmpimg = new Image();
                        tmpimg.Source = new BitmapImage(new Uri("img/wallv.bmp", UriKind.Relative));
                        Button createShelfV = new Button
                        {
                            Content = tmpimg,
                            Width = 50,
                            Height = 50
                        };
                        elementIn.Children.Add(createShelfV);
                        Canvas.SetTop(createShelfV, nach + 50);
                        Canvas.SetLeft(createShelfV, elementIn.ActualWidth / 2 - createShelfV.Width / 2);
                        createShelfV.Click += win.CreateShelfVG_Click;
                        nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                        if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                        Line separelot = new Line
                        {
                            X1 = 0,
                            X2 = elementIn.ActualWidth,
                            Y1 = nach + 60,
                            Y2 = nach + 60,
                            StrokeThickness = 2,
                            Stroke = Brushes.Black
                        };
                        elementIn.Children.Add(separelot);
                    }
                }
                
            }
            if (select1 != null && select2 != null && select3 != null && select4 != null)
            {
                nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                Image tmpimg = new Image();
                tmpimg.Source = new BitmapImage(new Uri("img/door2.bmp", UriKind.Relative));
                Button createShelfV = new Button
                {
                    Tag = "1",
                    Content = tmpimg,
                    Width = 50,
                    Height = 50
                };
                elementIn.Children.Add(createShelfV);
                Canvas.SetTop(createShelfV, nach + 50);
                Canvas.SetLeft(createShelfV, elementIn.ActualWidth / 2 - createShelfV.Width / 2+25);
                createShelfV.Click += win.CreateDOOR_Click;
                Image tmpimg2 = new Image();
                tmpimg2.Source = new BitmapImage(new Uri("img/door1.bmp", UriKind.Relative));
                Button createRack = new Button
                {
                    Tag="2",
                    Content = tmpimg2,
                    Width = 50,
                    Height = 50
                };
                elementIn.Children.Add(createRack);
                Canvas.SetTop(createRack, nach + 50);
                Canvas.SetLeft(createRack, elementIn.ActualWidth / 2 - createRack.Width / 2 - 25);
                createRack.Click += win.CreateDOOR_Click;
                TextBlock postwall = new TextBlock();
                postwall.FontSize = 17;
                postwall.Text = "Количество елементов";
                ft = new FormattedText(postwall.Text, CultureInfo.CurrentUICulture, postwall.FlowDirection, new Typeface(postwall.FontFamily, postwall.FontStyle, postwall.FontWeight, postwall.FontStretch),postwall.FontSize, postwall.Foreground);
                postwall.Height = ft.Height; ;
                postwall.Width = ft.Width;
                elementIn.Children.Add(postwall);
                Canvas.SetTop(postwall, Canvas.GetTop(createRack) + 50);
                Canvas.SetLeft(postwall, elementIn.ActualWidth / 2 - postwall.Width / 2);
                TextBox polw = new TextBox
                {
                    Width = elementIn.ActualWidth / 2 - 40,
                    Height = 30,
                    Tag = "COUNTEL",
                    FontSize = 17
                };
                win.CountB = polw;
                polw.PreviewTextInput += win._PreviewTextInput;
                elementIn.Children.Add(polw);
                Canvas.SetTop(polw, Canvas.GetTop(postwall) + 50);
                Canvas.SetLeft(polw, elementIn.ActualWidth / 2 + 30);            
                Slider slid = new Slider
                {
                    Width = elementIn.ActualWidth / 2 + 30,
                    TickFrequency = 1,
                    IsSnapToTickEnabled = true,
                    IsDirectionReversed = true,
                    Tag = "COUNTEL",
                    Minimum = 0,
                    Maximum = 20,
                    Value = 1
                };
                elementIn.Children.Add(slid);
                Canvas.SetTop(slid, Canvas.GetTop(postwall) + 50);
                Canvas.SetLeft(slid, 0);
                Binding binding = new Binding();
                binding.Source = slid;
                binding.Path = new PropertyPath("Value");
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                polw.SetBinding(TextBox.TextProperty, binding);
                Image tmpimg3 = new Image();
                tmpimg3.Source = new BitmapImage(new Uri("img/wallg.bmp", UriKind.Relative));
                Button createShelf = new Button
                {
                    Content = tmpimg3,
                    Width = 50,
                    Height = 50
                };
                elementIn.Children.Add(createShelf);
                Canvas.SetTop(createShelf, Canvas.GetTop(slid) + 50);
                Canvas.SetLeft(createShelf, elementIn.ActualWidth / 2 - createShelf.Width / 2);
                createShelf.Click += win.CreateNS_Click;
                Image tmpimg4 = new Image();
                tmpimg4.Source = new BitmapImage(new Uri("img/boxg.bmp", UriKind.Relative));
                Button createBox = new Button
                {
                    Content = tmpimg4,
                    Width = 50,
                    Height = 50
                };
                elementIn.Children.Add(createBox);
                Canvas.SetTop(createBox, Canvas.GetTop(slid) + 50);
                Canvas.SetLeft(createBox, elementIn.ActualWidth / 2 - createBox.Width / 2 + 50);
                createBox.Click += win.CreateNB_Click;
                Image tmpimg5 = new Image();
                tmpimg5.Source = new BitmapImage(new Uri("img/wallv.bmp", UriKind.Relative));
                Button createWall = new Button
                {

                    Content = tmpimg5,
                    Width = 50,
                    Height = 50
                };
                elementIn.Children.Add(createWall);
                Canvas.SetTop(createWall, Canvas.GetTop(slid) + 50);
                Canvas.SetLeft(createWall, elementIn.ActualWidth / 2 - createWall.Width / 2 - 50);
                createWall.Click += win.CreateNW_Click;
                nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                Line separelot = new Line
                {
                    X1 = 0,
                    X2 = elementIn.ActualWidth,
                    Y1 = nach + 60,
                    Y2 = nach + 60,
                    StrokeThickness = 2,
                    Stroke = Brushes.Black
                };
                elementIn.Children.Add(separelot);
            }


            String []tags1 = { "slidW1", "slidW2", "slidW3", "slidW4"};
            countmasel = 0;
            for (int i = 0; i < 4; i++) if (masel[i] != null) countmasel++;           
            for (int i = 0; i < countmasel; i++)
            {
                nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                ElementCabinet tmpel1 = hashtable[masel[i].GetHashCode()] as ElementCabinet;
                if (tmpel1.ingroupel == "WallC" || tmpel1.ingroupel == "ShelfC")
                {
                    TextBlock postwall = new TextBlock();
                    if (tmpel1.ingroupel == "WallC") postwall.Text = "Положение стенки ";
                    else postwall.Text = "Положение полочки ";
                    postwall.FontSize = 17;
                    ft = new FormattedText(postwall.Text, CultureInfo.CurrentUICulture, postwall.FlowDirection, new Typeface(postwall.FontFamily, postwall.FontStyle,postwall.FontWeight, postwall.FontStretch),postwall.FontSize, postwall.Foreground);
                    postwall.Height = ft.Height; ;
                    postwall.Width = ft.Width;
                    elementIn.Children.Add(postwall);
                    Canvas.SetTop(postwall, nach + 60);
                    Canvas.SetLeft(postwall, elementIn.ActualWidth / 2 - postwall.Width / 2);
                    postwall.MouseMove += win.OBJ_MouseMoveTB;
                    postwall.MouseLeave += win.OBJ_MouseLeave;
                    TextBlock textpol = new TextBlock();
                    if (tmpel1.ingroupel == "WallC") textpol.Text = "относительно правой стены";
                    else textpol.Text = "относительно нижней стены";
                    textpol.FontSize = 17;
                    ft = new FormattedText(textpol.Text, CultureInfo.CurrentUICulture, textpol.FlowDirection,new Typeface(textpol.FontFamily, textpol.FontStyle,textpol.FontWeight, textpol.FontStretch), textpol.FontSize, textpol.Foreground);
                    textpol.Height = ft.Height; ;
                    textpol.Width = ft.Width;
                    elementIn.Children.Add(textpol);
                    Canvas.SetTop(textpol, Canvas.GetTop(postwall) + 30);
                    Canvas.SetLeft(textpol, elementIn.ActualWidth / 2 - textpol.Width / 2);
                    textpol.MouseMove += win.OBJ_MouseMoveTB;
                    textpol.MouseLeave += win.OBJ_MouseLeave;
                    textpol.Tag = tags1[i];
                    TextBox polw = new TextBox
                    {
                        Width = elementIn.ActualWidth / 2 - 40,
                        Height = postwall.Height + 4,
                        Tag = tags1[i],
                        FontSize = 17
                    };
                    polw.PreviewTextInput += win._PreviewTextInput;
                    elementIn.Children.Add(polw);
                    Canvas.SetTop(polw, Canvas.GetTop(textpol) + 50);
                    Canvas.SetLeft(polw, elementIn.ActualWidth / 2 + 30);
                    polw.MouseMove += win.OBJ_MouseMove;
                    polw.MouseLeave += win.OBJ_MouseLeave;
                    Slider slid = new Slider
                    {
                        Width = elementIn.ActualWidth / 2 + 30,
                        TickFrequency = 0.5,
                        IsSnapToTickEnabled = true,
                        IsDirectionReversed = true,
                        Tag = tags1[i],
                        Minimum = (tmpel1.min ) * 100,
                        Maximum = (tmpel1.max ) * 100 ,
                        Value = (tmpel1.center ) * 100
                    };
                    
                    if (tmpel1.ingroupel == "WallC") {
                        slid.Minimum = (tmpel1.min ) * 100;
                        slid.Maximum = (tmpel1.max ) * 100 ;
                        slid.Value = (tmpel1.center ) * 100;
                    }
                    elementIn.Children.Add(slid);
                    Canvas.SetTop(slid, Canvas.GetTop(textpol) + 50);
                    Canvas.SetLeft(slid, 0);
                    slid.MouseMove += win.OBJ_MouseMove;
                    slid.MouseLeave += win.OBJ_MouseLeave;
                    slid.ValueChanged += win.SlideRast_ChangeZpos;
                    Binding binding = new Binding();
                    binding.Source = slid;
                    binding.Path = new PropertyPath("Value");
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    polw.SetBinding(TextBox.TextProperty, binding);
                }
            }
            String[,] tags2 = { { "slibBox1","lenBox1","otXBox1", "otYBox1", "otZBox1" }, { "slibBox2", "lenBox2", "otXBox2", "otYBox2", "otZBox2" },{ "slibBox3", "lenBox2", "otXBox2", "otYBox2", "otZBox2" }, { "slibBox4", "lenBox4", "otXBox4", "otYBox4", "otZBox4" } };
            countmasel = 0;
            for (int i = 0; i < 4; i++) if (masel[i] != null) countmasel++;
            for (int i = 0; i < countmasel; i++)
            {
                nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
               
                ElementCabinet tmpel1 = hashtable[masel[i].GetHashCode()] as ElementCabinet;
                string[] stringtpm = tmpel1.ingroupel.Split('-');
                ElementCabinet takeel = tmpel1;
                if (i > 0)
                {
                    ElementCabinet tmpel2 = hashtable[masel[i-1].GetHashCode()] as ElementCabinet;
                    if (tmpel2.ingroupel == tmpel1.ingroupel) continue;
                }
                if (stringtpm[0] == "Box")
                {
                    TextBlock postwall = new TextBlock
                    {
                        Text = "Положение ящика ",
                        Tag = tags2[i,0],
                        FontSize = 17
                    };
                    ft = new FormattedText(postwall.Text, CultureInfo.CurrentUICulture, postwall.FlowDirection, new Typeface(postwall.FontFamily, postwall.FontStyle,postwall.FontWeight, postwall.FontStretch), postwall.FontSize, postwall.Foreground);
                    postwall.Height = ft.Height; ;
                    postwall.Width = ft.Width;
                    elementIn.Children.Add(postwall);
                    Canvas.SetTop(postwall, nach + 50);
                    Canvas.SetLeft(postwall, elementIn.ActualWidth / 2 - postwall.Width / 2);
                    postwall.MouseMove += win.OBJ_MouseMoveTB;
                    postwall.MouseLeave += win.OBJ_MouseLeave;
                    TextBlock textpol = new TextBlock
                    {
                        Text = "относительно нижней стены",
                        Tag = tags2[i,0],
                        FontSize = 17
                    };
                    ft = new FormattedText(textpol.Text, CultureInfo.CurrentUICulture, textpol.FlowDirection, new Typeface(textpol.FontFamily, textpol.FontStyle,textpol.FontWeight, textpol.FontStretch), textpol.FontSize, textpol.Foreground);
                    textpol.Height = ft.Height; ;
                    textpol.Width = ft.Width;
                    elementIn.Children.Add(textpol);
                    Canvas.SetTop(textpol, Canvas.GetTop(postwall) + 40);
                    Canvas.SetLeft(textpol, elementIn.ActualWidth / 2 - textpol.Width / 2);
                    textpol.MouseMove += win.OBJ_MouseMoveTB;
                    textpol.MouseLeave += win.OBJ_MouseLeave;
                    TextBox polw = new TextBox
                    {
                        Width = elementIn.ActualWidth / 2 - 40,
                        Height = postwall.Height + 4,
                        Tag = tags2[i,0],
                        FontSize = 17
                    };
                    polw.PreviewTextInput += win._PreviewTextInput;
                    elementIn.Children.Add(polw);
                    Canvas.SetTop(polw, Canvas.GetTop(textpol) + 40);
                    Canvas.SetLeft(polw, elementIn.ActualWidth / 2 + 30);
                    polw.MouseMove += win.OBJ_MouseMove;
                    polw.MouseLeave += win.OBJ_MouseLeave;
                    Slider slid = new Slider
                    {
                        Width = elementIn.ActualWidth / 2 + 30,
                        TickFrequency = 0.5,
                        IsSnapToTickEnabled = true,
                        IsDirectionReversed = true,
                        Tag = tags2[i,0],
                        Minimum = (takeel.min ) * 100,
                        Maximum = (takeel.max ) * 100 - takeel.lenbox * 100,
                        Value = (takeel.center ) * 100
                    };
                    elementIn.Children.Add(slid);
                    Canvas.SetTop(slid, Canvas.GetTop(textpol) + 40);
                    Canvas.SetLeft(slid, 0);
                    slid.ValueChanged += win.SlideRast_ChangeZpos;
                    slid.MouseMove += win.OBJ_MouseMove;
                    slid.MouseLeave += win.OBJ_MouseLeave;
                    Binding binding = new Binding
                    {
                        Source = slid,
                        Path = new PropertyPath("Value"),

                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    polw.SetBinding(TextBox.TextProperty, binding);
                  
                    for (int j = 1; j < 5; j++)
                    {
                        nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                        if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                        
                        TextBlock raz = new TextBlock
                        {
                            Tag = tags2[i, j],
                            FontSize = 17
                        };
                        if (j == 1) raz.Text = "Высота ящика";
                        if (j == 2) raz.Text = "Отступ от боковых стенок ";
                        if (j == 3) raz.Text = "Отступ от задней стены ";
                        if (j == 4) raz.Text = "Отступ снизу и сверху";
                        ft = new FormattedText(raz.Text, CultureInfo.CurrentUICulture, raz.FlowDirection,new Typeface(raz.FontFamily, raz.FontStyle,raz.FontWeight, raz.FontStretch),raz.FontSize, raz.Foreground);
                        raz.Height = ft.Height; ;
                        raz.Width = ft.Width;
                        elementIn.Children.Add(raz);
                        Canvas.SetTop(raz, nach + 40);
                        Canvas.SetLeft(raz, elementIn.ActualWidth / 2 - raz.Width / 2);
                        raz.MouseMove += win.OBJ_MouseMoveTB;
                        raz.MouseLeave += win.OBJ_MouseLeave;
                        TextBox lenchb = new TextBox
                        {
                            Width = elementIn.ActualWidth / 2 - 40,
                            Height = raz.Height + 4,
                            FontSize = 17,
                            Tag = tags2[i, j],
                            Text = this.descreaselen * 100 + ""
                        };
                        if (j == 1) lenchb.Text = this.lenbox*100+"";
                        if (j == 2) lenchb.Text = this.otsx * 100 + "";
                        if (j == 3) lenchb.Text = this.otsy * 100 + "";
                        if (j == 4) lenchb.Text = this.otsz * 100 + "";
                        lenchb.PreviewTextInput += win._PreviewTextInput;
                        lenchb.TextChanged += win._TextChangedTH;
                        elementIn.Children.Add(lenchb);
                        Canvas.SetTop(lenchb, Canvas.GetTop(raz) + 40);
                        Canvas.SetLeft(lenchb, elementIn.ActualWidth / 2 - elementIn.ActualWidth / 4 + 20);
                        lenchb.MouseMove += win.OBJ_MouseMove;
                        lenchb.MouseLeave += win.OBJ_MouseLeave;
                    }
                }
            }
            String[,] tags3 = { { "slidRZ1", "slidRY1" }, { "slidRZ2", "slidRY2" }, { "slidRZ3", "slidRY3" }, { "slidRZ4", "slidRY4" } };
            countmasel = 0;
            for (int i = 0; i < 4; i++) if (masel[i] != null) countmasel++;
            for (int i = 0; i < countmasel; i++)
            {
                nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);

                ElementCabinet tmpel1 = hashtable[masel[i].GetHashCode()] as ElementCabinet;
                ElementCabinet takeel = tmpel1;               
                if (tmpel1.ingroupel == "Rack")
                {
                    TextBlock postwall = new TextBlock
                    {
                        Text = "Положение стойки ",
                        Tag = tags3[i, 0],
                        FontSize = 17
                    };
                    ft = new FormattedText(postwall.Text, CultureInfo.CurrentUICulture, postwall.FlowDirection,
                                                         new Typeface(postwall.FontFamily, postwall.FontStyle,
                                                                      postwall.FontWeight, postwall.FontStretch),
                                                         postwall.FontSize, postwall.Foreground);
                    postwall.Height = ft.Height; ;
                    postwall.Width = ft.Width;
                    elementIn.Children.Add(postwall);
                    Canvas.SetTop(postwall, nach + 60);
                    Canvas.SetLeft(postwall, elementIn.ActualWidth / 2 - postwall.Width / 2);
                    postwall.MouseMove += win.OBJ_MouseMoveTB;
                    postwall.MouseLeave += win.OBJ_MouseLeave;
                    TextBlock textpol = new TextBlock
                    {
                        Text = "относительно пола",
                        Tag = tags3[i, 0],
                        FontSize = 17
                    };
                    ft = new FormattedText(textpol.Text, CultureInfo.CurrentUICulture, textpol.FlowDirection,new Typeface(textpol.FontFamily, textpol.FontStyle,textpol.FontWeight, textpol.FontStretch),textpol.FontSize, textpol.Foreground);
                    textpol.Height = ft.Height; ;
                    textpol.Width = ft.Width;
                    elementIn.Children.Add(textpol);
                    Canvas.SetTop(textpol, Canvas.GetTop(postwall) + 40);
                    Canvas.SetLeft(textpol, elementIn.ActualWidth / 2 - textpol.Width / 2);
                    textpol.MouseMove += win.OBJ_MouseMoveTB;
                    textpol.MouseLeave += win.OBJ_MouseLeave;
                    TextBox polw = new TextBox
                    {
                        Width = elementIn.ActualWidth / 2 - 40,
                        Height = postwall.Height + 4,
                        Tag = tags3[i, 0],
                        FontSize = 17
                    };
                    polw.PreviewTextInput += win._PreviewTextInput;
                    elementIn.Children.Add(polw);
                    Canvas.SetTop(polw, Canvas.GetTop(textpol) + 40);
                    Canvas.SetLeft(polw, elementIn.ActualWidth / 2 + 30);
                    polw.MouseMove += win.OBJ_MouseMove;
                    polw.MouseLeave += win.OBJ_MouseLeave;
                    Slider slid = new Slider
                    {
                        Width = elementIn.ActualWidth / 2 + 30,
                        TickFrequency = 0.5,
                        IsSnapToTickEnabled = true,
                        IsDirectionReversed = true,
                        Tag = tags3[i,0],
                        Minimum = (takeel.min ) * 100,
                        Maximum = (takeel.max ) * 100,
                        Value = (takeel.center ) * 100
                    };
                    elementIn.Children.Add(slid);
                    Canvas.SetTop(slid, Canvas.GetTop(textpol) + 40);
                    Canvas.SetLeft(slid, 0);
                    slid.ValueChanged += win.SlideRast_ChangeZpos;
                    slid.MouseMove += win.OBJ_MouseMove;
                    slid.MouseLeave += win.OBJ_MouseLeave;
                    Binding binding = new Binding
                    {
                        Source = slid,
                        Path = new PropertyPath("Value"),

                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    polw.SetBinding(TextBox.TextProperty, binding);
                    TextBlock textpol1 = new TextBlock
                    {
                        Text = "относительно задней стены",
                        Tag = tags3[i, 1],
                        FontSize = 17
                    };
                    ft = new FormattedText(textpol1.Text, CultureInfo.CurrentUICulture, textpol1.FlowDirection,
                                                         new Typeface(textpol1.FontFamily, textpol1.FontStyle,
                                                                      textpol1.FontWeight, textpol1.FontStretch),
                                                         textpol1.FontSize, textpol1.Foreground);
                    textpol1.Height = ft.Height; ;
                    textpol1.Width = ft.Width;
                    elementIn.Children.Add(textpol1);
                    Canvas.SetTop(textpol1, Canvas.GetTop(slid) + 40);
                    Canvas.SetLeft(textpol1, elementIn.ActualWidth / 2 - textpol1.Width / 2);
                    textpol1.MouseMove += win.OBJ_MouseMoveTB;
                    textpol1.MouseLeave += win.OBJ_MouseLeave;
                    TextBox poly = new TextBox
                    {
                        Width = elementIn.ActualWidth / 2 - 40,
                        Height = postwall.Height + 4,
                        Tag = tags3[i, 1],
                        FontSize = 17
                    };
                    poly.PreviewTextInput += win._PreviewTextInput;
                    elementIn.Children.Add(poly);
                    Canvas.SetTop(poly, Canvas.GetTop(textpol1) + 40);
                    Canvas.SetLeft(poly, elementIn.ActualWidth / 2 + 30);
                    poly.MouseMove += win.OBJ_MouseMove;
                    poly.MouseLeave += win.OBJ_MouseLeave;
                    Slider slid1 = new Slider
                    {
                        Width = elementIn.ActualWidth / 2 + 30,
                        TickFrequency = 0.5,
                        IsSnapToTickEnabled = true,
                        IsDirectionReversed = true,
                        Tag = tags3[i,1],
                        Minimum = (takeel.miny ) * 100,
                        Maximum = (takeel.maxy ) * 100,
                        Value = (takeel.centery ) * 100
                    };                
                    elementIn.Children.Add(slid1);
                    Canvas.SetTop(slid1, Canvas.GetTop(textpol1) + 40);
                    Canvas.SetLeft(slid1, 0);
                    slid1.ValueChanged += win.SlideRast_ChangeZpos;
                    slid1.MouseMove += win.OBJ_MouseMove;
                    slid1.MouseLeave += win.OBJ_MouseLeave;
                    Binding binding1 = new Binding
                    {
                        Source = slid1,
                        Path = new PropertyPath("Value"),

                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    poly.SetBinding(TextBox.TextProperty, binding1);
                }
            }
            String[,] tags4 = { { "elM1", "elPX1", "elPY1", "elPZ1", "elAX1", "elAY1", "elAZ1" }, { "elM2", "elPX2", "elPY2", "elPZ2", "elAX2", "elAY2", "elAZ2" }, { "elM3", "elPX3", "elPY3", "elPZ3", "elAX3", "elAY3", "elAZ3" }, { "elM4", "elPX4", "elPY4", "elPZ4", "elAX4", "elAY4", "elAZ4" } };
            countmasel = 0;
            for (int i = 0; i < 4; i++) if (masel[i] != null) countmasel++;
            for (int i = 0; i < countmasel; i++)
            {
                ElementCabinet tmpel1 = hashtable[masel[i].GetHashCode()] as ElementCabinet;
                for(int j = 0; j < 7; j++) {
                    nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                    if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);                   
                    ElementCabinet takeel = tmpel1;
                    if (i > 0)
                    {
                        ElementCabinet tmpel2 = hashtable[masel[i - 1].GetHashCode()] as ElementCabinet;
                        if (tmpel2.ingroupel == tmpel1.ingroupel) continue;
                    }
                    if (tmpel1.iningroupel == "El")
                    {
                        TextBlock raz = new TextBlock
                        {
                            Tag = tags4[i, j],
                            FontSize = 17
                        };
                        if (j == 0) raz.Text = "Размер елемента ";
                        if (j == 1) raz.Text = "Позиция елемента по X ";
                        if (j == 2) raz.Text = "Позиция елемента по Y ";
                        if (j == 3) raz.Text = "Позиция елемента по Z ";
                        if (j == 4) raz.Text = "Угол поворота по X ";
                        if (j == 5) raz.Text = "Угол поворота по Y ";
                        if (j == 6) raz.Text = "Угол поворота по Z ";
                        ft = new FormattedText(raz.Text, CultureInfo.CurrentUICulture, raz.FlowDirection, new Typeface(raz.FontFamily, raz.FontStyle,raz.FontWeight, raz.FontStretch),raz.FontSize, raz.Foreground);
                        raz.Height = ft.Height; ;
                        raz.Width = ft.Width;
                        elementIn.Children.Add(raz);
                        Canvas.SetTop(raz, nach + 60);
                        Canvas.SetLeft(raz, elementIn.ActualWidth / 2 - raz.Width / 2);
                        raz.MouseMove += win.OBJ_MouseMoveTB;
                        raz.MouseLeave += win.OBJ_MouseLeave;
                        TextBox polw = new TextBox
                        {
                            Width = elementIn.ActualWidth / 2 - 40,
                            Height = raz.Height + 4,
                            Tag = tags4[i, j],
                            FontSize = 17
                        };
                        polw.PreviewTextInput += win._PreviewTextInput;
                        elementIn.Children.Add(polw);
                        Canvas.SetTop(polw, Canvas.GetTop(raz) + 40);
                        Canvas.SetLeft(polw, elementIn.ActualWidth / 2 + 30);
                        polw.MouseMove += win.OBJ_MouseMove;
                        polw.MouseLeave += win.OBJ_MouseLeave;
                        Slider slid = new Slider
                        {
                            Width = elementIn.ActualWidth / 2 + 30,
                            TickFrequency = 0.1,
                            IsSnapToTickEnabled = true,
                            IsDirectionReversed = true,
                            Tag = tags4[i, j]
                           
                        };
                        Transform3DGroup group3d = tmpel1.select.thismodel.Transform as Transform3DGroup;                        
                        ScaleTransform3D sel = group3d.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                        TranslateTransform3D tel = group3d.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                        Transform3DGroup group3d1 = tmpel1.thismodel.Transform as Transform3DGroup;
                        TranslateTransform3D tt11 = group3d1.Children.OfType<TranslateTransform3D>().FirstOrDefault();
                        ScaleTransform3D tt12 = group3d1.Children.OfType<ScaleTransform3D>().FirstOrDefault();
                        RotateTransform3D rr1 = group3d1.Children[2] as RotateTransform3D;
                        RotateTransform3D rr2 = group3d1.Children[3] as RotateTransform3D;
                        RotateTransform3D rr3 = group3d1.Children[4] as RotateTransform3D;
                        if (j == 0)
                        {
                            slid.Minimum = 0;
                            slid.Maximum = Math.Max(Math.Max(lenX,lenY),lenZ)*100;
                            slid.Value = tt12.ScaleX*100;
                        }
                        if (j == 1)
                        {
                            slid.Minimum = 0- thismodel.Bounds.SizeX * 100;
                            slid.Maximum = sel.ScaleX * 100 + thismodel.Bounds.SizeX * 100;
                            slid.Value = tmpel1.otsx * 100;
                        }
                        if (j == 2)
                        {
                            slid.Minimum = 0 - thismodel.Bounds.SizeY * 100;
                            slid.Maximum = sel.ScaleY * 100 + thismodel.Bounds.SizeX * 100;
                            slid.Value = tmpel1.otsy * 100;
                        }
                        if (j == 3)
                        {
                            slid.Minimum = 0 - thismodel.Bounds.SizeZ * 100;
                            slid.Maximum = sel.ScaleZ * 100 + thismodel.Bounds.SizeX * 100;
                            slid.Value = tmpel1.otsz * 100;
                        }
                        if (j == 4)
                        {
                            slid.Minimum = 0;
                            slid.Maximum = 360;
                            AxisAngleRotation3D tmprot = rr1.Rotation as AxisAngleRotation3D;
                            slid.Value = tmprot.Angle;
                        }
                        if (j == 5)
                        {
                            slid.Minimum = 0;
                            slid.Maximum = 360;
                            AxisAngleRotation3D tmprot = rr2.Rotation as AxisAngleRotation3D;
                            slid.Value = tmprot.Angle;
                        }
                        if (j == 6)
                        {
                            slid.Minimum = 0;
                            slid.Maximum = 360;
                            AxisAngleRotation3D tmprot = rr3.Rotation as AxisAngleRotation3D;
                            slid.Value = tmprot.Angle;
                        }
                        elementIn.Children.Add(slid);
                        Canvas.SetTop(slid, Canvas.GetTop(raz) + 40);
                        Canvas.SetLeft(slid, 0);
                        slid.ValueChanged += win.SlideElement_Change;
                        slid.MouseMove += win.OBJ_MouseMove;
                        slid.MouseLeave += win.OBJ_MouseLeave;
                        Binding binding = new Binding
                        {
                            Source = slid,
                            Path = new PropertyPath("Value"),

                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };
                        polw.SetBinding(TextBox.TextProperty, binding);                        
                    }
                }
                if (tmpel1.iningroupel == "El")
                {
                    nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
                    if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
                    Line separcrel1 = new Line
                    {
                        X1 = 0,
                        X2 = elementIn.ActualWidth,
                        Y1 = nach + 30,
                        Y2 = nach + 30,
                        StrokeThickness = 2,
                        Stroke = Brushes.Black
                    };
                    elementIn.Children.Add(separcrel1);
                }
            }
            nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 1]);
            if (double.IsNaN(nach)) nach = Canvas.GetTop(elementIn.Children[elementIn.Children.Count - 2]);
            elementIn.Height = nach + 50;
        }
    }
}
