﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.Core.Contracts
{
    public interface IOrderService
    {
        void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems); // void method that creates Orders based on basket items
        
        // copied form OrderService functionality for management 
        List<Order> GetOrderList(); 
        Order GetOrder(string Id);
        void UpdateOrder(Order updateOrder);

    }
}
