namespace SupportDesk.Data.Entities;


public enum CustomerTier { Standard, Silver, Gold }

public enum OrderStatus { Placed, Processing, Packed, Shipped, Delivered, Cancelled, Returned }

public enum OrderItemStatus { Placed, Shipped, Delivered, Cancelled, ReturnRequested, Returned }

public enum ShipmentStatus { Pending, InTransit, OutForDelivery, Delivered, Delayed, Exception }
