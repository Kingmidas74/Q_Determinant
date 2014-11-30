using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Serializers
{
    internal class BinarySerializer : AbstractSerializer
    {
        public override void SerializeSolution(string fileName, SerializationModels.SolutionModels.Solution solution)
        {
            Stream testFileStream = File.Create(fileName);
            var serializer = new BinaryFormatter();
            serializer.Serialize(testFileStream, solution);
            testFileStream.Close();
        }

        public override void DeserializeSolution(string fileName, out SerializationModels.SolutionModels.Solution solution)
        {
            Stream fileStream = File.OpenRead(fileName);
            var deserializer = new BinaryFormatter();
            solution = (SerializationModels.SolutionModels.Solution)deserializer.Deserialize(fileStream);
            fileStream.Close();
        }

        public override void SerializeProject(string fileName, SerializationModels.ProjectModels.Project project)
        {
            Stream testFileStream = File.Create(fileName);
            var serializer = new BinaryFormatter();
            serializer.Serialize(testFileStream, project);
            testFileStream.Close();
        }

        public override void DeserializeProject(string fileName, out SerializationModels.ProjectModels.Project project)
        {
            Stream fileStream = File.OpenRead(fileName);
            var deserializer = new BinaryFormatter();
            project = (SerializationModels.ProjectModels.Project)deserializer.Deserialize(fileStream);
            fileStream.Close();
        }
    }
}
