var http = require('http');
var path = require('path');
var express = require('express');

var app = express();
var server = http.Server(app);

var expiry = require('static-expiry');
var staticDir = path.join(__dirname, 'public');
app.use(expiry(app, {
  dir: staticDir,
  debug: true
}));
app.use('/', express.static(staticDir));


server.listen(3000, '0.0.0.0', function onStart(req, res) {
  console.info('application is listening at port http://localhost:3000');
});
