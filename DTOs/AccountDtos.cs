namespace TutorOS.Api.DTOs;

/// <summary>
/// Data transfer object for user registration.
/// </summary>
public record RegisterDto
{
    /// <summary>
    /// The user's full name.
    /// </summary>
    /// <example>John Doe</example>
    public required string FullName { get; init; }

    /// <summary>
    /// The user's email address.
    /// </summary>
    /// <example>john.doe@example.com</example>
    public required string Email { get; init; }

    /// <summary>
    /// The user's password.
    /// </summary>
    /// <example>StrongPassword123!</example>
    public required string Password { get; init; }

    /// <summary>
    /// The name of the institute the user belongs to.
    /// </summary>
    /// <example>Alpha Academy</example>
    public required string InstituteName { get; init; }
}

/// <summary>
/// Data transfer object for user login.
/// </summary>
public record LoginDto
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    /// <example>john.doe@example.com</example>
    public required string Email { get; init; }

    /// <summary>
    /// The user's password.
    /// </summary>
    /// <example>StrongPassword123!</example>
    public required string Password { get; init; }
}

/// <summary>
/// Data transfer object for the authentication response.
/// </summary>
public record AuthResponseDto
{
    /// <summary>
    /// The JWT access token.
    /// </summary>
    public string Token { get; init; } = null!;

    /// <summary>
    /// The expiration time of the token.
    /// </summary>
    public DateTime ExpiresAt { get; init; }

    /// <summary>
    /// The authenticated user's email.
    /// </summary>
    public string Email { get; init; } = null!;
}
