using UnityEngine;

public class PrinterCap : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    [SerializeField] private Transform pivot;

    [SerializeField] private Vector3 closedRotation;
    [SerializeField] private Vector3 openRotation;

    [SerializeField] private float speed = 500f;

    private bool isOpen;

    public Printer printer;


    private void Update()
    {
        Quaternion targetRotation = Quaternion.Euler(
            isOpen ? openRotation : closedRotation
        );

        pivot.localRotation = Quaternion.RotateTowards(
            pivot.localRotation,
            targetRotation,
            speed * Time.deltaTime
        );
    }

    public void Interact()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            printer.ChangeState(Printer.PrinterState.Opend);
        }
        else
        {

            printer.ChangeState(Printer.PrinterState.Closed);
        }
    }
}