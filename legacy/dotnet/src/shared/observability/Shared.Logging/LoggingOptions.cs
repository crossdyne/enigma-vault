namespace Shared.Logging
{
    public class LoggingOptions
    {
        public const string SectionName = "LoggingOptions";
        public bool UseConsole { get; set; } = true;
        public bool UseFile { get; set; } = false;
        public string FilePath { get; set; } = "logs/log-.log";
        public bool UseSeq { get; set; } = false;
        public string SeqUrl { get; set; } = null!;
        public string Level { get; set; } = "Information";
    }
}