using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using UnityEngine;

public class TCPClient : MonoBehaviour 
{
    private const string CRLF = "\r\n";
    private static TcpClient clientSocket;
    private Thread clientThread;

    private void Start() {
        ConnectToSpikes(); 
    }

    private void ConnectToSpikes() {
        try {
            clientThread = new Thread(new ThreadStart(ListenForSpikeData));
            clientThread.IsBackground = true;
            clientThread.Start();
        } catch (Exception e) {
            Debug.Log("Client connection exception: " + e);
        }
    }

    private void ListenForSpikeData() {
        try {
            clientSocket = new TcpClient("localhost", 4040);
            Byte[] bytes = new Byte[1024];
            while (true) {
                using (NetworkStream stream = clientSocket.GetStream()) {
                    int byteArrayLength;

                    // Parse incoming stream to byte array
                    while((byteArrayLength = stream.Read(bytes, 0, bytes.Length)) != 0) {
                        byte[] incomingData = new byte[byteArrayLength];
                        Array.Copy(bytes, 0, incomingData, 0, byteArrayLength);

                        // Convert byte array to string message and trigger event
                        string receivedMessage = Encoding.ASCII.GetString(incomingData);

                        // TODO: Segregate logic for retrieved messages (Chat, Gameplay, etc.)
                        // TODO: Add data in binary to differentiate these requests 
                        ChatManager.incomingMessageQueue.Enqueue(receivedMessage);

                        Debug.Log(receivedMessage);
                    }
                }
            }
        } catch (SocketException e) {
            Debug.Log("Socket read exception: " + e);
        }
    }

    public static void SendBinaryMessageToSpike(string clientMessage) {
        if (clientSocket == null)
            return;

        try {
            NetworkStream stream = clientSocket.GetStream();
            if (stream.CanWrite) {
                // Configure appropriate line endings 
                clientMessage += CRLF;

                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(clientMessage);
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                Debug.Log("Client message successfully sent!");
            }
        } catch (SocketException e) {
            Debug.Log("Socket write exception: " + e);
        }
    }
}
