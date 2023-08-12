namespace EntityLayer.Concrete
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public int AddressId { get; set; }

        public string ShareCode { get; set; }

        public bool IsMain { get; set; }

        public Address Address { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }



        
    }
}
