using EshoppingZone.Controllers;
using EshoppingZone.Dtos;
using EshoppingZone.Models;
using EshoppingZone.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace EshoppingZone.Tests
{
    [TestFixture]
    public class OrdersControllerTests
    {
        private Mock<IOrderService> _mockOrderService;
        private OrdersController _controller;
        private string _testUserId;

        [SetUp]
        public void Setup()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrdersController(_mockOrderService.Object);
            _testUserId = "test-user-123";

            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, _testUserId) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Test]
        public async Task PlaceOrder_ReturnsOkResult_WithOrderDetails()
        {
            var request = new PlaceOrderRequest("CreditCard");
            var order = new Order { OrderId = 1, UserId = _testUserId, Status = "Pending" };
            _mockOrderService.Setup(s => s.PlaceOrderAsync(_testUserId, "CreditCard")).ReturnsAsync(order);
            _mockOrderService.Setup(s => s.CalculateTotalAsync(1)).ReturnsAsync(100.00m);

            var result = await _controller.Place(request);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.Not.Null);
        }

        [Test]
        public async Task GetOrder_ReturnsOkResult_WhenOrderExists()
        {
            var order = new Order { OrderId = 1, UserId = _testUserId };
            _mockOrderService.Setup(s => s.GetOrderAsync(1)).ReturnsAsync(order);

            var result = await _controller.Get(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetOrder_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            _mockOrderService.Setup(s => s.GetOrderAsync(1)).ReturnsAsync((Order?)null);

            var result = await _controller.Get(1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetMyOrders_ReturnsOkResult_WithOrdersList()
        {
            var orders = new List<Order>
            {
                new Order { OrderId = 1, UserId = _testUserId },
                new Order { OrderId = 2, UserId = _testUserId }
            };
            _mockOrderService.Setup(s => s.GetUserOrdersAsync(_testUserId)).ReturnsAsync(orders);

            var result = await _controller.GetMyOrders();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task PlaceOrder_WithDebitCard_ReturnsOkResult()
        {
            var request = new PlaceOrderRequest("DebitCard");
            var order = new Order { OrderId = 2, UserId = _testUserId, Status = "Pending" };
            _mockOrderService.Setup(s => s.PlaceOrderAsync(_testUserId, "DebitCard")).ReturnsAsync(order);
            _mockOrderService.Setup(s => s.CalculateTotalAsync(2)).ReturnsAsync(250.00m);

            var result = await _controller.Place(request);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task PlaceOrder_WithUPI_ReturnsOkResult()
        {
            var request = new PlaceOrderRequest("UPI");
            var order = new Order { OrderId = 3, UserId = _testUserId, Status = "Pending" };
            _mockOrderService.Setup(s => s.PlaceOrderAsync(_testUserId, "UPI")).ReturnsAsync(order);
            _mockOrderService.Setup(s => s.CalculateTotalAsync(3)).ReturnsAsync(500.00m);

            var result = await _controller.Place(request);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetOrder_VerifiesServiceCalled()
        {
            var order = new Order { OrderId = 5, UserId = _testUserId };
            _mockOrderService.Setup(s => s.GetOrderAsync(5)).ReturnsAsync(order);

            await _controller.Get(5);

            _mockOrderService.Verify(s => s.GetOrderAsync(5), Times.Once);
        }

        [Test]
        public async Task GetMyOrders_ReturnsEmptyList_WhenNoOrders()
        {
            _mockOrderService.Setup(s => s.GetUserOrdersAsync(_testUserId)).ReturnsAsync(new List<Order>());

            var result = await _controller.GetMyOrders();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
