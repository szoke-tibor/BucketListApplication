public void OnPostAdd()
{
    var listName = Request.Form["listSelect"];
    var ListToUpdate = await _context.Lists.FindAsync(listName);
    if (ListsToUpdate == null)
    {
        return NotFound();
    }
    if (await TryUpdateModelAsync<List>(ListToUpdate,"",l => l.Add()))
    {
        await _context.SaveChangesAsync();
    }

    return RedirectToPage("./Index");
}
