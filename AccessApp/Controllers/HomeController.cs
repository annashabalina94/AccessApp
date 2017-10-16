using AccessApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Antlr.Runtime.Tree.TreeWizard;

namespace AccessApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Конектимся к базе
            SqlConnection conn = new SqlConnection();
            SqlConnection conn2 = new SqlConnection();
            conn.ConnectionString = "Server=DESKTOP-QPDIULV;Database=UProxIP_01;Trusted_Connection=true";
            conn.Open();
            

            
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("select ObjectId, DateTime, CardCode, CardHolderName, Comment from Visitors;", conn);
                //SqlCommand myCommand = new SqlCommand("SELECT t1.*, t2.CardHolderName FROM  UProxIP_01.dbo.HardwareEvent AS t1 JOIN UProxIP.dbo.CardIssue AS t2 ON t1.CardCode = t2.CardCode; ");

               SqlDependency dependency = new SqlDependency(myCommand);
               SqlDependency.Start(conn.ConnectionString);
               dependency.OnChange += new OnChangeEventHandler(OnDependencyChange);

                myReader = myCommand.ExecuteReader();

                Visitors visitor = new Visitors();
                List<Visitors> VisitorsList = new List<Visitors>();

                while (myReader.Read())
                {
                    //Console.Write(" {0} ", myReader["CardCode"].ToString());
                    //Необходимо получить список из базы UProxIP_01 из ее таблицы HardwareEvent и ее столбцов [ObjectId] ,[DateTime],[Comment] ,[CardCode] ,?[CardHolder],[Name]
                    visitor = new Visitors();
                    visitor.Id = myReader["ObjectId"].ToString();
                    visitor.DateTime = myReader["DateTime"].ToString();// ПОМЕНЯТЬ НА DATETIME
                    visitor.Card = myReader["CardCode"].ToString();
                    visitor.CardHolderName = myReader["CardHolderName"].ToString();
                    visitor.Message = myReader["Comment"].ToString();
                    VisitorsList.Add(visitor);
                }

             void OnDependencyChange(object sender, SqlNotificationEventArgs e)
            {
                // Handle the event (for example, invalidate this cache entry).  
                if (sender != null)
                {
                    sender = e;
                }
            }

            var data = VisitorsList;
            return View(data);
        }
   
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}