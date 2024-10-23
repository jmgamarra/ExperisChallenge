namespace ProductManager.Api.DTOs
{
    public class AddProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
    }
}
