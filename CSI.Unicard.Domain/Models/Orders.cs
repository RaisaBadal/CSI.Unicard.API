﻿namespace CSI.Unicard.Domain.Models
{
    public class Orders
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
    }
}