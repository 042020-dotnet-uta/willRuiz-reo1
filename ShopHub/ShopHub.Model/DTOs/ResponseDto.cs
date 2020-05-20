using System;
using System.Collections.Generic;
using System.Text;

namespace ShopHub.Model.DTOs
{
    public class ResponseDto<T>
    {
        public T Data { get; set;}
    }
}
