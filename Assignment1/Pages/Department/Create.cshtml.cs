using Assignment1.Pages.Departments;
using Assignment1.Pages.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Assignment1.Pages.Department
{
    public class CreateModel : PageModel
    {
        public Departments.Department department = new Departments.Department();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
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
                    String sql = "INSERT INTO Department (DeptName, Description)" +
                                    " VALUES (@name, @description)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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

            Response.Redirect("/Department/Index");
        }
    }
}
