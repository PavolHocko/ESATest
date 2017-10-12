using System.Collections.Generic;

namespace ESATest.Common
{
    internal class FinalResult
    {
        public string FileName { get; set; }

        public int NumberOfLines { get; set; }

        public List<PartialResult> PartialResult { get; set; } = new List<PartialResult>();

        public string Summary
        {
            get
            {
                return $"######## {TestName} results for file: {FileName} with {NumberOfLines} lines";
            }
        }

        public string TestName { get; set; }
    }
}
