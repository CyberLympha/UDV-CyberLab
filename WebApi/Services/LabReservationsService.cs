using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Models;
namespace WebApi.Services;
public class LabReservationsService
{
    private readonly IMongoCollection<LabReservation> _labReservationsCollection;
    private readonly UserService _userService;

    public LabReservationsService(
        IMongoCollection<LabReservation> labReservationsCollection, UserService userService)
    {
        _labReservationsCollection = labReservationsCollection;
        _userService = userService;
    }
    public async Task CreateAsync(LabReservation newLabReservation, Lab reservationLab)
    {
        if (newLabReservation == null) throw new Exception();
        if (await Intersects(newLabReservation, reservationLab))
            throw new Exception("The time has already been resereved");
        try
        {
            await _labReservationsCollection.InsertOneAsync(newLabReservation);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<LabReservation> GetByIdAsync(string id)
    {
        try
        {
            return (await _labReservationsCollection.FindAsync(bson => bson.Id == id)).First();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task UpdateAsync(LabReservation labReservation, Lab reservationLab, string userId)
    {
        var reservationToUpdate = await _labReservationsCollection.FindAsync(bson => bson.Id == labReservation.Id) ??
            throw new Exception("Reservation not found");
        var user = await _userService.GetAsyncById(userId);
        try
        {
            if (labReservation.Reservor.Id != userId || user.Role != UserRole.Admin)
                throw new Exception("User is not the reservor");
            if (await Intersects(labReservation, reservationLab))
                throw new Exception("The time has already been resereved");

            var update = Builders<LabReservation>.Update
                .Set("theme", labReservation.Theme)
                .Set("description", labReservation.Description)
                .Set("time_start", labReservation.TimeStart)
                .Set("time_end", labReservation.TimeEnd);
            await _labReservationsCollection.UpdateOneAsync((FilterDefinition<LabReservation>)reservationToUpdate, update);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task DeleteAsync(string labReservationId, string userId)
    {
        var labReservationToDelete = await GetByIdAsync(labReservationId) ??
                throw new Exception("Reservation not found");
        var user = await _userService.GetAsyncById(userId);
        try
        {
            if (labReservationToDelete.Reservor.Id != userId || user.Role != UserRole.Admin)
                throw new Exception("User is not the reservor");
            await _labReservationsCollection.DeleteOneAsync(bson => bson.Id == labReservationId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<List<LabReservation>> GetAllAsync()
    {
        try
        {
            return await _labReservationsCollection.Find(_ => true).ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<List<LabReservation>> GetAllLabreservationsAsync(Lab reservationLab)
    {
        try
        {
            return await _labReservationsCollection
            .Find(reservation => reservation.Lab.Id == reservationLab.Id)
            .ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> Intersects(LabReservation labReservation, Lab reservationLab)
    {
        var labs = await GetAllLabreservationsAsync(reservationLab);
        foreach (var _labReservation in labs)
        {
            if (labReservation.Intersects(_labReservation))
                return true;
        }
        return false;
    }
}
