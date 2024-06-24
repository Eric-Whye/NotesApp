using Newtonsoft.Json;
using NotesApp.Models;
using System.Windows.Input;

namespace NotesApp.ViewModels
{
    public class NotePageViewModel : BindableObject, IQueryAttributable
    {

        public NotePageViewModel()
        {
            GoBackCommand = new Command(GoBack);
            SaveCommand = new Command(Save);
        }

        Note note;
        public Note Note
        {
            get => note;
            set
            {
                if (value == null)
                    return;

                note = value;
                Text = value.S;
            }
        }

        string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }


        public ICommand GoBackCommand { get; }
        async void GoBack() //Go Back to Main Page
        {
            string JsonNote = JsonConvert.SerializeObject(Note);
            await Shell.Current.GoToAsync($"..?JsonNote={Uri.EscapeDataString(JsonNote)}");
        }

        public ICommand SaveCommand { get; }
        void Save()
        {
            Note.S = Text;
            //Save the String somewhere   
        }

        //Receiving Navigation Data (Note object) from Main Page 
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("JsonNote"))
            {
                string JsonNote = query["JsonNote"] as string;
                Note note = JsonConvert.DeserializeObject<Note>(Uri.UnescapeDataString(JsonNote));
                Note = note;
            }

        }
    }
}
