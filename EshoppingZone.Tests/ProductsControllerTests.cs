using EshoppingZone.Controllers;
using EshoppingZone.Models;
using EshoppingZone.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EshoppingZone.Tests
{
    [TestFixture]
    public class ProductsControllerTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private ProductsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _controller = new ProductsController(_mockProductRepository.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkResult_WithProductsList()
        {
            var products = new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Laptop", Price = 1200.00m },
                new Product { ProductId = 2, ProductName = "Mouse", Price = 25.00m }
            };
            _mockProductRepository.Setup(r => r.ListAsync()).ReturnsAsync(products);

            var result = await _controller.GetAll();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.Not.Null);
        }

        [Test]
        public async Task Get_ReturnsOkResult_WhenProductExists()
        {
            var product = new Product { ProductId = 1, ProductName = "Laptop", Price = 1200.00m };
            _mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _controller.Get(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task Get_ReturnsNotFound_WhenProductDoesNotExist()
        {
            _mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

            var result = await _controller.Get(1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_ReturnsCreatedAtAction_WithProduct()
        {
            var product = new Product { ProductId = 0, ProductName = "Keyboard", Price = 85.00m };
            var createdProduct = new Product { ProductId = 3, ProductName = "Keyboard", Price = 85.00m };
            _mockProductRepository.Setup(r => r.AddAsync(product)).ReturnsAsync(createdProduct);

            var result = await _controller.Create(product);

            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(((Product?)createdResult?.Value)?.ProductId, Is.EqualTo(3));
        }

        [Test]
        public async Task Update_ReturnsOkResult_WhenProductExists()
        {
            var existingProduct = new Product { ProductId = 1, ProductName = "Laptop", Price = 1200.00m };
            var updatedProduct = new Product { ProductId = 1, ProductName = "Gaming Laptop", Price = 1500.00m };
            _mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _controller.Update(1, updatedProduct);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task Update_ReturnsNotFound_WhenProductDoesNotExist()
        {
            var product = new Product { ProductId = 1, ProductName = "Laptop", Price = 1200.00m };
            _mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

            var result = await _controller.Update(1, product);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_ReturnsNoContent_WhenProductExists()
        {
            var product = new Product { ProductId = 1, ProductName = "Laptop", Price = 1200.00m };
            _mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mockProductRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _controller.Delete(1);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenProductDoesNotExist()
        {
            _mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

            var result = await _controller.Delete(1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_VerifiesRepositoryCalled()
        {
            var product = new Product { ProductId = 0, ProductName = "Monitor", Price = 300.00m };
            var createdProduct = new Product { ProductId = 4, ProductName = "Monitor", Price = 300.00m };
            _mockProductRepository.Setup(r => r.AddAsync(product)).ReturnsAsync(createdProduct);

            await _controller.Create(product);

            _mockProductRepository.Verify(r => r.AddAsync(product), Times.Once);
        }

        [Test]
        public async Task Update_VerifiesRepositoryCalled()
        {
            var existingProduct = new Product { ProductId = 2, ProductName = "Mouse", Price = 25.00m };
            var updatedProduct = new Product { ProductId = 2, ProductName = "Gaming Mouse", Price = 50.00m };
            _mockProductRepository.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(existingProduct);
            _mockProductRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            await _controller.Update(2, updatedProduct);

            _mockProductRepository.Verify(r => r.Update(existingProduct), Times.Once);
        }

        [Test]
        public async Task Delete_VerifiesRepositoryCalled()
        {
            var product = new Product { ProductId = 3, ProductName = "Keyboard", Price = 85.00m };
            _mockProductRepository.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(product);
            _mockProductRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            await _controller.Delete(3);

            _mockProductRepository.Verify(r => r.Delete(product), Times.Once);
        }

        [Test]
        public async Task GetAll_ReturnsEmptyList_WhenNoProducts()
        {
            _mockProductRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<Product>());

            var result = await _controller.GetAll();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
