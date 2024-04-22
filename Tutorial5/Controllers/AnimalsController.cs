using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial5.Models;
using Tutorial5.Models.DTOs;

namespace Tutorial5.Controllers;

[ApiController]
// [Route("api/animals")]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet]
    public IActionResult GetAnimals()
    {
        // Otwieramy połączenie
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        // Definiujemy commanda
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animal";
        
        // Wykonanie commanda
        var reader = command.ExecuteReader();
        
        List<Animal> animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");

        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal)
            });
        }
        
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimal animal)
    {
        // Otwieramy połączenie
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        // Definiujemy commanda
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "INSERT INTO Animal VALUES(@animalName, '', '', '')";
        command.Parameters.AddWithValue("@animalName", animal.Name);

        command.ExecuteNonQuery();
        
        return Created("", null);
    }
}