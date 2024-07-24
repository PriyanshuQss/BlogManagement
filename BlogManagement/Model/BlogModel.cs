namespace BlogManagement.Model
{
    public class BlogModel
    {
        public int BlogId { get; set; } = 0;
        public string Text { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
    }
}
