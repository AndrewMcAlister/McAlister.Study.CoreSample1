using McAlister.Study.CoreSample1.Definitions.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using df=McAlister.Study.CoreSample1.Definitions;

namespace McAlister.Study.CoreSample1.Definitions
{
    public interface IOrder
    {
        void Delete(int id);
        df.Entities.Orders FindExact(Orders ord);
        Definitions.Models.Order GetOrder(int orderId);
        List<Definitions.Models.Order> GetOrders(int? customerId, int? page);
        List<Definitions.Models.Order> GetOrdersNoEF();
        DataTable GetOrdersNoEFDT();
        void Insert(Definitions.Models.Order order);
        bool IsValid(Orders entity, ref string msg);
        void Update(Definitions.Models.Order order);
    }
}
