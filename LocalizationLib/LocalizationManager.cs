using System.Globalization;

namespace LocalizationLib;

public class LocalizationManager : ILocalizationManager
{
    private readonly CultureInfo _currentCulture = CultureInfo.CurrentCulture;
    private IDictionary<string, ILocalizationSource> _sources;

    public LocalizationManager(){
        _sources = new Dictionary<string, ILocalizationSource>();
    }

    public async Task<string?> GetStringAsync(Guid code, CultureInfo? culture = default, CancellationToken cancellationToken = default)
    {
        if(code == Guid.Empty)
        {
            return null;
        }

        var targetCulture = culture ?? _currentCulture;
       
        if(!_sources.TryGetValue(targetCulture.Name, out ILocalizationSource? targetSource))
        {
            return null;
        }

        return await targetSource!.FindStringAsync(code, cancellationToken);
    }

    public void RegisterSource(ILocalizationSource source, CultureInfo culture)
    {
        _sources.Add(culture.Name, source);
    }
}
