﻿using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Model;

namespace StokTakip.Contract.Request.Product
{
    public class DeleteProductRequest : IRequest<ResponseBase<ProductViewModel>>
    {
        public int Id { get; set; }
    }
}
