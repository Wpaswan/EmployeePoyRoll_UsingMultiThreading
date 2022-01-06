using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayrollUsingMultiThreadingProject
{
    public class EmployeePayrollOperation
    {
        public static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                                         Initial Catalog=payroll_service1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public List<EmployeeDetails> employeePayrollDetailsList = new List<EmployeeDetails>();
        public void addEmployeeToPayroll(List<EmployeeDetails> employeePayrollDetailsList)
        {
            employeePayrollDetailsList.ForEach(employeeData =>
            {
                Console.WriteLine("Employee being added: " + employeeData.EmployeeName);
                this.addEmployeeToPayroll(employeeData);
                Console.WriteLine("Employee added: "+employeeData.EmployeeName);
            });
            Console.WriteLine(this.employeePayrollDetailsList.ToString());
        }


        public void addEmployeeToPayroll(EmployeeDetails emp)
        {
            employeePayrollDetailsList.Add(emp);
        }
        public void addEmployeeToPayrollWithThread(List<EmployeeDetails> employeePayrollDetailsList)
        {
            employeePayrollDetailsList.ForEach(employeeData =>
            {
                //Console.WriteLine("abc");
                Task thread = new Task(() =>
                {
                    Console.WriteLine("Employee being added: "+employeeData.EmployeeName);
                    this.addEmployeeToPayroll(employeeData);
                    Console.WriteLine("Employee added: "+employeeData.EmployeeName);
                });
                thread.Start();
            });
            Console.WriteLine(this.employeePayrollDetailsList.Count);
        }public void Add()
        {
            EmployeeModel model = new EmployeeModel();
            model.EmployeeName ="Ram";
            model.BasicPay=6000;
            model.PhoneNumber ="7976688";
            model.Tax=57395;
            model.Gender ='M';
            model.Address ="RamNagar";
            model.StartDate = DateTime.Now;
            model.TaxablePay=53535;
            model.Deductions=539583;
            AddEmployee(model);


        }
        public bool AddEmployee(EmployeeModel model)
        {
            try
            {

                SqlConnection connection = new SqlConnection(connectionString);
                using (connection)
                {
                    Task thread = new Task(() =>
                    {
                        //var qury=values()
                        SqlCommand command = new SqlCommand("SpAddEmployeeDetails1", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmployeeName", model.EmployeeName);
                        command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                        command.Parameters.AddWithValue("@Address", model.Address);
                        command.Parameters.AddWithValue("@Department", model.Department);
                        command.Parameters.AddWithValue("@Gender", model.Gender);
                        command.Parameters.AddWithValue("@BasicPay", model.BasicPay);
                        //command.Parameters.AddWithValue("@Deductions", model.Deductions);
                        command.Parameters.AddWithValue("@TaxablePay", model.TaxablePay);
                        command.Parameters.AddWithValue("@Tax", model.Tax);
                        command.Parameters.AddWithValue("@NetPay", model.NetPay);
                        command.Parameters.AddWithValue("@StartDate", DateTime.Now);
                        //command.Parameters.AddWithValue("@City", model.City);
                        //command.Parameters.AddWithValue("@Country", model.Country);
                        connection.Open();
                        var result = command.ExecuteNonQuery();
                        connection.Close();
                        if(result != 0)
                            Console.WriteLine("records added successfully!!");
                    });
                    thread.Start();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Close();
            }
            return false;
        }
    }
}
