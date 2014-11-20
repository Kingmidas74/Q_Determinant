using System;

namespace Core.Serializers.SerializationModels.SolutionModels
{
    [Serializable()]
    public class Project
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public ProjectTypes Type { get; set; }
    }
}
