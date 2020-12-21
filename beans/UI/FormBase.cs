using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Polenter.Serialization;

namespace beans
{
    public class FormBase
    {
        static public void Load(object sender, EventArgs e)
        {
            Form form = (Form)sender;
            if (File.Exists(fileName(form)))
            {
                var serializer = new Polenter.Serialization.SharpSerializer();
                form = (Form)serializer.Deserialize(fileName(form));
            }
        }

        static public void Closing(object sender, FormClosingEventArgs e)
        {
            Form form = (Form)sender;
            var serializer = new Polenter.Serialization.SharpSerializer();
            serializer.Serialize(form, fileName(form));
        }

        static private string fileName(Form form)
        {
            return Path.Combine(Application.StartupPath, form.Name + ".xml");
        }
    }
}
