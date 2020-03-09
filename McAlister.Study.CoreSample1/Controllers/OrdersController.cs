using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using McAlister.Study.CoreSample1.Models;
using df = McAlister.Study.CoreSample1.Definitions;
using AutoMapper;
using System.Net;
using McAlister.Study.CoreSample1.Definitions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace McAlister.Study.CoreSample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private Order _order;
        private ILogger<DebugLoggerProvider> _loggerDebug;

        public OrdersController(df.IRepository repo, IMapper mapper, ILogger<DebugLoggerProvider> logger)
        {
            _order = new Order(repo, mapper);
            _loggerDebug = logger;
        }

        // GET: api/orders/order
        [HttpGet]
        [Route("GetOrder/{orderId}")]
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

        // GET: api/orders/order
        [HttpGet]
        [Route("GetOrders/{customerId}")]
        public APIResponse GetOrders(int? customerId)
        {
            HttpStatusCode status = HttpStatusCode.OK;
            List<df.Models.Order> ords = null;
            Exception exForReponse = null;
            try
            {
                ords = _order.GetOrders(customerId);
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

        // GET: api/ordersNoEF
        [HttpGet]
        [Route("GetOrdersNoEF")]
        public APIResponse GetOrdersNoEF()
        {
            HttpStatusCode status = HttpStatusCode.OK;
            List<df.Models.Order> lst = null;
            Exception exForReponse = null;
            try
            {
                lst = _order.GetOrdersNoEF();
                if (lst == null || !lst.Any())
                {
                    status = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                exForReponse = ex;
            }
            var res = Utility.CreateAPIResponse(lst, status, _loggerDebug, exForReponse);
            return res;
        }

        // GET: api/ordersNoEFDT
        [HttpGet]
        [Route("GetOrdersNoEFDT")]
        public APIResponse GetOrdersNoEFDT()
        {
            HttpStatusCode status = HttpStatusCode.OK;
            DataTable dt = null;
            Exception exForReponse = null;
            try
            {
                dt = _order.GetOrdersNoEFDT();
                if (dt == null || dt.Rows.Count == 0)
                {
                    status = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                exForReponse = ex;
            }
            var res = Utility.CreateAPIResponse(dt, status, _loggerDebug, exForReponse);
            return res;
        }

        // POST: api/order
        [HttpPost]
        [Route("New")]
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
        [Route("Update")]
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
        [Route("Delete")]
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
    }
}
