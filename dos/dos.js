
var reqCount;
var http = require('http');
var request = require('request');
http.Agent.defaultMaxSockets = 100;

process.argv.forEach(function(val,index,array){
if(index==2)
	reqCount=val;
});

if(!reqCount)
reqCount=1;


var options = {
	url: 'http://localhost:9080',
    headers: {
        'User-Agent': 'Mozilla/5.0'
    },
    	method:"GET"
};
var I=0;
var E=0;
var resp;
var Res;
var MR;
function callback(res){
I++;
	var MR = res.socket;
	 if(MR.readyState==='open'){
		console.log('pause');
		Res = res;
	}
}
function callbackBody(err,res,body){
I++;
	if(res){
		console.log(I+" "+res.statusCode);
		console.log(body);
	}
}
function errorcall(er){
E++;
console.log(I+' err');
};


function loopIt(){
	for(var i=0;i<reqCount;i++)
try{
request(options).on('error',errorcall).on('response',callback);
}catch(ex){
console.log(ex);
}
}

loopIt();
