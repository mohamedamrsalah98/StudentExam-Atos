import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

const SignalR = () => {
  const [hubConnection, setHubConnection] = useState(null);
  const [examResults, setExamResults] = useState([]);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:7112/examHub")
      .build();

    setHubConnection(connection);

    return () => {
      if (connection.state === signalR.HubConnectionState.Connected) {
        connection.stop();
      }
    };
  }, []);

  const startConnection = async () => {
    try {
      await hubConnection.start();
    } catch (err) {
      console.error(err);
    }
  };

  const subscribeToExamResult = (callback) => {
    if (hubConnection) {
      hubConnection.on("SendExamResultNotification", (userId, score) => {
        callback({ userId, score });
      });
    }
  };

  useEffect(() => {
    if (hubConnection && hubConnection.state === signalR.HubConnectionState.Disconnected) {
      startConnection();
    }

    subscribeToExamResult((result) => {
      setExamResults((prevResults) => [...prevResults, result]);
    });

    return () => {
      if (hubConnection) {
        hubConnection.off("SendExamResultNotification");
      }
    };
  }, [hubConnection]);

  return (
    <div>
      <h2>Exam Results</h2>
      <ul>
        {examResults.map((result, index) => (
          <li key={index}>
            User: {result.userId}, Score: {result.score}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default SignalR;
