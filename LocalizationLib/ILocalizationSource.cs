
namespace LocalizationLib;

public interface ILocalizationSource {
    Task<string?> FindStringAsync(Guid code, CancellationToken cancellationToken = default);
}