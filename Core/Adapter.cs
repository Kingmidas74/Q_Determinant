using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Core.Atoms;
using Core.Interfaces;

namespace Core
{
    public class Adapter
    {
        private ISyntaxTree _syntaxTreeModule;
        private IDeterminant _determinantModule;
        private IPlan _planModule;

        public string Status { get; private set; }
        public Graph SyntaxTree { get; private set; }

        public List<QTerm> QDeterminant { get; private set; }
        public Graph ImplementationPlan { get; private set; }

        private void LoadDll(string pathToDLL, dynamic Object, string defaultPath)
        {
            Assembly dll = null;
            try
            {
                dll = Assembly.LoadFile(pathToDLL);
            }
            catch (Exception)
            {
                dll = Assembly.LoadFile(defaultPath);
            }
            finally
            {
                foreach (var type in dll.GetTypes())
                {
                    foreach (var currentInterface in type.GetInterfaces())
                    {
                        if (currentInterface.ToString().Equals(Object.GetType().ToString()))
                        {
                            Object = Activator.CreateInstance(type);
                            break;
                        }
                    }
                }
            }
        }

        public Adapter(string pathToCodeFile, string pathToSyntaxTreeModule = null, string pathToDeterminantModule = null, string pathToPlanModule = null)
        {
            _syntaxTreeModule = null;
            LoadDll(pathToSyntaxTreeModule, _syntaxTreeModule,"C:\\");
            SyntaxTree=_syntaxTreeModule.GetSyntaxTree(pathToCodeFile);
            _determinantModule = null;
            LoadDll(pathToDeterminantModule, _determinantModule, "C:\\");
            QDeterminant = _determinantModule.GetDeterminant(SyntaxTree);
            _planModule = null;
            LoadDll(pathToPlanModule, _planModule, "C:\\");
            ImplementationPlan = _planModule.GetImplementationPlan(QDeterminant);

        }

        public void OptimizePlan(ulong countCPU)
        {
            ImplementationPlan=_planModule.GetOptimizePlan(countCPU);
        }

        public ulong GetMaxLevel(List<QTerm> qDeterminant = null)
        {
            return _planModule.GetMaxLevel();
        }

        public ulong GetCPUCount(List<QTerm> qDeterminant = null)
        {
            return _planModule.GetCPUCount();
        }
    }
}
