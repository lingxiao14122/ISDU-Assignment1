using Assignment1.Pages.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Assignment1.Pages.GrantAccess
{
    public class IndexModel : PageModel
    {
        public List<DepartmentUserAccess> listDeptUserAccess = new List<DepartmentUserAccess>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignment;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM [dbo].[DepartmentUserAccess] ORDER BY UserName";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DepartmentUserAccess departmentUserAccess = new DepartmentUserAccess();
                                departmentUserAccess.user_name = reader.GetString(0);
                                departmentUserAccess.department_name = reader.GetString(1);
                                departmentUserAccess.user_access_name = reader.GetString(2);
                                departmentUserAccess.user_id = "" + reader.GetInt32(3);
                                departmentUserAccess.access_id = "" + reader.GetInt32(4);
                                listDeptUserAccess.Add(departmentUserAccess);
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

    public class UserAccessMap
    {
        public String user_id;
        public String access_id;
    }

    public class DepartmentUserAccess
    {
        public String user_name;
        public String department_name;
        public String user_access_name;
        public String user_id;
        public String access_id;
    }
}
