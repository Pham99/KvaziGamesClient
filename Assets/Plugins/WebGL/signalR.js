
function Connect(serverURL) {
    startConnection(serverURL)
}


async function startConnection(serverURL) {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(serverURL)
        .withAutomaticReconnect()
        .build();

    connection.on("ReceivedDirection", function (direction, id) {
        console.log("Direction received: " + direction);
        SendToUnity("OnReceiveDirection", direction, id);
    });
    connection.on("AddPlayerToGame", function (id, name) {
        SendToUnity("AddPlayer", id, name);
    });

    connection.on("SendQRCode", function (arraybuffer) {
        console.log(arraybuffer);
        SendToUnity("DisplayQRCodeFromBase64", arraybuffer);
    });

    try {
        await connection.start();
        SendToUnity("NotifyConnectionSuccess")
        console.log("Connected to SignalR hub!");
    } catch (err) {
        console.error("Failed to connect to SignalR hub:", err.toString());
        setTimeout(startConnection, 5000); // Retry connection
    }
}

function SendToUnity(command, ...parameters) {
    if (unityInstance) {
        let jsonString = JSON.stringify(parameters);
        unityInstance.SendMessage("SignalRJSAdapter", command, jsonString)
    }
}

function arrayBufferToBase64(buffer) {
    let binary = '';
    const bytes = new Uint8Array(buffer);
    const len = bytes.byteLength;
    for (let i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}

