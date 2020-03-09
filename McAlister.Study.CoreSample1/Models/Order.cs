using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Data;
using AutoMapper;
using df = McAlister.Study.CoreSample1.Definitions;

namespace McAlister.Study.CoreSample1.Models
{
    /// <summary>
    /// Application model saves and retrieves data - gateway to business libraries
    /// </summary>
    public class Order : ModelBase<df.Entities.Orders>
    {
        private IMapper _mapper;

        public Order(df.IRepository repo, IMapper mapper): base(repo)
        {
            _mapper = mapper;
        }

        public List<df.Models.Order> GetOrdersNoEF()
        {
            var lstModel = new List<df.Models.Order>();
            try
            {
                var lstEntity = base.Repo.GetOrdersNoEF().ToList();
                lstModel = _mapper.Map<List<df.Entities.Orders>, List<df.Models.Order>>(lstEntity);
            }
            catch (Exception ex)
            {

            }
            return lstModel;
        }

        public DataTable GetOrdersNoEFDT()
        {
            return Repo.GetOrdersNoEFDT();
        }

        public df.Models.Order GetOrder(int orderId)
        {
            df.Models.Order m=null;
            try
            {
                var e = base.Get(o=>o.OrderId==orderId);
                if(e!=null)
                    m = _mapper.Map<df.Entities.Orders, df.Models.Order>(e);
            }
            catch (Exception ex)
            {

            }
            return m;
        }

        public List<df.Models.Order> GetOrders(int? customerId)
        {
            var lstModel = new List<df.Models.Order>();
            try
            {
                var lstEntity = base.GetList(o=>o.CustomerId== customerId || !customerId.HasValue);
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

        public override bool IsValid(df.Entities.Orders entity,ref string msg)
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
            var ent = base.Get(p => p.OrderId == id);
            if (ent != null) //as its a delete, no error id doesn't exist
                base.Delete(ent);
        }

        public override df.Entities.Orders FindExact(df.Entities.Orders ord)
        {     
            //intended for use internally by ModelBase
            //entity is used as a search object
            var ent = base.Get(p => p.OrderId == ord.OrderId);
            return ent;
        }

    }
}
 