using System;

namespace Crucial.App.Exceptions;

public class PageInvalidArgsException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);
