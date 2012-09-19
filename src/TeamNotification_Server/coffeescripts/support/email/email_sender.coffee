email_transport = require('./email_transport')
###
#
# mailOptions =
#    from: "Sender Name ✔ <sender@example.com>", // sender address
#    to: "receiver1@example.com, receiver2@example.com", // list of receivers
#    subject: "Hello ✔", // Subject line
#    text: "Hello world ✔", // plaintext body
#    html: "<b>Hello world ✔</b>" // html body
#
###

send = (email_message) ->
    email_transport.send_email email_message

module.exports =
    send: send
