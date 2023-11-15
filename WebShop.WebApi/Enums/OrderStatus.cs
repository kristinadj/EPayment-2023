using Microsoft.AspNetCore.Mvc;

namespace WebShop.WebApi.Enums
{
    public enum OrderStatus
    {
        CREATED, EXPIRED, CANCELED, INVALID, COMPLETED
    }
}
