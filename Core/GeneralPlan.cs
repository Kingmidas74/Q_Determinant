using System.Collections.Generic;

namespace Core
{
    public class GeneralPlan
    {
        public List<Operation> Operations { get; private set; }
        public QDet QDereminant { get; private set; }
        public ulong NumberOfThreads { get; private set; }
        public ulong NumberOfCycles { get; private set; }

        public List<StepOfImplementation> Steps { get; private set; }

        public GeneralPlan(List<Operation> operations, QDet qDereminant, ulong numberOfThreads, ulong numberOfCycles, List<StepOfImplementation> steps)
        {
            Steps = steps;
            NumberOfCycles = numberOfCycles;
            NumberOfThreads = numberOfThreads;
            QDereminant = qDereminant;
            Operations = operations;
        }
    }
}
