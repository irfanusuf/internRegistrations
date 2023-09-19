using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;


namespace internRegistration.Pages
{
    public class EditModel : PageModel
    {
        public InternInfo internInfo=new InternInfo();
        public string errorMessage = "";
        public string successMessage = "";

        private readonly string connectionString;

        public EditModel(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM interns WHERE id =@id";
                    using (SqlCommand command = new SqlCommand( sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                internInfo.id = "" + reader.GetInt32(0);
                                internInfo.name = reader.GetString(1);
                                internInfo.email= reader.GetString(2);
                                internInfo.phone = reader.GetString(3);
                                internInfo.address = reader.GetString(4);

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

        public void OnPost() { 
            internInfo.id =Request.Form["id"];
            internInfo.name = Request.Form["name"];
            internInfo.email = Request.Form["email"];
            internInfo.phone = Request.Form["phone"];
            internInfo.address= Request.Form["address"];
             

            if ( internInfo.name.Length==0 || internInfo.email.Length == 0 ||
                internInfo.phone.Length == 0 || internInfo.address.Length==0)
            {
                errorMessage = "All feilds are necessary";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE interns " +
             "SET name = @name, email = @email, phone = @phone, address = @address " +
             "WHERE id = @id";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", internInfo.name);
                        command.Parameters.AddWithValue("@email", internInfo.email);
                        command.Parameters.AddWithValue("@phone", internInfo.phone);
                        command.Parameters.AddWithValue ("@address", internInfo.address);
                        command.Parameters.AddWithValue("@id", internInfo.id);


                        command.ExecuteNonQuery();
                    }
                }

                successMessage = "Intern information updated successfully.";
                Response.Redirect("/List");

            }
            catch (Exception ex)
            {
                errorMessage= ex.Message;
                return;

            }
            

        }
    }
}
