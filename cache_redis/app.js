//let PORT = 8080;

const HTTP = require('http');
const SERVER = HTTP.createServer().listen(8080, '127.0.0.1');
//PORT = SERVER.address().port;
//console.log(PORT);

SERVER.on('request', function (req, res) {
    res.writeHead(200, { "Content-Type": "text/html; charset: UTF-8" });
    res.write("Hello from Node! ");
    res.write(" Server listening on port " + this.address().port + ' at ' + new Date().toLocaleString());
    res.end();
});

////#region [ READLINE ]

//const READ_LINE = require("readline");
//const RL = READ_LINE.createInterface({ input: process.stdin, output: process.stdout });
//RL.on("line", function (line) {
//    let text = line.trim();
//    const a = text.split(' ');
//    let cmd = a[0].toLowerCase();
//    switch (cmd) {
//        case 'exit':
//            process.exit();
//            break;
//        case 'cls':
//        case 'clear':
//            console.clear();
//            break;
//        case 'info':
//            console.clear();
//            break;
//        default:
//            console.clear();
//            break;
//    }
//});

////#endregion