using System;
using System.Configuration;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace SecretSanta.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Next()
        {
            string cnnString = ConfigurationManager.ConnectionStrings["InfocenterConnectionString"].ConnectionString;
            Person person = new Person();
            if (!String.IsNullOrWhiteSpace(cnnString))
            {
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cnnString))
                {
                    connection.Open();

                    var dbcommand = new System.Data.SqlClient.SqlCommand("SELECT TOP 1 * FROM SecretSanta WHERE ReceivedAGift = 0 ORDER BY NEWID()", connection);

                    var dr = dbcommand.ExecuteReader();
                    dr.Read();

                    person.FullName = dr["DisplayName"].ToString();
                    person.Department = dr["Department"].ToString();
                    person.EmployeeId = dr["EmployeeID"].ToString();

                    dr.Close();
                    connection.Close();
                }
            }
            return Json(new
                        {
                            person.FullName,
                            person.Department,
                            person.EmployeeId
                        }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Received(string employeeId)
        {
            try
            {
                string cnnString = ConfigurationManager.ConnectionStrings["InfocenterConnectionString"].ConnectionString;
                Person person = new Person();
                if (!String.IsNullOrWhiteSpace(cnnString))
                {
                    using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cnnString))
                    {
                        connection.Open();

                        var dbcommand = new System.Data.SqlClient.SqlCommand($"UPDATE SecretSanta SET ReceivedAGift = 1 WHERE EmployeeID = '{employeeId}'", connection);

                        dbcommand.ExecuteNonQuery();

                        connection.Close();
                    }
                }
                return Json(new {Error = false}, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Error = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [Serializable]
        [DataContract(IsReference = true)]
        public class Person
        {
            [DataMember]
            public string FullName { get; set; }

            public string Department { get; set; }

            public string EmployeeId { get; set; }
        }
    }
}