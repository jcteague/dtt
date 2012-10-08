https = require('https')
Q = require('q')

request = (data, request_parameters) ->
    console.log 'the data to post', data
    deferred = Q.defer()
    request = https.request request_parameters, (response) ->
        response.setEncoding('utf8')
        response.on 'data', (chunk) ->
            deferred.resolve(success:true, data: chunk)
        response.on 'error', (e) ->
            console.log("Got error: " + e.message)
            deferred.reject(success: false, messages: "Got error: #{e.message}")

    request.end(data)
    deferred.promise

module.exports =
    request: request
