using static akvelon_test_data.DataModels.Enums;

namespace akvelon_test_data.DataModels
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskItemStatus CurrentStatus { get; set; }
        public string ProjectName { get; set; }
        public Project Project { get; set; }
    }
    
}
