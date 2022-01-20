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
	//Nested queries
	//sometimes referred to as subqueries

	//simply put: it is a query within a query [....]

	//List all sales support employees showing their
	//fullname (last, first), title, phone.
	//For each employee, show a list of the customers
	//they support. List the customer fullname (last, first),
	//City and State.

	//Smith, Bob  Sales Support 7801236542  //this is employee
	//		Kan, Jerry Edmonton ab			//this is customer
	//		Ujest, Shirley Edmonton ab		//this is customer
	//		Stewant, Iam Edmonton ab		//this is customer
	//		Behold, Lowan Edmonton ab		//this is customer
	//Kake, Patty  Sales Support Supervisor 7801236542  //this is employee
	//		Kan, Jerry Edmonton ab			//this is customer
	//		Ujest, Shirley Edmonton ab		//this is customer
	//		Stewant, Iam Edmonton ab		//this is customer
	//		Behold, Lowan Edmonton ab		//this is customer
	//Jones, Mike  Sales Support 7801236542  //this is employee
	//		Kan, Jerry Edmonton ab			//this is customer
	//		Ujest, Shirley Edmonton ab		//this is customer
	//		Stewant, Iam Edmonton ab		//this is customer
	//		Behold, Lowan Edmonton ab		//this is customer

	//There appears to be 2 separate lists that need to  be
	// within one final dataset collection
	//one for employees
	//one for customers
	//concern: the list are intermixed!!!!!
	
	//C# point of view in a class definition
	//Composite classes have single occuring fields AND uses other classes
	//the other class maybe a single instance OR List<T>
	//List<T> is a collection with a defined datatype of <T>
	//classname
	//		property
	// 		property
	//		...
	//		collection<T> (set of records) (still a property)
	
	var results = Employees
				.Where(e => e.Title.Contains("Sales Support"))
				.Select(e => new EmployeeItem
						{
							FullName = e.LastName + ", " + e.FirstName,
							Title = e.Title,
							Phone = e.Phone,
							CustomerList = e.SupportRepCustomers

										.Select(c => new CustomerItem
										{
											FullName = c.LastName + ", " + c.FirstName,
											City = c.City,
											State = c.State

										})
						});
	results.Dump();
}

// You can define other methods, fields, classes and namespaces here

public class CustomerItem
{
	public string FullName {get;set;}
	public string City {get; set;}
	public string State {get;set;}
}

public class EmployeeItem
{
	public string FullName { get; set; }
	public string Title { get; set; }
	public string Phone { get; set; }
	//how to declare the nest query definition
	//the results of a nested query is a collection
	public IEnumerable<CustomerItem> CustomerList {get; set;}
}