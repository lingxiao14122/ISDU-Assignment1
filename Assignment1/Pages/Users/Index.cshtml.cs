using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;
using static Assignment1.Pages.Users.IndexModel;

namespace Assignment1.Pages.Users
{
    public class IndexModel : PageModel
    {
        public List<User> listUsers = new List<User>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignment;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Users";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User();
                                user.id = "" + reader.GetInt32(0);
                                user.user_name = reader.GetString(1);
                                user.email = reader.GetString(2);
                                user.employee_number = reader.GetString(3);
                                user.age = "" + reader.GetInt32(4);
                                // 5 password
                                user.dept_id = "" + reader.GetInt32(6);
                                user.active = "" + reader.GetBoolean(7);
                                user.created_at = reader.GetDateTime(8).ToString();

                                listUsers.Add(user);
                                Debug.WriteLine(user.id);
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
    }
    public class User
    {
        public String id;
        public String user_name;
        public String email;
        public String employee_number;
        public String age;
        public String password;
        public String dept_id;
        public String active;
        public String created_at;
    }


}
