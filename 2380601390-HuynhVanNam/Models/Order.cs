namespace _2380601390_HuynhVanNam.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }

        public List<OrderDetail> Details { get; set; }
    }
}