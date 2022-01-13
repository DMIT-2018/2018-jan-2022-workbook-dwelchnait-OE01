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
	//ProgramIDE
	//you can have multiple queries in this IDE environment
	//this environment works "like" a console application

	//this allows one to pre-test complete components that can
	//		be move directly into your backend application (class library)

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
	var resultsq = GetAllQ(paramyear);
	resultsq.Dump();

	paramyear = 2000;
	var resultsm = GetAllM(paramyear);
	resultsm.Dump();
}

// You can define other methods, fields, classes and namespaces here

//imagine this is a method in your BLL service class
public List<Albums> GetAllQ(int paramyear)
{
	var resultsq = from x in Albums
				   where x.ReleaseYear == paramyear
				   select x;
	return resultsq.ToList();
}

public List<Albums> GetAllM(int paramyear)
{
	var resultsq = Albums
					.Where(x => x.ReleaseYear == paramyear)
					.Select(x => x);
	return resultsq.ToList();
}




