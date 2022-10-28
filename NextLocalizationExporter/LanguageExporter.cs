namespace NextLocalizationExporter;

public class LanguageExporter
{
    private LanguageSetting _source;
    private LanguageSetting[] _targers;
    
    private Dictionary<string, string> _sourceDictionary = new Dictionary<string, string>();
    private Dictionary<string, int> _testResult = new Dictionary<string, int>();

    public LanguageExporter(LanguageSetting sourceLanguage, LanguageSetting[] otherLanguages)
    {
        _source = sourceLanguage;
        _targers = otherLanguages;
    }

    public void Export(string directory)
    {
        Clear();
        Console.WriteLine("Exporting...");
        Console.WriteLine("WorkDirectory: " + directory);
        ReadLanguage(directory);
        ReadCodes(directory);
        GenerateLanguageDictionary();
        ExportLanguageFiles(_source, directory);
        foreach (var language in _targers)
        {
            ExportLanguageFiles(language, directory);
        }
        //ReplaceCodes(directory);
    }

    private void Clear()
    {
        _sourceDictionary.Clear();
        _testResult.Clear();
    }

    private DirectoryInfo GetCodeDirectory(string directory) => new(Path.Combine(directory, "Scr"));
    private DirectoryInfo GetLanguageDirectory(string directory) => new(Path.Combine(directory, "NextConfig/Language"));

    /// <summary>
    /// 读取原有的语言文件
    /// </summary>
    /// <param name="directory"></param>
    private void ReadLanguage(string directory)
    {
        
    }
    
    private void ReadCodes(string directory)
    {
        var codeDir = GetCodeDirectory(directory);
        foreach (var file in codeDir.GetFiles("*.cs", SearchOption.AllDirectories))
        {
            var text = File.ReadAllText(file.FullName);
            var todoStrings = CodeParser.ParseI18NTodoString(text);
            var namespaceName = CodeParser.ParseNamespace(text);
            var fileName = Path.GetFileNameWithoutExtension(file.FullName);
            if (namespaceName != null)
            {
                TryAddToSourceDictionary($"{namespaceName}.{fileName}", todoStrings);
            }
            else
            {
                TryAddToSourceDictionary($"file.{fileName}", todoStrings);
            }
        }
    }

    private void TryAddToSourceDictionary(string rawKey, IEnumerable<string> todoStrings)
    {
        foreach (var todoString in todoStrings)
        {
            var key = TestKey(rawKey);
            _sourceDictionary.Add(key, todoString);
            Console.WriteLine($"Add [{key} : {todoString}] to source dictionary");
        }
    }

    private string TestKey(string rawKey)
    {
        var key = rawKey.Replace(".", "_");
        var keyCount = 0;
        if(_testResult.ContainsKey(key))
        {
            keyCount = _testResult[key];
        }

        while (_sourceDictionary.ContainsKey($"{key}_{keyCount}"))
        {
            keyCount++;
        }

        _testResult[key] = keyCount;
        return $"{key}_{keyCount}";
    }

    /// <summary>
    /// 根据源语言生成语言字典
    /// </summary>
    private void GenerateLanguageDictionary()
    {
        
    }
    
    /// <summary>
    /// 导出语言文件
    /// </summary>
    /// <param name="language"></param>
    /// <param name="directory"></param>
    private void ExportLanguageFiles(LanguageSetting language, string directory)
    {
        
    }
}