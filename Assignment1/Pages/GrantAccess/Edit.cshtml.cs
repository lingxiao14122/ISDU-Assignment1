using Assignment1.Pages.Users;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Assignment1.Pages.GrantAccess
{
    public class EditModel : PageModel
    {
        public UserAccessMap userAccessMap = new UserAccessMap();
        public List<Users.User> listUsers = new List<Users.User>();
        public List<Access.Access> listAccess = new List<Access.Access>();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            userAccessMap.user_id = Request.Query["user_id"];
            userAccessMap.access_id = Request.Query["access_id"];
            // populate listUsers
            listUsers = getListUsers();

            // populate listAccess
            listAccess = getListAccess();
        }
        private List<Access.Access> getListAccess()
        {
            List<Access.Access> list = new List<Access.Access>();
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignment;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM UserAccess";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Access.Access department = new Access.Access();
                                department.id = "" + reader.GetInt32(0);
                                department.name = reader.GetString(1);
                                department.description = reader.GetString(2);
                                list.Add(department);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            return list;
        }

        private List<Users.User> getListUsers()
        {
            List<Users.User> list = new List<Users.User>();
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignment;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM [dbo].[Users] INNER JOIN [dbo].[Department] ON Users.DeptID=Department.DeptID";
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
                                user.dept_name = reader.GetString(10);
                                list.Add(user);
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
            return list;
        }

        public void OnPost()
        {
            String original_user_id = Request.Form["original_user_id"];
            String original_access_id = Request.Form["original_access_id"];
            userAccessMap.user_id = Request.Form["user_id"];
            userAccessMap.access_id = Request.Form["access_id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignment;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE UserAccessMap SET UserID=@user_id, UserAccessID=@access_id WHERE UserID=@original_user_id AND UserAccessID=@original_access_id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@original_user_id", original_user_id);
                        command.Parameters.AddWithValue("@original_access_id", original_access_id);
                        command.Parameters.AddWithValue("@user_id", userAccessMap.user_id);
                        command.Parameters.AddWithValue("@access_id", userAccessMap.access_id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            userAccessMap.user_id = ""; userAccessMap.access_id = "";
            successMessage = "Edit Successful";

            Response.Redirect("/GrantAccess");
        }
    }
}
