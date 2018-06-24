﻿//using KodisoftTestAssignment.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Extensions.Logging;

//namespace KodisoftTestAssignment.Controllers
//{
//    [Route("api/[controller]")]
//    public class GroceryListController : Controller
//    {
//        private readonly GroceryListContext _context;
//        ILogger<GroceryListController> _logger;

//        public GroceryListController(GroceryListContext context, ILogger<GroceryListController> logger)
//        {
//            _context = context;
//            _logger = logger;

//            if (_context.GroceryList.Count() == 0)
//            {
//                _context.GroceryList.Add(new GroceryItem { Description = "Item1" });
//                _context.SaveChanges();
//            }
//        }

//        [HttpGet]
//        public IEnumerable<GroceryItem> GetAll()
//        {
//            _logger.LogInformation("GroceryListController.GetAll() Call.");
//            return _context.GroceryList.ToList();
//        }

//        [HttpGet("{id}", Name = "GetGroceryItem")]
//        public IActionResult GetById(long id)
//        {
//            var item = _context.GroceryList.FirstOrDefault(t => t.Id == id);
//            if (item == null)
//            {
//                return NotFound();
//            }
//            return new ObjectResult(item);
//        }

//        [HttpPost]
//        public IActionResult Create([FromBody] GroceryItem item)
//        {
//            if (item == null)
//            {
//                return BadRequest();
//            }

//            _context.GroceryList.Add(item);
//            _context.SaveChanges();

//            return CreatedAtRoute("GetGroceryItem", new { id = item.Id }, item);
//        }

//        [HttpDelete("{id}")]
//        public IActionResult Delete(long id)
//        {
//            var item = _context.GroceryList.First(t => t.Id == id);
//            if (item == null)
//            {
//                return NotFound();
//            }

//            _context.GroceryList.Remove(item);
//            _context.SaveChanges();
//            return new NoContentResult();
//        }
//    }
//}
