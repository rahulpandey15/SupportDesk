namespace SupportDesk.Http;

public interface ICustomerContext
{
 int GetCustomerId();
}


public sealed class CustomerContext : ICustomerContext
{
 
 private readonly IHttpContextAccessor _httpContextAccessor;

 public CustomerContext(IHttpContextAccessor  httpContextAccessor)
 {
  _httpContextAccessor = httpContextAccessor;
 }


 public int GetCustomerId()
 {
  var customerId
   = _httpContextAccessor.HttpContext.Request.Headers["CustomerId"];
  
  if(string.IsNullOrEmpty(customerId))
    return 0;
  return int.Parse(customerId);
 }
}