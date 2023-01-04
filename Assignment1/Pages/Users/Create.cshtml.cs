using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Assignment1.Pages.Users
{
    public class CreateModel : PageModel
    {
        public User user = new User();
        public List<Departments.Department> listDepartments = new List<Departments.Department>();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignment;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Department";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Departments.Department department = new Departments.Department();
                                department.id = "" + reader.GetInt32(0);
                                department.name = reader.GetString(1);
                                department.description = reader.GetString(2);
                                listDepartments.Add(department);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        public void OnPost()
        {
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

            // hash password
            password = hashpass(password);

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
            successMessage = "New Added Correctly";

            Response.Redirect("/Users/Index");
        }

        public static String hashpass(String password)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] result;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                result = sha256Hash.ComputeHash(data);
            }

            // Return the hexadecimal string
            return System.Text.Encoding.UTF8.GetString(result);
        }
    }
}
