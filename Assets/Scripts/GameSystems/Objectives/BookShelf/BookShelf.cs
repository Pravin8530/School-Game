using System.Collections.Generic;
using UnityEngine;

public class BookShelf : MonoBehaviour
{
    [Header("Books")]
    [SerializeField] private List<Book> allBooks = new List<Book>();
    [SerializeField] private List<Book> initialBooksOnShelf = new List<Book>();

    [Header("Shelf Slots")]
    [SerializeField] private Transform[] bookSlots;
    [SerializeField] private float dropDistance = 1.5f;

    [Header("Puzzle")]
    [SerializeField] private PasswordText passwordText;

    [Header("Drawer")]
    [SerializeField] private Transform drawer;
    [SerializeField] private Transform drawerOpenPosition;
    [SerializeField] private float drawerMoveSpeed = 10f;

    private readonly List<Book> currentBooks = new List<Book>();
    private readonly List<Book> solution = new List<Book>();

    private int lastHoverIndex = -1;
    private bool isSolved;

    private void Start()
    {
        currentBooks.Clear();
        currentBooks.AddRange(initialBooksOnShelf);

        solution.Clear();
        solution.AddRange(allBooks);
        ShuffleBooks(solution);

        if (passwordText != null)
        {
            passwordText.UpdatePasswordText(solution);
        }

        UpdateBookPositions(currentBooks);
    }

    private void Update()
    {
        if (!isSolved || drawer == null || drawerOpenPosition == null)
            return;

        drawer.localPosition = Vector3.Lerp(
            drawer.localPosition,
            drawerOpenPosition.localPosition,
            Time.deltaTime * drawerMoveSpeed
        );

        if (Vector3.Distance(drawer.localPosition, drawerOpenPosition.localPosition) < 0.01f)
        {
            drawer.localPosition = drawerOpenPosition.localPosition;
        }
    }

    private void ShuffleBooks(List<Book> books)
    {
        for (int i = 0; i < books.Count; i++)
        {
            int randomIndex = Random.Range(i, books.Count);
            (books[i], books[randomIndex]) = (books[randomIndex], books[i]);
        }
    }

    public void OnBookPickedUp(Book book)
    {
        if (book == null)
            return;

        book.IsOnShelf = false;
        currentBooks.Remove(book);
        lastHoverIndex = -1;

        UpdateBookPositions(currentBooks);
    }

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

    public bool TryPlaceBook(Book book, Vector3 position)
    {
        lastHoverIndex = -1;

        if (book == null || !IsNearShelf(position))
        {
            UpdateBookPositions(currentBooks);
            return false;
        }

        int index = GetClosestSlotIndex(position);
        index = Mathf.Clamp(index, 0, currentBooks.Count);

        book.IsOnShelf = true;
        currentBooks.Insert(index, book);
        UpdateBookPositions(currentBooks);

        CheckPassword();
        return true;
    }

    private void CheckPassword()
    {
        if (isSolved || currentBooks.Count != allBooks.Count || currentBooks.Count != solution.Count)
            return;

        for (int i = 0; i < solution.Count; i++)
        {
            if (currentBooks[i] != solution[i])
                return;
        }

        isSolved = true;
        Debug.Log("Book puzzle solved.");
    }

    private bool IsNearShelf(Vector3 position)
    {
        if (bookSlots == null)
            return false;

        foreach (Transform slot in bookSlots)
        {
            if (slot != null && Vector3.Distance(position, slot.position) <= dropDistance)
                return true;
        }

        return false;
    }

    private int GetClosestSlotIndex(Vector3 position)
    {
        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        if (bookSlots == null)
            return closestIndex;

        for (int i = 0; i < bookSlots.Length; i++)
        {
            if (bookSlots[i] == null)
                continue;

            float distance = Vector3.Distance(position, bookSlots[i].position);
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
        if (bookSlots == null)
            return;

        for (int i = 0; i < books.Count && i < bookSlots.Length; i++)
        {
            if (books[i] != null && bookSlots[i] != null)
            {
                books[i].SetTargetSlot(bookSlots[i]);
            }
        }
    }
}