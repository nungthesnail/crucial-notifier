using System;

namespace Crucial.App.Exceptions;

public class PageCreatingException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);
