﻿using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);

        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);

        Task DeleteBasket(string userName);

        //Task<bool> IsbasketOwner(string ownerBasket);
    }
}
