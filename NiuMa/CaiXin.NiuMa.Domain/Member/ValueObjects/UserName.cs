using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace CaiXin.NiuMa.Domain.Member.ValueObjects;

[ComplexType]
public record UserName([property: Column("Name")] string Value)
{
    public static UserName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("用户名不能为空");

        if (value.Length < 2 || value.Length > 20)
            throw new InvalidOperationException("用户名长度必须在2-20个字符之间");

        if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z0-9_\u4e00-\u9fa5]+$"))
            throw new InvalidOperationException("用户名只能包含字母、数字、下划线或中文字符");

        return new UserName(value);
    }
}

/// <summary>
///
/// </summary>
/// <param name="Hash"></param>
/// <param name="Salt"></param>
[ComplexType]
public record UserPassword(
    [property: Column("Password")] string Password,
      [property: Column("Salt")] string Salt)
{
    public static UserPassword Create(string plainPassword)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
            throw new InvalidOperationException("密码不能为空");

        if (plainPassword.Length < 6)
            throw new InvalidOperationException("密码长度不能少于6位");

        var salt = GenerateSalt();
        var hash = HashPassword(plainPassword, salt);

        return new UserPassword(hash, salt);
    }

    public bool Verify(string plainPassword)
    {
        return Password == HashPassword(plainPassword, Salt);
    }

    private static string GenerateSalt()
    {
        var bytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = sha256.ComputeHash(combined);
        return Convert.ToBase64String(hash);
    }
}