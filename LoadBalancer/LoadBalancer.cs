using Common;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace LoadBalancer
{
    public class LoadBalancer : ILoadBalancer
    {
        public List<string> _services = new List<string>();
        public ILoadBalancerStrategy _strategy = new LoadBalancerRoundRobinStrategy();

        public LoadBalancer()
        {
            Console.WriteLine("Added services to loadbalancer");
            foreach (var service in Config.Services)
            {
                _services.Add(service.ToString());
            }
        }

        public int AddService(string url)
        {
            _services.Add(url);
            return _services.IndexOf(url);
        }

        public ILoadBalancerStrategy GetActiveStrategy()
        {
            return _strategy;
        }

        public List<string> GetAllServices()
        {
            return _services;
        }

        public string NextService()
        {
            return _strategy.NextService(_services);
        }

        public int RemoveService(string url)
        {
            var index = _services.IndexOf(url);

            if (index != -1)
            {
                _services.RemoveAt(index);
            }
            return index;
            
        }

        public void SetActiveStrategy(ILoadBalancerStrategy strategy)
        {
            _strategy = strategy;
        }
    }
}
