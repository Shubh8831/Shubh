using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
namespace ShubhPrj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShubhProductController : ControllerBase
    {
        SqlConnection con;
        SqlDataAdapter da;
        IConfiguration _configuration;

        public ShubhProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            con = new SqlConnection();
            da = new SqlDataAdapter();
            string str = _configuration.GetConnectionString("ConStr");
            con.ConnectionString = str;
        }

        [HttpGet]
        public IEnumerable<ShubhProduct> GetProducts()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * from ShubhProduct";
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            List<ShubhProduct> lst = new List<ShubhProduct>();
            ShubhProduct obj = new ShubhProduct();
            obj.ProductId = System.Convert.ToInt32(dr["ProductId"]);
            obj.Name = dr["Name"].ToString();
            obj.Description = dr["Description"].ToString();
            obj.Price = (float)(dr["Price"]);
            obj.Category = dr["Category"].ToString();
            lst.Add(obj);
            return lst;
        }

        [HttpPost]
        public ActionResult<string> AddProducts(ShubhProduct obj)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "Insert into ShubhProduct values(@ProductId, '@Name', '@Description' ,@Price, '@Category')";
            cmd.Parameters.Add(new SqlParameter("ProductId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("Name", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("Description", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("Price", SqlDbType.Float));
            cmd.Parameters.Add(new SqlParameter("Category", SqlDbType.VarChar));

            cmd.Parameters["ProductId"].Value = obj.ProductId;
            cmd.Parameters["Name"].Value = obj.Name;
            cmd.Parameters["Description"].Value = obj.Description;
            cmd.Parameters["Price"].Value = obj.Price;
            cmd.Parameters["Category"].Value = obj.Category;
            int ans = cmd.ExecuteNonQuery();
            return ans.ToString() + "Product Added Successfully";
        }

        [HttpDelete]
        public ActionResult<string> DeleteProducts(int ProductId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "Delete from ShubhProduct where ProductId = @id";
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            int rowsAffected = cmd.ExecuteNonQuery();
            return "$ProductId { ProductId} Deleted Successfully";
        }

        //[HttpPut]
        //public ActionResult<string> UpdateProducts(int ProductId)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = con;
        //    con.Open();
        //}
    }
}
