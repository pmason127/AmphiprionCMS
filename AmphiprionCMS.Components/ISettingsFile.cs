using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AmphiprionCMS.Components
{
  
    public static class SettingsFile
    {
      

        //public void WriteToFile(string path, string fileName, IDictionary<string, string> settings,
        //    bool reCreate = false)
        //{
        //    var fullPath = Path.Combine(path, fileName);
        //    IDictionary<string, string> existing = null;
        //    if (File.Exists(fullPath))
        //    {
        //        if (reCreate)
        //            existing = ReadFile(path, fileName);

        //         File.Delete(fullPath);
        //    }

        //    var toWrite = Combine(existing, settings);
        //    if (toWrite == null)
        //        return;

        //    using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        //    {
        //        using (StreamWriter sw = new StreamWriter(fs))
        //        {
        //            foreach (var kvp in toWrite)
        //            {
        //                sw.Write(string.Format("{0}:{1}",kvp.Key,kvp.Value));
        //            }
        //            sw.Flush();
        //            sw.Close();
        //        }
        //        fs.Close();
        //    }


        //}

        //public IDictionary<string, string> ReadFile(string path, string fileName)
        //{
        //    var fullPath = Path.Combine(path, fileName);
        //    IDictionary<string, string> existing = null;
        //    if (!File.Exists(fullPath))
        //        return null;

        //    var data = new Dictionary<string, string>();
           
        //    using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        //    {
        //        using (StreamReader  sr = new StreamReader(fs))
        //        {
        //            while (!sr.EndOfStream)
        //            {
        //                var line = sr.ReadLine();
        //                if (!string.IsNullOrWhiteSpace(line))
        //                {
        //                    var split = line.Split(new char[] {':'}, StringSplitOptions.RemoveEmptyEntries);
        //                    if (split.Length == 2)
        //                    {
        //                        data.Add(split[0],split[1]);
        //                    }
        //                    else if(split.Length > 2)
        //                    {
        //                        data.Add(split[0], string.Join(":",split,1,split.Length -1));
        //                    }
        //                }
        //            }
        //            sr.Close();
        //        }
        //        fs.Close();
        //    }
        //    return data;
        //}

        //private IDictionary<string, string> Combine(IDictionary<string, string> existing,
        //    IDictionary<string, string> newData)
        //{
        //    if (existing == null)
        //        return newData;

        //    var data = new Dictionary<string, string>();
        //    foreach (var kvp in newData)
        //    {
        //        if (!existing.ContainsKey(kvp.Key))
        //        {
        //            existing.Add(kvp);
        //            continue;
        //        }

        //        if (!existing[kvp.Key].Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            existing[kvp.Key] = kvp.Value;
        //        }
        //    }
        //    return existing;
        //}

        public static  void WriteToFile(string path, string fileName, dynamic data)
        {
            var fullPath = Path.Combine(path, fileName);
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            var dataStr = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(dataStr);
                    sw.Flush();
                    sw.Close();
                }
                fs.Close();
            }
        }

        public static  dynamic ReadFile(string path, string fileName)
        {
            var fullPath = Path.Combine(path, fileName);
            IDictionary<string, string> existing = null;
            if (!File.Exists(fullPath))
                return null;
            string strData = "";
            dynamic data = null;
            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    strData = sr.ReadToEnd();
                }
                fs.Close();
            }

            if (!string.IsNullOrEmpty(strData))
                data = JObject.Parse(strData);

            return data;
        }
    }
}
