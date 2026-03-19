using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using CaiXin.NiuMa.Domain.Shared.Response;

namespace CaiXin.NiuMa.Application.Contracts.MemberApp;

public interface IMemberApp
{
    Task<ApiResult<string>> MemberRegistrationAsync(MemberRegistrationDto cmd, CancellationToken token);
}