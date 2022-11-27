/*
 * WCF Service
 * lufer & Oscar
 * 2022-2023
 * */

using System;

public class CalcService : ICalcService
{

	public int Sum(int x, int y)
    {
		return x + y;
    }

	public SumSub SumAndSub(int x, int y)
    {
		SumSub aux= new SumSub();
		aux.Sum = x + y;
		aux.Sub = x - y;
		return aux;
    }
}
