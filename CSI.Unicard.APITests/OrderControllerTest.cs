using CSI.Unicard.API.Controllers;
using CSI.Unicard.Application.Interfaces;
using CSI.Unicard.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CSI.Unicard.APITests
{
    public class OrderControllerTest
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<ILogger<OrderController>> _mockLogger;
        private readonly OrderController _orderController;

        public OrderControllerTest()
        {
            _mockOrderService = new Mock<IOrderService>();
            _mockLogger = new Mock<ILogger<OrderController>>();
            _orderController = new OrderController(_mockOrderService.Object, _mockLogger.Object);
        }


        [Fact]
        public async Task Add_OrderServiceThrowsException_ReturnsInternalServerError()
        {
            var order = new Orders { OrderDate=DateTime.Now,OrderId=1,UserId=1,TotalAmount=34 };
            _mockOrderService.Setup(service => service.Add(order)).ThrowsAsync(new Exception());

            var actionResult = await _orderController.Add(order);

            var result = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsOrder()
        {
            int id = 1;
            var order = new Orders { OrderDate = DateTime.Now, OrderId = 1, UserId = 1, TotalAmount = 34 };
            _mockOrderService.Setup(service => service.GetById(id)).ReturnsAsync(order);

            var response = await _orderController.GetById(id);

            Assert.True(response.Succeeded);
            Assert.Equal(order, response.Data);
        }


        [Fact]
        public async Task GetAll_ReturnsAllOrders()
        {
            var orders = new List<Orders> {
            new Orders { OrderDate = DateTime.Now, OrderId = 1, UserId = 1, TotalAmount = 34 },
            new Orders { OrderDate = DateTime.Now, OrderId = 2, UserId = 2, TotalAmount = 435 },
            new Orders { OrderDate = DateTime.Now, OrderId = 3, UserId = 3, TotalAmount = 4324 },
            new Orders { OrderDate = DateTime.Now, OrderId = 4, UserId = 4, TotalAmount = 43 },
            new Orders { OrderDate = DateTime.Now, OrderId = 5, UserId = 5, TotalAmount = 12 },
            new Orders { OrderDate = DateTime.Now, OrderId = 6, UserId = 6, TotalAmount = 43454 }
            };
            _mockOrderService.Setup(service => service.GetAll()).ReturnsAsync(orders);

            var response = await _orderController.GetAll();

            Assert.True(response.Succeeded);
            Assert.Equal(orders, response.Data);
        }

        [Fact]
        public async Task DeleteById_ValidId_CallsService()
        {
            int id = 1;
            _mockOrderService.Setup(service => service.DeleteById(id)).Returns(Task.CompletedTask);

            await _orderController.DeleteById(id);

            _mockOrderService.Verify(service => service.DeleteById(id), Times.Once);
        }
    }
}
