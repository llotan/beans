using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using beans.measure;
using Emgu.CV.Util;
using Emgu.CV.Shape;

namespace beans.filters
{
    public class morphoMinEncloseCircle
    {

        public static Bitmap Run(Bitmap arg, ConfigSetting cfg)
        {
            ((beans.blob_tag)arg.Tag).minEncloseCircle = CvInvoke.MinEnclosingCircle( ((beans.blob_tag)arg.Tag).contour);






            // or
            var  m = ((beans.blob_tag)arg.Tag).moments = ((beans.blob_tag)arg.Tag).moments ??  CvInvoke.Moments(((beans.blob_tag)arg.Tag).contour, false);
            int cx = (int )(m.M10 / m.M00);
            int cy = (int)(m.M01 / m.M00);

            Point p_Centroid = new Point(cx,cy);
            var vp_Contour = ((beans.blob_tag) arg.Tag).contour;

            Point p_Contour_first = vp_Contour[0];
            double circumCirc_Radius = ComputeDistance((p_Centroid),p_Contour_first);
            double inscriCirc_Radius = ComputeDistance(p_Centroid , p_Contour_first);
            for (int p = 0; p < vp_Contour.Size; p++)
            {
                Point p_Contour_current = vp_Contour[p];
                double r =  ComputeDistance(p_Centroid , p_Contour_current);
                if (r < inscriCirc_Radius) inscriCirc_Radius = r;
                if (r > circumCirc_Radius) circumCirc_Radius = r;
            }

            ((beans.blob_tag) arg.Tag).Rmax = new distance(circumCirc_Radius);
            ((beans.blob_tag) arg.Tag).Rmin = new distance(inscriCirc_Radius);

            return arg;
        }

        /// <summary>
        /// Compute the shape distance between two shapes defined by its contours.
        /// </summary>
        /// <param name="contour1">Contour defining first shape</param>
        /// <param name="contour2">Contour defining second shape</param>
        /// <returns>The shape distance between two shapes defined by its contours.</returns>
        public static  double ComputeDistance(Point contour1, Point contour2)
        {
            using (Emgu.CV.Util.VectorOfPoint c1 = new Emgu.CV.Util.VectorOfPoint(new Point[]{contour1}))
            using (Emgu.CV.Util.VectorOfPoint c2 = new Emgu.CV.Util.VectorOfPoint(new Point[] { contour2 }))
            {
                return CvInvoke.Norm( c1 , c2);
            }
        }

    }
}
