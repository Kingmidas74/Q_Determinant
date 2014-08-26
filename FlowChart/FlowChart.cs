using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace FlowChart
{
    public class FlowChart
    {
      /*  public enum BlockTypes
        {
            Process,
            Condition,
            InputOutput
        }
        private int BlocksCount=0;
        private JObject FlowChartObject;
        public Analyzer(JObject json)
        {
            this.FlowChartObject = json;
            this.ValidateJSON();

        }

        public Analyzer(String jsonFilePath)
        {
            this.FlowChartObject = JObject.Parse(File.ReadAllText(@jsonFilePath));
            this.ValidateJSON();
            
        }

        private void ValidateJSON()
        {
            if (this.FlowChartObject.IsValid(JsonSchema.Parse(File.ReadAllText(@"JSONScheme.json"))))
            {
                this.Analyze();
            }
            else
            {
                throw new Exception("JSON is not valid!");
            }
        }

        private void Analyze()
        {
            dynamic d = this.FlowChartObject;
            this.BlocksCount = this.FlowChartObject.Count+((JArray) d.blocks).Count-1;
        }

        public dynamic GetObject()
        {
            return this.FlowChartObject;
        }

        public void AddBlock(Analyzer.BlockTypes type, string context, int[] inputLink)
        {
            throw new NotImplementedException();
        }

        public void RemoveBlock(int id)
        {
            throw new NotImplementedException();
        }
        
        public int GetBlocksCount()
        {
            return BlocksCount;
        }*/
    }
}
