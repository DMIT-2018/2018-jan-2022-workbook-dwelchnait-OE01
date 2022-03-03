#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.ComponentModel.DataAnnotations;
#endregion

namespace ChinookSystem.ViewModels
{
	public class AlbumItem
	{
		public int AlbumId { get; set; }
		[Required(ErrorMessage = "Album title is required")]
		[StringLength(160, ErrorMessage = "Album title is limited to 160 characters")]
		public string Title { get; set; }
		public int ArtistId { get; set; }
		public int ReleaseYear { get; set; }
		[StringLength(50, ErrorMessage = "Album label is limited to 50 characters")]
		public string ReleaseLabel { get; set; }
	}
}
