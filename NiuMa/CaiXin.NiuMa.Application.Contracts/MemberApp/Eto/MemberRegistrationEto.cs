namespace CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;

/// <summary>
///
/// </summary>
/// <param name="OrderId"></param>
/// <param name="UserId"></param>
/// <param name="UserPhone"></param>
/// <param name="TotalAmount"></param>
public record MemberRegistrationEto(int OrderId, int UserId, string UserPhone, decimal TotalAmount);