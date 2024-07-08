using CSI.Unicard.Application.Interfaces;
using CSI.Unicard.Application.response;
using CSI.Unicard.Application.StaticFiles;
using CSI.Unicard.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSI.Unicard.API.Controllers
{
    /// <summary>
    /// Controller for managing orders.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">The service for managing orders.</param>
        /// <param name="logger">The logger instance.</param>
        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new order.
        /// </summary>
        /// <param name="orders">The order to add.</param>
        /// <returns>A task representing the asynchronous operation, with an IActionResult containing the result.</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Add([FromBody] Orders orders)
        {
            _logger.LogInformation("Adding order: {@Order}", orders);
            try
            {
                var result = await _orderService.Add(orders);
                _logger.LogInformation("Order added successfully with result: {Result}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding order.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>A task representing the asynchronous operation, with a Response containing the order.</returns>
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<Response<Orders>> GetById([FromRoute] int id)
        {
            _logger.LogInformation("Getting order by id: {Id}", id);
            try
            {
                var order = await _orderService.GetById(id);
                if (order == null)
                {
                    _logger.LogWarning("Order with id {Id} not found", id);
                    return Response<Orders>.Error($"Order with id {id} not found");
                }
                _logger.LogInformation("Order retrieved: {@Order}", order);
                return Response<Orders>.Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving order.");
                return Response<Orders>.Error(ErrorKeys.InternalServerError);
            }
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a Response containing a list of orders.</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<Orders>>> GetAll()
        {
            var res = await _orderService.GetAll();
            if (!res.Any()) throw new InvalidOperationException();
            return Response<IEnumerable<Orders>>.Ok(res);
        }

        /// <summary>
        /// Deletes an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [HttpDelete]
        [Route("[action]")]
        public async Task DeleteById(int id)
        {
            await _orderService.DeleteById(id);
        }
    }
}
