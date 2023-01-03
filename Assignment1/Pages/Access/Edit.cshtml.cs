using Assignment1.Pages.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Assignment1.Pages.Access
{
    public class EditModel : PageModel
    {
        public Access access = new Access();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignment;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM UserAccess WHERE UserAccessID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                access.id = "" + reader.GetInt32(0);
                                access.name = reader.GetString(1);
                                access.description = reader.GetString(2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            access.id = Request.Form["id"];
            access.name = Request.Form["name"];
            access.description = Request.Form["description"];

            if (access.name.Length == 0 || access.description.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignment;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE UserAccess SET UserAccessName=@name, Description=@description WHERE UserAccessID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", access.id);
                        command.Parameters.AddWithValue("@name", access.name);
                        command.Parameters.AddWithValue("@description", access.description);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            access.name = ""; access.description = "";
            successMessage = "New Added Correctly";

            Response.Redirect("/Access");
        }
    }
}
