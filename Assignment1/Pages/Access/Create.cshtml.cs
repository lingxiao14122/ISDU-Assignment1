using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Assignment1.Pages.Access
{
    public class CreateModel : PageModel
    {
        public Access access = new Access();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
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
                    String sql = "INSERT INTO UserAccess (UserAccessname, Description)" +
                                    " VALUES (@name, @description)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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
