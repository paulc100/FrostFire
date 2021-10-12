using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour 
{
    [Header("Message and Chat References")]
    [SerializeField]
    private int maxMessages = 20;
    [SerializeField]
    private GameObject chatPanel, textObject;
    [SerializeField]
    private InputField chatInput;

    [Header("Chat colors")]
    [SerializeField]
    private Color clientMessageColor; 
    [SerializeField]
    private Color spikeBroadcastColor;

    private List<Message> messageList = new List<Message>();
    public static Queue<string> incomingMessageQueue = new Queue<string>();
    private object asyncLock = new object();

    void Update() {
        if (chatInput.text != "") {
            if(Input.GetKeyDown(KeyCode.Return)) {
                TCPClient.SendBinaryMessageToSpike(chatInput.text);
                chatInput.text =  "";
            }
        } else {
            if (!chatInput.isFocused && Input.GetKeyDown(KeyCode.Return)) {
                chatInput.ActivateInputField();
            }
        }

        if (incomingMessageQueue.Count == 0) return;

        lock(asyncLock) {
            foreach(var message in incomingMessageQueue) {
                // TODO: Add data in binary to distinguish these message types
                AppendMessageToChat(message, Message.MessageType.clientMessage);
            }
            incomingMessageQueue.Clear();
        }
    }

    public void AppendMessageToChat(string text, Message.MessageType messageType) {
        LimitMessageHistory();

        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = ApplyMessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    private void LimitMessageHistory() {
        if (messageList.Count >= maxMessages) {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
    }

    private Color ApplyMessageTypeColor(Message.MessageType messageType) {
        Color messageColor = Color.grey;

        switch(messageType) {
            case Message.MessageType.clientMessage:
                messageColor = clientMessageColor;
                break;
            case Message.MessageType.spikeBroadcast:
                messageColor = spikeBroadcastColor;
                break;
        }

        return messageColor;
    }
}

[System.Serializable]
public class Message {
    public string text;
    public Text textObject;
    public MessageType type;

    public enum MessageType {
        clientMessage,
        spikeBroadcast
    }
}