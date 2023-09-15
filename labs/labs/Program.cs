using Npgsql;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace labs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) 
            .AddJsonFile("appsettings.json"); 

            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("MyPostgresConnection");

            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string retrieveEmp = "SELECT * FROM employees";
                    string retrieveProjects = "SELECT * FROM projects";

                    //Краще згрупувати працівників по проектах та де проектів >= 2 то і буде резулльтат
                    // string retrieveEmpProj = "SELECT ep1.employee_id, emp.first_name, emp.middle_name, emp.last_name, emp.position\r\nFROM employee_projects ep1\r\nJOIN employee_projects ep2 ON ep1.employee_id = ep2.employee_id\r\n  AND ep1.project_id < ep2.project_id\r\nJOIN employees emp ON ep1.employee_id = emp.id\r\nWHERE ep1.days_quantity > 0 AND ep2.days_quantity > 0;\r\n";
                    string retrieveEmpProj = "";
                    Console.WriteLine("--------------------------------------------------------------");

                    Console.WriteLine("Employees: \n");
                    using (NpgsqlCommand cmd = new NpgsqlCommand(retrieveEmp, con))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string firstName = reader.GetString(reader.GetOrdinal("first_name"));
                                string lastName = reader.GetString(reader.GetOrdinal("last_name"));
                                string position = reader.GetString(reader.GetOrdinal("position"));
                                string homeAddress = reader.GetString(reader.GetOrdinal("home_address"));
                                string characteristic = reader.GetString(reader.GetOrdinal("characteristics"));

                                Console.WriteLine(
                                        $"First Name: {firstName}" +
                                        $", Last Name: {lastName}" +
                                        $", Position: {position}" +
                                        $", Home address: {homeAddress}" +
                                        $", Characteristics: {characteristic}\n"
                                    );
                            }
                        }
                    }
                    Console.WriteLine("--------------------------------------------------------------");
                    Console.WriteLine("\nProjects:\n");
                    using (NpgsqlCommand cmd = new NpgsqlCommand(retrieveProjects, con))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string title = reader.GetString(reader.GetOrdinal("title"));
                                string description = reader.GetString(reader.GetOrdinal("description"));

                                Console.WriteLine(
                                        $"Title of the project: {title}\n" +
                                        $"Description: {description}\n"
                                    );
                            }
                        }
                    }
                    Console.WriteLine("--------------------------------------------------------------");

                    Console.WriteLine("Employees that work at 2 different projects simultaneously:");

                    using (NpgsqlCommand cmd = new NpgsqlCommand(retrieveEmpProj, con))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int empId = reader.GetInt32(reader.GetOrdinal("employee_id"));
                                string firstName = reader.GetString(reader.GetOrdinal("first_name"));
                                string middleName = reader.GetString(reader.GetOrdinal("middle_name"));
                                string lastName = reader.GetString(reader.GetOrdinal("last_name"));
                                string position = reader.GetString(reader.GetOrdinal("position"));

                                Console.WriteLine(
                                        $"Emp id: {empId}\n" +
                                        $"Full name: {firstName} {middleName} {lastName}\n" +
                                        $"Position: {position}\n"

                                    );
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Smth went wrong :(  : " + ex.Message);
                }

            }
        }
       

    }
}
