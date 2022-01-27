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
	//Conversions
	//.ToList()
	//can convert a collection to a list at any time
	
	//display all ablums and their tracks. display the album title
	//artist name and album tracks. For each track display the song
	//name and playtime in seconds. Show only albums with 25 or more tracks
	
	List<AlbumTracks> albumlist = Albums
					.Where(a => a.Tracks.Count() >= 25)
					.Select(a => new AlbumTracks
						{
							Title = a.Title,
							Artist = a.Artist.Name,
						    Songs = a.Tracks
									.Select(t => new SongItem
									        {
											   Song = t.Name,
											   Playtime = t.Milliseconds / 1000.0
											} )
									.ToList()
						})
					.ToList()
					//.Dump()
					;
					
	//if done in a BLL service method
	//typically you would be returning the collection as a List<T>
	//return statement:  return albumlist.ToList();
	
	
	//Using FirstOrDefault()
	//this method returns the first instance of a collection
	//the expected collection may have 0, 1 or more instances (rows)
	//if there is a row (s), the first is used
	//if there is no row(s), using First(), one get an Exception error
	//if there is no row(s), using FirstOrDefault(), on gets an object default - > null
	
	//Find the first album of Deep Purple
	
	string artistparam = "Deep Purple";
	var resultsFOD = Albums
					.Where(a => a.Artist.Name.Equals(artistparam))
					.Select(a => a)
					.OrderBy(a => a.ReleaseYear)
					.FirstOrDefault()
					//.Dump()
					;
	//after the query, I am going to do further processing
	//if (resultsFOD != null)
	//	resultsFOD.Dump();
	//else
	//	Console.WriteLine($"No albums found for artist {artistparam}");
		
	//Using SingleOrDefault()
	//this method is similar to FirstOrDefault in all aspects except:
	//	the collection is expect only 0 or 1 instance (row)
	//If you collection can possible have more than one instance, DO NOT USE,
	//	you will generate an Exception
	
	//Find the album by the albumid
	
	int albumid = 1;
	var resultsSOD = Albums	
					.Where(a => a.AlbumId == albumid)
					.Select(a => a)
					.SingleOrDefault()
					;
	//if (resultsSOD != null)
	//	resultsSOD.Dump();
	//else
	//	Console.WriteLine($"No albums found for supplied id {albumid}");
		
	//Using Distinct()
	//removes duplicate collection instances
	
	//display a list of customer countries
	var resultsC = Customers
					.OrderBy(c => c.Country )
					.Select(c => c.Country)
					.Distinct()
					//.Dump()
					;
					
	
	//.Take() and .Skip()
	//these methods expect a numeric value
	//.TakeWhile(delegate) and .SkipWhile(delegate), action continues
	//		while delegate is true
	
	//in 1517, when using the paginator to do paging on your web page
	//  you called your BLL service method with additional parameters
	//  (pagesize, pagenumber) so that only need instances from your
	//	query collection were actually returned to the web page for display
	
	//a) service method receive: query parameter, pagesize, pagenumber
	//b) your query was executed
	//c) obtianed the total count of the return collection (.Count())
	//d) calculated the number of records to skip (pagenumber - 1) * pagesize
	//e) on the return statement, you selected a certain set of rows of the
	//		collection using
	//		return variablename.Skip(skipRows).Take(pagesize).ToList();
	
	//Any and ALL
	//Genres.Count().Dump();  25
	
	//show genres that have tracks which are not on any playlist
	Genres
		.Where(g => g.Tracks.Any(tr => tr.PlaylistTracks.Count( ) == 0))
		.Select(g => g)
		.Dump()
		;

	//show genres that have all their tracks appearing at least once
	//  on a playlist
	Genres
		.Where(g => g.Tracks.All(tr => tr.PlaylistTracks.Count() > 0))
		.Select(g => g)
		.Dump()
		;
		
	//there maybe times that using a !Any() -> All() and a !All() -> Any() result
	
}

// You can define other methods, fields, classes and namespaces here

public class SongItem
{
	public string Song {get;set;}
	public double Playtime {get;set;}
}

public class AlbumTracks
{
	public string Title{get;set;}
	public string Artist{get;set;}
	public List<SongItem> Songs {get;set;}
}