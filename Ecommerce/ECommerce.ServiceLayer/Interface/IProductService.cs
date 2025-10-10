﻿using ECommerce.ServiceLayer.DTO.Products.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.Interface
{
    public interface IProductService
    {
        Task<int> AddNewProductService(AddNewProductDTO dto);
    }
}
