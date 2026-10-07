using UnityEngine;

public class Printer : MonoBehaviour
{  
    public enum PrinterState
    {
        Opend,
        Closed, 
        Droped,
        HasPaper

    }

    public PrinterCap printerCap;

    private PrinterState currentState;

   

    void Update()
    {
        
    }
    public void ChangeState(PrinterState state)
    {
        currentState = state;

    }


    

     // need to check if paper in hand or not 
    // closed => if no paper dont accept paper  if paper  ( button red can press and print)\
    // opened =>  if paper place paper if not just close

     


}
