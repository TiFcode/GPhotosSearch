using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPhotosSearch.Dependencies
{
    public class Constants
    {
        public const string CONST_URL_GoogleAPI_Auth_Photos_ReadOnly = "https://www.googleapis.com/auth/photoslibrary.readonly";
        public const string CONST_URL_GoogleAPI_Photos_Search = $"https://photoslibrary.googleapis.com/v1/mediaItems:search";
    }
}
