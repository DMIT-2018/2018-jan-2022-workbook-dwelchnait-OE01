<Query Kind="Program">
  <Connection>
    <ID>ac19c5d1-a507-4043-bafb-b647e09ebfb9</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	//Strongly typed query datasets
	
	//Strongly typed refers to a C# defined datatype (int, string, decimal, ..)
	//OR developer defined datatype (class, struct, array)
	
	//Anonymous dataset from a query does NOT have a specified class definition
	//Strongly typed query datasets HAS a specified class definition
	
	//Find all songs that contain a partial string of the Track name.
	//List Album title, Song name, and Artist Name.
	
	//imagine the following is in the code-behind of our Razor Page
	string partialSongName = "dance"; //[BindProperty] partialSongName{get;set;}
	List<SongList> results = SongByPartialName(partialSongName);
	results.Dump();
}

// You can define other methods, fields, classes and namespaces here

public class SongList
{
	public string Title{get;set;}
	public string Song{get;set;}
	public string Artist{get;set;}
}

//image you have the following method in a BLL service class

List<SongList> SongByPartialName(string partialSongName)
{
	IEnumerable<SongList> songCollection = Tracks
						.Where(t => t.Name.Contains(partialSongName))
						.Select(t => new SongList
								{
									Title = t.Album.Title,
									Song = t.Name,
									Artist = t.Album.Artist.Name
								});
	return songCollection.ToList();
}