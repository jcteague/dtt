email_transport = require('./email_transport')

send = (email_message) ->
    email_transport.send_email email_message

module.exports =
    send: send
