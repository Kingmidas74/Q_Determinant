using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Atoms;
using Core.Serializers.SerializationModels.SolutionModels;

namespace ImplementationPlan.InternalClasses
{
    public class GraphBuilder
    {
        private List<Block> _blocks;
        private List<Link> _links;
        private Graph _graph;
        private List<Function> _functions; 


        public GraphBuilder(List<Function> Functions)
        {
            _functions = Functions;
            
        }

        public Graph BuildGraph(List<Block> Lexems, ulong startId=1)
        {
            _links = new List<Link>();
            Lexems.Reverse();
            foreach (var lexem in Lexems)
            {
                lexem.Id = startId;
                startId++;
            }
            _blocks = Lexems;
            SetLinks();
            SetLevels();
            _graph = new Graph(_blocks, _links);
            /*RemoveDuplicateParameters();
            RemoveDuplicateFunctions();*/
            return _graph;
        }

        private bool IsLexem(string part)
        {
            return _functions.FirstOrDefault(function => function.Signature.Equals(part)) != null;
        }

        private void SetLevels()
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                if (!IsLexem(_blocks[i].Content))
                {
                    _blocks[i].Level = 0;
                }
                else
                {
                     _blocks[i].Level = _links.FindAll(x => x.To == _blocks[i].Id).Select(link => _blocks.First(x => x.Id == link.From)).Max(x => x.Level) + 1;
                }

            }
        }

        private void SetLinks()
        {
            var verticesList = _blocks;
            for (var i = 0; i < verticesList.Count; i++)
            {
                var currentFunction = _functions.FirstOrDefault(x => x.Signature.Equals(verticesList[i].Content));
                if (currentFunction != null)
                {
                    ulong countLink = 0;
                    for (var k = i - 1 ; k >=0; k--)
                    {
                        if (countLink >= currentFunction.Parameters)
                        {
                            break;
                        }
                        if (_links.Count(link => link.From == verticesList[k].Id) == 0)
                        {
                            _links.Add(new Link { From = verticesList[k].Id, To = verticesList[i].Id });
                            countLink++;
                        }
                    }
                }
            }
            Debug.WriteLine(_links.Count, "COUNTLINKS");
        }

        private void RemoveDuplicateParameters()
        {
            var graph = _graph;
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
                            foreach (var link in graph.Edges.Where(x => x.From == tempParametr.Id))
                            {
                                link.From = currentParametr.Id;
                            }
                        }
                    }
                    checkedParameters.Add(currentParametr.Content);
                }
            }
            _graph = RemoveAllUnusedVertices(graph);
        }

        private Graph RemoveAllUnusedVertices(Graph graph)
        {
            var resultVertices = graph.Vertices.FindAll(x => x.Id >= 0);
            foreach (var vertex in graph.Vertices.Where(vertex => graph.Edges.LongCount(x => x.From == vertex.Id) == 0 && vertex.Level < graph.GetMaxLevel()))
            {
                resultVertices.RemoveAll(x => x.Id == vertex.Id);
            }
            graph.Vertices = resultVertices;
            return graph;
        }

        private void RemoveDuplicateFunctions()
        {
            var maxLevel = _graph.GetMaxLevel();
            for (ulong level = 1; level < maxLevel; level++)
            {
                var functions = _graph.Vertices.FindAll(x => x.Level == level);
                var checkedFunctions = new List<FunctionSignature>();
                for (var i = 0; i < functions.Count; i++)
                {
                    var currentFunction = functions[i];
                    var currentPreviousIds = _graph.Edges.Where(x => x.To == currentFunction.Id).Select(link => link.From).ToList();
                    var currentSignature = new FunctionSignature { PreviousIds = currentPreviousIds, Signature = currentFunction.Content };
                    if (checkedFunctions.Count(x => x.Signature.Equals(currentFunction.Content) && x.PreviousIds.SequenceEqual(currentPreviousIds)) == 0)
                    {
                        for (var j = i + 1; j < functions.Count; j++)
                        {
                            var tempFunction = functions[j];
                            var tempPreviousIds = _graph.Edges.Where(x => x.To == tempFunction.Id).Select(link => link.From).ToList();
                            var tempSignature = new FunctionSignature { Signature = tempFunction.Content, PreviousIds = tempPreviousIds };
                            if (tempSignature.Signature.Equals(currentSignature.Signature) && tempSignature.PreviousIds.SequenceEqual(currentSignature.PreviousIds))
                            {
                                foreach (var link in _graph.Edges.Where(x => x.From == tempFunction.Id))
                                {
                                    link.From = currentFunction.Id;
                                }
                                foreach (var link in _graph.Edges.Where(x => x.To == tempFunction.Id))
                                {
                                    link.To = currentFunction.Id;
                                }
                            }
                        }
                        checkedFunctions.Add(currentSignature);
                    }
                }
            }

            _graph = RemoveAllUnusedVertices(_graph);
        }
    }
}
