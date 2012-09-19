Q = require('q')
transport = transport_factory = require('./email_transport_factory').get()

send_email = (email_message) ->
    deferred = Q.defer()
    transport.sendMail email_message, (error, response) ->
        if error
            console.log 'email had error', error
            deferred.reject error
        else
            console.log 'email was a success', response
            deferred.resolve response

    deferred.promise

module.exports =
    send_email: send_email
