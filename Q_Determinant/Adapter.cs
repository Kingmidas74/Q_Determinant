using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Core;

namespace Q_Determinant
{
    
    public class Adapter
    {
        static public List<BlockUI> TransformBlock()
        {
            var result = new List<BlockUI>();
            foreach (BlockTypes type in (BlockTypes[])Enum.GetValues(typeof(BlockTypes)))
            {
                var b = new BlockUI();
                b.Name = type.ToString();
                result.Add(b);
            }
            return result;
        }

        static public List<AlgorithmFolder> ConvertTreeFolderTo(string folderPath)
        {
            var validExtensions = new List<string>{".fc", ".qd", ".ip"};
            var result = new List<AlgorithmFolder>();
            var rootDirectory = new DirectoryInfo(@folderPath);
            foreach (var item in rootDirectory.GetDirectories())
            {
                var currentAlgorithmProject = new AlgorithmFolder();
                currentAlgorithmProject.Name = item.Name;
                foreach (var file in item.GetFiles("*.*").Where(s => validExtensions.Any(e => s.Extension.EndsWith(e))))
                {
                    var currentAlgorithmFile = new AlgorithmFile();
                    currentAlgorithmFile.Name = file.Name;
                    currentAlgorithmFile.Path = file.FullName;
                    currentAlgorithmProject.Files.Add(currentAlgorithmFile);
                }
                result.Add(currentAlgorithmProject);
            }
            return result;
        }
    }
}
