﻿using Grpc.Net.Client;
using GrpcMessageClient;
using GrpcService;

var channel = GrpcChannel.ForAddress("http://localhost:5166");
var messageClient=new Message.MessageClient(channel);

MessageResponse response= await messageClient.sendMessageAsync(new MessageRequest
{
	Message = "Selam",
	Name = "Nurullah"

});
Console.WriteLine(response.ToString());
//var greetClient=new Greeter.GreeterClient(channel);

//HelloReply result= await greetClient.SayHelloAsync(new HelloRequest { Name = "Nurullah" });

//Console.WriteLine(result.ToString());