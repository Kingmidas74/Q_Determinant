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

        public Project()
        {
            Files = new List<File>();
            References = new List<Reference>();
            Properties = new Properties();
        }
    }
}
