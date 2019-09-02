


1. AcceptWebSocketAsync会获取一个websocket
2. result.CloseStatus.HasValue 不是false，连接就一直存在；
3. 执行ReceiveAsync，直到收到消息为止；



------------------------------------------
1.来自同一个地方的访问，server端可能建立不同的sessionid；
2.当没有合适的处理机制，当客户端主动断开连接后，server端还会继续发送消息给client；
3.建立SessionManager只负责连接，具体的发送逻辑放到发送线程处理；
4.一个sessionID可以订阅多种消息，所以应该是以sessionID对应多种reqMsgType

