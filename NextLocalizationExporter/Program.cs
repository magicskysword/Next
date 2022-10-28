namespace NextLocalizationExporter;

public class Program
{
    public static readonly LanguageSetting SourceLanguage = new LanguageSetting("简体中文", "schinese");

    public static readonly LanguageSetting[] OtherLanguages = new[]
    {
        new LanguageSetting("English", "english"),
    };
    
    public static void Main(string[] args)
    {
        var targetPath = @"D:\MiChangSheng\Mod\Next\Next";
        var export = new LanguageExporter(SourceLanguage, OtherLanguages);
        
        export.Export(targetPath);
    }
}