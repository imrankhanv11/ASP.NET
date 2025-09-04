namespace Web_API_Practice.DTO
{
    public class AddProduct
    {
        public string ProductName { get; set; }
        public decimal UnitPrize { get; set; }

        public int CategoryID { get; set; }
        public int ShipperID { get; set; }
    }
}
