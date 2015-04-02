using System;

namespace RegTesting.Contracts
{
    /// <summary>
    /// Model for a testsystem summary
    /// </summary>
    public class TestsystemSummaryModel
    {
        /// <summary>
        /// The TestsuiteName
        /// </summary>
        public string TestsuiteName { get; set; }
        /// <summary>
        /// The TestsystemName
        /// </summary>
        public string TestsystemName { get; set; }
        /// <summary>
        /// The TestsystemId
        /// </summary>
        public int TestsystemId { get; set; }
        /// <summary>
        /// The TestsuiteId
        /// </summary>
        public int TestsuiteId { get; set; }
        /// <summary>
        /// The Date when the testsystem changed
        /// </summary>
        public DateTime LastChangeDate { get; set; }
        /// <summary>
        /// The Status of the TestsystemStatus
        /// </summary>
        public int TestsystemStatus { get; set; }

    }
}