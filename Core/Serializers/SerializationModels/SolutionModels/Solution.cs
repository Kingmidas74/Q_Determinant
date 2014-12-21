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
        public List<Function> Functions { get; set; } 
        
        public Solution()
        {
            Projects = new List<Project>();
            Functions = new List<Function>();
            Properties = new Properties();
        }

        public void AddProject(Core.Serializers.SerializationModels.SolutionModels.Project project)
        {
            if (!Projects.Exists(x => x.Path.Equals(project.Path)))
            {
                Projects.Add(project);
            }
            else
            {
                throw new Exception("This project already exist!");
            }
        }

        public void AddFunction(Core.Serializers.SerializationModels.SolutionModels.Function function)
        {
            if (!Functions.Exists(x => x.Signature.Equals(function.Signature)))
            {
                Functions.Add(function);
            }
            else
            {
                throw new Exception("This function already exist!");
            }
        }
    }
}
