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

    public void Interact()
    {
        if (isPrinting) return;

        if (PlayerStateManager.Instance.currentState == PlayerStateManager.PlayerState.HasPaper)
        {
            Paper heldPaper = PlayerStateManager.Instance.HeldPaper;

            if (heldPaper != null)
            {
                // Kick off the fully automated loop!
                StartCoroutine(FullyAutomatedPrintSequence(heldPaper));
            }
        }
        else
        {
            // Optional: If player clicks without paper, just toggle the door normally
            isOpen = !isOpen;
        }
    }

    private IEnumerator FullyAutomatedPrintSequence(Paper placedPaper)
    {
        isPrinting = true;
        string identityName = placedPaper.GetPaperName();

        // 1. AUTOMATICALLY OPEN DOOR
        isOpen = true;
        Debug.Log("Printer Lid opening automatically...");

        // Wait for the lid to finish swinging open
        yield return new WaitForSeconds(1.0f);

        // 2. AUTOMATICALLY DROP PAPER THERE
        placedPaper.transform.SetParent(paperSlot);
        placedPaper.transform.localPosition = Vector3.zero;
        placedPaper.transform.localRotation = Quaternion.identity;

        // Clear the player's hands right away
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);
        Debug.Log("Paper dropped into slot.");

        // Wait a tiny bit so the player sees the paper on the glass
        yield return new WaitForSeconds(0.8f);

        // 3. AUTOMATICALLY CLOSE DOOR
        isOpen = false;
        Debug.Log("Printer Lid closing automatically...");

        // Wait for lid to close completely
        yield return new WaitForSeconds(1.0f);

        // 4. PRINTING DELAY
        Debug.Log("Printer: BZZZ... Copying document...");
        yield return new WaitForSeconds(3.0f);

        // 5. DESTROY ORIGINAL AND SPIT OUT NEW ONE
        Destroy(placedPaper.gameObject);

        // Pop lid back open automatically so player can use it again later
        isOpen = true;

        // Spawn the copy out of the machine tray
        GameObject printedPage = Instantiate(printedPaperPrefab, printExitLocation.position, printExitLocation.rotation);

        Paper newPaperScript = printedPage.GetComponent<Paper>();
        if (newPaperScript != null)
        {
            Debug.Log($"Boom! {identityName} Printed!");
        }

        isPrinting = false;
    }
}
