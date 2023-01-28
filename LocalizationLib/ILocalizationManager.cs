
using System.Globalization;

namespace LocalizationLib;

public interface ILocalizationManager {
    Task<string?> GetStringAsync(Guid code, CultureInfo? culture = default, CancellationToken cancellationToken = default);

    void RegisterSource(ILocalizationSource source, CultureInfo culture);
}