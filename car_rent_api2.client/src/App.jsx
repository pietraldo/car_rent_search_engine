import { useEffect, useState } from 'react';
import './App.css';

function App()
{
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState('');

    useEffect(() =>
    {
        fetchMessages(); // Fetch messages on initial load
    }, []);

    const messageList = messages.length === 0 ? (
        <p><em>No messages yet...</em></p>
    ) : (
        <div>
            {messages.map((message, index) => (
                <div key={index}>{message}</div>
            ))}
        </div>
    );

    return (
        <div>
            <h1 id="messageLabel">Chat with Julka</h1>
            {messageList}
            <button onClick={fetchMessages}>Refresh Messages</button>
            <input
                type="text"
                id="newMessageInput"
                value={newMessage}
                onChange={(e) => setNewMessage(e.target.value)}
                placeholder="Type your message here"
            />
            <button onClick={() => sendMessage(newMessage)}>Send</button>
        </div>
    );

    // Fetch messages from the backend
    async function fetchMessages()
    {
        try
        {
            const response = await fetch('/weatherforecast');  // Use relative URL
            if (response.ok)
            {
                const data = await response.json();
                console.log(data);
                setMessages(data);  // Update state with fetched messages
            } else
            {
                console.error('Failed to fetch messages');
            }
        } catch (error)
        {
            console.error('Error fetching messages:', error);
        }
    }

    // Send a new message to the backend
    async function sendMessage(message)
    {
        if (!message) return;  // Prevent sending empty messages

        try
        {
            const response = await fetch('/weatherforecast', {  // Use relative URL
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ message }),  // Send the message in the expected JSON format
            });

            if (response.ok)
            {
                setNewMessage('');  // Clear the input field after sending
                fetchMessages();    // Refresh the message list
            } else
            {
                const errorText = await response.text(); // Log error message if POST failed
                console.error('Failed to send message:', errorText);
            }
        } catch (error)
        {
            console.error('Error sending message:', error);
        }
    }


}

export default App;
