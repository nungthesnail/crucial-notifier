namespace Web.Site.Services.Exceptions;

public class DataNotFoundException(string? message = null) : Exception(message);
