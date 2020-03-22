using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace McAlister.Study.CoreSample1.Tests.Controllers
{
    [TestClass]
    public class OrderControllerTests
    {
        public OrderControllerTests()
        {

        }

        //Conclusion: Repository pattern is no slower than direct DBContext, probably thanks to the compiler.
        [TestMethod]
        public void GetOrdersSpeedTest()
        {
            for (int j = 1; j < 8; j++) //8 reps is about right for a 16Gb computer, anymore and tests tend to fail
            {
                for (int i = 1; i < 1062; i++)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://localhost/api/Orders/Customer/{i}/page=1");
                    var r = (HttpWebResponse)request.GetResponse();
                    Stream receiveStream = r.GetResponseStream();
                    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    using (StreamReader sr = new StreamReader(receiveStream, encode))
                    {
                        String str = sr.ReadToEnd();
                        Console.Write(str);
                    }
                }
            }
        }

        //[TestMethod]
        //public void GetApplicationsNoEFSpeedTest()
        //{
        //    var oc = new OrdersController(_Repo, _Mapper, _loggerDebug);
        //    var obj = oc.GetOrdersNoEF();
        //    var lst = (List<df.Models.Order>)obj.Payload;
        //    Assert.IsTrue(lst.Count > 30);
        //}

        //[TestMethod]
        //public void GetApplicationsNoEFDTSpeedTest()
        //{
        //    var oc = new OrdersController(_Repo, _Mapper, _loggerDebug);
        //    var obj = oc.GetOrdersNoEFDT();
        //    var dt = (DataTable)obj.Payload;
        //    Assert.IsTrue(dt.Rows.Count > 30);
        //}

        //[TestMethod]
        //public void ModifyApplication()
        //{
        //    var ac = new OrdersController();
        //    var appl = ac.GetApplication("46115235D");
        //    if (appl == null)
        //    {
        //        var json = GetJsonFile("NewApplication46115235D");
        //        //ac.Post(json);
        //        appl = ac.GetApplication("46115235D");
        //    }
        //    if (appl != null)
        //    {
        //        var json = GetJsonFile("Application46115235DUpdate");
        //        var ap = JsonConvert.DeserializeObject<df.Models.vis_application>(json);
        //        ac.Put(ap); //7 sec
        //        var r = ac.GetApplication("46115235D");
        //        var modAppl = (df.Models.vis_application)r.Content.Payload;
        //        Assert.IsTrue(modAppl.applicant_type_code == "F" && modAppl.state_code == "VIC");
        //    }
        //    else
        //        Assert.Fail("This test requires the application to pre-exist");
        //}

        //[TestMethod]
        //public void InsertNewApplication()
        //{
        //    //Prepare DB by deleting applicant
        //    var appNbr = "46115235D";
        //    var sql = $"DELETE FROM vis_addresses WHERE applicant_nbr='{appNbr}';" +
        //        $"DELETE FROM vis_applicant_history WHERE applicant_nbr='{appNbr}';" +
        //        $"DELETE FROM vis_person_details WHERE applicant_nbr='{appNbr}';" +
        //        $"DELETE FROM rd_app_email WHERE vtac_id='{appNbr}';" +
        //        $"DELETE FROM rd_sec_answer WHERE vtac_id='{appNbr}';" +
        //        $"DELETE FROM vis_systemtask_history WHERE applicant_nbr='{appNbr}';" +
        //        $"DELETE FROM vis_applications WHERE applicant_nbr='{appNbr}';";
        //    MySqlDB.ExecuteQuery(sql);
        //    var json = GetJsonFile("NewApplication46115235D");
        //    var ap = JsonConvert.DeserializeObject<df.Models.vis_application>(json);

        //    var ac = new ApplicationController();
        //    ac.Post(ap); //7 sec
        //    var r = ac.GetApplication("46115235D");
        //    var appl = (df.Models.vis_application)r.Content.Payload;
        //    Assert.IsTrue(appl.applicant_nbr == "46115235D");
        //}

    }
}