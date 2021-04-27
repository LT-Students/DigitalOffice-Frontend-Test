using CloudAPITestProject.Models;
using CloudAPITestProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CloudAPITestProject
{
    class Program
    {
        private static List<RequestResult> StartCloudAPITest()
        {
            Assembly assembly = Assembly.LoadFrom("CloudAPITestProject.dll");
            List<Type> interfaceInheritors = new();

            interfaceInheritors.AddRange(assembly.ExportedTypes
                .Where(
                    t =>
                        t.IsClass
                        && t.IsAbstract == false
                        && t.GetInterface(nameof(ICheckEndpoint)) != null
                ));

            List<RequestResult> results = new();

            foreach (var type in interfaceInheritors)
            {
                var realization = Activator.CreateInstance(type);
                MethodInfo method = type.GetMethod("Check");

                results.Add(method.Invoke(realization, null) as RequestResult);
            }

            return results;
        }

        static void Main(string[] args)
        {
            var responses = StartCloudAPITest();

            foreach (var response in responses)
            {
                Console.WriteLine(response.IsSuccess);
                foreach (var error in response.Errors)
                {
                    Console.WriteLine(error);
                }
            }
        }
    }
}
