<Query Kind="Statements">
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

//Statement IDE

//you can have mulitple queries written in this IDE environmnet
//you can execute a query individually by highlighting
//	the desired query first
//BY DEFAULT executing the file in this environment executes
//		ALL queries, top to bottom

//IMPORTANT
//queries in this environment MUST be written using the
//		C# language grammar for a statement. This means
//		that each statement must end in a semi-colon


//********************************
//it appears that using Linqpad 7 that a method syntax query
//	does NOT need the semi-colon. Be careful about trying to
//  implement the query in a C# Class library project without
//  the semi-colon.
//********************************

//results MUST be placed in areciving variable for query syntax
//to display the results, use the Linqpad method .Dump()

//query syntax
//Find all albums released in a particular year. Display the
//  entire album record

//year 2000
var paramyear = 1990; //this value is being passed into the BLL method

var resultsq = from x in Albums
				where x.ReleaseYear == paramyear
				select x;
				
resultsq.Dump();

//method syntax
Albums
	.Where(x => x.ReleaseYear == 2000)
	.Select(x => x)
	.Dump();






