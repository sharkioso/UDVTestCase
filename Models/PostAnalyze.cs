public class PostAnalyze
{
    public int ID { get; set; }
    public string PostText { get; set; }
    public DateTime AnalyzeDate { get; set; }
    public Dictionary<char, int> LetterCount { get; set; }

}