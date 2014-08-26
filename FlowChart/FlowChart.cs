using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace FlowChart
{
    public class FlowChart:IFlowChart
    {
      
        private JObject _flowChart;

        private void ValidJson()
        {
            if (_flowChart.IsValid(JsonSchema.Parse(File.ReadAllText(@"JSONScheme.json"))) == false)
            {
                throw new Exception("Invalid JSON!");
            }
        }
        public void AddBlock(BlockTypes type, string context, int[] inputLink)
        {
           // (JArray)(this.flowChart.blocks).Add()
        }

        public void RemoveBlock(int id)
        {
            var newBlocks = new JArray();

            foreach (var block in _flowChart["blocks"].Where(block => (int)block["id"] != id))
            {
                newBlocks.Add(block);
            }
            _flowChart["blocks"].Replace(newBlocks);
        }

        public int GetBlocksCount(bool withSystemBlocks = false)
        {
            return withSystemBlocks
                ? _flowChart["blocks"].Count() + 2
                : _flowChart["blocks"].Count();
        }

        public void ChangeContentBlock(int id, string content)
        {
            foreach (var block in _flowChart["blocks"].Where(block => (int) block["id"] == id))
            {
                block["content"] = content;
            }
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
            throw new NotImplementedException();
        }
    }
}
