
var s = '';
var obj = {};

var request = {
	"headers": {
		"url": "https://vnexpress.net/",
		"method": "GET"
	},
	"data": ""
};

//s = ___api.request_async(request);
//s = ___api.request_async(JSON.stringify(request));
s = ___api.request_async(JSON.stringify(___para));

obj = JSON.parse(s);
// do something ...


return s;