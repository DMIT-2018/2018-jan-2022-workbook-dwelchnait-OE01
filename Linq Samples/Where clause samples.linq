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

//Where clause
//filter method
//the conditions are setup as you would in C#
//beware that Linqpad (Linq) may NOT like some C# syntax (DateTime)
//beware that Linq is converted to SQL which may not
//	like certain C# syntax because it could not be converted

//syntax
//query
// where condition [logical operator condition2 ...]

//method
//Notice that this syntax makes uses of LambDa expressions
//LambDa are common when perfroming Linq with the method syntax
//  .Where(LambDa expression
//  .Where(x => condition [logical operator condition2 ...])


//Find all albums released in a particular year. Display the
//  entire album record

//year 2000
Albums
	.Where(x => x.ReleaseYear == 2000)
	.Select(x => x)

//Find all albums released in the 90s (1990 - 1999)
//Display the entire album record

Albums
	.Where(x => x.ReleaseYear >= 1990
			&& x.ReleaseYear < 2000)
	.Select(x => x)

//Find all the albums of the artist Queen.
//Display the entire album record

//concern: the artist name is in another table
//			in an sql Select you would be using an inner Join
//			in Linq you DO NOT need to specific your inner Joins
//			if there is an existing "navigational properties" within
//			your entity that reflects the relationship between
//			the tables.

//.Equals(....) or == is an exact match, in sql = or like 'string'
//.Contains(...) is a string match, in sql like '%' + string + '%'

Albums
	.Where(x => x.Artist.Name =="Queen")
	.Select(x => x)


//find all albums where the producer (Label) is unknown (null)

Albums
	.Where(x => x.ReleaseLabel == null)
	.Select(x => x)


