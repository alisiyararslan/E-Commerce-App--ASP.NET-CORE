namespace EntityLayer.Concrete
{
    public class Item
    {
        public int Id { get; set; }



        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int CategoryDetailId { get; set; }


        public int UserId { get; set; }

        public User User { get; set; }

        public int FavoriteCount { get; set; }

        public string Title { get; set; }

        public string Brand { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public int Discount { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        

        public List<FavoriteItemUser> FavoriteItemUsers { get; set; }
    }
}
