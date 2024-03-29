using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Dtos;
using DAL.Entities;

namespace BLL.Services.ProductServices
{
    public interface IProductService
    {
        Task<string> ProductWip(int productId, string token);

        Task<Boolean> ProductWipDelete(int productId, string token);

        Task<List<Product>> GetProducts();

        Task<Product> GetProduct(int id);

        Task<bool> DeleteProduct(int id);

        Task<Product> CreateProduct(CreateUpdateProductDto productDto);

        Task<Product> UpdateProduct(CreateUpdateProductDto productDto, int productId);



    }
}
