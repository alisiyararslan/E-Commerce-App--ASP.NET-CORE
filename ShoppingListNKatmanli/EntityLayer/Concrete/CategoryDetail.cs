namespace EntityLayer.Concrete
{
    public class CategoryDetail
    {
        public int Id { get; set; }

        public int SubCategoryId { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public string Name { get; set; }
    }
}
