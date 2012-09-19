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
    ###
    email_transport.send_email {
        from: 'Eddy Espinal <eespinal@intellisys.com.do'
        to: 'eddy.w.espinal@gmail.com, eespinal@intellisys.com.do'
        subject: 'Test Email From Nodemailer'
        text: 'This is a test email from the nodemailer package of node'
    }
    ###

module.exports =
    send: send
