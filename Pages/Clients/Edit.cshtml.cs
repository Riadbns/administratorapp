using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebApplication1.Pages.Clients
{
    public class EditModel : PageModel
    {
        public CLientInfo clientInfo = new CLientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];
			try
			{
				//connect to the database using the connection string
				String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
				//creat sql connection
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					//open the connection
					connection.Open();
					//creat the sqlquery to read the data from our clients table 
					string sql = "select * from clients where id=@id";
					//creat the sqlcommand wich allows us to execute our sqlquery
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								clientInfo.id = "" + reader.GetInt32(0);
								clientInfo.fullname = reader.GetString(1);
								clientInfo.email = reader.GetString(2);
								clientInfo.phone = reader.GetString(3);
								clientInfo.adresse = reader.GetString(4);
								clientInfo.created_at = reader.GetDateTime(5).ToString();

							}
						}
					}
				}
			}catch(Exception ex)
			{
				errorMessage = ex.Message;
			}
        }
        public void OnPost()
        {
			clientInfo.id = Request.Form["id"];
			clientInfo.fullname = Request.Form["fullname"];
			clientInfo.email = Request.Form["email"];
			clientInfo.phone = Request.Form["phone"];
			clientInfo.adresse = Request.Form["address"];
			if (clientInfo.id.Length == 0||clientInfo.fullname.Length == 0 || clientInfo.email.Length == 0 ||
				clientInfo.phone.Length == 0 || clientInfo.adresse.Length == 0)
			{
				errorMessage = "All the fields are required";
				return;
			}
			try
			{
				//connect to the database using the connection string
				String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
				//creat sql connection
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					//open the connection
					connection.Open();
					//creat the sqlquery to read the data from our clients table 
					string sql = "UPDATE clients " +
								"SET fullname=@fullname, email=@email, phone=@phone, adresse=@address " +
								"WHERE id=@id";
					//creat the sqlcommand wich allows us to execute our sqlquery
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@fullname", clientInfo.fullname);
						command.Parameters.AddWithValue("@email", clientInfo.email);
						command.Parameters.AddWithValue("phone", clientInfo.phone);
						command.Parameters.AddWithValue("@address", clientInfo.adresse);
						command.Parameters.AddWithValue("@id", clientInfo.id);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			Response.Redirect("/Clients/Index");
		}
    }
}
