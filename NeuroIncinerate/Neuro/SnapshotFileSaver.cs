using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NeuroIncinerate.Neuro
{
    public class SnapshotFileSaver
    {
        public const string FileNameLayout = "[{0}]{1}-{2:yyyy-MM-dd hhmmss}.svt";
        public string PathToSnapshotFolder { get; private set; }

        public SnapshotFileSaver(string pathToSnapshotFolder)
        {
            PathToSnapshotFolder = pathToSnapshotFolder;
        }

        public void Save(HistorySnapshot e, DateTime date)
        {
            if (e.PID.Name == null)
                return;
            FileStream fs = null;
            try
            {
                string fileName = String.Format(FileNameLayout, e.PID.ToString(), e.PID.Name, date);
                string filePath = Path.Combine(PathToSnapshotFolder, fileName);
                fs = File.Create(filePath);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(fs, e);
            }
            catch
            {
                int i = 0;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public HistorySnapshot Load(string fileName)
        {
            FileStream fs = null;
            try
            {
                string filePath = Path.Combine(PathToSnapshotFolder, fileName);
                fs = File.OpenRead(filePath);
                BinaryFormatter serializer = new BinaryFormatter();
                HistorySnapshot hs = (HistorySnapshot) serializer.Deserialize(fs);
                return hs;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
    }
}
