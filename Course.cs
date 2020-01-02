using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TermManager
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string ObjectiveName { get; set; }
        public DateTime ObjectiveStart { get; set; }
        public DateTime ObjectiveEnd { get; set; }
        public string PerformanceName { get; set; }
        public DateTime PerformanceStart { get; set; }
        public DateTime PerformanceEnd { get; set; }

        public bool startNotify { get; set; }

        public bool endNotify { get; set; }
        public bool startObjNotify { get; set; }
        public bool endObjNotify { get; set; }
        public bool startPerfNotify { get; set; }
        public bool endPerfNotify { get; set; }
    }
}
