namespace CaiXin.NiuMa.Domain.Ports;

public interface IPageQuery<TRequest, TResultDto>
{
    public Task<(List<TResultDto> list, long total)> PageQueryAsync(TRequest request, CancellationToken cancellationToken = default);
}