namespace Moses.Models
{
    public class MosesTreeNode
    {
        public int? Id { get; set; }
        public int? Depth { get; set; }
        public int? ParentId { get; set; }
        public object Content { get; set; }
        public string Description { get; set; }
    }
}
