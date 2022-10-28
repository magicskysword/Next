namespace NextLocalizationExporter;

public class LanguageSetting
{
    public string LanguageName { get; set; }
    public string LanguageDir { get; set; }
    
    public LanguageSetting(string languageName, string languageDir)
    {
        LanguageName = languageName;
        LanguageDir = languageDir;
    }
}