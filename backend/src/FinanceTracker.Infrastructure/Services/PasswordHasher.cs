using FinanceTracker.Application.Common.Interfaces;
using B = BCrypt.Net.BCrypt;

namespace FinanceTracker.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => B.HashPassword(password);
    public bool Verify(string password, string hash) => B.Verify(password, hash);
}
