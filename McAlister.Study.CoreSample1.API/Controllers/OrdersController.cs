using AutoMapper;
using McAlister.Study.CoreSample1.Business;
using McAlister.Study.CoreSample1.Definitions;
using McAlister.Study.CoreSample1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using df = McAlister.Study.CoreSample1.Definitions;

namespace McAlister.Study.CoreSample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private Order _order;
        private OrderCtx _orderCtx;
        private ILogger<DebugLoggerProvider> _loggerDebug;

        public OrdersController(IMapper mapper, ILogger<DebugLoggerProvider> logger, Order order)
        {
            _order = order;
            _loggerDebug = logger;
        }

        //public OrdersController(IMapper mapper, ILogger<DebugLoggerProvider> logger, OrderCtx orderCtx)
        //{
        //    _orderCtx = orderCtx;
        //    _loggerDebug = logger;
        //}

        // GET: api/orders/order
        [HttpGet]
        [Route("Order/{orderId}")]
        public APIResponse GetOrder(int orderId)
        {
            HttpStatusCode status = HttpStatusCode.OK;
            df.Models.Order ord = null;
            Exception exForReponse = null;
            try
            {
                ord = _order.GetOrder(orderId);
                if (ord == null)
                {
                    status = HttpStatusCode.NotFound;
                }
                //else if (other conditions)
            }
            catch (Exception ex)
            {
                exForReponse = ex;
                //status = HttpStatusCode.BadRequest; etc
            }
            var res = Utility.CreateAPIResponse(ord, status, _loggerDebug, exForReponse);

            return res;
        }

        // GET: api/orders/Customer
        [HttpGet]
        [Route("Customer/{customerId}/page={page}")]
        public APIResponse GetOrders(int? customerId=null, int? page=null)
        {
            HttpStatusCode status = HttpStatusCode.OK;
            List<df.Models.Order> ords = null;
            Exception exForReponse = null;
            try
            {
                ords = _order.GetOrders(customerId,page);
                //ords = _orderCtx.GetOrders(customerId, page);
                if (ords == null || !ords.Any())
                {
                    status = HttpStatusCode.NotFound;
                }
                //else if (other conditions)
                //status = HttpStatusCode.BadRequest; etc
            }
            catch (Exception ex)
            {
                exForReponse = ex;
            }
            var res = Utility.CreateAPIResponse(ords, status, _loggerDebug, exForReponse);
            return res;
        }

        // POST: api/orders
        [HttpPost]
        public APIResponse Post([FromBody]df.Models.Order value)
        {
            HttpStatusCode status = HttpStatusCode.OK;
            Exception exForReponse = null;
            try
            {
                _order.Insert(value);
                _order.SaveChanges();
            }
            catch (Exception ex)
            {
                exForReponse = ex;
                if (ex.Message.Contains("blah blah"))
                    status = HttpStatusCode.BadRequest;
                else
                    status = HttpStatusCode.InternalServerError;
            }
            var res = Utility.CreateAPIResponse(null, status, _loggerDebug, exForReponse);
            return res;
        }

        [HttpPut]
        // PUT: api/order
        public APIResponse Put([FromBody]df.Models.Order value)
        {
            HttpStatusCode status = HttpStatusCode.OK;
            Exception exForReponse = null;
            try
            {
                _order.Update(value);
                _order.SaveChanges();
            }
            catch (Exception ex)
            {
                exForReponse = ex;
                if (ex.Message.Contains("blah blah"))
                    status = HttpStatusCode.BadRequest;
                else
                    status = HttpStatusCode.InternalServerError;
            }
            var res = Utility.CreateAPIResponse(null, status, _loggerDebug, exForReponse);
            return res;
        }

        // DELETE: api/orders
        [HttpDelete]
        [Route("Order")]
        public APIResponse Delete(int id)
        {
            HttpStatusCode status = HttpStatusCode.OK;
            Exception exForReponse = null;
            try
            {
                _order.Delete(id);
                _order.SaveChanges();
            }
            catch (Exception ex)
            {
                exForReponse = ex;
                if (ex.Message.Contains("blah blah"))
                    status = HttpStatusCode.BadRequest;
                else
                    status = HttpStatusCode.InternalServerError;
            }
            var res = Utility.CreateAPIResponse(null, status, _loggerDebug, exForReponse);
            return res;
        }
        
        //// GET: api/ordersNoEF
        //[HttpGet]
        //[Route("GetOrdersNoEF")]
        //public APIResponse GetOrdersNoEF()
        //{
        //    HttpStatusCode status = HttpStatusCode.OK;
        //    List<df.Models.Order> lst = null;
        //    Exception exForReponse = null;
        //    try
        //    {
        //        lst = _order.GetOrdersNoEF();
        //        if (lst == null || !lst.Any())
        //        {
        //            status = HttpStatusCode.NotFound;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        exForReponse = ex;
        //    }
        //    var res = Utility.CreateAPIResponse(lst, status, _loggerDebug, exForReponse);
        //    return res;
        //}

        //// GET: api/ordersNoEFDT
        //[HttpGet]
        //[Route("GetOrdersNoEFDT")]
        //public APIResponse GetOrdersNoEFDT()
        //{
        //    HttpStatusCode status = HttpStatusCode.OK;
        //    DataTable dt = null;
        //    Exception exForReponse = null;
        //    try
        //    {
        //        dt = _order.GetOrdersNoEFDT();
        //        if (dt == null || dt.Rows.Count == 0)
        //        {
        //            status = HttpStatusCode.NotFound;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        exForReponse = ex;
        //    }
        //    var res = Utility.CreateAPIResponse(dt, status, _loggerDebug, exForReponse);
        //    return res;
        //}
    }
}
