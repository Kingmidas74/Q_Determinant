using System.Collections.Generic;

namespace Q_Determinant
{
    public class AlgorithmFolder
    {
        public string Name { get; set; }
        public List<AlgorithmFile> Files { get; set; }

        public AlgorithmFolder()
        {
            Files=new List<AlgorithmFile>();
        }
    }
}
