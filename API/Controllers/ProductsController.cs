using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // kendimiz BaseApiController yapıp ordan türettik. Süreki ApiController ve Route yazmamak için
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepos;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productsRepos,
            IGenericRepository<ProductBrand> productBrandRepo,
            IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productsRepos = productsRepos;
        }

    [HttpGet]
    public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts(string sort)
    {
        // eski 1
        //var products = await _productsRepos.GetAllAsync();
        //return Ok(products);

        var spec = new ProductWithTypesAndBrandSpecification(sort);

        var products = await _productsRepos.ListAsync(spec);

        // eski 2
        /* return products.Select(product => new ProductToReturnDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            PictureUrl = product.PictureUrl,
            Price = product.Price,
            ProductBrand = product.ProductBrand.Name,
            ProductType = product.ProductType.Name
        }).ToList(); // Async eklenmedi çünkü burası memory üzerinden seçiliyor. Db den gelmiş oluyor. */

        return Ok(_mapper.Map<List<Product>, List<ProductToReturnDto>>(products));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
        //return await _productsRepos.GetByIdAsync(id); // eski 1

        var spec = new ProductWithTypesAndBrandSpecification(id);

        var product = await _productsRepos.GetEntityWithSpec(spec);

        // eski 2
        /* return new ProductToReturnDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            PictureUrl = product.PictureUrl,
            Price = product.Price,
            ProductBrand = product.ProductBrand.Name,
            ProductType = product.ProductType.Name
        }; */

        return _mapper.Map<Product, ProductToReturnDto>(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<ProductBrand>> GetProductBrands()
    {
        return Ok(await _productBrandRepo.GetAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<ProductType>> GetProductTypes()
    {
        return Ok(await _productTypeRepo.GetAllAsync());
    }
}
}