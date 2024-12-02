using Microsoft.AspNetCore.Mvc;

namespace _2_Controllers; 

public class Test4Controller
{
    [HttpGet]
    public string GetData()
        => "Test4 controller entered...";
}
