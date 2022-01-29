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

//Use non adjacent tables for reporting in a query
//Using Multiple reporting tables TableA -> TableB -> TableC (grandparent->parent->child)
//report from TableA and TableC but not from TableB (directly)
//trying to reduce the number of nested queries

//report from Albums (A) and Playlistracks (C) but not from Tracks (B)

//one possible way of doing the query s to physically join the involved tables
//	using the join clause

//AVOID!!: this limits and confinds the optimization that Linq and Sql can create.
//			it works BUT you should FIRST ALWAYS consider using navigational
//				properties BEFORE doing your own join conditions

//List all albums (title) of the 70's with the number of songs that exist
//	on the album (aggregate against Tracks). Also, list the PlaylistName and
//	the owner of the playlist, and the song

//Album -> Tracks -> PlaylistTracks (is a child to Playlist and Tracks)
//aggregation Tracks.Count()
//PlaylistTracks can extract info from parents (Playlist and Tracks)

//method and query (multiple from clauses) syntax
Albums
	.Where(x => x.ReleaseYear > 1969 && x.ReleaseYear < 1980)
	.Select(x => new
				{
					title = x.Title,
					trackcount = x.Tracks.Count( ),
					playlistsongs = from tr in x.Tracks
									 from pltr in tr.PlaylistTracks
									 select new
									 {
									 	song = pltr.Track.Name,
										playlistname = pltr.Playlist.Name,
										listowner = pltr.Playlist.UserName
									 }
				})
	.OrderBy(x => x.title )

//method syntax only
Albums
   .Where(x => ((x.ReleaseYear > 1969) && (x.ReleaseYear < 1980)))
   .Select(
	  x =>
		 new
		 {
			 title = x.Title,
			 trackcount = x.Tracks.Count(),
			 playlistsongs = x.Tracks
			   .SelectMany(
				  tr => tr.PlaylistTracks,
				  (tr, pltr) =>
					 new
					 {
						 song = pltr.Track.Name,
						 playlistname = pltr.Playlist.Name,
						 listowner = pltr.Playlist.UserName
					 }
			   )
		 }
   )
   .OrderBy(x => x.title)









