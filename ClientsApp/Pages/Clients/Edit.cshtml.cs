using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {

        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {
                String sqlString = "SELECT * FROM clients WHERE id=@id";
                String connectionString = "Data Source=RedmoBookPro14;Initial Catalog=mystore;Integrated Security=True;TrustServerCertificate=True";

                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                using SqlCommand command = new SqlCommand(sqlString, connection);
                command.Parameters.AddWithValue("@id", id);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    clientInfo.id = "" + reader.GetInt32(0);
                    clientInfo.name = "" + reader.GetString(1);
                    clientInfo.email = "" + reader.GetString(2);
                    clientInfo.phone = "" + reader.GetString(3);
                    clientInfo.address = "" + reader.GetString(4);
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Response.WriteAsync(errorMessage).Wait();
            }
        }

        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.address = Request.Form["address"];
            clientInfo.phone = Request.Form["phone"];


            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=RedmoBookPro14;Initial Catalog=mystore;Integrated Security=True;TrustServerCertificate=True";
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                String sql = "UPDATE clients SET name=@name, email=@email, phone=@phone, address=@address WHERE id=@id";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", clientInfo.id);
                command.Parameters.AddWithValue("@name", clientInfo.name);
                command.Parameters.AddWithValue("@phone", clientInfo.phone);
                command.Parameters.AddWithValue("@address", clientInfo.address);
                command.Parameters.AddWithValue("@email", clientInfo.email);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Response.WriteAsync(errorMessage).Wait();
            }

            Response.Redirect("/Clients/Index");

        }
    }
}
