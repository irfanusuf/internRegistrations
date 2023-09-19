using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace internRegistration.Pages
{


    public class InternInfo
    {
        public string id;
        public string name; 
        public string email;
        public string phone;
        public string address;
        public string created_at;
    }
    public class ListModel : PageModel

    {

        private readonly string connectionString; 

        public ListModel(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }




        public List<InternInfo> listInterns = new List<InternInfo>();
        public void OnGet()
        {
            try
            {
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM interns";
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                InternInfo internInfo = new InternInfo();
                                internInfo.id = "" + reader.GetInt32(0);
                                internInfo.name = reader.GetString(1);
                                internInfo.email = reader.GetString(2);
                                internInfo.phone= reader.GetString(3);
                                internInfo.address = reader.GetString(4);
                                internInfo.created_at = reader.GetDateTime(5).ToString();

                                listInterns.Add(internInfo);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine( "Exception " + ex.ToString());
            }
        }
    }



    
}