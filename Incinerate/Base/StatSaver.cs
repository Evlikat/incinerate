using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Incinerate.Base
{
    class StatSaver : ISaver
    {
        private ProcessWatcher m_processWatcher;

        public StatSaver(ProcessWatcher watcher){
            m_processWatcher = watcher;
        }

        public bool SaveInfo(string storagePath)
        {

            FileStream fstream = null;
            try
            {
                fstream = new FileStream(storagePath, FileMode.Create, FileAccess.Write);
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fstream, m_processWatcher);
            }
            catch(Exception)
            {
                return false;
            }
            finally
            {
                if(fstream != null) fstream.Close();
            }
            return true;
        }

        public object LoadInfo(string storagePath)
        {
            FileStream fstream = null;
            object result;
            try
            {
                fstream = new FileStream(storagePath, FileMode.Open, FileAccess.Read);
                IFormatter formatter = new BinaryFormatter();
                result = formatter.Deserialize(fstream);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (fstream != null) fstream.Close();
            }
            return result;
        }
    }
}
