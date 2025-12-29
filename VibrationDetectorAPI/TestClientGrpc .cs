//using Google.Protobuf.WellKnownTypes;
//using Grpc.Net.Client;
//using System;
//using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

//// TestClientGrpc.cs
////using System;
//using System.Net.Http;
//using System.Threading.Tasks;
////using Google.Protobuf.WellKnownTypes;
////using Grpc.Net.Client;
////using vdstatus;
//using VibrationDetectorAPI;

//namespace VibrationDetectorAPI
//{
//    public class TestClientGrpc
//    {
//        public async Task Testserver()
//        {
//            // Delay for 2 seconds
//            await Task.Delay(5000); // 2000 ms = 2 seconds

//            //var channel = GrpcChannel.ForAddress("https://localhost:5001");

//            var httpHandler = new HttpClientHandler();
//            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

//            var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
//            {
//                HttpHandler = httpHandler
//            });


//            var client = new VDStatusHandler.VDStatusHandlerClient(channel);
//            Console.WriteLine("Testserver:Grpc test");

//            var request = new GetVDStatusRequest
//            {
//                VibrationDetectorId = 123,  // example detector id
//                UserId = 42,                // example user id
//                UserPanelActionDate = Timestamp.FromDateTime(DateTime.UtcNow) // current UTC time
//            };

//            var reply = await client.GetVDStatusAsync(request);

//            Console.WriteLine("Testserver:Print status list.");

//            foreach (var status in reply.Vdstatus)
//            {
//                Console.WriteLine(
//                    $"Id: {status.VibrationDetectorId}" +
//                    $"DeviceName: {status.DeviceName}" +
//                    $"Location: {status.Location}" +
//                    //Larmad eller inte larmad
//                    $"AlarmArmed: {status.AlarmArmed}" +
//                    //Alarm utlöst eller inte   
//                    $"AlarmTriggered:{status.AlarmTriggered}" +
//                    $"VibrationLevel:{status.VibrationLevel}" +
//                    $"VibrationLevelThreshold:{status.VibrationLevelThreshold}" +
//                    $"RequestSuccessful:{status.RequestSuccessful}" +
//                    $"ErrorMessage:{status.ErrorMessage}");
//            };

//            Console.WriteLine("Anneli: Press a key to continue");

//            //Console.ReadKey();












//        }

//    }
//}

