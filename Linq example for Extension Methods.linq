<Query Kind="Program" />

void Main()
{
	//create an instance of the string class called message
	string message = "hello world"; //instance of string class
	
	Console.WriteLine(message); // access the instance's content
	
	//classes have properties and methods
	Console.WriteLine(message.Length);	//Length is a property of string class
	Console.WriteLine(message.Substring(3)); //Substring() is a method of string class
	
	//What if I would like my string to "quack"
	Console.WriteLine(message.Quack()); //.Quack() is NOT part of the C# string class, you need to create an extension method
	Console.WriteLine(message);
	Console.WriteLine(message.Quack(5)); //.Quck(argument) method does not exist; you need to create an overload extension method
	string cheers = "Go Ducks Go";
	Console.WriteLine(cheers.Quack(20));
}

// You can define other methods, fields, classes and namespaces here

//Create extension method(S) for the following C# class; string

//step 1: make a static class to hold the extension method(s)
//		  this class can be called anything you like
public static class MyExtensionStringMethods
{
	//step 2: Add your public static string method(s) to this class
	
	public  static string Quack(this string self)
	{
		//the return datatype from this method will be a string
		//this is the datatype of the class instance we are extending
		//
		//NOTE: you do NOT necessarily need to return a value; that is the rdt could be void
		//
		//the 1st parameter (the error msg does use the word argument) of the method
		//	signature identifies the class the extension method is assoicate with
		//
		//the parameter requires the following syntax -> this datatype parametername
		//the contents of the parameter will be the contents of the calling instance (eg. message)
		
		//your logic for the method
		string result = "quack " + self + " quack";
		return result;
		
	}

	public static string Quack(this string self, int quacktimes)
	{
		//any additional parameters for the extension method follows
		//	the required datatype 1st parameter
		//you may have any number of additional parameters
		//code the additional parameters just like any other method parameter

		//your logic for the method
		string quacks = "";
		for(int i = 0; i < quacktimes; i++)
		{
			quacks += "..quack.. ";
		}
		return $"{self} ({quacks})";

	}
}