using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace FlowChart
{
    public partial class Chart
    {
        private JObject _flowChart;

        private bool brunchContainsId(IEnumerable<JToken> brunch, int id)
        {
            var result = false;
            foreach (var element in brunch)
            {
                if (!((string)element["type"]).Equals("condition"))
                {
                    result = result || ((int)element["id"] == id);
                }
                else
                {
                    result = result || brunchContainsId(element["truePath"], id) || brunchContainsId(element["falsePath"], id);
                }
            }
            return result;
        }

        private bool isLoopedBrunch(IEnumerable<JToken> branch, int id)
        {
            var result = false;
            foreach (var element in branch)
            {
                if (!((string)element["type"]).Equals("condition"))
                {
                    result = result || ((int)element["nextBlock"] == id);
                }
                else
                {
                    result = result || isLoopedBrunch(element["truePath"], id) || isLoopedBrunch(element["falsePath"], id);
                }
            }
            return result;
        }

        private void MoveElementsFromTo(JToken arrayFrom, JToken arrayTo, IEnumerable<JToken> alternativeArray = null)
        {
            var conditionElementId = (from element in (JArray)arrayFrom.First["previousBlock"]
                where !brunchContainsId(arrayFrom, (int) element)
                select (int) element).FirstOrDefault();
            var conditionBlockPreviousIds = new JArray();
            if (alternativeArray != null)
            {
                foreach (
                    var element in
                        recursiveFindBlock(arrayTo, conditionElementId)["previousBlock"].Where(
                            element => !brunchContainsId(alternativeArray, (int) element)))
                {
                    conditionBlockPreviousIds.Add(element);
                }
            }
            var newPreviousIdsForArrayFrom = new JArray();
            foreach (var element in arrayFrom.First["previousBlock"].Where(element => (int)element != conditionElementId))
            {
                newPreviousIdsForArrayFrom.Add(element);
            }
            newPreviousIdsForArrayFrom.Merge(conditionBlockPreviousIds);
            arrayFrom.First["previousBlock"] = newPreviousIdsForArrayFrom;
            var newNextBlockId = (int)arrayFrom.First["id"];
            foreach (var element in newPreviousIdsForArrayFrom)
            {
                recursiveFindBlock(arrayTo, (int) element)["nextBlock"] = newNextBlockId;
            }

            foreach (var element in arrayFrom)
            {
                ((JArray)arrayTo).Add(element);
            }
        }

        private void ValidJson()
        {
            if (_flowChart.IsValid(JsonSchema.Parse(File.ReadAllText(@"JSONScheme.json"))) == false)
            {
                //throw new Exception("Invalid JSON!");
            }
        }

        private int GetBlocksCount(IEnumerable<JToken> array)
        {
            var result = 0;
            foreach (var block in array)
            {
                result++;
                if (!((string) block["type"]).Equals("condition")) continue;
                result += (GetBlocksCount(block["truePath"]) + GetBlocksCount(block["falsePath"]));
            }
            return result;
        }

        private JToken recursiveFindBlock(IEnumerable<JToken> array, int id)
        {
            foreach (var block in array)
            {
                if ((int)block["id"] == id)
                {
                    return block;
                }
                if (!((string) block["type"]).Equals("condition")) continue;
                var result = recursiveFindBlock(block["truePath"], id);
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
            return null;
        }
    }
}
