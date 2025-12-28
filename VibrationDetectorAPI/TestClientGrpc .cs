using Grpc.Net.Client;

namespace VibrationDetectorAPI
{
    public class TestClientGrpc
    {
        public async void Testserver()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7034");
            var client = new VDStatusHandler.VDStatusHandlerClient(channel);
            Console.WriteLine("Grpc test");


            //var reply = await client.GetVDStatusAsync(new GetProductsRequest { });
            //foreach (var product in productsReply.Products)
            //{
            //    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}");

            //}
            Console.ReadKey();
        }

    }
}

