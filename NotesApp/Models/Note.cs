using SQLite;

namespace NotesApp.Models
{
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? S { get; set; }

    }
}
