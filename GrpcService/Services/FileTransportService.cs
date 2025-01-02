using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using grpcFileTransportServer;


namespace GrpcService.Services
{
	public class FileTransportService : FileService.FileServiceBase
	{
		private readonly IWebHostEnvironment webHostEnvironment;

		public FileTransportService(IWebHostEnvironment webHostEnvironment)
		{
			this.webHostEnvironment = webHostEnvironment;
		}


		public override async Task<Empty> FileUpload(IAsyncStreamReader<BytesContent> requestStream, ServerCallContext context)
		{
			// Stream'in yapılacağı dizini belirleyelim
			string path = Path.Combine(webHostEnvironment.WebRootPath, "files");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			FileStream fileStream = null;
			try
			{
				int count = 0;
				decimal chunkSize = 0;
				
				while (await requestStream.MoveNext())
				{
					var current = requestStream.Current;
					
					if (count++ == 0)
					{
						string filePath = Path.Combine(path, $"{current.Info.FileName}{current.Info.FileExtension}");
						fileStream = new FileStream(filePath, FileMode.Create);
						fileStream.SetLength(current.FileSize);
					}

					var buffer = current.Buffer.ToByteArray();
					await fileStream.WriteAsync(buffer, 0, current.ReadedByte);
					Console.WriteLine($"Upload Progress: {Math.Round((chunkSize += current.ReadedByte) * 100 / current.FileSize)}%");
				}

				return new Empty();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during file upload: {ex.Message}");
				throw;
			}
			finally
			{
				if (fileStream != null)
				{
					await fileStream.FlushAsync();
					await fileStream.DisposeAsync();
				}
			}
		}


		public override async Task FileDownload(grpcFileTransportServer.FileInfo request, IServerStreamWriter<BytesContent> responseStream, ServerCallContext context)
		{
			string path = Path.Combine(webHostEnvironment.WebRootPath, "files");

			using FileStream fileStream = new FileStream($"{path}/{request.FileName}{request.FileExtension}", FileMode.Open, FileAccess.Read);


			byte[] buffer = new byte[2048];
			BytesContent content = new BytesContent
			{
				FileSize = fileStream.Length,
				Info = new grpcFileTransportServer.FileInfo { FileName = Path.GetFileNameWithoutExtension(fileStream.Name), FileExtension = Path.GetExtension(fileStream.Name) },

				ReadedByte = 0
			};

			while ((content.ReadedByte=await fileStream.ReadAsync(buffer,0,buffer.Length))>0)
			{
				content.Buffer=ByteString.CopyFrom(buffer);
				await responseStream.WriteAsync(content);	
			}
			fileStream.Close();
		}
	}
}
