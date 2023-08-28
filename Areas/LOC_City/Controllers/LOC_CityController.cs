using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using StartAdminPanel.Areas.LOC_City.Models;

namespace StartAdminPanel.Areas.LOC_City.Controllers
{
    [Area("LOC_City")]
    [Route("LOC_City/[Controller]/[action]")]
    public class LOC_CityController : Controller
    {

        private IConfiguration Configuration;
        public LOC_CityController(IConfiguration _configuration)
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
            cmd.CommandText = "PR_City_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader objSDR = cmd.ExecuteReader();
            dt.Load(objSDR);
            conn.Close();
            return View("LOC_CityList", dt);
        }

        #endregion


        #region Delete
        public IActionResult Delete(int CityID)
        {
            string str = Configuration.GetConnectionString("defautString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_delete";
            cmd.Parameters.AddWithValue("@CityID", CityID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }

        #endregion


        #region Add/Edit 
        public IActionResult Add(int? CityID)
        {
            if (CityID != null)
            {
                //prepare connection
                string str = Configuration.GetConnectionString("defautString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();

                //prepare Command
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_City_SelectByPK";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = cmd.ExecuteReader();
                dt.Load(objSDR);
                LOC_CityModel modelLOC_City = new LOC_CityModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelLOC_City.CityID = Convert.ToInt32(dr["CityID"]);
                    modelLOC_City.CityName = dr["CityName"].ToString();
                    modelLOC_City.Citycode = dr["CityCode"].ToString();
                    modelLOC_City.StateID = Convert.ToInt32(dr["StateID"]);
                    modelLOC_City.CountryID = Convert.ToInt32(dr["CountryID"]);

                    //modelLOC_City.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    //modelLOC_City.Modified = Convert.ToDateTime(dr["Modified"]);
                }
                return View("LOC_CityAddEdit", modelLOC_City);
            }

            return View("LOC_CityAddEdit");
        }
        #endregion


        #region Save
        [HttpPost]
        public IActionResult Save(LOC_CityModel modelLOC_City)
        {
            string str = Configuration.GetConnectionString("defautString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (modelLOC_City.CityID == 0)
            {
                cmd.CommandText = "PR_City_Insert";
            }
            else
            {
                cmd.CommandText = "PR_City_Update";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = modelLOC_City.CityID;
            }
            cmd.Parameters.Add("@CityName", SqlDbType.VarChar).Value = modelLOC_City.CityName;
            cmd.Parameters.Add("@Citycode", SqlDbType.VarChar).Value = modelLOC_City.Citycode;
            cmd.Parameters.Add("@CountryID", SqlDbType.VarChar).Value = modelLOC_City.CountryID;
            cmd.Parameters.Add("@StateID", SqlDbType.VarChar).Value = modelLOC_City.StateID;
            //cmd.Parameters.Add("@Created", SqlDbType.Date).Value = modelLOC_Country.Created;
            //cmd.Parameters.Add("@Modified", SqlDbType.Date).Value = modelLOC_Country.Modified;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_City.CityID == 0)
                {
                    TempData["CountryInsertString"] = "Record inserted successfully";
                }
                else
                {
                    TempData["CountryInsertString"] = "Record Updated successsfully";
                }
            }
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
