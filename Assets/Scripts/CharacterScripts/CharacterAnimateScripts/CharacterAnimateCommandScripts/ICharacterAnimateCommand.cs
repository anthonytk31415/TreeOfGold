using System; 

public interface ICharacterAnimateCommand {
    public void InstantiateCommand();  
    public void ProcessCommand();  
    public void TerminateCommand();  
    public bool Processing(); 


}