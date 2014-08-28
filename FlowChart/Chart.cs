﻿using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace FlowChart
{
    public class Chart:IFlowChart
    {
        private JObject _flowChart;

        private void ValidJson()
        {
            if (_flowChart.IsValid(JsonSchema.Parse(File.ReadAllText(@"JSONScheme.json"))) == false)
            {
                //throw new Exception("Invalid JSON!");
            }
        }
        public void AddBlock(BlockTypes type, string context, int[] inputLink)
        {
            // (JArray)(this.flowChart.blocks).Add()
        }

        
        public void RemoveBlock(int id)
        {
            var targetBlock = recursiveFindBlock(_flowChart["blocks"], id);
            if (!((string) targetBlock["type"]).Equals("condition"))
            {
                var previousBlocks = targetBlock["previousBlock"];
                var nextBlock = targetBlock["nextBlock"];
                foreach (var block in targetBlock.Parent) 
                {
                    foreach (var i in previousBlocks)
                    {
                        if ((int) i == (int) block["id"])
                        {
                            block["nextBlock"] = nextBlock;
                        }
                    }
                    if ((int) block["id"] != (int) nextBlock) continue;
                    var newPreviousBlocks = new JArray();
                    foreach (var i in block["previousBlock"])
                    {
                        if ((int) i != (int) targetBlock["id"])
                        {
                            newPreviousBlocks.Add(i);
                        }
                    }
                    newPreviousBlocks.Merge(previousBlocks);
                    block["previousBlock"] = newPreviousBlocks;
                }
                targetBlock.Remove();
            }
            else
            {
                if (isLoopedBrunch(targetBlock["truePath"], id))
                {
                    var mergePreviousBlock = new JArray();
                    foreach (var element in targetBlock["falsePath"][0]["previousBlock"])
                    {
                        if ((int) element != id)
                        {
                            mergePreviousBlock.Add(element);
                        }
                    }
                    foreach (var i in targetBlock["previousBlock"])
                    {
                        if (!brunchContainsId(targetBlock["truePath"], (int) i))
                        {
                            mergePreviousBlock.Add(i);
                        }
                    }
                    targetBlock["previousBlock"] = mergePreviousBlock;
                    targetBlock["falsePath"][0]["previousBlock"] = mergePreviousBlock;
                    foreach (var elemet in targetBlock["falsePath"])
                    {
                        targetBlock.Parent.Add(elemet);
                    }
                    var previousBlocks = targetBlock["previousBlock"];
                    foreach (var element in targetBlock.Parent)
                    {
                        foreach (var i in previousBlocks)
                        {
                            if ((int)i == (int)element["id"])
                            {
                                element["nextBlock"] = targetBlock["falsePath"][0]["id"];
                            }
                        }
                    }
                    targetBlock.Remove();
                }
                else
                {
                    var mergePreviousBlock = new JArray();
                    foreach (var element in targetBlock["truePath"][0]["previousBlock"])
                    {
                        if ((int)element != id)
                        {
                            mergePreviousBlock.Add(element);
                        }
                    }
                    foreach (var element in targetBlock["previousBlock"])
                    {
                        if (!brunchContainsId(targetBlock["falsePath"], (int) element))
                        {
                            mergePreviousBlock.Add(element);
                        }
                    }
                    targetBlock["truePath"][0]["previousBlock"] = mergePreviousBlock;
                    foreach (var elemet in targetBlock["truePath"])
                    {
                        targetBlock.Parent.Add(elemet);
                    }

                    var previousBlocks = targetBlock["previousBlock"];
                    foreach (var element in targetBlock.Parent)
                    {
                        foreach (var i in previousBlocks)
                        {
                            if ((int)i == (int)element["id"])
                            {
                                element["nextBlock"] = targetBlock["truePath"][0]["id"];
                            }
                        }
                    }
                    targetBlock.Remove();
                }
            }
            
        }

        private bool brunchContainsId(JToken brunch, int id)
        {
            var result = false;
            foreach (var element in brunch)
            {
                if (!((string) element["type"]).Equals("condition"))
                {
                    result = result || ((int) element["id"] == id);
                }
                else
                {
                    result = result || brunchContainsId(element["truePath"], id) || brunchContainsId(element["falsePath"], id);
                }
            }
            return result;
        }
        private bool isLoopedBrunch(JToken branch, int id)
        {
            var result = false;
            foreach (var element in branch)
            {
                if (!((string) element["type"]).Equals("condition"))
                {
                    result = result || ((int) element["nextBlock"] == id);
                }
                else
                {
                    result = result || isLoopedBrunch(element["truePath"], id) || isLoopedBrunch(element["falsePath"], id);
                }
            }
            return result;
        }

        public string GetPreviousBlock(int id)
        {
            return (string)recursiveFindBlock(_flowChart["blocks"], id)["previousBlock"].ToString();
        }

        public string GetNextBlock(int id)
        {
            return (string)recursiveFindBlock(_flowChart["blocks"], id)["nextBlock"].ToString();
        }

        private int GetBlocksCount(JToken array)
        {
            var result = 0;
            foreach (var block in array)
            {
                result++;
                if (!((string) block["type"]).Equals("condition")) continue;
                result += GetBlocksCount(block["truePath"]);
                result += GetBlocksCount(block["falsePath"]);
            }
            return result;
        }

        public int GetTotalBlocksCount()
        {
            return GetBlocksCount(_flowChart["blocks"]);
        }

        public void ChangeContentBlock(int id, string content)
        {
            recursiveFindBlock(_flowChart["blocks"], id)["content"] = content;
        }

        private JToken recursiveFindBlock(JToken array, int id)
        {
            foreach (var block in array)
            {
                if ((int)block["id"] == id)
                {
                    return block;
                }
                if (((string)block["type"]).Equals("condition"))
                {
                    var result = recursiveFindBlock(block["truePath"],id);
                    if (result != null)
                    {
                        return result;
                    }
                    result = recursiveFindBlock(block["falsePath"], id);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        public string GetContentBlock(int id)
        {
            return (string)recursiveFindBlock(_flowChart["blocks"], id)["content"];
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
