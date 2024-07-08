namespace CSI.Unicard.Application.DTOs
{
    public class OrderDTO
    {
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
