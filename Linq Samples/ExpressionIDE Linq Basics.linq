<Query Kind="Expression">
  <Connection>
    <ID>2621ab68-ddd0-477b-bd31-283968b8e170</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

//Our code is using C# grammar/syntax

//comments are done with slashes
//hotkey to comment ctrl+k, ctrl+c
//         uncomment ctrl+k, ctrl+u
//alternately use ctrl + / as a toggle

//Expressions
// single linq query statements without a semi-colon//
//	you can have multiple statements in your file BUT
//		if you do so, you MUST highlight the statement to execute

//executing using F5 or the green triangle on the query menu

//to toggle your results on and off (visible) use ctrl + r

//Query syntax
//	use a "sql-like" syntax
//	view the Student Notes for examples under
//		Notes/Linq Intro
// or 	Demo/eRestaurant/Linq Query and Method syntax

//query: Find all albums released in 2000. Display the entire
//			album record

from instancerowofcollection in Albums
where instancerowofcollection.ReleaseYear == 2000
select instancerowofcollection

//Method syntax
//uses C# method syntax (OOP language grammar)
//Collection (Albums)
// to excute a method on the collection you need to use
//		the access operator (dot operator)
// this results in the returning of an other collection from the method !!***
// method name starts with a capital
// methods contain contents with a delgate
// a delegate desrcibes the action to be done

//C# method
//access [static] void/datatype methodname ([list of parameters])

Albums
   .Where(instancerowofcollection => (instancerowofcollection.ReleaseYear == 2000))
   .Select(instancerowofcollection => instancerowofcollection)