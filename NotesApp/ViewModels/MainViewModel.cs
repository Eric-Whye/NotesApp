using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using NotesApp.Models;
using NotesApp.Services;

namespace NotesApp.ViewModels
{
    public class MainViewModel : BindableObject, IQueryAttributable
    {
        public ObservableRangeCollection<Note> Notes { get; set; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand<Note> DeleteCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<Note> GoToNoteCommand { get; }

        public MainViewModel()
        {
            Notes = new ObservableRangeCollection<Note>();
            AddCommand = new AsyncCommand(Add);
            DeleteCommand = new AsyncCommand<Note>(Remove);
            RefreshCommand = new AsyncCommand(Refresh);
            GoToNoteCommand = new AsyncCommand<Note>(GoToNote);

            _ = Refresh();
        }

        //These are async void and not async Task because they are events
        async Task Add()
        {
            await NoteService.AddNote(String.Empty);
            await Refresh();
        }

        async Task Remove(Note note)
        {
            await NoteService.RemoveNote(note.Id);
            await Refresh();
        }
            
        async Task Refresh()
        {
            Notes.Clear();
            var notes = await NoteService.GetNotes();
            Notes.AddRange(notes);
        }

        //Go To Note when Note from ListView is clicked/pressed
        async Task GoToNote(Note note)
        {
            string jsonNote = JsonConvert.SerializeObject(note);
            await Shell.Current.GoToAsync($"{nameof(NotePage)}?JsonNote={Uri.EscapeDataString(jsonNote)}");
        }


        //Receive Navigation Data from NotePage
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("JsonNote"))
            {
                string JsonNote = query["JsonNote"] as string;
                Note Note = JsonConvert.DeserializeObject<Note>(Uri.UnescapeDataString(JsonNote));
                _ = NoteService.EditNote(Note);
                Refresh();
            }

        }
    }

}
