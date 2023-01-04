using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Assignment1.Pages.Access
{
    public class IndexModel : PageModel
    {
        public List<Access> listAccess = new List<Access>();
        public void OnGet()
        {
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
                                Access department = new Access();
                                department.id = "" + reader.GetInt32(0);
                                department.name = reader.GetString(1);
                                department.description = reader.GetString(2);
                                listAccess.Add(department);
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

    public class Access
    {
        public String id;
        public String name;
        public String description;
    }
}
