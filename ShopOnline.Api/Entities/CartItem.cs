
namespace ShopOnline.Api.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }

        public int UserId { get; set; }

        internal object ConvertToDto(IEnumerable<Product> products)
        {
            throw new NotImplementedException();
        }
    }
}
