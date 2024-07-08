using CSI.Unicard.API.Controllers;
using CSI.Unicard.Application.Interfaces;
using CSI.Unicard.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CSI.Unicard.APITests
{
    public class OrderItemControllerTest
    {
        private readonly Mock<IOrderItemService> _mockOrderItemService;
        private readonly Mock<ILogger<OrderItemController>> _mockLogger;
        private readonly OrderItemController _orderItemController;

        public OrderItemControllerTest()
        {
            _mockOrderItemService = new Mock<IOrderItemService>();
            _mockLogger = new Mock<ILogger<OrderItemController>>();
            _orderItemController = new OrderItemController(_mockOrderItemService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Add_OrderItemServiceThrowsException_ReturnsInternalServerError()
        {
            var orderItem = new OrderItems { OrderId=1,OrderItemId=1,price=123,ProductId=1,Quantity=12 };
            _mockOrderItemService.Setup(service => service.Add(orderItem)).ThrowsAsync(new Exception());

            var actionResult = await _orderItemController.Add(orderItem);

            var result = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsOrderItem()
        {
            int id = 1;
            var orderItem = new OrderItems { OrderId = 1, OrderItemId = 1, price = 123, ProductId = 1, Quantity = 12 };
            _mockOrderItemService.Setup(service => service.GetById(id)).ReturnsAsync(orderItem);

            var response = await _orderItemController.GetById(id);

            Assert.True(response.Succeeded);
            Assert.Equal(orderItem, response.Data);
        }

        [Fact]
        public async Task GetAll_ReturnsAllOrderItems()
        {
            var orderItems = new List<OrderItems> {
                  new OrderItems { OrderId = 1, OrderItemId = 1, price = 123, ProductId = 1, Quantity = 12 },
                new OrderItems { OrderId = 2, OrderItemId = 2, price = 43345, ProductId = 2, Quantity = 1 },
              new OrderItems { OrderId = 3, OrderItemId = 1, price = 32, ProductId = 4, Quantity = 32 }
            };
            _mockOrderItemService.Setup(service => service.GetAll()).ReturnsAsync(orderItems);

            var response = await _orderItemController.GetAll();

            Assert.True(response.Succeeded);
            Assert.Equal(orderItems, response.Data);
        }

        [Fact]
        public async Task DeleteById_ValidId_CallsService()
        {
            int id = 1;
            _mockOrderItemService.Setup(service => service.DeleteById(id)).Returns(Task.CompletedTask);

            await _orderItemController.DeleteById(id);

            _mockOrderItemService.Verify(service => service.DeleteById(id), Times.Once);
        }
    }
}
