using CSI.Unicard.Application.Interfaces;
using CSI.Unicard.Application.response;
using CSI.Unicard.Application.StaticFiles;
using CSI.Unicard.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSI.Unicard.API.Controllers
{
    /// <summary>
    /// Controller for managing order items.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService orderItemService;
        private readonly ILogger<OrderItemController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemController"/> class.
        /// </summary>
        /// <param name="orderItemService">The service for managing order items.</param>
        /// <param name="logger">The logger instance.</param>
        public OrderItemController(IOrderItemService orderItemService, ILogger<OrderItemController> logger)
        {
            this.orderItemService = orderItemService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new order item.
        /// </summary>
        /// <param name="orderItem">The order item to add.</param>
        /// <returns>A task representing the asynchronous operation, with an IActionResult containing the result.</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Add([FromBody] OrderItems orderItem)
        {
            _logger.LogInformation("Adding orderItem: {@orderItem}", orderItem);
            try
            {
                var result = await orderItemService.Add(orderItem);
                _logger.LogInformation("orderItem added successfully with result: {Result}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding orderItem.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets an order item by its ID.
        /// </summary>
        /// <param name="id">The ID of the order item.</param>
        /// <returns>A task representing the asynchronous operation, with a Response containing the order item.</returns>
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<Response<OrderItems>> GetById([FromRoute] int id)
        {
            _logger.LogInformation("Getting OrderItems by id: {Id}", id);
            try
            {
                var orderItem = await orderItemService.GetById(id);
                if (orderItem == null)
                {
                    _logger.LogWarning("OrderItems with id {Id} not found", id);
                    return Response<OrderItems>.Error($"OrderItems with id {id} not found");
                }
                _logger.LogInformation("orderItem retrieved: {@orderItem}", orderItem);
                return Response<OrderItems>.Ok(orderItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving OrderItems.");
                return Response<OrderItems>.Error(ErrorKeys.InternalServerError);
            }
        }

        /// <summary>
        /// Gets all order items.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a Response containing a list of order items.</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<OrderItems>>> GetAll()
        {
            var res = await orderItemService.GetAll();
            if (!res.Any()) throw new InvalidOperationException();
            return Response<IEnumerable<OrderItems>>.Ok(res);
        }

        /// <summary>
        /// Deletes an order item by its ID.
        /// </summary>
        /// <param name="id">The ID of the order item to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [HttpDelete]
        [Route("[action]")]
        public async Task DeleteById(int id)
        {
            await orderItemService.DeleteById(id);
        }
    }
}
