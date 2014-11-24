using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Core.Atoms;

namespace ImplementationPlan
{
    internal static class Optimization
    {
        public static List<Link> SetLinks(ref List<Block> vertices)
        {
            var verticesList = vertices;
            var result = new List<Link>();
            for (var i = 0; i < verticesList.Count; i++)
            {
                var currentFunction =
                    ReversePolishNotation.Functions.FirstOrDefault(x => x.Signature.Equals(verticesList[i].Content));
                if (currentFunction != null)
                {
                    ulong countLink = 0;
                    for (var k = i - 1; k >= 0; k--)
                    {
                        if (countLink >= currentFunction.Parameters)
                        {
                            break;
                        }
                        if (result.Count(link => link.From == verticesList[k].Id) == 0)
                        {
                            result.Add(new Link() {From = verticesList[k].Id,To = verticesList[i].Id});
                            countLink++;
                        }
                    }
                }
            }
            return result;
        }

        public static Graph SetLevels(Graph graph)
        {
            foreach (var vertex in graph.Vertices)
            {
                var currentOperation =
                    ReversePolishNotation.Functions.FirstOrDefault(x => x.Signature.Equals(vertex.Content));
                if (currentOperation != null)
                {
                    var allIntoLinks = graph.Edges.FindAll(x => x.To == vertex.Id);
                    var previousVertex = allIntoLinks.Select(link => graph.Vertices.First(x => x.Id == link.From)).ToList();
                    vertex.Level = GetMaxLevel(previousVertex)+1;
                }
            }
            return graph;
        }

        public static Graph RemoveDuplicateParameters(Graph graph)
        {
            var parameters = graph.Vertices.FindAll(x => x.Level == 0);
            var checkedParameters = new List<string>();
            
            for (var i = 0; i < parameters.Count; i++)
            {
                var currentParametr = parameters[i];
                if (!checkedParameters.Contains(currentParametr.Content))
                {
                    for (var j = i + 1; j < parameters.Count; j++)
                    {
                        var tempParametr = parameters[j];
                        if (tempParametr.Content.Equals(currentParametr.Content))
                        {
                            foreach (var link in graph.Edges.Where(x=>x.From==tempParametr.Id))
                            {
                                link.From = currentParametr.Id;
                            }
                        }
                    }
                    checkedParameters.Add(currentParametr.Content);
                }
            }
            return RemoveAllUnusedVertices(graph);
        }

        private static Graph RemoveAllUnusedVertices(Graph graph)
        {
            var resultVertices = graph.Vertices.FindAll(x=>x.Id>0);
            foreach (var vertex in graph.Vertices)
            {
                if (graph.Edges.LongCount(x => x.From == vertex.Id) == 0 && vertex.Level<graph.GetMaxLevel())
                {
                    resultVertices.RemoveAll(x => x.Id == vertex.Id);
                }
            }
            graph.Vertices = resultVertices;
            return graph;
        }

        private static ulong GetMaxLevel(List<Block> vertices)
        {
            return vertices.Max(x=>x.Level);
        }

        internal static Graph RemoveDuplicateFunctions(Graph graph)
        {
            var maxLevel = graph.GetMaxLevel();
            for (ulong level = 1; level < maxLevel; level++)
            {
                var functions = graph.Vertices.FindAll(x => x.Level == level);
                var checkedFunctions = new List<FunctionSignature>();
                for (var i = 0; i < functions.Count; i++)
                {
                    var currentFunction = functions[i];
                    var currentPreviousIds = graph.Edges.Where(x => x.To == currentFunction.Id).Select(link => link.From).ToList();
                    var currentSignature = new FunctionSignature() {PreviousIds = currentPreviousIds, Signature = currentFunction.Content};
                    Debug.WriteLine(currentFunction.Content,"CHECK FUNCTION:");

                    if (checkedFunctions.Count(x => x.Signature.Equals(currentFunction.Content) && x.PreviousIds.SequenceEqual(currentPreviousIds)) == 0)
                    {
                        for (var j = i + 1; j < functions.Count; j++)
                        {
                            var tempFunction = functions[j];
                            var tempPreviousIds = graph.Edges.Where(x => x.To == tempFunction.Id).Select(link => link.From).ToList();
                            var tempSignature = new FunctionSignature(){Signature = tempFunction.Content, PreviousIds = tempPreviousIds};
                            if (tempSignature.Signature.Equals(currentSignature.Signature) && tempSignature.PreviousIds.SequenceEqual(currentSignature.PreviousIds))
                            {
                                foreach (var link in graph.Edges.Where(x => x.From == tempFunction.Id))
                                {
                                    link.From = currentFunction.Id;
                                }
                                foreach (var link in graph.Edges.Where(x => x.To == tempFunction.Id))
                                {
                                    link.To = currentFunction.Id;
                                }
                            }
                        }
                        checkedFunctions.Add(currentSignature);
                    }
                }
            }
            
            graph=RemoveAllUnusedVertices(graph);

            return graph;
        }

        internal static Graph OptimizateGraph(Graph graph, ulong maxVertexOnLevel)
        {
            return graph;
        }
    }
}
