using GPhotosSearch.Dependencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPhotosSearch.Dependencies.UserInputOutput
{
    public interface IInputOutputHandler
    {
        string ReadInput();
        void WriteOutput(string message);
        void WriteOutput(IEnumerable<MediaItem> mediaItems);
    }
}
