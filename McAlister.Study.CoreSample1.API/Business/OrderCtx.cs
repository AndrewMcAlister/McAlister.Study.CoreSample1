using AutoMapper;
using McAlister.Study.CoreSample1.DAL;
using McAlister.Study.CoreSample1.Definitions;
using McAlister.Study.CoreSample1.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using df = McAlister.Study.CoreSample1.Definitions;

namespace McAlister.Study.CoreSample1.Business
{
    /// <summary>
    /// Application model saves and retrieves data - gateway to business libraries
    /// </summary>
    public class OrderCtx : BusinessBaseCtx<df.Entities.Orders>, IOrderCtx
    {
        private IMapper _mapper;
        private int _pageSize = 50;

        public OrderCtx(WideWorldImportersContext repo, IMapper mapper) : base(repo)
        {
            _mapper = mapper;
        }

        public df.Models.Order GetOrder(int orderId)
        {
            df.Models.Order m = null;
            try
            {
                var e = Get(o => o.OrderId == orderId);
                if (e != null)
                    m = _mapper.Map<df.Entities.Orders, df.Models.Order>(e);
            }
            catch (Exception ex)
            {

            }
            return m;
        }

        public List<df.Models.Order> GetOrders(int? customerId, int? page = 1)
        {
            var lstModel = new List<df.Models.Order>();
            try
            {
                //var lstEntity = base.GetList(o => o.CustomerId == customerId || !customerId.HasValue);
                var lstEntity = GetList(o => o.CustomerId == customerId || !customerId.HasValue,orderBy: (p=>p.OrderDate),true, _pageSize,page.Value);
                lstModel = _mapper.Map<List<df.Entities.Orders>, List<df.Models.Order>>(lstEntity);
            }
            catch (Exception ex)
            {

            }
            return lstModel;
        }

        public List<df.Models.Order> GetOrders2(int? customerId, int? page = 1)
        {
            var lstModel = new List<df.Models.Order>();
            try
            {
                var lstEntity = Repo.Orders.Where(p => (!customerId.HasValue || p.CustomerId == customerId.Value))
                    .OrderByDescending(p => p.OrderDate).Skip(_pageSize * (page.Value - 1)).Take(_pageSize).ToList();
                lstModel = _mapper.Map<List<df.Entities.Orders>, List<df.Models.Order>>(lstEntity);
            }
            catch (Exception ex)
            {

            }
            return lstModel;
        }

        public void Insert(df.Models.Order order)
        {
            var eOrder = _mapper.Map<df.Models.Order, df.Entities.Orders>(order);
            base.Insert(eOrder);
        }

        public void Update(df.Models.Order order)
        {
            var eOrder = _mapper.Map<df.Models.Order, df.Entities.Orders>(order);
            base.Update(eOrder);
        }

        public override bool IsValid(df.Entities.Orders entity, ref string msg)
        {
            Boolean valid = true;
            if (entity.OrderLines == null || !entity.OrderLines.Any())
            {
                msg += $"No orderlines {Environment.NewLine}";
                valid = false;
            }

            return valid;
        }

        public void Delete(int id)
        {
            var ent = Get(p => p.OrderId == id);
            if (ent != null) //as its a delete, no error id doesn't exist
                base.Delete(ent);
        }

        public override df.Entities.Orders FindExact(df.Entities.Orders ord)
        {
            //intended for use internally by ModelBase
            //entity is used as a search object
            var ent = Get(p => p.OrderId == ord.OrderId);
            return ent;
        }

        public List<df.Models.Order> GetOrdersNoEF()
        {
            throw new NotImplementedException();
        }

        public DataTable GetOrdersNoEFDT()
        {
            throw new NotImplementedException();
        }
    }
}
 