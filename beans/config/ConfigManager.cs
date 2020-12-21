using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace beans
{
    
    public class SettingsManager
    {

            private static SettingsManager mgr;
            private string SETTINGSFILENAME = Application.ProductName + ".xml";

            //variable to store (caching) settings
            private static ConfigSetting curSettings;
            public static ConfigSetting Settings { get; set; }

            /// <summary>
            /// An event that occurs when the settings have been successfully saved.
            /// </summary>
            public event SettingsEventHandler OnSettingsSaved = null;

            private SettingsManager()
            {
                //string fullName  = System.Reflection.Assembly.GetEntryAssembly().Location;
                //SETTINGSFILENAME = Path.GetFileNameWithoutExtension(fullName);
            }

            public static SettingsManager Instance { get { return mgr ?? (mgr = new SettingsManager()); } }

            /// <summary>
            /// Get the default settings
            /// </summary>
            /// <returns></returns>
            public ConfigSetting GetDefault(bool isFileExist = true)
            {
                ConfigSetting settings = new ConfigSetting();
                 return settings;
            }

            /// <summary>
            /// Get the current setting (from configuration file) (if the file does not exist - back to default settings)
            /// </summary>
            /// <param name="folderPath">The path to the destination folder.</param>
            /// <returns></returns>
            public ConfigSetting GetCurrent(string folderPath)
            {
                try
                {
                    string path = string.Format("{0}\\{1}", folderPath, SETTINGSFILENAME);
                    if (!File.Exists(path))
                    {
                        ConfigSetting tmpSetting = GetDefault();
                        SaveCurrent(tmpSetting, folderPath);
                        return tmpSetting;
                    }

                    if (curSettings == null) //первое обращение к настройкам - тогда получаем их из файла
                    {
                        curSettings = Reader.ReadFromFile<ConfigSetting>(path);
                    }
                    else
                    {
                        return curSettings;
                    }

                    return curSettings ?? GetDefault();
                }
                catch (Exception err)
                {
                    throw new Exception(err.Message);
                }
            }

            /// <summary>
            /// Save the current settings to a file.
            /// </summary>
            /// <param name="obj">Acquisition settings</param>
            /// <param name="folderPath">The path to the destination folder.</param>
            /// <returns></returns>
            public bool SaveCurrent(ConfigSetting obj, string folderPath)
            {
                bool res;
                try
                {
                    string path = string.Format("{0}\\{1}", folderPath, SETTINGSFILENAME);
                    res = Writter.WriteToFile(path, obj);

                    //refresh value
                    curSettings = obj;

                    if (res && OnSettingsSaved != null)
                        OnSettingsSaved(this, new ConfigSettingArgs(obj));

                }
                catch (Exception err)
                {
                    throw new Exception(err.Message);
                }

                return res;
            }


            /// <summary>
            /// Delete all the files that store settings
            /// </summary>
            /// <param name="folderPath"></param>
            /// <returns></returns>
            public bool DeleteSettingFiles(string folderPath)
            {
                bool res = false;
                try
                {

                    if (Directory.Exists(folderPath))
                    {
                        string[] flist = Directory.GetFiles(folderPath, "*.xml", SearchOption.TopDirectoryOnly);
                        foreach (string fn in flist)
                        {
                            File.Delete(fn);
                        }

                        res = true;
                    }

                }
                catch (Exception err)
                {
                    throw new Exception(err.Message);
                }

                return res;
            }

        }

        //************************************************************************************************************************************************************** Writter ********************************

        public class Writter
        {
            /// <summary>
            /// Serialized object to a file.
            /// </summary>
            /// <typeparam name="T">The type of object to be serialized</typeparam>
            /// <param name="path">Full path to file.</param>
            /// <param name="obj">Object for serialization.</param>
            public static bool WriteToFile(string path, object obj)
            {
                try
                {
                    XmlSerializer xml = new XmlSerializer(obj.GetType(), string.Empty);
                    using (FileStream fStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        xml.Serialize(fStream, obj);
                    }

                    return true;
                }
                catch (Exception err)
                {
                    Flokal.Protocol.Logging.Error(err.Message);
                    return false;
                }
            }
        }

        //************************************************************************************************************************************************************** Reader ********************************

        public class Reader
        {
            /// <summary>
            /// Deserialized object from the file.
            /// </summary>
            /// <typeparam name="T">The type of object that has been serialized</typeparam>
            /// <param name="path">Full path to file.</param>
            /// <returns></returns>
            public static T ReadFromFile<T>(string path)
            {
                T res = default(T);
                try
                {
                    if (!File.Exists(path))
                        return res;


                    XmlSerializer xml = new XmlSerializer(typeof(T), string.Empty);
                    using (FileStream fStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        res = (T)xml.Deserialize(fStream);
                    }

                }
                catch (Exception err)
                {
                    Flokal.Protocol.Logging.Error(err.Message);
                }

            return res;
            }
        }

  

}
