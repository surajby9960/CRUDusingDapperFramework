using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDusingDapperFramework.Models
{
    public class Product
    {

        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Product Price")]
        public double Price { get; set; }

        public static string ConnectionString = "Server=WIN-EQ1SUAAV235\\SURAJ;Database=Shaurya_ADO;Integrated Security=True;";
    }
    public class ProductDAL
    {
        public static IEnumerable<Product> GetAllProducts()
        {
            List<Product> productList = new List<Product>();
            using (IDbConnection con = new SqlConnection(Product.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                productList = con.Query<Product>("SP_SelectAll_Product").ToList();

            }
            return productList;
        }
        public static Product GetProductById(int id)
        {
            Product product = new Product();
            using (IDbConnection con = new SqlConnection(Product.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@id", id);
                product = con.Query<Product>("SP_SelectById_Product", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return product;
        }

        public static int AddProduct(Product prod)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(Product.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@name", prod.Name);
                dynamicParameters.Add("@price", prod.Price);
                result = con.Execute("SP_Insert_Product", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        public static int UpdateProduct(Product prod)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(Product.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@id", prod.Id);
                dynamicParameters.Add("@name", prod.Name);
                dynamicParameters.Add("@price", prod.Price);
                result = con.Execute("SP_Update_Product", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        public static int DeleteProduct(int id)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(Product.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@id", id);
                result = con.Execute("SP_Delete_Product", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }
    }


}

