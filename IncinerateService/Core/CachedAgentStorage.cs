using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace IncinerateService.Core
{
    class CachedAgentStorage : IAgentStorage
    {
        private const string AgentStoragePath = @"c:\ProgramData\Incinerate";
        private const string AgentSuffix = ".agent";

        public CachedAgentStorage()
        {
            if (!Directory.Exists(AgentStoragePath))
            {
                Directory.CreateDirectory(AgentStoragePath);
            }
        }

        public void SaveAgent(string name, Agent agent)
        {
            string pathToAgent = GetAgentPath(name);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(pathToAgent, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, agent);
            stream.Close();
        }

        public Agent LoadAgent(string name)
        {
            string pathToAgent = GetAgentPath(name);
            if (!File.Exists(pathToAgent))
            {
                return null;
            }
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(pathToAgent, FileMode.Open, FileAccess.Read, FileShare.None);
            Agent agent = (Agent)formatter.Deserialize(stream);
            return agent;
        }

        public IList<string> GetAgentNames()
        {
            IList<string> paths = new List<string>(Directory.GetFiles(AgentStoragePath, "*" + AgentSuffix));
            IList<string> result = new List<string>();
            foreach (string path in paths)
            {
                result.Add(Path.GetFileNameWithoutExtension(path));
            }
            return result;
        }

        private string GetAgentPath(string name)
        {
            if (!name.EndsWith(AgentSuffix))
            {
                name += AgentSuffix;
            }
            return Path.Combine(AgentStoragePath, name);
        }
    }
}
