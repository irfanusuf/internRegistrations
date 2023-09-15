using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace internRegistration.Pages
{
    public class CreateModel : PageModel

    {
        public InternInfo internInfo = new InternInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        { 
            internInfo.name =Request.Form["name"];
            internInfo.email = Request.Form["email"];
            internInfo.phone = Request.Form["phone"];
            internInfo.address = Request.Form["address"];

            if (internInfo.name.Length == 0 || internInfo.email.Length == 0 || internInfo.phone.Length == 0 || internInfo.address.Length == 0)
            {
                errorMessage = "All feilds are required";
                return;
            }
            // save the new client into the database
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection .Open();
                    String sql = "INSERT INTO interns" +
                        "(name , email , phone ,address ) VALUES " +
                        "( @name , @email , @phone ,@address );";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", internInfo.name);
                        command.Parameters.AddWithValue("@email", internInfo.email);
                        command.Parameters.AddWithValue("@phone", internInfo.phone);
                        command.Parameters.AddWithValue("@address", internInfo.address);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage= ex.Message;
                return;
            }




            internInfo.name = " ";
            internInfo.email = " "; 
            internInfo.phone = " "; 
            internInfo.address = " ";
            successMessage = "New client added suceesfully";

            Response.Redirect("/List");
        }
    }
}
