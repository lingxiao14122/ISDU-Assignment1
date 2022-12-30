using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Assignment1.Pages.Users
{
    public class EditModel : PageModel
    {
        public User user = new User();
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
                    String sql = "SELECT * FROM Users WHERE userID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user.id= "" + reader.GetInt32(0);
                                user.user_name = reader.GetString(1);
                                user.email = reader.GetString(2);
                                user.employee_number = reader.GetString(3);
                                user.age = "" + reader.GetInt32(4);
                                // 5 password
                                user.password = reader.GetString(5);
                                user.dept_id = "" + reader.GetInt32(6);
                                user.active = "" + reader.GetBoolean(7);
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
            user.id = Request.Form["id"];
            user.user_name = Request.Form["user_name"];
            user.email = Request.Form["email"];
            user.employee_number = Request.Form["employee_number"];
            user.age = Request.Form["age"];
            user.dept_id = Request.Form["dept_id"];
            user.active = Request.Form["active"] == "on" ? "1" : "0";
            String password = Request.Form["password"];

            if (user.user_name.Length == 0 || user.email.Length == 0 ||
                user.employee_number.Length == 0 || user.age.Length == 0 || password.Length == 0 ||
                user.dept_id.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            // save new client to db
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignment;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Users SET UserName=@user_name, Email=@email, EmployeeNumber=@employee_number, Age=@age, Password=@password, DeptID=@dept_id, Active=@active WHERE userID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", user.id);
                        command.Parameters.AddWithValue("@user_name", user.user_name);
                        command.Parameters.AddWithValue("@email", user.email);
                        command.Parameters.AddWithValue("@employee_number", user.employee_number);
                        command.Parameters.AddWithValue("@age", user.age);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@dept_id", user.dept_id);
                        command.Parameters.AddWithValue("@active", user.active);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            user.user_name = ""; user.email = ""; user.employee_number = ""; user.age = ""; user.dept_id = ""; user.active = "";
            successMessage = "New Client Added Correctly";

            Response.Redirect("/Users/Index");
        }
    }
}
