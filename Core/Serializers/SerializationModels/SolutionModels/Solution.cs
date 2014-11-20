using System;
using System.Collections.Generic;

namespace Core.Serializers.SerializationModels.SolutionModels
{
    [Serializable()]
    public class Solution
    {
        public string Title { get; set; }
        public List<Project> Projects { get; set; }
        public Properties Properties { get; set; }
        
        public Solution()
        {
            Projects = new List<Project>();
        }
    }
}
