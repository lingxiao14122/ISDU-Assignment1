using Assignment1.Pages.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Assignment1.Pages.GrantAccess
{
    public class CreateModel : PageModel
    {
        public UserAccessMap userAccessMap = new UserAccessMap();
        public List<Users.User> listUsers = new List<Users.User>();
        public List<Access.Access> listAccess = new List<Access.Access>();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            // populate listUsers
            listUsers = getListUsers();

            // populate listAccess
            listAccess = getListAccess();
        }

        private List<Access.Access> getListAccess()
        {
            List <Access.Access> list = new List<Access.Access>();
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
            List < Users.User > list = new List<Users.User>();
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
            userAccessMap.user_id = Request.Form["user_id"];
            userAccessMap.access_id = Request.Form["access_id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignment;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO UserAccessMap (UserID, UserAccessID) VALUES (@user_id, @access_id)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", userAccessMap.user_id);
                        command.Parameters.AddWithValue("@access_id", userAccessMap.access_id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                // populate listUsers
                listUsers = getListUsers();

                // populate listAccess
                listAccess = getListAccess();
                return;
            }

            userAccessMap.user_id = ""; userAccessMap.access_id = "";;
            successMessage = "New Added Correctly";

            Response.Redirect("/GrantAccess");
        }
    }
}
