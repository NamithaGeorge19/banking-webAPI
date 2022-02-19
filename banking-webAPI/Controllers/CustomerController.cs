using banking_webAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace banking_webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration varConfiguration;

        public CustomerController(IConfiguration configuration)
        {
            varConfiguration = configuration;
        }
        // GET: api/<CustomerController>
        [HttpGet]
        public IEnumerable<Customer> GetCustomersList()
        {
            List<Customer> customers = new List<Customer>();
            string connectionString = varConfiguration.GetConnectionString("BankDbConnection");
            DataSet dataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand("GetCustomers", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                dataAdapter.Fill(dataset);
            }
            for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
            {
                Customer customer = new Customer();
                customer.Id = Convert.ToInt32(dataset.Tables[0].Rows[i]["Id"]);
                customer.Name = dataset.Tables[0].Rows[i]["Name"].ToString();
                customer.CurrentBalance = Convert.ToDecimal(dataset.Tables[0].Rows[i]["CurrentBalance"]);
                customers.Add(customer);
            }
            return customers;
        }

        [HttpGet("Beneficiaries/{id}")]
        public IEnumerable<Customer> GetCustomerBeneficiaries(int id)
        {
            List<Customer> customers = new List<Customer>();
            string connectionString = varConfiguration.GetConnectionString("BankDbConnection");
            DataSet dataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand("GetCustomerBeneficiaries", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                dataAdapter.Fill(dataset);
            }
            for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
            {
                Customer customer = new Customer();
                customer.Id = Convert.ToInt32(dataset.Tables[0].Rows[i]["Id"]);
                customer.Name = dataset.Tables[0].Rows[i]["Name"].ToString();
                customers.Add(customer);
            }
            return customers;
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public Customer GetCustomer(int id)
        {
            Customer customer = new Customer();
            string connectionString = varConfiguration.GetConnectionString("BankDbConnection");
            DataSet dataSet = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand("GetCustomer", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                dataAdapter.Fill(dataSet);
                customer.Id = Convert.ToInt32(dataSet.Tables[0].Rows[0]["Id"]);
                customer.Name = dataSet.Tables[0].Rows[0]["Name"].ToString();
                customer.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
                customer.Email = dataSet.Tables[0].Rows[0]["Email"].ToString();
                customer.CurrentBalance = Convert.ToDecimal(dataSet.Tables[0].Rows[0]["CurrentBalance"]);
                
            }
            return customer; 
        }

        [HttpPost("MoneyTransfer")]
        public int MoneyTransfer([FromBody] TransferRequest transfer)
        {
            string connectionString = varConfiguration.GetConnectionString("BankDbConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand Command = new SqlCommand("TransferMoney", connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@FromId", SqlDbType.Int).Value = transfer.FromId;
                Command.Parameters.AddWithValue("@ToId", SqlDbType.VarChar).Value = transfer.ToId;
                Command.Parameters.AddWithValue("@TransferAmount", SqlDbType.Decimal).Value = transfer.Amount;
                var TransactionId = Command.Parameters.Add("@TransactionId", SqlDbType.Int);
                TransactionId.Direction = ParameterDirection.ReturnValue;
                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.Parameters["@TransactionId"].Value);
            }
        }
    }
}
