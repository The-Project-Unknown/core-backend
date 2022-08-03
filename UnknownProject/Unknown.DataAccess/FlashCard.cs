namespace Unknown.DataAccess;

public class FlashCard : BaseEntity, ITimeTracked
{
    public FlashCard(string question, List<string> answers)
    {
        Question = question;
        Answers = answers;
    }

    public string Question { get; set; }
    public List<string> Answers { get; set; } 
    
    public DateTime CreationDateTime { get; set; } 
    public DateTime UpdateDateTime { get; set; }
}