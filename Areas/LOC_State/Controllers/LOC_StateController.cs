using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using StartAdminPanel.Areas.LOC_State.Models;

namespace StartAdminPanel.Areas.LOC_State.Controllers
{
    [Area("LOC_State")]
    [Route("LOC_State/[Controller]/[action]")]
    public class LOC_StateController : Controller
    {
        private IConfiguration Configuration;
        public LOC_StateController(IConfiguration _configuration)
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
            cmd.CommandText = "PR_STATE_SelectALL";
            DataTable dt = new DataTable();
            SqlDataReader objSDR = cmd.ExecuteReader();
            dt.Load(objSDR);
            conn.Close();
            return View("LOC_StateList", dt);
        }

        #endregion

        #region Delete
        public IActionResult Delete(int StateId)
        {
            string str = Configuration.GetConnectionString("defautString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_delete";
            cmd.Parameters.AddWithValue("@StateId", StateId);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }

        #endregion


        #region Add/Edit 
        public IActionResult Add(int? StateID)
        {
            if (StateID != null)
            {
                //prepare connection
                string str = Configuration.GetConnectionString("defautString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();

                //prepare Command
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_STATE_SelectByPk";
                cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = StateID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = cmd.ExecuteReader();
                dt.Load(objSDR);
                LOC_StateModel modelLOC_State = new LOC_StateModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelLOC_State.StateId = Convert.ToInt32(dr["StateId"]);
                    modelLOC_State.StateName = dr["StateName"].ToString();
                    modelLOC_State.StateCode = dr["StateCode"].ToString();
                    modelLOC_State.CountryId = Convert.ToInt32(dr["CountryId"]);
                    modelLOC_State.Created = Convert.ToDateTime(dr["Created"]);
                    modelLOC_State.Modified = Convert.ToDateTime(dr["Modified"]);
                }
                return View("LOC_StateAddEdit", modelLOC_State);
            }

            return View("LOC_StateAddEdit");
        }
        #endregion


        #region Save
        [HttpPost]
        public IActionResult Save(LOC_StateModel modelLOC_State)
        {
            string str = Configuration.GetConnectionString("defautString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (modelLOC_State.StateId == 0)
            {
                cmd.CommandText = "PR_State_Insert";
            }
            else
            {
                cmd.CommandText = "PR_state_Update";
                cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = modelLOC_State.StateId;
            }
            cmd.Parameters.Add("@StateName", SqlDbType.VarChar).Value = modelLOC_State.StateName;
            cmd.Parameters.Add("@StateCode", SqlDbType.VarChar).Value = modelLOC_State.StateCode;
            cmd.Parameters.Add("@CountryID", SqlDbType.VarChar).Value = modelLOC_State.CountryId;
            //cmd.Parameters.Add("@Created", SqlDbType.Date).Value = modelLOC_Country.Created;
            //cmd.Parameters.Add("@Modified", SqlDbType.Date).Value = modelLOC_Country.Modified;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_State.CountryId == 0)
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
