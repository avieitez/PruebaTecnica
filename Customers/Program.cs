using System;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string servidor = @"Data Source=LAPTOP-H4HFAQA0\SQLEXPRESS;Initial Catalog=Prueba;Integrated Security=True";
            string connectionString = servidor;
            string filePath = @"C:\Alberto\PruebasCandidatos\PruebasCandidatos\Fuentes\Customers\Customers.csv";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Crear la tabla temporal para cargar los datos del CSV
                using (SqlCommand createTableCommand = new SqlCommand("CREATE TABLE #TempCustomers (Id CHAR(10), Name VARCHAR(100), Address VARCHAR(100), City VARCHAR(100), Country VARCHAR(100), PostalCode VARCHAR(100), Phone VARCHAR(100))", connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }

                // Utilizar la clase SqlBulkCopy para cargar los datos del archivo CSV en la tabla temporal
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "#TempCustomers";
                    bulkCopy.ColumnMappings.Add("Id", "Id");
                    bulkCopy.ColumnMappings.Add("Name", "Name");
                    bulkCopy.ColumnMappings.Add("Address", "Address");
                    bulkCopy.ColumnMappings.Add("City", "City");
                    bulkCopy.ColumnMappings.Add("Country", "Country");
                    bulkCopy.ColumnMappings.Add("PostalCode", "PostalCode");
                    bulkCopy.ColumnMappings.Add("Phone", "Phone");
                    bulkCopy.WriteToServer(GetDataTableFromCsv(filePath));
                }

                // Insertar los datos de la tabla temporal en la tabla Customers
                using (SqlCommand insertCommand = new SqlCommand("INSERT INTO Customers (Id, Name, Address, City, Country, PostalCode, Phone) SELECT Id, Name, Address, City, Country, PostalCode, Phone FROM #TempCustomers", connection))
                {
                    insertCommand.ExecuteNonQuery();
                }

                // Eliminar la tabla temporal
                using (SqlCommand dropTableCommand = new SqlCommand("DROP TABLE #TempCustomers", connection))
                {
                    dropTableCommand.ExecuteNonQuery();
                }

                Console.WriteLine("Datos cargados correctamente.");
                Console.ReadLine();
            }
        }

        static DataTable GetDataTableFromCsv(string filePath)
        {
            DataTable dataTable = new DataTable();

            using (System.IO.StreamReader reader = new System.IO.StreamReader(filePath))
            {
                string[] headers = reader.ReadLine().Split(';');
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header.Trim());
                }

                while (!reader.EndOfStream)
                {
                    string[] rows = reader.ReadLine().Split(';');
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dataRow[i] = rows[i].Trim();
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }
    }
}
