using System;
using System.Collections.Generic;
using static akvelon_test_data.DataModels.Enums;

namespace akvelon_test_data.DataModels
{
    public class Project
    {
        public string Name { get; set; }
        public int Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public ProjectStatus CurrentStatus { get; set; }
        public List<TaskItem> TaskItems { get; set; }
    }
    
}
