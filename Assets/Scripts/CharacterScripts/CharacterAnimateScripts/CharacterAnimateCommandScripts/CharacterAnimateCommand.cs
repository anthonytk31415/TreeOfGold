using System; 

public interface CharacterAnimateCommand {
    public void InstantiateCommand();  
    public void ProcessCommand();  
    public void TerminateCommand();  
    public bool Processing(); 
}