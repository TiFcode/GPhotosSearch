using GPhotosSearch.Dependencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPhotosSearch.Dependencies.UserInputOutput
{
    internal class ConsoleInputOutputHandler : IInputOutputHandler
    {
        public string ReadInput()
        {
            return Console.ReadLine();
        }

        public void WriteOutput(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteOutput(IEnumerable<MediaItem> mediaItems)
        {
            foreach (var mediaItem in mediaItems)
            {
                Console.WriteLine($"ID: {mediaItem.Id}, Filename: {mediaItem.Filename}, MIME Type: {mediaItem.MimeType}");
            }
        }
    }
}
