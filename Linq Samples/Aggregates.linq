<Query Kind="Expression">
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

//Aggregates
//.Count()				Counts the number of instances in the collection
//.Sum(x => ...)		Sums (totals) a numeric field (numeric expression) in the collection
//.Min(x => ...)		Finds the minumum value of a collection of values based on a field
//.Max(x => ...)		Finds the maximum value of a collection of values based on a field
//.Average(x => ...)	Finds the numeric average value of a collection of numeric values on a field

//IMPORTANT!!!
//Aggregates work ONLY on a collection of values for a particular field
//Aggregates DO NOT work on a single instance (based on datatype)

//syntax
//query
//  (from ....
//    ...
//  select expression).aggregate()

//method
//	collectionset.aggregate(x => expression)
//	collectionset.Select(x => expression).aggregate()
// 	collectionset.Count() //.Count() does not contain an delegate
//the expression is resolved to a single field value per instance for Sum,Min,Max and Average

//Find the average playing time (length) of tracks in our music collection


//though process
//average is an aggregate
//what is the collection? a track is a member of the Tracks table
//What is the expression? Millisecond indicates track playing time (length)

//query
(from tr in Tracks
select tr.Milliseconds).Average()

//method
Tracks.Average() //you want average to work but average does not know what 
				// property to use???
Tracks.Select(tr => tr.Milliseconds).Average()
Tracks.Average(tr => tr.Milliseconds)
Tracks.Count()
Tracks.Sum(tr => tr.Milliseconds)


//List all Albums of the 60s showing the title, artist and various
//aggregates for the albums containing tracks.

//For each album, show the number of tracks, the longest playing track,
//the shortest playing track, the total price of all tracks and the
//average playing length of the album tracks.

//Hint: Albums has two navigational properties
//		Artist: points to the single parent record
//		Tracks: points to the collection of child records (Tracks) of the album

Albums
	.Where(x => x.ReleaseYear > 1959 && x.ReleaseYear < 1970
	        && x.Tracks.Count() > 0)
	.Select(x => new 
	       {
		      Title = x.Title,
			  Artist = x.Artist.Name,
		      NumberofTracks = x.Tracks.Count(),
		      LongestTrack = x.Tracks.Max(tr => tr.Milliseconds),
		      ShortestTrack = (from tr in x.Tracks
			                   select tr.Milliseconds).Min(),
			  TotalPrice = x.Tracks.Select(tr => tr.UnitPrice ).Sum(),
			  AverageLength = x.Tracks.Average(tr => tr.Milliseconds)
		   } )













