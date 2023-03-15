namespace LoadBalancer
{
    public interface ILoadBalancer
    {
        public List<string> GetAllServices();
        public int AddService(string url);
        public int RemoveService(string url);
        public ILoadBalancerStrategy GetActiveStrategy();
        public void SetActiveStrategy(ILoadBalancerStrategy strategy);
        public string NextService();
    }
}
