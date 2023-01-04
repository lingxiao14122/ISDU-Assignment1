using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Assignment1.Pages.Department
{
    public class EditModel : PageModel
    {
        public Departments.Department department = new Departments.Department();
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
                    String sql = "SELECT * FROM Department WHERE DeptID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                department.id = "" + reader.GetInt32(0);
                                department.name = reader.GetString(1);
                                department.description = reader.GetString(2);
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
            department.id = Request.Form["id"];
            department.name = Request.Form["name"];
            department.description = Request.Form["description"];

            if (department.name.Length == 0 || department.description.Length == 0)
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
                    String sql = "UPDATE Department SET DeptName=@name, Description=@description WHERE DeptID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", department.id);
                        command.Parameters.AddWithValue("@name", department.name);
                        command.Parameters.AddWithValue("@description", department.description);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            department.name = ""; department.description = "";
            successMessage = "New Added Correctly";

            Response.Redirect("/Department");
        }
    }
}
