using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace internRegistration.Pages
{
    public class LoginModel : PageModel
    {
        public LoginInfo loginInfo = new LoginInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }


        public void OnPost()


        {
            loginInfo.Email = Request.Form["Email"];
            loginInfo.Password = Request.Form["Password"];

            try
            {
                String connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True";

                //creating a  sql connection by paasing the connection string 
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT Email, Password FROM Users WHERE Email = @Email";



                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // passing the data from userinfo into sql query


                        command.Parameters.AddWithValue("@Email", loginInfo.Email);






                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                String storedHash = reader["Password"].ToString();


                                if (BCrypt.Net.BCrypt.Verify(loginInfo.Password, storedHash))
                                {

                                    Response.Redirect("/List");
                                }
                            }
                        }
                    }
                }

                errorMessage = "Correct credentailals Required";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
