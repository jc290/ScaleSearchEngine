namespace LoadBalancer
{
    public class LoadBalancerRoundRobinStrategy : ILoadBalancerStrategy
    {
        int currentIndex = 0;

        string ILoadBalancerStrategy.NextService(List<string> services)
        {
            if (services.Count == 0)
                return "";

            if (currentIndex >= services.Count - 1)
                currentIndex = 0;

            var next = services.ElementAtOrDefault(currentIndex);
            currentIndex++;

            return next;
        }
    }
}
