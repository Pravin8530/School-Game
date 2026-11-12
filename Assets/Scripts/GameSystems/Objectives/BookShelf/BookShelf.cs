
using System.Collections.Generic;
using UnityEngine;

public class BookShelf : MonoBehaviour
{
    [Header("Books")]
    [SerializeField] private List<Book> allBooks = new List<Book>();

    [Header("Shelf Slots")]
    [SerializeField] private Transform[] bookSlots;

    [SerializeField] private float dropDistance = 1.5f;

    [Header("Puzzle")]
    [SerializeField] private PasswordText passwordText;

    [Header("Drawer")]
    [SerializeField] private Transform drawer;
    [SerializeField] private Transform drawerOpenPosition;

    private List<Book> currentBooks = new List<Book>();
    private List<Book> solution = new List<Book>();

    private int lastHoverIndex = -1;
    private bool isSolved;

    [Header("Books")]
    //[SerializeField] private List<Book> allBooks = new List<Book>(); // ALL books in the puzzle (solution pool)
    [SerializeField] private List<Book> initialBooksOnShelf = new List<Book>(); // Only the books on the shelf at start

    // private void Start()
    // {
    //     currentBooks.Clear();

    //     // Generate random solution.
    //     solution = new List<Book>(allBooks);

    //     ShuffleBooks(solution);

    //     passwordText.UpdatePasswordText(solution);

    //     Debug.Log("Book puzzle solution generated.");
    // }


    // private void Start()
    // {
    //     // Populate currentBooks with the initial books on the shelf
    //     currentBooks = new List<Book>(allBooks);

    //     // Generate random solution.
    //     solution = new List<Book>(allBooks);

    //     ShuffleBooks(solution);

    //     passwordText.UpdatePasswordText(solution);

    //     // Update their starting positions on the shelf slots immediately
    //     UpdateBookPositions(currentBooks);

    //     Debug.Log("Book puzzle solution generated.");
    // }

    private void Start()
    {
        // Only track the books that actually start on the shelf
        currentBooks = new List<Book>(initialBooksOnShelf);

        // Generate random solution using all puzzle books.
        solution = new List<Book>(allBooks);
        ShuffleBooks(solution);

        passwordText.UpdatePasswordText(solution);

        // Position only the initial shelf books onto their slots
        UpdateBookPositions(currentBooks);

        Debug.Log("Book puzzle solution generated.");
    }


    private void Update()
    {
        if (isSolved && drawer != null)
        {
            drawer.localPosition = Vector3.Lerp(
                drawer.localPosition,
                drawerOpenPosition.localPosition,
                Time.deltaTime * 10f
            );
        }
    }


    private void ShuffleBooks(List<Book> books)
    {
        for (int i = 0; i < books.Count; i++)
        {
            int randomIndex = Random.Range(i, books.Count);

            (books[i], books[randomIndex]) =
                (books[randomIndex], books[i]);
        }
    }


    // Called when player picks up a book.
    public void OnBookPickedUp(Book book)
    {
        currentBooks.Remove(book);

        lastHoverIndex = -1;

        UpdateBookPositions(currentBooks);
    }


    // Called while player moves a book.
    public void OnBookHover(Vector3 position)
    {
        if (!IsNearShelf(position))
        {
            ResetHover();
            return;
        }

        int index = GetClosestSlotIndex(position);

        if (index == lastHoverIndex)
            return;

        lastHoverIndex = index;

        List<Book> preview = new List<Book>(currentBooks);

        index = Mathf.Clamp(index, 0, preview.Count);

        preview.Insert(index, null);

        UpdateBookPositions(preview);
    }


    // Called when player drops a book.
    public void OnBookDropped(Book book, Vector3 position)
    {
        lastHoverIndex = -1;

        // Dropped outside shelf.
        if (!IsNearShelf(position))
        {
            UpdateBookPositions(currentBooks);
            return;
        }

        int index = GetClosestSlotIndex(position);

        index = Mathf.Clamp(index, 0, currentBooks.Count);

        currentBooks.Insert(index, book);
        UpdateBookPositions(currentBooks);

        CheckPassword();
    }


    private void CheckPassword()
    {
        if (isSolved)
            return;

        if (currentBooks.Count != allBooks.Count)
            return;

        for (int i = 0; i < solution.Count; i++)
        {
            if (currentBooks[i] != solution[i])
                return;
        }

        isSolved = true;
        if (isSolved && drawer != null)
        {
            drawer.localPosition = Vector3.Lerp(
                drawer.localPosition,
                drawerOpenPosition.localPosition,
                Time.deltaTime * 10f
            );

            if (Vector3.Distance(
                drawer.localPosition,
                drawerOpenPosition.localPosition) < 0.01f)
            {
                drawer.localPosition = drawerOpenPosition.localPosition;
            }
        }
        Debug.Log("BOOK PUZZLE SOLVED!");
    }


    private bool IsNearShelf(Vector3 position)
    {
        foreach (Transform slot in bookSlots)
        {
            if (Vector3.Distance(position, slot.position) <= dropDistance)
                return true;
        }

        return false;
    }


    private int GetClosestSlotIndex(Vector3 position)
    {
        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < bookSlots.Length; i++)
        {
            float distance = Vector3.Distance(
                position,
                bookSlots[i].position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }


    private void ResetHover()
    {
        lastHoverIndex = -1;

        UpdateBookPositions(currentBooks);
    }


    private void UpdateBookPositions(List<Book> books)
    {
        for (int i = 0; i < books.Count; i++)
        {


            if (books[i] != null && i < bookSlots.Length)
            {
                books[i].SetTargetSlot(bookSlots[i]);
            }
        }
    }


}
