using Microsoft.AspNetCore.Mvc;
using StartAdminPanel.Areas.LOC_Country.Models;
using System.Data;
using System.Data.SqlClient;

namespace StartAdminPanel.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("LOC_Country/[Controller]/[action]")]
    public class LOC_CountryController : Controller
    {
        private IConfiguration Configuration;
        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult Index()
        {
            string str = Configuration.GetConnectionString("defautString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Country_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader objSDR = cmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_CountryList", dt);
        }
        #endregion


        #region delete
        public IActionResult Delete(int CountryId)
        {
            string str = Configuration.GetConnectionString("defautString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Country_delete";
            cmd.Parameters.AddWithValue("@CountryID", CountryId);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }

        #endregion


        #region Add/Edit 
        public IActionResult Add(int? CountryID)
        {
            if (CountryID != null)
            {
                //prepare connection
                string str = Configuration.GetConnectionString("defautString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();

                //prepare Command
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Country_SelectByPK";
                cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = cmd.ExecuteReader();
                dt.Load(objSDR);
                LOC_CountryModel modelLOC_Country = new LOC_CountryModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelLOC_Country.CountryId = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_Country.CountryName = dr["CountryName"].ToString();
                    modelLOC_Country.CountryCode = dr["CountryCode"].ToString();
                    modelLOC_Country.Created = Convert.ToDateTime(dr["Created"]);
                    modelLOC_Country.Modified = Convert.ToDateTime(dr["Modified"]);
                }
                return View("LOC_CountryAddEdit", modelLOC_Country);
            }

            return View("LOC_CountryAddEdit");
        }


        #endregion


        #region Save
        [HttpPost]
        public IActionResult Save(LOC_CountryModel modelLOC_Country)
        {
            string str = Configuration.GetConnectionString("defautString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (modelLOC_Country.CountryId == 0)
            {
                cmd.CommandText = "PR_Country_Insert";
            }
            else
            {
                cmd.CommandText = "PR_Country_Update";
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = modelLOC_Country.CountryId;
            }
            cmd.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = modelLOC_Country.CountryName;
            cmd.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = modelLOC_Country.CountryCode;
            //cmd.Parameters.Add("@Created", SqlDbType.Date).Value = modelLOC_Country.Created;
            //cmd.Parameters.Add("@Modified", SqlDbType.Date).Value = modelLOC_Country.Modified;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_Country.CountryId == 0)
                {
                    TempData["CountryInsertString"] = "Record inserted successfully";
                }
                else
                {
                    TempData["CountryInsertString"] = "Record Updated successsfully";
                }
            }
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
