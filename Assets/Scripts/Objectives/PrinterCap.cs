using System.Collections;
using UnityEngine;

public class PrinterCap : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    [SerializeField] private Transform pivot;
    [SerializeField] private Vector3 closedRotation;
    [SerializeField] private Vector3 openRotation;
    [SerializeField] private float speed = 500f;

    [SerializeField] private Transform paperSlot;
    [SerializeField] private GameObject printedPaperPrefab;
    [SerializeField] private Transform printExitLocation;

    private bool isOpen;
    private bool isPrinting;

    private void Update()
    {
        Quaternion targetRotation = Quaternion.Euler(isOpen ? openRotation : closedRotation);
        pivot.localRotation = Quaternion.RotateTowards(pivot.localRotation, targetRotation, speed * Time.deltaTime);
    }

    public void Interact(InventoryNew inventory)
    {
        if (isPrinting) return;

        if (PlayerStateManager.Instance.currentState == PlayerStateManager.PlayerState.HasPaper)
        {
            Paper heldPaper = PlayerStateManager.Instance.HeldPaper;

            if (heldPaper != null)
            {
                StartCoroutine(FullyAutomatedPrintSequence(heldPaper));
            }
        }
        else
        {
            isOpen = !isOpen;
        }
    }

    private IEnumerator FullyAutomatedPrintSequence(Paper placedPaper)
    {
        isPrinting = true;
        string identityName = placedPaper.GetPaperName();

        isOpen = true;
        Debug.Log("Printer Lid opening automatically...");

        yield return new WaitForSeconds(0.01f);

        placedPaper.transform.SetParent(paperSlot);
        placedPaper.transform.localPosition = Vector3.zero;
        placedPaper.transform.localRotation = Quaternion.identity;

        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);
        Debug.Log("Paper dropped into slot.");

        yield return new WaitForSeconds(0.8f);

        isOpen = false;
        Debug.Log("Printer Lid closing automatically...");

        yield return new WaitForSeconds(1.0f);

        Debug.Log("Printer: BZZZ... Copying document...");
        yield return new WaitForSeconds(3.0f);

        Destroy(placedPaper.gameObject);

        isOpen = true;

        GameObject printedPage = Instantiate(printedPaperPrefab, printExitLocation.position, printExitLocation.rotation);

        Paper newPaperScript = printedPage.GetComponent<Paper>();
        if (newPaperScript != null)
        {
            Debug.Log($"Boom! {identityName} Printed!");
        }

        isPrinting = false;
    }
}
