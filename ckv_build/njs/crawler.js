
//https://zingnews.vn/hot-girl-trung-quoc-noi-phau-thuat-tham-my-300-lan-tu-nam-14-tuoi-post1076701.html


var util = require("util");
const https = require('https');
const fs = require('fs');

//var content = "";

var casigningcert = fs.readFileSync('cacert.pem');

var options = {
    host: 'zingnews.vn',
    port: 443,
    path: '/hot-girl-trung-quoc-noi-phau-thuat-tham-my-300-lan-tu-nam-14-tuoi-post1076701.html',
    ca: casigningcert
};

let content = '';
const req = https.request(options, (res) => {
    console.log('statusCode:', res.statusCode);
    console.log('headers:', res.headers);

    //res.on('data', (d) => {
    //    process.stdout.write(d);
    //});

    //res.setEncoding("utf8");
    res.on("data", function (chunk) {
        console.log('????')
        content += chunk;
    });

    res.on("end", function () {
        util.log(content);
    });
});

req.on('error', (e) => {
    console.error(e);
});
req.end();

//var req = http.request(options, function (res) {
//    res.setEncoding("utf8");
//    res.on("data", function (chunk) {
//        content += chunk;
//    });

//    res.on("end", function () {
//        util.log(content);
//    });
//});

//req.end();