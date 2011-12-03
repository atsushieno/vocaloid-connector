using System;
using Commons.VocaloidApi;

namespace Commons.VocaloidApi.Test
{
	public class SocketVocaloidConnectorTest
	{
		public static void Main ()
		{
			var client = new SocketVocaloidConnector ();
			client.Open ("localhost");
			client.MessageBox ("sent from client", MessageBoxStyle.OK);
			client.Close ();
		}
	}
}

