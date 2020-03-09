using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using df=McAlister.Study.CoreSample1.Definitions;
using System.Net;
using Microsoft.Extensions.Logging;

namespace McAlister.Study.CoreSample1
{
    public static class Utility
    {
        #region DataTables to Objects

        /// <summary>
        /// Converts a datatable into a list of any given type.
        /// Warning: May not work for all property types.  Test it!
        /// Also, slow with large datasets
        /// </summary>
        public static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }


        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {

                    if (String.Compare(pro.Name, column.ColumnName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        if (pro.PropertyType.Name == "Boolean" || pro.PropertyType.UnderlyingSystemType.FullName.Contains("Boolean"))
                        {
                            var res = false;
                            switch (dr[column.ColumnName].ToString())
                            {
                                case "1":
                                    res = true;
                                    break;
                                case "True":
                                    res = true;
                                    break;
                                case "true":
                                    res = true;
                                    break;
                                case "Y":
                                    res = true;
                                    break;
                                case "y":
                                    res = true;
                                    break;
                            }
                            pro.SetValue(obj, res, null);
                            break;
                        }
                        else if ((pro.PropertyType.Name == "Int" || pro.PropertyType.UnderlyingSystemType.FullName.Contains("Int32")) && !pro.PropertyType.UnderlyingSystemType.FullName.ToLower().Contains("uint32"))
                        {
                            Nullable<System.Int32> i = ConvertToStandardIntOrNull(dr[column.ColumnName]);
                            pro.SetValue(obj, i, null);
                        }
                        else if ((pro.PropertyType.Name == "long" || pro.PropertyType.UnderlyingSystemType.FullName.Contains("Int64")) && !pro.PropertyType.UnderlyingSystemType.FullName.ToLower().Contains("uint64"))
                        {
                            Nullable<System.Int64> i = ConvertToInt64OrNull(dr[column.ColumnName]);
                            pro.SetValue(obj, i, null);
                        }
                        else
                        {
                            if (dr[column.ColumnName] == System.DBNull.Value)
                                pro.SetValue(obj, null, null);
                            else
                                pro.SetValue(obj, dr[column.ColumnName], null);

                            break;
                        }
                    }
                    else
                        continue;

                }
            }
            return obj;
        }

        public static int? ConvertToStandardIntOrNull(Object obj)
        {
            if (obj != null && obj.ToString() != String.Empty)
                return Convert.ToInt32(obj.ToString());
            else
                return null;
        }

        public static long? ConvertToInt64OrNull(Object obj)
        {
            if (obj != null && obj.ToString() != String.Empty)
                return Convert.ToInt64(obj.ToString());
            else
                return null;
        }

        #endregion

        public static df.APIResponse CreateAPIResponse(Object payload, HttpStatusCode code, ILogger log, Exception ex = null)
        {
            var ar = new df.APIResponse();
            ar.Successful = false;
            ar.Status = (int)code;
            if (ex == null)
            {
                ar.Payload = payload;
                ar.Successful = true;
            }
            else
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += $" InnerExcepion: {ex.InnerException.Message}";
                log.LogError(ex,msg);
                ar.Message = msg;
            }

            return ar;
        }

    }
}
