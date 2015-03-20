var http = require('http')
var port = process.env.PORT || 3000;
var host = process.env.HOST || '127.0.0.1';

http.createServer(function(req, res) {
  res.writeHead(200, { 'Content-Type': 'text/plain' });
  res.end('Hello World');
}).listen(port, host);
console.log("Server running at "+host+":"+port);