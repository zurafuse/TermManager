using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TermManager
{
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Content { get; set; }
    }
}
