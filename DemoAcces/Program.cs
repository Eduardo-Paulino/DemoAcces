

using Microsoft.Data.SqlClient;
using System.Text;
using System.Data;
using DemoAcces.Models;
using System.Data.SqlClient;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;

char response;

do
{
    ShowMenuOptions();
    response = GetSelectedOption();
    DoAction (response);    
} while (response != 'S');


void ShowMenuOptions()
{
    Console.WriteLine("[A]gregar nuevo autor");
    Console.WriteLine("[M]odificar un autor registrado");
    Console.WriteLine("[E]liminar un autor registrado");
    Console.WriteLine("[S]alir");
}

char GetSelectedOption()
{
    Console.WriteLine();
    Console.Write("Seleccione una opción: ");
    char selectedOption = Console.ReadKey().KeyChar;
    Console.WriteLine();
    return selectedOption;
}

void DoAction(char response)
{
    switch (response)
    {
        case 'A':
            Insert();
            break;
        case 'E':
            Eliminar();
            break;
        case 'S':
            Console.WriteLine("Adios.");
            break;
        default:
            break;
    }

    List();
    //Insert();

    void List()
    {
        Console.WriteLine($"\nAutores registrados: {GetTotalOfAuthors}");
        Console.WriteLine("{0, -12}{1, -30}{2}", "Id", "Nombre", "Apellidos");
        Console.WriteLine("-------------------------------------------------------------------");
        IEnumerable<Author> authors = GetAllAuthors();
        foreach (var author in authors)
        {
            Console.WriteLine("{0, -12}{1, -30}{2}", author.Id, author.FirstName, author.LastName);
        }
    }



    void Insert()
    {
        string? firstName;
        string? lastName;

        Console.WriteLine("Modulo de registro de autores.\n");

        Console.WriteLine("Nombre del autor: ");
        firstName = Console.ReadLine();
        Console.WriteLine("Apellido del autor: ");
        lastName = Console.ReadLine();

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
        {
            return;
        }
        Author author = new Author(firstName, lastName);
        Console.WriteLine($"Renglones afectados: {InsertAuthor(author)}");
    }

    void Eliminar()
    {
        int id;
        Console.WriteLine("Modulo de eliminación de autores.\n");

        Console.WriteLine("Identificador del autor: ");
        string idAsString = Console.ReadLine();
        if (string.IsNullOrEmpty(idAsString))
        {
            Console.WriteLine("Debe proporcionar un identificador valido del autor");
            return;
        }
        bool isVaid = int.TryParse(idAsString, out id);
        if (isVaid)
        {
            DeleteAutor(id);
            Console.WriteLine($"Se elimino cuyo id es {id}");
        }
        else
        {
            Console.WriteLine("Debe proporcionar un identificador valido del autor");
        }
    }

    //string GetConnectionInformation(SqlConnection conexion)
    //{
    //    StringBuilder s = new StringBuilder(1024);
    //
    //    s.AppendLine($"Información de la conexión:");
    //    s.AppendLine($"\tCadena de conexión: {conexion.ConnectionString}");
    //    s.AppendLine($"\tEstado: {conexion.State}");
    //    s.AppendLine($"\tTiempo de espera: {conexion.ConnectionTimeout.ToString()}");
    //    s.AppendLine($"\tBase de datos: {conexion.Database}");
    //    s.AppendLine($"\tFuente de datos: {conexion.DataSource}");
    //    s.AppendLine($"\tVersión del servidor: {conexion.ServerVersion}");
    //    s.AppendLine($"\tId. de la estación de trabajo: {conexion.WorkstationId}");
    //
    //    return s.ToString();
    //}

    int InsertAuthor(Author author)
    {
        string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Libros;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        int renglonesAfectados = 0;
        string sqlCommandString = "INSERT INTO Authors (FirstName, LastName) VALUES ('@FirstName', '@LastName')";

        try
        {
            using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlCommandString, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FirstName", author.FirstName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LastName", author.LastName));
                    renglonesAfectados = command.ExecuteNonQuery();
                }

            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Ha aparecido un error: {ex.Message}");
        }
        return renglonesAfectados;
    }

    int DeleteAutor(int id)
    {
        string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Libros;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        int renglonesAfectados = 0;
        string sqlCommandString = "DELETE FROM Authors WHERE AuthorId = @AuthorID";

        try
        {
            using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlCommandString, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@AuthorId", id));
                    renglonesAfectados = command.ExecuteNonQuery();
                }

            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Ha aparecido un error: {ex.Message}");
        }
        return renglonesAfectados;
    }

    int GetTotalOfAuthors()
    {
        string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Libros;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        int renglonesAfectados = 0;
        string sqlCommandString = "SELECT COUNT(*) FROM Authors";
        int totalAuthors = 0;

        try
        {
            using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlCommandString, connection))
                {
                    command.CommandType = CommandType.Text;
                    totalAuthors = (int)command.ExecuteScalar();
                }

            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Ha aparecido un error: {ex.Message}");
        }
        return totalAuthors;
    }
}


