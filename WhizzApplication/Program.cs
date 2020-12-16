using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json.Linq;
using WhizzJsonRepository.Interfaces;
using WhizzJsonRepository.Services;
using IContainer = Autofac.IContainer;

namespace WhizzApplication
{
    public static class Program
    {
        public static IContainer Container { get; private set; }
        public static QuitePosDb QuitePosDb { get; private set; }
        public static async Task Main(string[] args)
        {
            Container = ContainerConfig.Configure();

            QuitePosDb = Container.Resolve<QuitePosDb>();

            var dto = new TestInsertDto
            {
                Name = "Someii",
                LongDescription = "Just a long, jlong desc",
                Quantity = (decimal) 12.41,
                Money = (decimal) 12.43,
                DateTime = DateTime.Now,
            };

            var dictionary = new Dictionary<string, string> {["id"] = "87fe400c-34f2-11eb-8a58-f72b7c428401"};
            // var jObject = new JObject {["id"] = "87fe400c-34f2-11eb-8a58-f72b7c428401", ["date"] = "2011-11-11T18:25:43.51Z"};
            // var jObject = new JObject
            // {
            //     ["id"] = "87fe400c-34f2-11eb-8a58-f72b7c428401"
            // };
            var jObject = new JObject
            {
                ["name"] = "Somiii",
                ["longDescription"] = "Just a long, jlong desc",
                ["quantity"] = 12.37,
                ["money"] = 12.43,
                ["dateTime"] = "2011-11-11T18:25:43.51Z",
                ["active"] = true,
            };
            var state = await repository.InsertInto("test").ValuesAsync(jObject);
            // var state = invoker.ValidatePrimaryKey(jObject, "test");
            //Int64, Boolean, Double, String

            Console.WriteLine(state.Response["data"]);
        }
    }
}