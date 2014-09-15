using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FlowChart
{
    public partial class Chart:IFlowChart
    {
        public void AddBlock(BlockTypes type, int previousBlock=0, string content=null, bool truePath=true)
        {
            var newElement = new JObject();
            var previousBlocksIds = new JArray {previousBlock};
            var newId = getMaxId(_flowChart["blocks"]) + 1;
            newElement["id"] = newId;
            newElement["type"] = type.ToString();
            newElement["content"] = content;
            newElement["previousBlock"] = previousBlocksIds;
            var prevBlock = recursiveFindBlock(_flowChart["blocks"], previousBlock);
            if (type != BlockTypes.condition)
            {
                if (!((string) prevBlock["type"]).Equals("condition"))
                {
                    var temp = prevBlock["nextBlock"];
                    prevBlock["nextBlock"] = newId;
                    newElement["nextBlock"] = temp;
                }
                else
                {
                    var temp = prevBlock["truePath"].First["id"];
                    prevBlock["truePath"].First["nextBlock"];
                }
            }
            else
            {
                
            }



            
            var cntBlocks = recursiveFindBlock(_flowChart["blocks"], previousBlock).Parent.Count;
            var itemIndex = 0;
            for (var i = 0; i < cntBlocks; i++)
            {
                if ((int)(recursiveFindBlock(_flowChart["blocks"], previousBlock).Parent[i]["id"]) != previousBlock) continue;
                itemIndex = i;
                break;
            }
            ((JArray)prevBlock.Parent).Insert(itemIndex+1, newElement);

            /*
            int indexNextElement = 0;
            foreach (var element in recursiveFindBlock(_flowChart["blocks"], nextBlock).Parent)
            {
                if ((int)element["id"] ==  nextBlock)
                {
                    indexNextElement=element.inde
                }
            }*/

        }

        public void RemoveBlock(int id)
        {
            var targetBlock = recursiveFindBlock(_flowChart["blocks"], id);
            if (id <= 1) return;
            if (!((string) targetBlock["type"]).Equals("condition"))
            {
                var previousBlocksIds = targetBlock["previousBlock"];
                var nextBlockId = targetBlock["nextBlock"];
                foreach (
                    var block in
                        previousBlocksIds.Select(element => recursiveFindBlock(_flowChart["blocks"], (int) element))
                            .Where(block => !((string) block["type"]).Equals("condition")))
                {
                    block["nextBlock"] = (int) nextBlockId;
                }
                var nextBlock = recursiveFindBlock(_flowChart["blocks"], (int) nextBlockId);
                var newPreviousBlocksIds = new JArray();
                foreach (var element in nextBlock["previousBlock"].Where(element => (int) element != id))
                {
                    newPreviousBlocksIds.Add(element);
                }
                newPreviousBlocksIds.Merge(previousBlocksIds);
                nextBlock["previousBlock"] = newPreviousBlocksIds;
            }
            else
            {
                if (isLoopedBrunch(targetBlock["truePath"], id))
                {
                    MoveElementsFromTo(targetBlock["falsePath"], targetBlock.Parent, targetBlock["truePath"]);
                }
                else
                {
                    MoveElementsFromTo(targetBlock["truePath"], targetBlock.Parent, targetBlock["falsePath"]);
                }
            }
            targetBlock.Remove();
        }

        public int GetBlocksCount()
        {
            return GetBlocksCount(_flowChart["blocks"]);
        }

        public string GetContentBlock(int id)
        {
            return (string)recursiveFindBlock(_flowChart["blocks"], id)["content"];
        }

        public void ChangeContentBlock(int id, string content)
        {
            recursiveFindBlock(_flowChart["blocks"], id)["content"] = content;
        }

        public void SetJsonFile(string filePath)
        {
            _flowChart = JObject.Parse(File.ReadAllText(@filePath));
            ValidJson();
        }

        public void SetJson(string json)
        {
            _flowChart = JObject.Parse(json);
            ValidJson();
        }

        public void SaveToFile(string filePath)
        {
            File.WriteAllText(filePath, _flowChart.ToString());
        }
    }
}
