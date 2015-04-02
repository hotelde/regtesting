using System.Collections.Generic;
using System.Runtime.Serialization;
using RegTesting.Contracts.Domain;

namespace RegTesting.Contracts
{
    /// <summary>
    /// A testresult
    /// </summary>
    [DataContract]
    public class TestResult
    {

        /// <summary>
        /// The Teststate
        /// </summary>
        [DataMember]
        public TestState TestState { get; set; }

        /// <summary>
        /// The exception if there was one, else null
        /// </summary>
        [DataMember]
        public Error Error { get; set; }


        /// <summary>
        /// The logentries
        /// </summary>
        [DataMember]
        public List<string> Log { get; set; }

        /// <summary>
        /// The screenshot
        /// </summary>
        [DataMember]
        public string Screenshot { get; set; }
    }
}
