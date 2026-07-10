using EshoppingZone.Controllers;
using EshoppingZone.Dtos;
using EshoppingZone.Models;
using EshoppingZone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EshoppingZone.Tests
{
    [TestFixture]
    public class PaymentsControllerTests
    {
        private Mock<IPaymentService> _mockPaymentService;
        private Mock<IOrderService> _mockOrderService;
        private PaymentsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockPaymentService = new Mock<IPaymentService>();
            _mockOrderService = new Mock<IOrderService>();
            _controller = new PaymentsController(_mockPaymentService.Object, _mockOrderService.Object);
        }

        [Test]
        public async Task MakePayment_ReturnsOkResult_WithPaymentDetails()
        {
            var request = new MakePaymentRequest(1, "CreditCard", 100.00m, null);
            var payment = new Payment
            {
                PaymentId = 1,
                OrderId = 1,
                PaymentMode = "CreditCard",
                Transactions = new List<PaymentTransaction>
                {
                    new PaymentTransaction { PaymentTransactionId = 1, Amount = 100.00m }
                }
            };
            _mockPaymentService.Setup(s => s.MakePaymentAsync(1, "CreditCard", 100.00m, null)).ReturnsAsync(payment);
            _mockOrderService.Setup(s => s.CalculateTotalAsync(1)).ReturnsAsync(100.00m);

            var result = await _controller.MakePayment(request);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.Not.Null);
        }

        [Test]
        public async Task MakePayment_WithWallet_ReturnsOkResult()
        {
            var request = new MakePaymentRequest(1, "Wallet", 50.00m, 1);
            var payment = new Payment
            {
                PaymentId = 2,
                OrderId = 1,
                PaymentMode = "Wallet",
                Transactions = new List<PaymentTransaction>
                {
                    new PaymentTransaction { PaymentTransactionId = 2, Amount = 50.00m }
                }
            };
            _mockPaymentService.Setup(s => s.MakePaymentAsync(1, "Wallet", 50.00m, 1)).ReturnsAsync(payment);
            _mockOrderService.Setup(s => s.CalculateTotalAsync(1)).ReturnsAsync(50.00m);

            var result = await _controller.MakePayment(request);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task MakePayment_VerifiesPaymentServiceCalled()
        {
            var request = new MakePaymentRequest(1, "UPI", 75.00m, null);
            var payment = new Payment { PaymentId = 3, OrderId = 1, PaymentMode = "UPI" };
            _mockPaymentService.Setup(s => s.MakePaymentAsync(1, "UPI", 75.00m, null)).ReturnsAsync(payment);
            _mockOrderService.Setup(s => s.CalculateTotalAsync(1)).ReturnsAsync(75.00m);

            await _controller.MakePayment(request);

            _mockPaymentService.Verify(s => s.MakePaymentAsync(1, "UPI", 75.00m, null), Times.Once);
        }

        [Test]
        public async Task MakePayment_WithDebitCard_ReturnsOkResult()
        {
            var request = new MakePaymentRequest(2, "DebitCard", 200.00m, null);
            var payment = new Payment { PaymentId = 4, OrderId = 2, PaymentMode = "DebitCard" };
            _mockPaymentService.Setup(s => s.MakePaymentAsync(2, "DebitCard", 200.00m, null)).ReturnsAsync(payment);
            _mockOrderService.Setup(s => s.CalculateTotalAsync(2)).ReturnsAsync(200.00m);

            var result = await _controller.MakePayment(request);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task MakePayment_VerifiesOrderServiceCalled()
        {
            var request = new MakePaymentRequest(3, "CreditCard", 150.00m, null);
            var payment = new Payment { PaymentId = 5, OrderId = 3, PaymentMode = "CreditCard" };
            _mockPaymentService.Setup(s => s.MakePaymentAsync(3, "CreditCard", 150.00m, null)).ReturnsAsync(payment);
            _mockOrderService.Setup(s => s.CalculateTotalAsync(3)).ReturnsAsync(150.00m);

            await _controller.MakePayment(request);

            _mockOrderService.Verify(s => s.CalculateTotalAsync(3), Times.Once);
        }
    }
}
