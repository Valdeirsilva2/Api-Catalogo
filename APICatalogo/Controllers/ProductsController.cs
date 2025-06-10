﻿using APICatalogo.Context;
using APICatalogo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _context.Products.ToList();
        return Ok(products);
    }
    
    [HttpGet("{id:int}", Name = "GetProductById")]
    public ActionResult<Product> Get(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound("Product not found.");
        }
        return Ok(product);
    }
    
    [HttpPost]
    public ActionResult Post(Product product)
    {
        if (product is null)
            return BadRequest("Product cannot be null.");
        
        _context.Products.Add(product);
        _context.SaveChanges(); // <- função para salvar as alterações no banco de dados

        return new CreatedAtRouteResult("GetProductById", new { id = product.ProductId }, product);
    }
    
    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Product product)
    {
        if (id != product.ProductId)
            return BadRequest("Product ID mismatch.");

        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound("Product not found.");
        }
        _context.Products.Remove(product);
        _context.SaveChanges();
        
        return Ok(product);
    }
}
