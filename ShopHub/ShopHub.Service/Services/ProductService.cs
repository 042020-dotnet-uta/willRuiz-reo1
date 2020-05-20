using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopHub.Model.Context;
using ShopHub.Model.DTOs;
using ShopHub.Model.Models;
using ShopHub.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopHub.Service.Services
{
    public class ProductService : IProductService
    {
        private ShopHubContext _context;
        private IMapper _mapper;
        public ProductService(ShopHubContext shopHubContext, IMapper mapper)
        {
            _context = shopHubContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a single product to database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ProductDto AddProduct(ProductDto product)
        {
            var mappedData = _mapper.Map<Product>(product);
            _context.Products.Add(mappedData);
            _context.SaveChanges();

            return _mapper.Map<ProductDto>(mappedData);
        }

        
        /// <summary>
        /// Updates a single product on teh DB
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ProductDto UpdateProduct(ProductDto product)
        {
            var record = _context.Products.Find(product.Id);
            _context.Entry(record).CurrentValues.SetValues(product);
            _context.SaveChanges();

            return _mapper.Map<ProductDto>(record);
        }

        
        /// <summary>
        /// Gets product by productId from database
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductDto GetProductById(int productId)
        {
            var product = _context.Products.Include(x=>x.Location).FirstOrDefault(x => x.Id == productId);
            return _mapper.Map<ProductDto>(product);
        }

        /// <summary>
        /// Subtracts the quantity of stock when an order is placed
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        public void MinusProductQuantity(int productId, int quantity)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == productId);
            product.Quantity = quantity;
            _context.SaveChanges();
        }

        /// <summary>
        /// Gets products by specific locationId
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<ProductDto> GetProductsByLocationId(int locationId)
        {
            var product = _context.Products.Include(x => x.Location).Where(x => x.LocationId == locationId).ToList();
            if (!(product is null) && product.Count > 0)
            {
                return _mapper.Map<List<ProductDto>>(product);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all products without specifying location
        /// </summary>
        /// <returns></returns>
        public List<ProductDto> GetAllProducts()
        {
            var products = _context.Products.Include(x=>x.Location).ToList();
            if (!(products is null) && products.Count > 0)
            {
                return _mapper.Map<List<ProductDto>>(products);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Removes a product from the DB based on passed productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool RemoveProduct(int productId)
        {
            var record = _context.Products.FirstOrDefault(x => x.Id == productId);
            var response = false;

            if (!(record is null))
            {
                _context.Products.Remove(record);
                _context.SaveChanges();
                response = true;
                return response;
            }
            else
            {
                return response;
            }
        }
    }
}
