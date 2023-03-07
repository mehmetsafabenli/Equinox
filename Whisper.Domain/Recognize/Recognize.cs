namespace Whisper.Domain.Recognize;

public class RecognizeSentence
{
    public List<Sentences> Sentences { get; set; }
    public DateTime RecognizeTime { get; set; }
}
public class Sentences
{
          string Text { get; set; }
}