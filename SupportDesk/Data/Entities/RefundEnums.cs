namespace SupportDesk.Data.Entities;

public enum PaymentMethod { Card, UPI, NetBanking, Wallet, CashOnDelivery }
public enum PaymentStatus { Pending, Paid, Failed, Refunded, PartiallyRefunded }
public enum RefundStatus { Pending, Approved, Rejected, Processed }
public enum ApprovalStatus { Pending, Approved, Rejected }
public enum WorkflowState { Running, WaitingForApproval, Completed, Rejected, Failed }
