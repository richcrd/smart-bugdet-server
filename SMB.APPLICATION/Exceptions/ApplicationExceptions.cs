namespace SMB.APPLICATION.Exceptions;

public class ValidationException(string message) : Exception(message);

public class DuplicateResourceException(string message) : Exception(message);

public class InvalidCredentialsException() : Exception("Credenciales incorrectas");

public class ForbiddenException(string message) : Exception(message);

public class ResourceNotFoundException(string message) : Exception(message);
