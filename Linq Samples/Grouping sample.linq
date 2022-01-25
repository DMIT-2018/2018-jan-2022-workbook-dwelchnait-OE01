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

//Grouping

//when your create a group it builds two (2) components
//	a) Key component (group by values)
//		reference this component using the groupname.Key[.Property]
//	(property < - > column < - > field < - > attribute < - > value)
//	b) data of the group (instances of the collection)

//ways to group
//a) by a single column (field,property,attribute,value)	groupname.Key
//b) by a set of columns (anonymous dataset)				groupname.Key.PropertyName
//c) by using an entity (x.navproperty) ** try to avoid **	groupname.Key.PropertyName

//concept processing
//start with a "pile" of data (original collection)
//specify the grouping property (ies)
//result of the group operation will be to "place the data into smaller piles"
//	the piles are dependant on the grouping property(ies) value(s)
//	the grouping property(ies) become the Key
//	the individual instances are the data in the smaller piles
//	the entire individual instance of the original collection is place in the
//			smaller pile
//Manipulate each of the "smaller piles" using your Linq commands

//grouping is an excellent way to organize your data especially if
//	you need to process data on a property that is NOT a "relative key"
//	such as a foreign key which forms a "natural" group using the
//	navigation properties

//grouping is different then Ordering
//Ordering is the re-sequencing of a collection for display
//grouping re-organizes a collection into separate, usually smaller
//		collections for processing

//Display albums by ReleaseYear
//	this request does not need grouping
//	this request is an ordering of output (OrderBy)
//	this affects display only
Albums
	.OrderBy(a => a.ReleaseYear)

//Display albums grouped by ReleaseYear
//	explicit  request to breakup the display into desired "piles" (collections)
Albums
	.GroupBy(a => a.ReleaseYear)

//query syntax
from a in Albums
group a by a.ReleaseYear

//processing on the created groups of the GroupBy command

Albums
	.GroupBy(a => a.ReleaseYear)  //this methods returns a collection of mini-collections
	.OrderBy(bob => bob.Key)
	.Select(eachgPile => new
		{
			Year = eachgPile.Key,
			NumberofAlbums = eachgPile.Count( )
		}
		) //the select processes each mini-collection one at a time

//query syntax
//using this syntax you MUST specify the name your wish to use to refer to the
//	grouped (mini-collections) collections
//AFTER coding your group command you MUST (are restricted to) use the name you
//	have given your group collection
from a in Albums
//orderby a.ReleaseYear would be valid becasue "a" is still in context
group a by a.ReleaseYear into eachgPile
//orderby a.ReleaseYear would be invalid because "a" is out of context, group name is eachgPile
//orderby eachgPile.Key would need to use eachgPile (in context) and reference the key value [.Key]
select( new
	{
		Year = eachgPile.Key,
		NumberofAlbums = eachgPile.Count()
	}
	)

//use a multiple set of properties to form the group
//include a nested query to report on the "mini-collection" (small piles) of the grouping

//Display albums grouped by ReleaseLabel, ReleaseYear. Display the
// ReleaseYear and the number of albums. List only the years with 2
// or more albums released

//original collection (large pile of data): Albums
//filtering cannot be decided until the groups are created
//grouping: ReleaseLabel, ReleaseYear (an anonymous set)
//filtering: group.Count >= 2
//report: year, count of albums, list of albums in the group (nested query)

Albums
	.GroupBy(a => new {a.ReleaseLabel, a.ReleaseYear})
	.Where(eachgPile => eachgPile.Count() >= 2)
	.OrderBy(eachgPile => eachgPile.Key.ReleaseLabel)
	.Select(eachgPile => new
			{
				Label = eachgPile.Key.ReleaseLabel,
				Year = eachgPile.Key.ReleaseYear,
				NumberofAlbums = eachgPile.Count(),
				//NumberofTracks = (eachgPile.Select(a => a.Tracks.Count())).Sum(),
				AlbumGroupItems = eachgPile
									.Select(eachgPileInstance => new
										{
											TitleOnAlbum = eachgPileInstance.Title,
											YearOnAlbum = eachgPileInstance.ReleaseYear,
											TrackCount = eachgPileInstance.Tracks.Count()
										})
			}
			)





