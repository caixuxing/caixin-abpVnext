namespace CaiXin.BackgroundJob;


public interface IJob<in TArgs>
{
    public Task<bool> ExecuteAsync(TArgs args);
}