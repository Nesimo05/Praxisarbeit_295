using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Ski_Service_Backend.Model;
using System;
using System.Linq;
using Ski_Service_Backend.Dto;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public RegistrationController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Alle Registrierungen abrufen.
    /// </summary>
    /// <returns>Die Liste aller Registrierungen.</returns>
    [HttpGet]
    public IActionResult Get()
    {
        var registrations = _dbContext.Registrations.ToList();
        return Ok(registrations);
    }

    /// <summary>
    /// Registrierung anhand des Namens abrufen.
    /// </summary>
    /// <param name="name">Der Name der zu suchenden Registrierung.</param>
    /// <returns>Die gefundene Registrierung oder eine Meldung, wenn nicht gefunden.</returns>
    [HttpGet("{name}")]
    public IActionResult Get(string name)
    {
        var registration = _dbContext.Registrations.FirstOrDefault(r => r.Name == name);

        if (registration == null)
        {
            return NotFound("Registration not found");
        }

        return Ok(registration);
    }

    /// <summary>
    /// Neue Registrierung erstellen.
    /// </summary>
    /// <param name="registrationDto">Die Daten für die zu erstellende Registrierung.</param>
    /// <returns>Eine Erfolgsmeldung oder eine Meldung im Fehlerfall.</returns>
    [HttpPost]
    public IActionResult Post([FromBody] RegistrationDto registrationDto)
    {
        if (registrationDto == null)
        {
            return BadRequest("Invalid data");
        }

        try
        {
            var registrationModel = new RegistrationUser
            {
                Name = registrationDto.Name,
                Email = registrationDto.Email,
                Phone = registrationDto.Phone,
                Priority = registrationDto.Priority,
                Service = registrationDto.Service,
                CreateDate = registrationDto.CreateDate,
                PickupDate = registrationDto.PickupDate
            };

            // Daten zur Datenbank hinzufügen und Änderungen speichern
            registrationModel.CreateDate = DateTime.Now;
            _dbContext.Registrations.Add(registrationModel);
            _dbContext.SaveChanges();

            return Ok("Data received successfully and saved to the database");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Registrierung aktualisieren.
    /// </summary>
    /// <param name="name">Der Name der zu aktualisierenden Registrierung.</param>
    /// <param name="registrationDto">Die aktualisierten Daten für die Registrierung.</param>
    /// <returns>Eine Erfolgsmeldung oder eine Meldung im Fehlerfall.</returns>
    [HttpPut("{name}")]
    public IActionResult Put(string name, [FromBody] RegistrationDto registrationDto)
    {
        var existingRegistration = _dbContext.Registrations.FirstOrDefault(r => r.Name == name);

        if (existingRegistration == null)
        {
            return NotFound("Registration not found");
        }

        // Eigenschaften der vorhandenen Registrierung mit den neuen Daten aktualisieren
        existingRegistration.Name = registrationDto.Name;
        existingRegistration.Email = registrationDto.Email;
        existingRegistration.Phone = registrationDto.Phone;
        existingRegistration.Priority = registrationDto.Priority;
        existingRegistration.Service = registrationDto.Service;
        existingRegistration.PickupDate = registrationDto.PickupDate;

        // Änderungen in der Datenbank speichern
        _dbContext.SaveChanges();

        return Ok("Registration updated successfully");
    }

    /// <summary>
    /// Registrierung löschen.
    /// </summary>
    /// <param name="name">Der Name der zu löschenden Registrierung.</param>
    /// <returns>Eine Erfolgsmeldung oder eine Meldung im Fehlerfall.</returns>
    [HttpDelete("{name}")]
    [Authorize]
    public IActionResult Delete(string name)
    {
        var registration = _dbContext.Registrations.FirstOrDefault(r => r.Name == name);

        if (registration == null)
        {
            return NotFound("Registration not found");
        }

        // Registrierung aus der Datenbank löschen
        _dbContext.Registrations.Remove(registration);
        _dbContext.SaveChanges();

        return Ok("Registration deleted successfully");
    }
}
