using NotesApp.Models;
using SQLite;

namespace NotesApp.Services
{
    public static class NoteService
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "NotesData");
            
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Note>();
        }

        public static async Task<Note> GetNoteById(int id)
        {
            await Init();

            return await db.Table<Note>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public static async Task AddNote(string text)
        {
            await Init();

            Note note = new Note()
            {
                S = text
            };

            int id = await db.InsertAsync(note);
        }

        public static async Task EditNote(Note note)
        {
            await Init();

            await db.InsertOrReplaceAsync(note);
        }

        public static async Task RemoveNote(int id)
        {
            await db.DeleteAsync<Note>(id);
        }
        

        public static async Task<IEnumerable<Note>> GetNotes()
        {
            await Init();

            var notes = await db.Table<Note>().ToListAsync();
            return notes;
        }
    }
}
