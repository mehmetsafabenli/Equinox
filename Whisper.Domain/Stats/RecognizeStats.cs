namespace Whisper.Domain.Stats;

public class RecognizeStats
{
    public string Path { get; set; }
    public List<Segment> Segments { get; set; }
}

public  class Segment
{
    public string Text { get; set; }
    public string Language { get; set; }
    public float Time { get; set; }
}