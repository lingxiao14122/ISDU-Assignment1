using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Assignment1.Pages.Users
{
    public class CreateModel : PageModel
    {
        public User user = new User();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            user.user_name = Request.Form["user_name"];
            user.email = Request.Form["email"];
            user.employee_number = Request.Form["employee_number"];
            user.age = Request.Form["age"];
            user.dept_id = Request.Form["dept_id"];
            user.active = Request.Form["active"]== "on" ? "1" : "0";
            String password = Request.Form["password"];

            if (user.user_name.Length == 0 || user.email.Length == 0 || 
                user.employee_number.Length == 0 || user.age.Length == 0 || password.Length== 0 ||
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
                    String sql = "INSERT INTO Users (UserName, Email, EmployeeNumber, Age, Password, DeptID, Active)" + 
                                    " VALUES (@user_name, @email, @employee_number, @age, @password, @dept_id, @active)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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
