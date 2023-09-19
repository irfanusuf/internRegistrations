using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace internRegistration.Pages
{
    public class LoginModel : PageModel
    {
        public LoginInfo loginInfo = new LoginInfo();
        public string errorMessage = "";




        public void OnGet()
        {
        }


        public void OnPost()


        {
            loginInfo.Email = Request.Form["Email"];
            loginInfo.Password = Request.Form["Password"];

            // throw error if feilds are empty

            if ( loginInfo.Email.Length == 0 || loginInfo.Password.Length == 0)
            {
                errorMessage = "All feilds are required";
                return;
            }


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



                        
                        //Executing Reader on data base 

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            //reading hashed Passowrd from data base 
                            if (reader.Read())
                            {
                                String storedHash = reader["Password"].ToString();



                                // comapring storedhash with password from login form 

                                if (BCrypt.Net.BCrypt.Verify(loginInfo.Password, storedHash))

                                    //if Succesful Login then redirect to the secured page 
                                {
                                    Response.Redirect("/List");
                                }
                                else
                                {
                                    errorMessage = "Database Error";
                                }
                            }
                        }
                    }
                }

                errorMessage = "Login failed";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
