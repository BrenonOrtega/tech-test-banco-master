using Awarean.Sdk.Result;

namespace TechTest.BancoMaster.Travels.Application;

public class ApplicationErrors
    {
        public static readonly Error TravelNotFound = Create("TRAVEL_NOT_FOUND", "Did not found travel for supplied informations");
        public static readonly Error FailedAddingTravel = Create("FAILED_SAVING_TRAVEL", "Problem ocurred while adding travel");
        public static readonly Error FailedDeletingTravel = Create("FAILED_DELETING_TRAVEL", "A problem occured while deleting travel");
        public static readonly Error CannotChangeTravelsName = Create("CANNOT_CHANGE_TRAVEL_ROUTE", "An attempt to change travel connections occured.");

        private static Error Create(string code, string message) => Error.Create(code, message);
    }
