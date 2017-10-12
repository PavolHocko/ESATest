using System.Collections.Generic;
using System.Linq;

namespace ESATest.Common
{
    internal static class Extensions
    {
        internal static PartialResult GetOrCreate(this List<PartialResult> partialResults, string methodName)
        {
            PartialResult partialResult = null;

            partialResult = partialResults.FirstOrDefault(w => w.Technique == methodName);
            if (partialResult == null)
            {
                partialResult = new PartialResult
                {
                    Technique = methodName,
                };

                partialResults.Add(partialResult);
            }

            return partialResult;
        }
    }
}
