using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json.Linq;
using WhizzJsonRepository.Interfaces;
using IContainer = Autofac.IContainer;

namespace WhizzApplication
{
    public static class Program
    {
        public static IContainer Container { get; private set; }
        public static async Task Main(string[] args)
        {
            Container = ContainerConfig.Configure();

            var repository = Container.Resolve<IRepository>();
            var dto = new TestInsertDto
            {
                Name = "SomeNa",
                LongDescription = "Just a long, jlong desc",
                Quantity = (decimal) 12.43,
                Money = (decimal) 12.43,
                DateTime = DateTime.Now,
            };

            var dictionary = new Dictionary<string, string> {["id"] = "87fe400c-34f2-11eb-8a58-f72b7c428401"};
            // var jObject = new JObject {["id"] = "87fe400c-34f2-11eb-8a58-f72b7c428401", ["date"] = "2011-11-11T18:25:43.51Z"};
            var jObject = new JObject {["id"] = "87fe400c-34f2-11eb-8a58-f72b7c428401"};
            var state = await repository.Find("test").OneAsync(jObject);
            // var state = invoker.ValidatePrimaryKey(jObject, "test");
            //Int64, Boolean, Double, String

            Console.WriteLine(state.Response["data"]);
        }
    }
}