namespace CSI.Unicard.Domain.Models
{
    public class OrderItems
    {
        public int OrderItemId {  get; set; }

        public int OrderId {  get; set; }

        public int ProductId { get; set; }

        public int Quantity {  get; set; }

        public decimal price {  get; set; }
    }
}
