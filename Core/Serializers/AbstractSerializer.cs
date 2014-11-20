namespace Core.Serializers
{
    public abstract class AbstractSerializer
    {
        public abstract void SerializeSolution(string fileName, SerializationModels.SolutionModels.Solution solution);
        public abstract void DeserializeSolution(string fileName, out SerializationModels.SolutionModels.Solution solution);

        public abstract void SerializeProject(string fileName, SerializationModels.ProjectModels.Project project);
        public abstract void DeserializeProject(string fileName, out SerializationModels.ProjectModels.Project project);
    }
}