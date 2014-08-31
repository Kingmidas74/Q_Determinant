using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FlowChart
{
    public partial class Chart:IFlowChart
    {
        public void AddBlock(BlockTypes type, int previousBlock=0, int nextBlock=1, string content=null)
        {
            var newElement = new JObject();
            var previousBlocksIds = new JArray();
            previousBlocksIds.Add(previousBlock);
            newElement["type"] = type.ToString();
            newElement["previousBlock"] = previousBlocksIds;
            newElement["content"] = content;
            if (type != BlockTypes.Condition)
            {
                newElement["nextBlock"] = nextBlock;
            }
            var cntBlocks = recursiveFindBlock(_flowChart["blocks"], previousBlock).Parent.Count;
            var itemIndex = 0;
            for (var i = 0; i < cntBlocks; i++)
            {
                if ((int)(recursiveFindBlock(_flowChart["blocks"], previousBlock).Parent[i]["id"]) != previousBlock) continue;
                itemIndex = i;
                break;
            }
            ((JArray)recursiveFindBlock(_flowChart["blocks"], previousBlock).Parent).Insert(itemIndex+1, newElement);

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
            if (!((string)targetBlock["type"]).Equals("condition"))
            {
                var previousBlocksIds = targetBlock["previousBlock"];
                var nextBlockId = targetBlock["nextBlock"];
                foreach (var block in previousBlocksIds.Select(element => recursiveFindBlock(_flowChart["blocks"], (int)element)).Where(block => !((string)block["type"]).Equals("condition")))
                {
                    block["nextBlock"] = (int)nextBlockId;
                }
                var nextBlock = recursiveFindBlock(_flowChart["blocks"], (int)nextBlockId);
                var newPreviousBlocksIds = new JArray();
                foreach (var element in nextBlock["previousBlock"].Where(element => (int)element != id))
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
