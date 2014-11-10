using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace ImplementationPlan.ActionListTemp
{
    public class QDCalculator
    {
        private QDet QD;
        private List<ActionList> AL;
        private List<Block> _blocks;
        private List<Link> _links; 
        public QDCalculator(List<Block> blocks, List<Link> links, List<Operation> operations)
        {
            AL=new List<ActionList>();
            QD = new QDet();
            _blocks = blocks;
            _links = links;
            ParseFlowChart(operations);
            var x = new QTerm();
            var y = new QTerm();
            var z = new QTerm();
            x.Logical = "dx>=(5*a+2*(b-1))";
            x.Definitive = "sin((8+2*5)/(1+3*2-4))";
            QD.QDeterminant.Add(x);
            y.Definitive = "3*x+1";
            QD.QDeterminant.Add(y);
        }

        private void ParseFlowChart(List<Operation> operations)
        {
            var block = _blocks.FirstOrDefault(x => x.Type == BlockTypes.End);
            var a = recursive(block,false);
          /*  Debug.WriteLine("RESULT");
            Debug.WriteLine("Header");
            Debug.WriteLine(a.Header.Du);
            Debug.WriteLine(a.Header.Dw);
            foreach (var s in a.Header.Morf)
            {
                Debug.WriteLine(s);
            }
*/
        }

        private ActionList recursive(Block block, bool body)
        {
            Debug.WriteLine("REC:"+block.Content);
            var currentAL = new ActionList();
            var stack = new Stack<Block>();
            while (_links.Count(x => x.To == block.Id) == 1)
            {
                stack.Push(block);
                block = _blocks.First(x => x.Id == _links.First(y => y.To == block.Id).From);
            }
            if (_links.Count(x => x.To == block.Id) == 2)
            {
                stack.Push(block);
                currentAL.Header = ParseStack(stack);
                foreach (var l in _links.Where(x => x.To == block.Id))
                {
                    Debug.WriteLine(isCan(l, block));
                    if (isCan(l, block))
                    {
                        Debug.WriteLine("Create Tile");
                        currentAL.Tile.Add(recursive(_blocks.First(x => x.Id == l.From), body));
                    }
                    else
                    {
                        Debug.WriteLine("Create Body");
                        //Debug.WriteLine(_blocks.First(x => x.Id == l.From).Content);
                        currentAL.Body.Add(recursive(_blocks.First(x => x.Id == l.From), true));
                        if (_blocks.First(x => x.Id == l.From) != block && body==false)
                        {
                            
                        }
                        return currentAL;
                    }
                }
            }
            if (block.Type == BlockTypes.Start)
            {
                currentAL.Header = ParseStack(stack);
            }
            return currentAL;
        }

        private bool isCan(Link link, Block block)
        {
            if (link.From == block.Id)
            {
                return false;
            }
            if (_blocks.First(x => x.Id == link.From).Type == BlockTypes.Start)
            {
                return true;
            }
            var previousLink = _links.Where(x => x.To == link.From);
            return previousLink.Aggregate(false, (current, currentLink) => current || isCan(currentLink, block));
        }

        private Element ParseStack(Stack<Block> blocks)
        {
            var element = new Element();
            while (blocks.Count>0)
            {
                var currentBlock = blocks.Pop();
                if (currentBlock.Type == BlockTypes.Condition)
                {
                    element.Du = currentBlock.Content;
                } else if (currentBlock.Type == BlockTypes.Output)
                {
                    element.Dw = currentBlock.Content;
                } else if (currentBlock.Type != BlockTypes.End)
                {
                    element.Morf.Add(currentBlock.Content);
                }
            }
            Debug.WriteLine("DU:"+element.Du);
            Debug.WriteLine("DW:"+element.Dw);
            Debug.WriteLine("MORF:::");
            foreach(var s in element.Morf)
            Debug.WriteLine(s);
            return element;
        }

        private string ParseBlock(Block block)
        {
            switch (block.Type)
            {
                case BlockTypes.End:
                    return "";
                case BlockTypes.Start:
                    return null;
            }
            return block.Content;
        }



        public QDet GetQD()
        {
            return QD;
        }
    }
}
