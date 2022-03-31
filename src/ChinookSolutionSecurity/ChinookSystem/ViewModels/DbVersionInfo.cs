using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.ViewModels
{
    public class DbVersionInfo
    {
        //the view is used by the "outsider user"
        //acces must match the method wher ethe class is used (typically public)
        //purpose: use to simply carry data
        //         consists of the "raw" data of the query
        //         create data fields as auto-implemented properties
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public DateTime ReleaseDate { get; set; }

    }
}
