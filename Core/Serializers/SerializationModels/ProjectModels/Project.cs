using System;
using System.Collections.Generic;

namespace Core.Serializers.SerializationModels.ProjectModels
{
    [Serializable()]
    public class Project
    {
        public string Title { get; set; }
        public List<File> Files { get; set; }
        public List<Reference> References { get; set; }
        public Properties Properties { get; set; }
        public Dictionary<string, string> SignificantVariables { get; set; } 

        public Project()
        {
            Files = new List<File>();
            References = new List<Reference>();
            Properties = new Properties();
            SignificantVariables = new Dictionary<string, string>();
        }

        public void AddFile(Core.Serializers.SerializationModels.ProjectModels.File file)
        {
            if (!Files.Exists(x => x.Path.Equals(file.Path)))
            {
                Files.Add(file);
            }
            else
            {
                throw new Exception("This file already exist!");
            }
        }

        public void AddReference(Core.Serializers.SerializationModels.ProjectModels.Reference reference)
        {
            if (!References.Exists(x => x.ProjectPath.Equals(reference.ProjectPath)))
            {
                References.Add(reference);
            }
            else
            {
                throw new Exception("This reference already exist!");
            }
        }

        public void AddSignificantVariable(string name, string value)
        {
            if (!SignificantVariables.ContainsKey(name))
            {
                SignificantVariables.Add(name,value);
            }
            else
            {
                throw new Exception("This variable already exist!");
            }
        }

        public static Project Deserialize(string pathToProject)
        {
            Project result;
            SerializersFactory.GetSerializer().DeserializeProject(pathToProject, out result);
            return result;

        }

        public void Serialize(string pathToProject)
        {
            SerializersFactory.GetSerializer().SerializeProject(pathToProject, this);
        }
    }
}
