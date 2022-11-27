/*
 * WCF Service: Contract
 * */
using System.Runtime.Serialization;
using System.ServiceModel;

[ServiceContract]
public interface ICalcService
{

	[OperationContract]
	int Sum(int x, int y);

	[OperationContract]
	SumSub SumAndSub(int x, int y);
}


[DataContract]
public class CompositeType
{
	bool boolValue = true;
	string stringValue = "Hello ";

	[DataMember]
	public bool BoolValue
	{
		get { return boolValue; }
		set { boolValue = value; }
	}

	[DataMember]
	public string StringValue
	{
		get { return stringValue; }
		set { stringValue = value; }
	}
}

[DataContract]
public class SumSub
{
	int sum;
	int sub;

	[DataMember]
	public int Sum
    {
		get;
		set;
    }
	
	[DataMember]
	public int Sub
    {
		get;set;
    }
}
