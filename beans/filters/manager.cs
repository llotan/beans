using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Polenter.Serialization;

namespace beans.filters
{                                                                                                               
    public class manager
    {
        //private List<Func<Bitmap, ConfigSetting, Bitmap>> listOfFilter;
        private List<fnctn4save> activs;
        private List<fnctn> availble;

        private string file_name;

        public manager()
        {
            file_name = Path.Combine(Application.StartupPath,"filters.xml");

            activs = new List<fnctn4save>();
            availble=new List<fnctn>();
            load();
        }

        public void load()
        {
            // availanle
            availble.Add(new fnctn(1, filters.morphoArea.Run, null, "Calculate area"));
            availble.Add(new fnctn(2, filters.morphoPerimeter.Run, null, "Calculate perimeter"));
            availble.Add(new fnctn(3, filters.morphoRoundness.Run, null, "Calculate  roundness "));
            availble.Add(new fnctn(4, filters.morphoElongation.Run, null, "Calculate  elongation (eccentricity) "));
            availble.Add(new fnctn(5, filters.morphoMinEncloseCircle.Run, null, "Calculate Rmax/min, enclosing circle"));

            availble.Add(new fnctn(7, filters.morphoConvexity.Run, null, "Calculate convexity (area,perimeter)"));
            availble.Add(new fnctn(8, filters.morphoOrientation.Run, null, "Calculate  orientation & max/min axis"));
            availble.Add(new fnctn(9, filters.morphoMoment.Run,null, "Calculate moments"));


            availble.Add(new fnctn(10, filters.colorRED.Run, filters.colorRED.GetIntensity, "Detect RED color"));
            availble.Add(new fnctn(11, filters.colorBLUE.Run, filters.colorBLUE.GetIntensity, "Detect BLUE color"));
            availble.Add(new fnctn(13, filters.colorGREEN.Run, filters.colorGREEN.GetIntensity, "Detect GREEN color"));
            availble.Add(new fnctn(14, filters.colorYELLOW.Run, filters.colorYELLOW.GetIntensity, "Detect YELLOW color"));


            availble.Add(new fnctn(99, filters.checkAll.Run, filters.checkAll.GetIntensity ,"Detect ALL  (for test)"));


            // activs
            if (File.Exists(file_name))
            {
                var serializer = new SharpSerializer();
                var list = serializer.Deserialize(file_name);
                activs = (List<fnctn4save>) list;
            }
            else
            {
                save();
            }

        }

        public List<fnctn4save> get_active()
        {
            return activs;
        }

        public List<fnctn> get_available()
        {
            return availble;
        }


        public List<Func<Bitmap, ConfigSetting, Bitmap>> get()
        {
           List<Func<Bitmap, ConfigSetting, Bitmap>> res =   new List<Func<Bitmap, ConfigSetting, Bitmap>>();

           foreach (var item in activs)
           {
                res.Add(availble.Find(v => v.id == item.id).name);
           }

           return res;
        }

        public Func<Image<Bgr, Byte> , ConfigSetting, double > getColorEmpty()
        {
            Func<Image<Bgr, Byte>, ConfigSetting, double> res = null;

            foreach (var item in activs)
            {
                if (item.id >=10)
                {
                    res = availble.Find(v => v.id == item.id).intensivity;
                }
                break;
            }

            return res;
        }

        public  void save(List<fnctn4save > _acvts = null )
        {
            if (_acvts == null)
            {
                _acvts = activs;
            }

            //for (int i = 0; i < _acvts.Count; i++) _acvts[i].name = null;

            var serializer = new SharpSerializer();
            serializer.Serialize(_acvts, file_name );
        }
    }


    public class fnctn4save
    {
        public int id { get; set; }
        public string text { get; set; }

       public override string ToString()
        {
            return text;
        }
    }


    public class fnctn
    {
        public int id { get; set; }
        public  Func<Bitmap, ConfigSetting, Bitmap> name { get; set; }
        public Func<Image<Bgr, Byte>, ConfigSetting, double>  intensivity { get; set; }

        public string text { get; set; }

        public fnctn(int _id, Func<Bitmap, ConfigSetting, Bitmap> _name, Func<Image<Bgr, Byte>, ConfigSetting, double>  _intensivity, string _text)
        {
            id = _id;
            name=_name;
            intensivity = _intensivity;
            text = _text;
        }

        public override string ToString()
        {
            return text;
        }
    }
}
